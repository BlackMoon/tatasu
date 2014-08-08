using ModelEditor.ViewModels;
using System.Collections.Generic;
using System.Threading;

namespace ModelEditor.Models
{
    public class FileData
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
        }

        public CancellationTokenSource TokenSource
        {
            get
            {
                return tokenSource;
            }
        }

        public List<TreeViewModel> Items { get; set; }

        public FileData()
        {
            Items = new List<TreeViewModel>();
        }

        public FileData(string name, string fullname)
            : this()
        {
            FileName = name;
            FileFullName = fullname;
        }
    }
}
