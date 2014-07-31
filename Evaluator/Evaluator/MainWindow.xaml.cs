using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Evaluator.Processors;

namespace Evaluator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModel vm = null;

        public string Result { get; set; }
        public string Expression { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            txtFormula.DataContext = this;
            txtResult.DataContext = this;

            var prefs = new UserPreferences();
            this.Height = prefs.WindowHeight;
            this.Width = prefs.WindowWidth;
            this.Top = prefs.WindowTop;
            this.Left = prefs.WindowLeft;
            this.WindowState = prefs.WindowState;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            Factory f = new Factory();
            Processor p = f.CreateProcessor(eProcType.ProcCS);
            Expression = "1.0f";// p.Process(Expression);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var prefs = new UserPreferences
            {
                WindowLeft = this.Left,
                WindowTop = this.Top,
                WindowWidth = this.Width,
                WindowHeight = this.Height,
                WindowState = this.WindowState
            };
            prefs.Save();
        }
    }

    public class ViewModel {
        public eProcType ProcType { get; set; }
    }

    public class RadioButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Equals(true) ? parameter : Binding.DoNothing;
        }
    }
}
