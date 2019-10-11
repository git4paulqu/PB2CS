using gen.Source.Structure;

namespace gen.Source
{
    public class Tuple2Code
    {
        public static string Parse(Structure.Tuple tuple, ObjectType type)
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

        private static string ParseClass(Structure.Tuple tuple)
        {
            if (null == tuple)
            {
                return string.Empty;
            }

            string defaultValue = null == tuple.defaultValue ? string.Empty : string.Format(" = {0}", tuple.defaultValue);
            return string.Format("public {0} {1}{2};", tuple.typeString, tuple.name, defaultValue);
        }

        private static string ParseEnum(Structure.Tuple tuple)
        {
            if (null == tuple)
            {
                return string.Empty;
            }

            return string.Format("{0} = {1},", tuple.name, tuple.index);
        }
    }
}
