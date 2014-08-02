
namespace Evaluator.Processors
{
    public abstract class Processor
    {
        protected bool ProperExpression(string expr, out string result)
        {
            result = expr.Replace(" ", string.Empty);
            return !string.IsNullOrEmpty(expr);
        }
        public abstract double Process(string input);
    }
}
