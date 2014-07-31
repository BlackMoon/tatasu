using System;
using System.Data;

namespace Evaluator.Processors
{
    class DataTableProcessor : Processor
    {
        public override double Process(string expr)
        {
            DataTable tab = new DataTable();
            DataColumn col = new DataColumn("Process", typeof(double), expr);
            tab.Columns.Add(col);
            tab.Rows.Add(0);

            return (double)(tab.Rows[0]["Process"]);
        }
    }
}
