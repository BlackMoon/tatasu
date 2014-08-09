using PluginInterface;

namespace ModelEditorPlugins
{
    /// <summary>
    /// Плагин для обработки узло с тегом <Commands>
    /// </summary>
    public class PluginModel2: IPlugin
    {
        private int version = 1;
        private string node = "Commands";

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
