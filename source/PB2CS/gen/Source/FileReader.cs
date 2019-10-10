using gen.Source.Structure;
using System.Collections.Generic;

namespace gen.Source
{
    public class FileReader
    {
        public static void Read()
        {
            objects = new Dictionary<string, GObject>();
            Utility.File.RecursionFileExecute(Options.inputPath, FILE_SUFFIX, HandleFile);
        }

        private static void HandleFile(string file)
        {
            Utility.Logger.Log("read file : {0}..", file);
            string content = Utility.File.ReadStringByFile(file);
            if (string.IsNullOrEmpty(content))
            {
                return;
            }

            content = Matcher.ReplaceBlank(content);
            try
            {
                AdpaterGObject(file, content);
            }
            catch (System.Exception e)
            {
                Utility.Logger.Log("parse file:{0} error.", file);
                throw e;
            }
        }

        private static void AdpaterGObject(string file, string content)
        {
            List<string> classs = Matcher.MatchClass(content);
            foreach (string item in classs)
            {
                ClassObject gobject = new ClassObject(file, item);
                TryAddGObject(gobject);
            }

            List<string> enums = Matcher.MatchEnum(content);
            foreach (string item in enums)
            {
                EnumObject gobject = new EnumObject(file, item);
                TryAddGObject(gobject);
            }
        }

        private static void TryAddGObject(GObject gobject)
        {
            if (gobject.Parse())
            {
                GObject exist = null;
                if (objects.TryGetValue(gobject.name, out exist))
                {
                    throw new System.Exception(string.Format("has repeat data : {0}, file1 : {1}, file2 : {2}.", gobject.name, exist.file, gobject.name));
                }
                objects.Add(gobject.name, gobject);
                Utility.Logger.Log("parse data : {0}", gobject.name);
            }
        }

        private static bool Contains(string name)
        {
            return objects.ContainsKey(name);
        }

        public static Dictionary<string, GObject> objects { get; private set; }

        private static string FILE_SUFFIX = "proto";
    }
}
