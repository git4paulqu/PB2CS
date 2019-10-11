using System;

namespace gen.Source
{
    public class Options
    {
        public static bool Initialize(string[] args)
        {
            foreach (string item in args)
            {
                if (item.StartsWith("-i:"))
                {
                    inputPath = item.Substring(3);
                }

                if (item.StartsWith("-o:"))
                {
                    outputPath = item.Substring(3);
                }

                if (item.StartsWith("-n:"))
                {
                    namespaceName = item.Substring(3);
                }
            }

            if (string.IsNullOrEmpty(inputPath) || string.IsNullOrEmpty(outputPath))
            {
                throw new Exception("input and output can not be null. please use -i:sourcepath, -o:outputpath..");
            }

            workPath = System.AppDomain.CurrentDomain.BaseDirectory;
            string codetemplete = Utility.File.ReadStringByFile(workPath + "templete/code.templete");
            if (string.IsNullOrEmpty(codetemplete))
            {
                throw new Exception("code templete is null, check the [templete/code.templete] file .");
            }
            codeTemplete = codetemplete;

            tempRootPath = workPath + "temp";
            tempCodePath = tempRootPath + "/code";
            tempDLLPath = tempRootPath + "/dll";

            return true;
        }

        public static string FormatOutputDLLPath {
            get {
                return outputPath + "/" + dllName;
            }
        }

        public static string FormatTempDLLPath
        {
            get
            {
                return tempDLLPath + "/" + dllName;
            }
        }

        public static string namespaceName { get; private set; }
        public static string inputPath { get; private set; }
        public static string outputPath { get; private set; }
        public static string codeTemplete { get; private set; }

        public static string tempRootPath { get; private set; }
        public static string tempCodePath { get; private set; }
        public static string tempDLLPath { get; private set; }
        public static string workPath { get; private set; }

        private static string dllName = "protocol.dll";
    }
}
