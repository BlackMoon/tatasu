using ModelEditor.Models;
using System.Collections.ObjectModel;

namespace ModelEditor.ViewModels
{
    public class ListViewModel: BaseViewModel
    {
        private bool loaded;
        private FileData selectedItem;
        private ObservableCollection<FileData> items;

        public bool Loaded { 
            get
            {
                return loaded;
            }
            set
            {
                loaded = value;
                OnPropertyChanged("Loaded");
            }
        }

        public ObservableCollection<FileData> Items
        {
            get
            {
                if (items == null)
                {                    
                    items = new ObservableCollection<FileData>();
                }
                return items;
            }
        }

        public FileData SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        public ListViewModel()
        {
            Loaded = true;
        }
    }
}
