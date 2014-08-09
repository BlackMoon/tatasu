using PluginInterface;

namespace ModelEditorPlugins
{
    /// <summary>
    /// Плагин для обработки узло с тегом <Equipment>
    /// </summary>
    public class PluginModel3: IPlugin
    {
        private int version = 1;
        private string node = "Equipment";

        public int Version
        {
            get
            {
                return version;
            }
        }        

        public string Node
        {
            get
            {
                return node;
            }
        }
    }
}
