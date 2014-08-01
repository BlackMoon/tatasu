using System;
using System.Data;

namespace Evaluator.Processors
{
    class DataTableProcessor : Processor
    {
        public override double Process(string expr)
        {
            DataTable tab = new DataTable();
            try
            {
                DataColumn col = new DataColumn("Process", typeof(double), expr);
                tab.Columns.Add(col);
                tab.Rows.Add(0);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return (double)(tab.Rows[0]["Process"]);
        }
    }
}
