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

        public IPlugin Plugin {
            get
            {
                return plugin;
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
    }
}
