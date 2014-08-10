using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using ModelEditor.ViewModels;
using ModelEditor.Models;
using ModelEditor.Plugins;
using PluginInterface;
using System.ComponentModel;

namespace ModelEditor
{  
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private PluginManager pluginManager = PluginManager.Instance;

        private List<Task> tasks = new List<Task>();
        private ListViewModel lvm = new ListViewModel();

        private bool saveChoice = true;
        private string path;
        private string state;

        public event PropertyChangedEventHandler PropertyChanged;
        
        private void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public bool SaveChoice
        {
            get
            {
                return saveChoice;
            }
            set
            {
                saveChoice = value;
                OnPropertyChanged("SaveChoice");
            }
        }

        public string Path
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
                OnPropertyChanged("Path");
            }
        }

        public string State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
                OnPropertyChanged("State");
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
            lst_Files.DataContext = lvm;

            var prefs = new UserPreferences();
            this.Height = prefs.WindowHeight;
            this.Width = prefs.WindowWidth;
            this.Top = prefs.WindowTop;
            this.Left = prefs.WindowLeft;
            this.WindowState = prefs.WindowState; 
        }

        private void CancelTasks()
        {
            IEnumerable<FileData> files = lvm.Items;
            if (files != null)
            { 
                foreach (FileData fd in files)
                    fd.TokenSource.Cancel();
            }
            tasks.Clear();                
        }

        private void IterateTree(TreeViewModel tvm, XmlDocument xd, XmlElement el)
        {
            foreach (XmlAttribute attr in tvm.Node.Attributes)
            {
                el.SetAttribute(attr.Name, attr.Value);
            }

            if (tvm.Items.Count > 0)
            {
                foreach (TreeViewModel item in tvm.Items)
                {
                    XmlElement child = xd.CreateElement(string.Empty, item.Node.Name, string.Empty);
                    IterateTree(item, xd, child);
                    el.AppendChild(child);
                }
            }
            // нет потомков --> клонирование узла
            else
            {
                foreach (XmlNode nd in tvm.Node.ChildNodes)
                {
                    el.AppendChild(xd.ImportNode(nd, true));
                }
            }
        }

        private void IterateXml(TreeViewModel tvm, XmlNode el)
        {
            foreach (XmlNode nd in el.ChildNodes)
            {
                TreeViewModel item = new TreeViewModel() { Node = nd, Owner = this, Parent = tvm };
                if (!item.AttachPlugin())
                    IterateXml(item, nd);
                
                tvm.Items.Add(item);
            }
        }

        private void ReadFile(FileData fd, CancellationToken ct)
        {
            if (ct.IsCancellationRequested == true)
                ct.ThrowIfCancellationRequested();
            
            using (FileStream fs = new FileStream(fd.FileFullName, FileMode.Open))
            {
                XmlDocument xd = new XmlDocument();
                xd.Load(fs);
                
                XmlElement el = xd.DocumentElement;
                TreeViewModel root = new TreeViewModel() { IsExpanded = true, Node = el, Owner = this };
                if (!root.AttachPlugin())
                    IterateXml(root, el);
                
                fd.Items.Add(root);                
            }
        }

        private void ReadFolder(string path)
        {
            CancelTasks();

            DirectoryInfo di = new DirectoryInfo(path);
            if (di.Exists)
            {
                lvm.Loaded = false;
                lvm.Items.Clear();

                try
                {
                    pluginManager.Task.Wait();
                }
                catch (System.AggregateException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }

                FileInfo[] fis = di.GetFiles("*.xml");
                tasks.Capacity = fis.Length;

                foreach (FileInfo fi in fis)
                {
                    FileData fd = new FileData(fi.Name, fi.FullName);

                    tasks.Add(Task.Factory.StartNew((_fd) => ReadFile((FileData)_fd, (_fd as FileData).Token), fd, fd.Token));
                    lvm.Items.Add(fd);
                }

                try
                {
                    int selected = Task.WaitAny(tasks.ToArray());
                    if (selected > -1)
                        lvm.SelectedItem = lvm.Items[selected];
                }
                catch (System.AggregateException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }

                lvm.Loaded = true;
                Path = path;
            }
        }

        private void AboutItem_Click(object sender, RoutedEventArgs e)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            AssemblyName an = assembly.GetName();

            MessageBox.Show("вер. " + an.Version, "О программе");
        }        

        private void OpenCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            var dlg = new System.Windows.Forms.FolderBrowserDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ReadFolder(dlg.SelectedPath);    
            }
        }

        private void SaveCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            List<Task> savingtasks = new List<Task>(lvm.Items.Count);
            foreach (FileData fd in lvm.Items)
            {
                Task t = Task.Factory.StartNew((_fd) =>
                {
                    FileData data = (FileData)_fd;

                    if (data.Items.Count > 0)
                    {
                        XmlDocument xd = new XmlDocument();
                        XmlDeclaration xmlDeclaration = xd.CreateXmlDeclaration("1.0", "Windows-1251", null);

                        XmlElement root = xd.DocumentElement;
                        xd.InsertBefore(xmlDeclaration, root);

                        TreeViewModel tvm = data.Items[0];      // root model
                        XmlElement el = xd.CreateElement(string.Empty, tvm.Node.Name, string.Empty);
                        
                        IterateTree(tvm, xd, el);

                        

                        xd.AppendChild(el);

                        xd.Save(data.FileFullName);
                    }

                }, fd);

                savingtasks.Add(t);
            }

            string msg = "OK";
            MessageBoxImage icon = MessageBoxImage.Information;

            try
            {
                Task.WaitAll(savingtasks.ToArray());
                State = null;
            }
            catch (System.AggregateException ex)
            {
                msg = ex.Message;
                icon = MessageBoxImage.Warning;
            }

            MessageBox.Show(msg, "Внимание", MessageBoxButton.OK, icon);
        }

        private void ExitItem_Click(object sender, RoutedEventArgs e)
        {            
            Close();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            MessageBoxResult ret = MessageBoxResult.Yes;

            // файл  был изменен
            if (!string.IsNullOrEmpty(state))            
                ret = MessageBox.Show("Модели были изменены.\nВсе равно закрыть?", "ModelEditor", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (ret == MessageBoxResult.No)
                e.Cancel = true;
            
            CancelTasks();

            var prefs = new UserPreferences
            {
                WindowLeft = this.Left,
                WindowTop = this.Top,
                WindowWidth = this.Width,
                WindowHeight = this.Height,
                WindowState = this.WindowState
            };
            prefs.Save();            
        }

        private void lst_Files_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int ix = lst_Files.SelectedIndex;
            if (ix != -1)
            {
                try
                {
                    Task t = tasks[lst_Files.SelectedIndex];
                    t.Wait();

                    trv_Models.ItemsSource = lvm.SelectedItem.Items;
                }
                catch (System.AggregateException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        private void trv_Models_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewModel tvm = (TreeViewModel)e.NewValue;
            if (tvm != null)
            {
                pn_Editor.Children.Clear();
                pn_Editor.Children.Add(tvm.Editor);

                if (tvm.Plugin != null)
                    pn_Editor.Children.Add(tvm.ApplyBtn);
            }
        }

        private void PluginItem_Click(object sender, RoutedEventArgs e)
        {
            new PluginWindow(){ Owner = this }.ShowDialog();            
        }        
    }
}
