using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gen.Source
{
    public class Compiler
    {
        public static void Start()
        {
            var options = new Dictionary<string, string> { { "CompilerVersion", "v3.5" } };
            var provider = new CSharpCodeProvider(options);

            //CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters parameters = new CompilerParameters();
            parameters.CompilerOptions = "/target:library /optimize /warn:0";
            parameters.GenerateInMemory = false;
            parameters.GenerateExecutable = false;
            parameters.IncludeDebugInformation = true;
            parameters.OutputAssembly = Options.FormatTempDLLPath;
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add("protobuf-net.dll");

            string codepath = Options.tempCodePath;

            string[] csharp_files = Utility.File.GetAllFileNamesByPath(codepath, new string[] { "cs" }).ToArray();
            CompilerResults results = provider.CompileAssemblyFromFile(parameters, csharp_files);

            if (results.NativeCompilerReturnValue != 0)
            {
                StringBuilder builder = new StringBuilder();

                builder.AppendLine(string.Format("return code: {0}", results.NativeCompilerReturnValue));

                foreach (CompilerError error in results.Errors)
                {
                    if (error.IsWarning)
                    {
                        builder.Append("<WARNING> ");
                    }
                    else
                    {
                        builder.Append("<ERROR> ");
                    }


                    builder.AppendLine(string.Format("{0}({1},{2}): error {3}: {4}", error.FileName
                                                                                    , error.Line
                                                                                    , error.Column
                                                                                    , error.ErrorNumber
                                                                                    , error.ErrorText));

                }

                throw new System.Exception(builder.ToString());
            }
        }


    }
}
