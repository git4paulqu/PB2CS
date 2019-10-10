using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gen.Source
{
    public partial class Utility
    {
        public class Logger
        {
            public static void Log(string message)
            {
                Console.WriteLine(message);
            }

            public static void Log(string message, params object[] args)
            {
                Console.WriteLine(string.Format(message, args));
            }
        }
    }
}
