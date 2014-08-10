using PluginInterface;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace ModelEditorPlugins
{
    class Row1
    {
        public string Name { get; set; }
        public string Parameter { get; set; }        
    }

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

            ObservableCollection<Row1> rows = new ObservableCollection<Row1>();
            foreach (XmlAttribute attr in nd.Attributes)
            {
                Row1 r = new Row1()
                {
                    Name = attr.Name,
                    Parameter = attr.Value,
                };
                
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
                if (item is Row1)
                {
                    Row1 r = (Row1)item;
                    el.SetAttribute(r.Name, r.Parameter);
                }
            }

            return el;
        }
    }
}
