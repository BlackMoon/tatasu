using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Evaluator.Processors;

namespace EvaluatorTest
{
    [TestClass]
    public class DataTableProcessorTest
    {
        [TestMethod]
        public void TestProcess()
        {
            string expr = ".1 + 28 / 7 * (45 + (3 - .8))";
            Processor p = new DataTableProcessor();

            double expected = 188.9;
            Assert.AreEqual(expected, p.Process(expr));
        }
    }
}
