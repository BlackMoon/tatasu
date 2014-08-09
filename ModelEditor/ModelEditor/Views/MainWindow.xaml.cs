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

namespace ModelEditor
{  
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {              
        private List<Task> tasks = new List<Task>();
        private ListViewModel lvm = new ListViewModel();

        public MainWindow()
        {
            InitializeComponent();

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

        private void ReadFile(FileData fd, CancellationToken ct)
        {
            if (ct.IsCancellationRequested == true)
                ct.ThrowIfCancellationRequested();
            
            using (FileStream fs = new FileStream(fd.FileFullName, FileMode.Open))
            {
                XmlDocument xd = new XmlDocument();
                xd.Load(fs);

                XmlElement el = xd.DocumentElement;
                TreeViewModel root = new TreeViewModel() {  IsExpanded = true };
                root.Name = el.Name;                
                fd.Items.Add(root);

                foreach (XmlNode nd in el.ChildNodes)
                {
                    TreeViewModel item = new TreeViewModel();
                    item.Name = nd.Name;
                    root.Items.Add(item);
                }
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
                CancelTasks();

                lvm.Loaded = false;
                lvm.Items.Clear();

                DirectoryInfo di = new DirectoryInfo(@"d:\dev\br\Portal\conf\dbmi\arm\"/*dlg.SelectedPath*/);
                if (di.Exists)
                {
                    FileInfo[] fis = di.GetFiles("*.xml");                    
                    tasks.Capacity = fis.Length;
                    
                    foreach (FileInfo fi in fis)
                    {
                        FileData fd = new FileData(fi.Name, fi.FullName);

                        tasks.Add(Task.Factory.StartNew((_fd) => ReadFile((FileData)_fd, (_fd as FileData).Token), fd, fd.Token));
                        lvm.Items.Add(fd);
                    }

                    int selected = Task.WaitAny(tasks.ToArray());                    

                    if (selected > -1)
                    {
                        lvm.Loaded = true;
                        lvm.SelectedItem = lvm.Items[selected];
                    }
                }
            }
        }

        private void SaveCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void ExitItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
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
                Task t = tasks[lst_Files.SelectedIndex];
                t.Wait();

                trv_Models.ItemsSource = lvm.SelectedItem.Items;
            }
        }

        private void trv_Models_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewModel tvm = (TreeViewModel)e.NewValue;
            if (tvm != null)
            {
                tb_Node.Text = tvm.Name;
            }     
        }

        private void PluginItem_Click(object sender, RoutedEventArgs e)
        {
            new PluginWindow(){ Owner = this }.ShowDialog();            
        }
    }
}
