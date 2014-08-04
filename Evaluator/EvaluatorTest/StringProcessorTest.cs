﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Evaluator.Processors;

namespace EvaluatorTest
{
    [TestClass]
    public class StringProcessorTest
    {
        [TestMethod]
        public void TestConvertToPostfixNotation()
        {
            string expr = "(5 - 6) * 7".Replace(" ", string.Empty);
            PrivateObject po = new PrivateObject(new StringProcessor());

            string[] expected = new string[] { "5", "6", "-", "7", "*" };            
            CollectionAssert.AreEqual(expected, (string[])po.Invoke("ConvertToPostfixNotation", expr));
        }

        [TestMethod]
        public void TestProcess()
        {
            string expr = ".1 + 28 / 7 * (45 + (3 - .8))";
            Processor p = new StringProcessor();

            double expected = 188.9;            
            Assert.AreEqual(expected, p.Process(expr));
        }       
    }
}
