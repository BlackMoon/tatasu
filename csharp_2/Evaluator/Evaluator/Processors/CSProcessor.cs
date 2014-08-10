using Microsoft.CSharp;
using System;
using System.Linq;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Collections.Generic;

namespace Evaluator.Processors
{
    public class CSProcessor : Processor
    {
        public override double Process(string input)
        {
            string expr = null;
            if (!ValidExpression(input, out expr)) return 0;

            CompilerParameters parms = new CompilerParameters()
            {
                GenerateExecutable = false, 
                GenerateInMemory = true,
                IncludeDebugInformation = false
            };

            CodeDomProvider compiler = CSharpCodeProvider.CreateProvider("CSharp");
            CompilerResults res = compiler.CompileAssemblyFromSource(parms, @"public static class Func { public static double Process() { return " + expr + ";} }");

            if (res.Errors.HasErrors)
            {   
                throw new InvalidOperationException("Expression has a syntax error.");
            }  

            Assembly assembly = res.CompiledAssembly;
            MethodInfo mi = assembly.GetType("Func").GetMethod("Process");

            return (double)mi.Invoke(null, null);
        }
    }
}
