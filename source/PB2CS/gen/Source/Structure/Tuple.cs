using System;

namespace gen.Source.Structure
{
    public class Tuple
    {
        public Tuple(string content)
        {
            this.content = content;
        }

        public bool Parse(bool isClass)
        {
            if (string.IsNullOrEmpty(content))
            {
                return false;
            }

            content = content.Trim();
            repeated = content.StartsWith(Matcher.REPEATED);

            string dv = null;
            content = Matcher.MatchDefaultValue(content, out dv);
            defaultValue = dv;

            string[] name2id = content.Split('=');
            if (name2id.Length != 2)
            {
                throw new Exception(string.Format("tuple content:{0} is error.", content));
            }

            name = name2id[0].Trim();
            string[] type2name = Matcher.SplitBlank(name);

            if (isClass)
            {
                name = type2name[2].Trim();
                typeString = TypeParser.Parse(type2name[1].Trim(), repeated);
            }

            index = name2id[1].Trim();
            return true;
        }

        public string name { get; private set; }
        public string index { get; private set; }
        public string typeString { get; private set; }
        public bool repeated { get; private set; }
        public string defaultValue { get; private set; }

        private string content;
    }
}
