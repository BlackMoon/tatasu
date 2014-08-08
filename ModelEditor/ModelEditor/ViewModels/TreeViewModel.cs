
using System.Collections.ObjectModel;

namespace ModelEditor.ViewModels
{
    public class TreeViewModel: BaseViewModel
    {
        public bool IsExpanded { get; set; }
        public string Name { get; set; }
        public ObservableCollection<ModelItem> Items { get; set; }
    }
}
