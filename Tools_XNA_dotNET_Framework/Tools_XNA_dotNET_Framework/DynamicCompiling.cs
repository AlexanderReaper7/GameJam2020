using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;
// TODO Comment DynamicCompiling class
namespace Tools_XNA_dotNET_Framework
{
    public static class DynamicCompiling
    {
        public static void CompileCode(string code, string name, string directory)
        {
            CSharpCodeProvider csc = new CSharpCodeProvider(new Dictionary<string, string>() {{"CompilerVersion", "v3.5"}});

            CompilerParameters parameters =
                new CompilerParameters(new[] {"mscorlib.dll", "System.Core.dll"}, name+".exe", true);

            parameters.GenerateExecutable = true;

            CompilerResults results = csc.CompileAssemblyFromSource(parameters, code);

            results.Errors.Cast<CompilerError>().ToList().ForEach(error => Console.WriteLine(error.ErrorText));
        }
        
        public static void CompileAndRun(params string[] code)
        {
            CompilerParameters compilerParams = new CompilerParameters();
            //string outputDirectory = Directory.GetCurrentDirectory();

            compilerParams.GenerateInMemory = true;
            compilerParams.TreatWarningsAsErrors = false;
            compilerParams.GenerateExecutable = false;
            compilerParams.CompilerOptions = "/optimize";

            string[] references = { "mscorlib.dll", "System.dll" };
            compilerParams.ReferencedAssemblies.AddRange(references);

            CSharpCodeProvider csc = new CSharpCodeProvider();
            CompilerResults results = csc.CompileAssemblyFromSource(compilerParams, code);

            results.Errors.Cast<CompilerError>().ToList().ForEach(error => throw new Exception(error.ErrorText));

            ExploreAssembly(results.CompiledAssembly); 
            // TODO
            //Module module = results.CompiledAssembly.GetModules()[0];
            //Type mt = null;
            //MethodInfo methodInfo = null;

            //if (module != null)
            //{
            //    mt = module.GetType("DynaCore.DynaCore");
            //}

            //if (mt != null)
            //{
            //    methodInfo = mt.GetMethod("Main");
            //}

            //if (methodInfo != null)
            //{
            //    Console.WriteLine(methodInfo.Invoke(null, new object[] { "here in dyna code" }));
            //}
        }

        static void ExploreAssembly(Assembly assembly)
        {
            Console.WriteLine("Modules in the assembly:");
            foreach (Module m in assembly.GetModules())
            {
                Console.WriteLine("{0}", m);

                foreach (Type t in m.GetTypes())
                {
                    Console.WriteLine("   {0}", t.Name);

                    foreach (MethodInfo mi in t.GetMethods())
                    {
                        Console.WriteLine("      {0}", mi.Name);
                    }
                }
            }
        }
    }
}

