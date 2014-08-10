using ModelEditor.Plugins;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ModelEditor.ViewModels
{
    public class PluginViewModel: BaseViewModel
    {
        private ObservableCollection<PluginDescription> items;

        public ObservableCollection<PluginDescription> Items
        {
            get
            {
                if (items == null)
                {
                    items = new ObservableCollection<PluginDescription>();
                }
                return items;
            }          
        }
    }
}
