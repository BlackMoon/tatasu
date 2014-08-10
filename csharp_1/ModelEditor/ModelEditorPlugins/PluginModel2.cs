using PluginInterface;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml;

namespace ModelEditorPlugins
{
    class Row2
    {
        public string Name { get; set; }
        public string Equipment { get; set; }
        public string Command { get; set; }
    }

    /// <summary>
    /// Плагин для обработки узлов с тегом <Commands>
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

        public string GetNodeName(XmlNode nd)
        {
            string name = null;

            if (nd != null)
            {
                name = nd.Name;

                var attr = nd.Attributes["Name"];
                if (attr != null)
                    name = attr.Value;
            }
            return name;
        }

        public UIElement GetEditor(XmlNode nd)
        {
            DataGrid dataGrid = new DataGrid()
            {  
                CanUserReorderColumns = true,
                CanUserResizeColumns = true  
            };

            ObservableCollection<Row2> rows = new ObservableCollection<Row2>();
            foreach (XmlNode child in nd.ChildNodes)
            {
                Row2 r = new Row2();
                
                var attr = child.Attributes["Name"];
                if (attr != null)
                    r.Name = attr.Value;

                attr = child.Attributes["Equipment"];
                if (attr != null)
                    r.Equipment = attr.Value;

                attr = child.Attributes["Command"];
                if (attr != null)
                    r.Command = attr.Value;

                rows.Add(r);
            }
            dataGrid.ItemsSource = rows;

            return dataGrid;
        }

        public XmlNode Save(UIElement control, XmlNode nd)
        {
            XmlDocument xd = nd.OwnerDocument;
            XmlElement el = xd.CreateElement(node);

            DataGrid dataGrid = (DataGrid)control;
            foreach (var item in dataGrid.Items)
            {
                if (item is Row2)
                {
                    Row2 r = (Row2)item;
                    
                    XmlElement child = xd.CreateElement("Command");
                    child.SetAttribute("Name", r.Name);
                    child.SetAttribute("Equipment", r.Equipment);
                    child.SetAttribute("Command", r.Command);

                    el.AppendChild(child);
                }
            }

            return el;
        }
    }
}
