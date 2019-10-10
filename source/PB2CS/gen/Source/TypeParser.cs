namespace gen.Source
{
    public class TypeParser
    {
        public static string Parse(string type, bool isRepeat)
        {
            if (!isRepeat)
            {
                return parse(type);
            }
            
            string cstype = parse(type);
            return string.Format("System.Collections.Generic.List<{0}>", cstype);
        }

        private static string parse(string type)
        {
            switch (type)
            {

                case "uint32":
                    return "uint";

                case "int32":
                    return "int";

                case "uint64":
                    return "ulong";

                case "int64":
                    return "long";

                case "float":
                    return "float";

                case "double":
                    return "double";

                case "bytes":
                    return "byte[]";

                case "bool":
                    return "bool";

                case "string":
                    return "string";

                default:
                    return type.Trim();
            }
        }
    }
}
