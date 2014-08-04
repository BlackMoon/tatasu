using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;

namespace ModelEditor
{

    class ModelItem
    {
        public string Name { get; set; }
    }

    class ModelData
    {
        public List<ModelItem> items { get; set; }

        public ModelData()
        {
            items = new List<ModelItem>();
        }
    }

    class TaskCancel
    {
        public CancellationTokenSource Token { get; set; }
        public Task Task { get; set; }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int selected;
        private ModelData[] modelData;
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
                FileInfo[] fis = new DirectoryInfo(dlg.SelectedPath).GetFiles("*.xml");
                int ix = 0, len = fis.Length;
                
                tasks.Clear();
                tasks.Capacity = len;
                modelData = new ModelData[len];

                lst_Files.Items.Clear();
                for (ix = 0; ix < fis.Length; ++ix)
                {
                    FileInfo fi = fis[ix];
                    lst_Files.Items.Add(fi);

                    tasks[ix] = Task.Factory.StartNew((Object i) =>
                    {
                        ModelData td = new ModelData();
                        using (FileStream fs = new FileStream(fi.FullName, FileMode.Open))
                        {
                            XmlDocument xd = new XmlDocument();
                            xd.Load(fs);

                            XmlElement root = xd.DocumentElement;
                            foreach(XmlNode nd in root.ChildNodes)
                            {
                                ModelItem item = new ModelItem();
                                item.Name = nd.Name;
                                td.items.Add(item);  
                            }                            
                        }
                        modelData[(int)i] = td;
                    }, ix);
                }
                selected = Task.WaitAny(tasks.ToArray());
                
                if (selected > -1)
                    lst_Files.SelectedItem = lst_Files.Items.GetItemAt(selected);               
            }
        }

        private void ExitItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //tasks.ForEach((t) => t. .Cancel());

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
            int ix = (sender as ListBox).SelectedIndex;
            Task t = tasks[ix];
            t.Wait();            

            trv_Models.ItemsSource = modelData[ix].items;
        }

    }
}
