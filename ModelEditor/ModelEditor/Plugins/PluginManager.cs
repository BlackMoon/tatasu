using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace ModelEditor.Plugins
{
    public sealed class PluginManager
    {
        private const string XMLDESCR = "plugins.xml";
        private static readonly PluginManager instance = new PluginManager();

        public Task Task { get; set; }
        
        private PluginManager() { }

        public static PluginManager Instance
        {
            get 
            {
                return instance; 
            }
        }

        public Dictionary<string, IPlugin> Plugins { get; set; }

        public void Load()
        {
            Load(System.AppDomain.CurrentDomain.BaseDirectory);
        }

        public void Load(string path)
        {
            Task = Task.Factory.StartNew(() =>
            {
                foreach (string f in Directory.GetFiles(path, "*.dll"))
                {
                    Assembly a = Assembly.LoadFrom(f);

                    try
                    {
                        foreach (Type t in a.GetTypes())
                        {
                            foreach (Type i in t.GetInterfaces())
                            {
                                //И если библиотека наследована от IPlugin
                                if (i.Equals(Type.GetType("HotKeyHelper.IPlugin")))
                                {
                                    //Подписываемся на события плагина и добавляем его в список плагинов
                                    IPlugin p = (IPlugin)Activator.CreateInstance(t);
                                    
                                    break;
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            });
        }
    }
}
