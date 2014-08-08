using System.Windows;
using ModelEditor.Plugins;

namespace ModelEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public PluginManager PluginManager = PluginManager.Instance;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            PluginManager.Load();
        }        
    }
}
