
using ModelEditor.Plugins;
using PluginInterface;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;

namespace ModelEditor.ViewModels
{
    public class TreeViewModel: BaseViewModel
    {
        private bool isExpanded;
        private IPlugin plugin;
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
                if (Plugin != null)
                    return Plugin.GetNodeName(Node);
                else
                    return Node.Name;                
            }
        }

        public XmlNode Node { get; set; }

        public IPlugin Plugin {
            get
            {
                return plugin;
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

        public void AttachPlugin()
        {
            if (Node != null)
            {
                Dictionary<string, PluginDescription> plugins = PluginManager.Instance.Plugins;
                if (plugins.ContainsKey(Node.Name))
                {
                    PluginDescription pd = plugins[Node.Name];
                    this.plugin = pd.Plugin;
                }
            }
        }
    }
}
