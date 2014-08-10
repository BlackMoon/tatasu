using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelEditor
{
    public class ReestrPrefernces
    {
        private bool saveChoice;
        private int selected;
        private string path;

        public bool SaveChoice
        {
            get
            {
                return saveChoice;
            }
            set
            {
                saveChoice = value;
            }
        }

        public int Selected
        {
            get
            {
                return selected;
            }
            set
            {
                selected = value;
            }
        }

        public string Path
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
            }
        }        
    }
}
