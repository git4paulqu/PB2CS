using gen.Source.Structure;
using System.Text;

namespace gen.Source
{
    public class CodeWriter
    {

        public static void Write()
        {
            foreach (var item in FileReader.objects)
            {
                WriteCS(item.Value);
            }
        }

        private static void WriteCS(GObject gobject)
        {
            bool hadNSName = !string.IsNullOrEmpty(Options.namespaceName);
            string tab = hadNSName ? "\t" : string.Empty;

            string ns1 = gobject.repeated ? "using System.Collections.Generic;\r\n\r\n" : string.Empty;
            string ns2 = string.Empty;
            if (hadNSName)
            {
                ns1 += string.Format("namespace {0}\r\n", Options.namespaceName);
                ns1 += "{";
                ns2 = "}";
            }

            string code = Options.codeTemplete.Replace("#namesapce1#", ns1);
            code = code.Replace("#namesapce2#", ns2);
            code = code.Replace("#tab#", tab);
            code = code.Replace("#type#", ObjectType2String(gobject));
            code = code.Replace("#classname#", gobject.name);

            StringBuilder sb = new StringBuilder();
            StringBuilder constructure = new StringBuilder();

            if (gobject.IsClass)
            {
                constructure.Append(tab);
                constructure.Append("\t");
                constructure.Append(string.Format("public {0}()\r\n{1}\t", gobject.name, tab));
                constructure.Append("{");
            }

            foreach (Tuple item in gobject.tuples)
            {
                sb.Append("\r\n");

                if (gobject.IsClass)
                {
                    sb.Append(tab);
                    sb.Append("\t");
                    sb.Append(string.Format("[ProtoMember({0})]\r\n", item.index));
                }

                sb.Append(tab);
                sb.Append("\t");
                sb.Append(Tuple2Code.Parse(item, gobject.objectType));

                if (gobject.IsClass && item.repeated)
                {
                    constructure.Append("\r\n");
                    constructure.Append(tab);
                    constructure.Append("\t\t");
                    constructure.Append(string.Format("{0} = new {1}();", item.name, item.typeString));
                }
            }

            if (gobject.IsClass)
            {
                constructure.Append(string.Format("\r\n{0}\t", tab));
                constructure.Append("}\r\n");
            }

            code = code.Replace("#code#", constructure.ToString() + sb.ToString());

            string filename = string.Format("{0}/{1}.cs", Options.tempCodePath, gobject.name);
            Utility.File.WriteString2File(filename, code);
            Utility.Logger.Log("write code : {0}.cs", gobject.name);
        }

        private static string ObjectType2String(GObject gobject)
        {
            if (gobject.IsClass)
            {
                return "class";
            }
            else if (gobject.objectType == ObjectType.EnumObject)
            {
                return "enum";
            }
            return string.Empty;
        }
    }
}
