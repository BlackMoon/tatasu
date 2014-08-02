using System;
using Evaluator.Processors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EvaluatorTest
{
    [TestClass]
    public class CSProcessorTest
    {
        [TestMethod]
        public void TestProcess()
        {
            string expr = ".1 + 28 / 7 * (45 + (3 - .8))";
            Processor p = new CSProcessor();
            
            double expected = 188.9;
            Assert.AreEqual(expected, p.Process(expr));
        }
    }
}
