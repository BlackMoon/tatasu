using Microsoft.Win32;
using PluginInterface;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Xml;

namespace ModelEditorPlugins
{
    class Row3 : INotifyPropertyChanged
    {
        private string fileName;
        
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get; set; }
        public string FileName {
            get
            {
                return fileName;
            }
            set
            {
                fileName = value;
                OnPropertyChanged("FileName");
            }
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }   

    /// <summary>
    /// Плагин для обработки узло с тегом <Equipment>
    /// </summary>
    public class PluginModel3: IPlugin
    {
        private int version = 1;
        private string node = "Equipments";

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
                AutoGenerateColumns = false,
                CanUserReorderColumns = true,
                CanUserResizeColumns = true  
            };

            DataGridTextColumn col1 = new DataGridTextColumn()
            {
                Header = "Name",               
                Binding = new Binding("Name")                
            };            
            dataGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn()
            {
                Header = "FileName",
                Binding = new Binding("FileName"),
                MinWidth = 200
            };
            dataGrid.Columns.Add(col2);

            var buttonTemplate = new FrameworkElementFactory(typeof(Button));
            buttonTemplate.SetValue(Button.ContentProperty, "...");
            buttonTemplate.AddHandler(
                Button.ClickEvent,
                new RoutedEventHandler((o, e) => {

                    //Row3 r = ((FrameworkElement)o).DataContext as Row3;

                    DependencyObject dep = (DependencyObject)e.OriginalSource;

                    // iteratively traverse the visual tree --> search row
                    while ((dep != null) && !(dep is DataGridRow))
                    {
                        dep = VisualTreeHelper.GetParent(dep);
                    }

                    if (dep != null)
                    {
                        DataGridRow row = (DataGridRow)dep;
                        
                        Row3 r = null;
                        if (row.Item is Row3)
                            r = (Row3)row.Item;
                        else
                            r = new Row3();                        

                        OpenFileDialog dlg = new OpenFileDialog();
                        bool? result = dlg.ShowDialog();

                        if (result == true)
                        {
                            r.FileName = dlg.SafeFileName;
                            row.Item = r;
                        }
                    }
                })
            );          

            DataGridTemplateColumn col3 = new DataGridTemplateColumn()
            {   
                CellTemplate = new DataTemplate() { VisualTree = buttonTemplate },                
                Width = 16
            };  
            dataGrid.Columns.Add(col3);
                
            ObservableCollection<Row3> rows = new ObservableCollection<Row3>();
            foreach (XmlNode child in nd.ChildNodes)
            {
                Row3 r = new Row3();
                
                var attr = child.Attributes["Name"];
                if (attr != null)
                    r.Name = attr.Value;

                attr = child.Attributes["FileName"];
                if (attr != null)
                    r.FileName = attr.Value;

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
                if (item is Row3)
                {
                    Row3 r = (Row3)item;

                    XmlElement child = xd.CreateElement("Equipment");
                    child.SetAttribute("Name", r.Name);                    
                    child.SetAttribute("FileName", r.FileName);

                    el.AppendChild(child);
                }
            }

            return el;
        }
    }
}
