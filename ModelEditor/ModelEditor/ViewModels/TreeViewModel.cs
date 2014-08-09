
using System.Collections.ObjectModel;

namespace ModelEditor.ViewModels
{
    public class TreeViewModel: BaseViewModel
    {
        private bool isExpanded;
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
        public string Name { get; set; }       

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
    }
}
