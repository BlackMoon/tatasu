﻿using PluginInterface;
using System.Xml;

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

        public string GetNodeName(XmlNode nd)
        {
            string name = nd.Name;

            var attr = nd.Attributes["Name"];
            if (attr != null)
                name = attr.Value;

            return name;
        }
    }
}
