
using System;
namespace Evaluator.Processors
{    
    public enum eProcType
    {
        ProcCS,
        ProcDataTable,
        ProcString
    }

    public abstract class AbstractFactory
    {
        public abstract Processor CreateProcessor(eProcType tp);
    }

    public class Factory: AbstractFactory
    {
        public override Processor CreateProcessor(eProcType tp)
        {           
            Processor p = null;
            switch(tp)
            {
                case eProcType.ProcCS:
                    p = new CSProcessor();
                    break;
                case eProcType.ProcDataTable:
                    p = new DataTableProcessor();
                    break;
                default:
                    p = new StringProcessor();
                    break;
            }
            return p;
        }
    }
}
