using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Evaluator.Processors
{
    public class StringProcessor : Processor
    {
        private List<string> operators;
        private List<string> standart_operators = new List<string>(new string[] { "(", ")", "+", "-", "*", "/" });

        public StringProcessor()
        {
            operators = new List<string>(standart_operators);
        }

        private string[] ConvertToPostfixNotation(string input)
        {
            List<string> outputSeparated = new List<string>();
            Stack<string> stack = new Stack<string>();
            foreach (string c in Separate(input))
            {
                if (operators.Contains(c))
                {
                    if (stack.Count > 0 && !c.Equals("("))
                    {
                        if (c.Equals(")"))
                        {
                            string s = stack.Pop();
                            while (s != "(")
                            {
                                outputSeparated.Add(s);
                                s = stack.Pop();
                            }
                        }
                        else if (GetPriority(c) > GetPriority(stack.Peek()))
                            stack.Push(c);
                        else
                        {
                            while (stack.Count > 0 && GetPriority(c) <= GetPriority(stack.Peek()))
                                outputSeparated.Add(stack.Pop());
                            stack.Push(c);
                        }
                    }
                    else
                        stack.Push(c);
                }
                else
                    outputSeparated.Add(c);
            }
            if (stack.Count > 0)
                foreach (string c in stack)
                    outputSeparated.Add(c);

            return outputSeparated.ToArray();
        }

        private byte GetPriority(string s)
        {
            switch (s)
            {
                case "(":
                    return 0;
                case ")":
                    return 1;
                case "+":
                    return 2;
                case "-":
                    return 3;
                case "*":
                case "/":
                    return 4;                
                default:
                    return 6;
            }
        }

        private string PrepareExpression(string input)
        {
            string res = input.Replace('.', ',');               // --> замена . на , (для конвертации в число)

            res = Regex.Replace(res, @"(\*|\/|\+|\-|\(),", m => m.Value[0] + "0" + m.Value[1]);            
            res = Regex.Replace(res, @"^(\-|\+|\,)\d+", m => "0" + m.Value);
            res = Regex.Replace(res, @"(\*|\/|\+|\-)\+\d+", m => m.Value[0] + "(0" + m.Value.Substring(1) + ")");
            res = Regex.Replace(res, @"(\*|\/|\+|\-)\-\d+", m => m.Value[0] + "(0" + m.Value.Substring(1) + ")");
            res = Regex.Replace(res, @"\((\+|\-)", m => m.Value[0] + "0" + m.Value.Substring(1));

            return res;
        }

        private IEnumerable<string> Separate(string input)
        {
            int pos = 0;            
            
            while (pos < input.Length)
            {
                string s = string.Empty + input[pos];
                if (!standart_operators.Contains(input[pos].ToString()))
                {
                    if (Char.IsDigit(input[pos]))
                    {
                        for (int i = pos + 1; i < input.Length && (Char.IsDigit(input[i]) || input[i] == ',' || input[i] == '.'); i++)
                        {
                            s += input[i];
                        }
                    }
                    else if (Char.IsLetter(input[pos]))
                    {
                        for (int i = pos + 1; i < input.Length && (Char.IsLetter(input[i]) || Char.IsDigit(input[i])); i++)
                        {
                            s += input[i];
                        }
                    }
                }
                yield return s;
                pos += s.Length;
            }
        }
        
        public override double Process(string input)
        {
            string expr = null;
            if (!ProperExpression(input, out expr)) return 0;

            expr = PrepareExpression(expr);
            
            Stack<string> stack = new Stack<string>();
            Queue<string> queue = new Queue<string>(ConvertToPostfixNotation(expr));            

            string str = queue.Dequeue();
            while (queue.Count >= 0)
            {
                if (!operators.Contains(str))
                {
                    stack.Push(str);
                    // для выражения только с 1 операндом
                    if (queue.Count == 0)
                        break;

                    str = queue.Dequeue();
                }
                else
                {   
                    double a = Convert.ToDouble(stack.Pop());
                    double b = Convert.ToDouble(stack.Pop());
                    double c = 0;
                        
                    switch (str)
                    {
                        case "+":
                        {                                    
                            c = a + b;
                            break;
                        }
                        case "-":
                        {   
                            c = b - a;
                            break;
                        }
                        case "*":
                        {
                            c = b * a;
                            break;
                        }
                        case "/":
                        {                                    
                            c = b / a;
                            break;
                        }
                    }

                    stack.Push(c.ToString());
                    if (queue.Count > 0)
                        str = queue.Dequeue();
                    else
                        break;
                }
            }            

            return Convert.ToDouble(stack.Pop()); 
        }
    }
}
