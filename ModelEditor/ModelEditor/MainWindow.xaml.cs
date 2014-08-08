using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using ModelEditor.ModelViews;
using ModelEditor.Plugins;

namespace ModelEditor
{  
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {              
        private List<Task> tasks = new List<Task>();        

        public MainWindow()
        {
            InitializeComponent();

            var prefs = new UserPreferences();
            this.Height = prefs.WindowHeight;
            this.Width = prefs.WindowWidth;
            this.Top = prefs.WindowTop;
            this.Left = prefs.WindowLeft;
            this.WindowState = prefs.WindowState; 
        }

        private void CancelTasks()
        {
            IEnumerable<ModelData> files = (IEnumerable<ModelData>)lst_Files.ItemsSource;
            if (files != null)
            { 
                foreach (ModelData md in files)
                    md.TokenSource.Cancel();
            }
        }

        private void ReadData(ModelData md, CancellationToken ct)
        {
            if (ct.IsCancellationRequested == true)
                ct.ThrowIfCancellationRequested();
            
            using (FileStream fs = new FileStream(md.FileFullName, FileMode.Open))
            {
                XmlDocument xd = new XmlDocument();
                xd.Load(fs);

                XmlElement el = xd.DocumentElement;
                ModelItem root = new ModelItem(true);
                root.Name = el.Name;                
                md.Items.Add(root);

                foreach (XmlNode nd in el.ChildNodes)
                {
                    ModelItem item = new ModelItem();
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

                tasks.Clear();
                lst_Files.Items.Clear();

                DirectoryInfo di = new DirectoryInfo(@"d:\dev\br\Portal\conf\dbmi\arm\"/*dlg.SelectedPath*/);
                if (di.Exists)
                {
                    FileInfo[] fis = di.GetFiles("*.xml");
                    int len = fis.Length;
                    tasks.Capacity = len;

                    foreach (FileInfo fi in fis)
                    {
                        ModelData md = new ModelData(fi.Name, fi.FullName);

                        tasks.Add(Task.Factory.StartNew((_md) => ReadData((ModelData)_md, (_md as ModelData).Token), md, md.Token));
                        lst_Files.Items.Add(md);
                    }
                    int selected = Task.WaitAny(tasks.ToArray());

                    if (selected > -1)
                        lst_Files.SelectedItem = lst_Files.Items.GetItemAt(selected);
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

                trv_Models.ItemsSource = (lst_Files.SelectedItem as ModelData).Items;
            }
        }

        private void trv_Models_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ModelItem mi = (ModelItem)e.NewValue;
            if (mi != null)
            {
                tb_Node.Text = mi.Name;
            }     
        }

        private void ReloadItem_Click(object sender, RoutedEventArgs e)
        {
            PluginManager.Instance.Load();   
        }
    }
}
