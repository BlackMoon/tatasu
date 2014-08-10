using System;
using System.Data;

namespace Evaluator.Processors
{
    public class DataTableProcessor : Processor
    {
        public override double Process(string input)
        {
            string expr = null;
            if (!ValidExpression(input, out expr)) return 0;

            DataTable tab = new DataTable();
            DataColumn col = new DataColumn("Process", typeof(double), expr);
            tab.Columns.Add(col);
            tab.Rows.Add(0);

            return (double)(tab.Rows[0]["Process"]);
        }
    }
}
