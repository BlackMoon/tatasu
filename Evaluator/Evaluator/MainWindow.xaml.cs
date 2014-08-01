using System;
using System.Windows;
using Evaluator.Processors;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Evaluator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<char> signes = new List<char>(new char[] { '+', '-', '.' });

        private string expression;
        public double Result { get; set; }

        public string Expression { 
            get
            {
                return expression;
            }
            set
            {
                try
                {
                    /*Regex rx = new Regex(@"\*");
                    if (rx.IsMatch(value))
                    {                        
                        throw new Exception("Expression can not be empty.");
                    }*/

                    if (expression != value)
                    {
                        expression = value;
                        OnPropertyChanged("Expression");
                    }

                    expression = value;
                    btnStart.IsEnabled = true;
                }
                catch(Exception ex)
                {
                    btnStart.IsEnabled = false;
                    throw ex;
                }
            }
        }

        public eProcType ProcType { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();

            ProcType = eProcType.ProcString;

            txtFormula.DataContext = this;
            txtResult.DataContext = this;
            DataContext = this;

            var prefs = new UserPreferences();
            this.Height = prefs.WindowHeight;
            this.Width = prefs.WindowWidth;
            this.Top = prefs.WindowTop;
            this.Left = prefs.WindowLeft;
            this.WindowState = prefs.WindowState;
        }
        
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            Factory f = new Factory();
            Processor p = f.CreateProcessor(ProcType);
            try
            {
                string expr = Expression.Trim();
                if (!string.IsNullOrEmpty(expr))
                {                     
                    // начинается с символа
                    if (signes.Contains(expr[0]))
                        expr = "0" + expr;

                    // заканчивается на символ
                    int last = expr.Length - 1;
                    if (last != 0 && signes.Contains(expr[last]))
                        expr += '0';                   

                    //expr = Regex.Replace(expr, @"\((\+|\-|\.)", m => m.Value[0] + "0" + m.Value[1]);

                    Result = p.Process(expr);
                    txtResult.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
                }
                
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

        private void txtFormula_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Add:
                case Key.Subtract:
                case Key.Multiply:
                case Key.Divide:
                case Key.D0:
                case Key.D1:
                case Key.D2:
                case Key.D3:
                case Key.D4:
                case Key.D5:
                case Key.D6:
                case Key.D7:
                case Key.D8:
                case Key.D9:
                case Key.NumLock:
                case Key.NumPad0:
                case Key.NumPad1:
                case Key.NumPad2:
                case Key.NumPad3:
                case Key.NumPad4:
                case Key.NumPad5:
                case Key.NumPad6:
                case Key.NumPad7:
                case Key.NumPad8:
                case Key.NumPad9:
                case Key.Back:
                case Key.Oem2:
                case Key.OemComma:
                case Key.OemMinus:
                case Key.OemPeriod:                    
                case Key.OemPlus:
                    break;               
                default:
                    e.Handled = true;
                    break;
            }
        }       
    }   
}
