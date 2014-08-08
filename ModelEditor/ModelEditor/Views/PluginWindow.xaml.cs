using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ModelEditor.Plugins;

namespace ModelEditor
{
    /// <summary>
    /// Interaction logic for PluginWindow.xaml
    /// </summary>
    public partial class PluginWindow : Window
    {
        private PluginManager pluginManager = PluginManager.Instance;

        public PluginWindow()
        {
            InitializeComponent();
        }  

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            pluginManager.Task.Wait();
        }
    }
}
