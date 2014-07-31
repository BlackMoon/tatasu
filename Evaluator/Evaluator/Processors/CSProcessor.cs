using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Reflection;

namespace Evaluator.Processors
{
    public class CSProcessor : Processor
    {
        public override double Process(string expr)
        {
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
