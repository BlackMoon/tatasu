
namespace ModelEditor.Plugins
{
    public sealed class PluginManager
    {
        private static readonly PluginManager instance = new PluginManager();

        private PluginManager() { }

        public static PluginManager Instance
        {
            get 
            {
                return instance; 
            }
        }

        public void Load()
        {
        }
    }
}
