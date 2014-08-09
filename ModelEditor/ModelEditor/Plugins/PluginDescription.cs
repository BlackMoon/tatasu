using PluginInterface;
using System;
using System.Xml.Serialization;

namespace ModelEditor.Plugins
{
    /// <summary>
    /// Класс-описание плагина
    /// </summary>
    [Serializable]
    [XmlType("plugin")]
    public class PluginDescription
    {
        public string AssemblyName { get; set; }
        public string ModelName { get; set; }        
        public string TypeName { get; set; }
        public string TypeFullName { get; set; }
        public int Version { get; set; }
        
        [XmlIgnore]
        public IPlugin Plugin { get; set; }
    }
}
