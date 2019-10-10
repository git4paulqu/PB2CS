namespace gen.Source
{
    public class Generator
    {
        public static void Generate()
        {
            Utility.Logger.Log("[clear file]");
            ClearTempFile();

            Utility.Logger.Log("\r\n");
            Utility.Logger.Log("[read file]");
            FileReader.Read();

            Utility.Logger.Log("\r\n");
            Utility.Logger.Log("[write file]");
            CodeWriter.Write();

            Utility.Logger.Log("\r\n");
            Utility.Logger.Log("[compiler code]");
            Compiler.Start();

            Utility.Logger.Log("\r\n");
            Utility.Logger.Log("[copy dll]");
            Utility.File.CopyFile(Options.FormatTempDLLPath, Options.FormatOutputDLLPath);

            Utility.Logger.Log("\r\n");
        }

        private static void ClearTempFile()
        {
            Utility.File.TryClearPath(Options.tempRootPath);
            Utility.File.CreatFolder(Options.tempCodePath);
            Utility.File.CreatFolder(Options.tempDLLPath);
        }
    }
}
