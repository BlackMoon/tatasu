using System.Windows;
using ModelEditor.Plugins;

namespace ModelEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            PluginManager.Instance.Load();
        }        
    }
}
