using gen.Source.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gen.Source
{
    public class Tuple2Code
    {
        public static string Parse(Tuple tuple, ObjectType type)
        {
            if (type == ObjectType.ClassObject)
            {
                return ParseClass(tuple);
            }
            else if (type == ObjectType.EnumObject)
            {
                return ParseEnum(tuple);
            }

            return string.Empty;
        }

        private static string ParseClass(Tuple tuple)
        {
            if (null == tuple)
            {
                return string.Empty;
            }

            string defaultValue = null == tuple.defaultValue ? string.Empty : string.Format(" = {0}", tuple.defaultValue);
            return string.Format("public {0} {1}{2};", tuple.typeString, tuple.name, defaultValue);
        }

        private static string ParseEnum(Tuple tuple)
        {
            if (null == tuple)
            {
                return string.Empty;
            }

            return string.Format("{0} = {1},", tuple.name, tuple.index);
        }
    }
}
