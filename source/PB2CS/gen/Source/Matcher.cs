using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace gen.Source
{
    public class Matcher
    {
        public static string ReplaceBlank(string code)
        {
            //string noBlankCode = Regex.Replace(code, BLANK_MATCHER, string.Empty);
            return Regex.Replace(code, COMMENT_MATCHER, string.Empty);
        }

        public static List<string> MatchClass(string content)
        {
            return Match(content, CLASS_MATCHER);
        }

        public static List<string> MatchEnum(string content)
        {
            return Match(content, ENUM_MATCHER);
        }

        public static string MatchClassName(string content)
        {
            Match match = Regex.Match(content, CLASSNAME_NAME_MATCHER);
            if (match.Success)
            {
                return match.Value.Trim();
            }
            return string.Empty;
        }

        public static string MatchEnumName(string content)
        {
            Match match = Regex.Match(content, ENUM_NAME_MATCHER);
            if (match.Success)
            {
                return match.Value.Trim();
            }
            return string.Empty;
        }

        public static List<string> MatchProperty(string content)
        {
            return Match(content, PROPERTY_MATCHER);
        }

        public static string MatchDefaultValue(string content, out string defaultvalue)
        {
            defaultvalue = null;
            Match match = Regex.Match(content, DEFAULTVALUE_MATCHER);
            if (match.Success)
            {
                defaultvalue = match.Value;
                content = content.Replace(defaultvalue, string.Empty);
                defaultvalue = MatchDefaultRealValue(defaultvalue);
                return content;
            }
            return content;
        }

        public static string[] SplitBlank(string content)
        {
            return Regex.Split(content, BLANK_MATCHER);
        }

        private static string MatchDefaultRealValue(string content)
        {
            Match match = Regex.Match(content, DEFAULTREALVALUE_MATCHER);
            if (match.Success)
            {
                return match.Value.Trim();
            }
            return content;
        }

        private static List<string> Match(string input, string match)
        {
            List<string> results = new List<string>();
            MatchCollection mc = Regex.Matches(input, match);
            foreach (Match m in mc)
            {
                results.Add(m.Value);
            }
            return results;
        }

        public static string OPTIONAL = "optional";
        public static string REQUIRED = "required";
        public static string REPEATED = "repeated ";

        private static string CLASSNAME_NAME_MATCHER = @"(?<=message)\s+(\w)*";
        private static string ENUM_NAME_MATCHER = @"(?<=enum)\s+(\w)*";
        private static string PROPERTY_MATCHER = @".*?(?=(;|,))";
        private static string CLASS_MATCHER = @"(message)((?:.|[\r\n])*?)}";
        private static string ENUM_MATCHER = @"(enum)((?:.|[\r\n])*?)}";
        private static string COMMENT_MATCHER = @"(/\*(.|\n)*?\*/)|(//[\s\S]*?\n)";
        private static string DEFAULTVALUE_MATCHER = @"\[.*(?:default).*?=.*\]";
        private static string DEFAULTREALVALUE_MATCHER = @"(?<==).*?(?=\])";
        private static string BLANK_MATCHER = @"\s+";
    }
}
