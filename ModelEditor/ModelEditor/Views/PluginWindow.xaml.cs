using System.Windows;
using ModelEditor.Plugins;
using System.Windows.Input;
using ModelEditor.ViewModels;

namespace ModelEditor
{
    /// <summary>
    /// Interaction logic for PluginWindow.xaml
    /// </summary>
    public partial class PluginWindow : Window
    {
        private PluginManager pluginManager = PluginManager.Instance;
        private PluginViewModel pvm = new PluginViewModel();

        public PluginWindow()
        {
            InitializeComponent();
            lst_Plugins.DataContext = pvm;

            pluginManager.Task.Wait();
            foreach (PluginDescription pd in pluginManager.Plugins.Values)
            {
                pvm.Items.Add(pd);
            }
        }  

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            pluginManager.LoadFromFolder();
            pluginManager.Task.Wait();
            foreach (PluginDescription pd in pluginManager.Plugins.Values)
            {
                pvm.Items.Add(pd);
            }
        }

        private void OnCloseCmd(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
