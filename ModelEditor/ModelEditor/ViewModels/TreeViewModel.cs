using ModelEditor.Plugins;
using PluginInterface;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Linq;

namespace ModelEditor.ViewModels
{
    public class TreeViewModel: BaseViewModel
    {
        private bool isExpanded;
        private string name;
        private IPlugin plugin;
       
        private Button applyBtn;
        private UIElement editor;

        private ObservableCollection<TreeViewModel> items;

        public bool IsExpanded {
            get
            {
                return isExpanded;
            }
            set
            {
                isExpanded = value;
                OnPropertyChanged("IsExpanded");
            }
        }

        public string Name
        {
            get
            {
                return name;                
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public XmlNode Node { get; set; }

        public MainWindow Owner { get; set; }

        public TreeViewModel Parent { get; set; }

        public IPlugin Plugin {
            get
            {
                return plugin;
            }
        }

        public Button ApplyBtn
        {
            get
            {
                if (applyBtn == null)
                {
                    applyBtn = new Button() 
                    { 
                        Content = "Применить", 
                        HorizontalAlignment = HorizontalAlignment.Left, 
                        Margin = new Thickness(4, 4, 0, 0),  
                        Width = 140                        
                    };
                    applyBtn.Click += new RoutedEventHandler(applyBtn_Click);
                }
                return applyBtn;
            }
        }

        public UIElement Editor
        {
            get
            {
                if (editor == null)
                {
                    if (Node != null)
                    {
                        if (plugin != null)
                            editor = plugin.GetEditor(Node);
                        else
                        {
                            editor = new TextBlock() { Text = FormatXml(Node.OuterXml), TextWrapping = TextWrapping.Wrap };
                        }
                    }
                }

                return editor;
            }
        }     

        public ObservableCollection<TreeViewModel> Items { 
            get
            {
                if (items == null)
                {                    
                    items = new ObservableCollection<TreeViewModel>();
                }
                return items;
            }             
        }

        public bool AttachPlugin()
        {
            bool ret = false;
            if (Node != null)
            {
                Dictionary<string, PluginDescription> plugins = PluginManager.Instance.Plugins;
                if (plugins.ContainsKey(Node.Name))
                {
                    PluginDescription pd = plugins[Node.Name];
                    plugin = pd.Plugin;
                    name = plugin.GetNodeName(Node);                    
                    ret = true;
                }
                else               
                    name = Node.Name;  
            }
            return ret;
        }

        private string FormatXml(string Xml)
        {
            try
            {
                XDocument doc = XDocument.Parse(Xml);
                return doc.ToString();
            }
            catch 
            {
                return Xml;
            }
        }

        private void applyBtn_Click(object sender, RoutedEventArgs e)
        {
            string msg = "OK";
            MessageBoxImage icon = MessageBoxImage.Information;

            try
            {
                if (plugin != null)
                {
                    XmlNode old = Node.Clone();

                    Node = plugin.Save(editor, Node);
                    Name = plugin.GetNodeName(Node);            // fire OnPropertyChanged event

                    // replace only parent nodes content                        
                    string xpath = ".//" + old.Name;
                    foreach (XmlAttribute attr in old.Attributes)
                    {
                        xpath += "[@" + attr.Name + "= '" + attr.Value + "'] ";
                    }       

                    TreeViewModel tvm = Parent;
                    while (tvm != null)
                    {       
                        XmlNode nd = tvm.Node.SelectSingleNode(xpath);
                        if (nd != null)
                        {
                            XmlElement el = (XmlElement)nd;
                            if (old.HasChildNodes)
                            {
                                el.RemoveAll();
                                foreach (XmlNode child in Node.ChildNodes)
                                {
                                    el.AppendChild(child.Clone());
                                }
                            }
                            else
                            {
                                el.RemoveAllAttributes();

                                foreach (XmlAttribute attr in Node.Attributes)
                                {
                                    el.SetAttribute(attr.Name, attr.Value);
                                }
                            }
                        }
                        tvm.editor = null;
                        tvm = tvm.Parent;
                    }
                }
            }
            catch (System.Exception ex)
            {
                msg = ex.Message;
                icon = MessageBoxImage.Warning;
            }

            MessageBox.Show(msg, "Внимание", MessageBoxButton.OK, icon);
            Owner.State = "Изменен";
        }
    }
}
