using PluginInterface;

namespace ModelEditorPlugins
{
    /// <summary>
    /// Плагин для обработки узло с тегом <Parameter>
    /// </summary>
    public class PluginModel1: IPlugin
    {
        private int version = 1;
        private string node = "Parameter";

        public int Version
        {
            get
            {
                return version;
            }
        }
        
        public string Node {
            get
            {
                return node;
            }
        }
    }
}
