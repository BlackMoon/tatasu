using PluginInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ModelEditor.Plugins
{
    public sealed class PluginManager: IDisposable 
    {
        private const string BASE_ASSEMBLY = "PluginInterface.dll";
        private const string PLUGINS_DESCRIPTION = "plugins.xml";        

        private static readonly PluginManager instance = new PluginManager();

        private Task task;
        private CancellationTokenSource tokenSource = new CancellationTokenSource();
        private Dictionary<string, PluginDescription> plugins = new Dictionary<string, PluginDescription>();        
        
        private PluginManager() { }

        public static PluginManager Instance
        {
            get 
            {
                return instance; 
            }
        }

        public Task Task
        {
            get
            {
                return task;
            }
        }

        public Dictionary<string, PluginDescription> Plugins
        {
            get
            {                
                return plugins;
            }
        }

        private void LoadFromFolder(string path)
        {
            if ((task != null) && (task.IsCompleted == false ||
                           task.Status == TaskStatus.Running ||
                           task.Status == TaskStatus.WaitingToRun ||
                           task.Status == TaskStatus.WaitingForActivation))
            {
                tokenSource.Cancel();
            }


            task = Task.Factory.StartNew((ct) =>
            {
                CancellationToken token = (CancellationToken)ct;

                if (token.IsCancellationRequested)
                    token.ThrowIfCancellationRequested();

                plugins.Clear();

                DirectoryInfo di = new DirectoryInfo(path);
                foreach (FileInfo fi in di.GetFiles("*.dll").Where(_fi => _fi.Name != BASE_ASSEMBLY))
                {
                    Assembly a = Assembly.LoadFrom(fi.Name);

                    try
                    {
                        var types = from type in a.GetTypes()
                                    where typeof(IPlugin).IsAssignableFrom(type)
                                    select type;

                        foreach (Type t in types)
                        {
                            IPlugin p = (IPlugin)Activator.CreateInstance(t);

                            PluginDescription pd = new PluginDescription()
                            {
                                AssemblyName = fi.Name,
                                TypeName = t.Name,
                                TypeFullName = t.FullName,
                                ModelName = p.Node,
                                Version = p.Version,
                                Plugin = p
                            };

                            if (!string.IsNullOrEmpty(pd.ModelName))
                            {
                                if (!plugins.ContainsKey(pd.ModelName))
                                    plugins.Add(pd.ModelName, pd);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                        continue;
                    }
                }

                using (TextWriter tw = new StreamWriter(PLUGINS_DESCRIPTION))
                {
                    List<PluginDescription> pds = new List<PluginDescription>(plugins.Values);
                    XmlHelper.Serialize<List<PluginDescription>>(pds, tw, "plugins");
                    tw.Flush();
                    tw.Close();
                }

            }, tokenSource.Token);
        }

        private void LoadFromXml()
        {
            if ((task != null) && (task.IsCompleted == false ||
                           task.Status == TaskStatus.Running ||
                           task.Status == TaskStatus.WaitingToRun ||
                           task.Status == TaskStatus.WaitingForActivation))
            {
                tokenSource.Cancel();
            }            

            task = Task.Factory.StartNew((ct) =>
            {                
                CancellationToken token = (CancellationToken)ct;

                if (token.IsCancellationRequested)
                    token.ThrowIfCancellationRequested();

                plugins.Clear();               

                using (TextReader tr = new StreamReader(PLUGINS_DESCRIPTION))
                {
                    List<PluginDescription> pds = XmlHelper.Deserialize<List<PluginDescription>>(tr, "plugins");
                    tr.Close();

                    foreach (PluginDescription pd in pds)
                    {
                        try
                        {
                            Assembly a = Assembly.LoadFrom(pd.AssemblyName);
                            if (a != null)
                            {
                                IPlugin p = (IPlugin)a.CreateInstance(pd.TypeFullName);

                                if (p != null)
                                {
                                    pd.Plugin = p;

                                    if (!plugins.ContainsKey(pd.ModelName))
                                        plugins.Add(pd.ModelName, pd);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.Message);
                            continue;
                        }
                    }
                }               
            }, tokenSource.Token);
        }

        public void Load()
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    LoadFromXml();
                    task.Wait();
                }
                catch (AggregateException ex)
                {
                    LoadFromFolder();
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            });
        }

        public void LoadFromFolder()
        {
            LoadFromFolder(System.AppDomain.CurrentDomain.BaseDirectory);
        }

        public void Dispose()
        {
            tokenSource.Cancel();
        }
    }
}
