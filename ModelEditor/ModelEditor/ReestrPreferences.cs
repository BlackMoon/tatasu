using Microsoft.Win32;

namespace ModelEditor
{
    public class ReestrPreferences
    {
        private RegistryKey key = Registry.CurrentUser.CreateSubKey("ModelEditor");

        private bool saveChoice = false;
        private int selected = -1;
        private string path = "";

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

        public ReestrPreferences()
        {   
            Load();
        }

        private void Load()
        {            
            path = (string)key.GetValue("Path");
            
            var val = key.GetValue("Selected");
            if (val != null)
                selected = (int)val;
                        
            val = key.GetValue("SaveChoice");
            if (val != null)
                saveChoice = bool.Parse((string)val);
        }

        public void Save()
        {            
            key.SetValue("Path", path);
            key.SetValue("Selected", selected);
            key.SetValue("SaveChoice", saveChoice);
            key.Close();
        }
    }
}
