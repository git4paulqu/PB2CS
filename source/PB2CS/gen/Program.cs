using gen.Source;
using System;
using System.Diagnostics;
using System.IO;

namespace gen
{
    class Program
    {
        static void Main(string[] args)
        {
            bool success = false;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                if (!Options.Initialize(args))
                {
                    return;
                }

                Generator.Generate();
                success = true;
            }
            catch (Exception e)
            {
                success = false;
                Utility.Logger.Log(e.ToString());
            }
            finally
            {
                string result = success ? "成功" : "失败";
                Utility.Logger.Log("{0}, cost time:{1}ms, press any key exit.", result, sw.ElapsedMilliseconds.ToString());
                Console.ReadLine();

                System.Environment.Exit(0);
            }
        }
    }
}
