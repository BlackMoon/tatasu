using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ModelEditor.ModelViews
{
    public class ModelItem
    {
        public bool IsExpanded { get; set; }
        public string Name { get; set; }
        public List<ModelItem> Items { get; set; }

        public ModelItem()
        {
            Items = new List<ModelItem>();
        }
        public ModelItem(bool isExpanded)
            : this()
        {
            IsExpanded = isExpanded;
        }
    }

    public class ModelData
    {
        private CancellationTokenSource tokenSource = new CancellationTokenSource();
        public string FileName { get; set; }
        public string FileFullName { get; set; }

        public CancellationToken Token
        {
            get
            {
                return tokenSource.Token;
            }
            set { }
        }

        public CancellationTokenSource TokenSource
        {
            get
            {
                return tokenSource;
            }
            set { }
        }

        public List<ModelItem> Items { get; set; }

        public ModelData()
        {
            Items = new List<ModelItem>();
        }

        public ModelData(string name, string fullname)
            : this()
        {
            FileName = name;
            FileFullName = fullname;
        }
    }

}
