namespace gen.Source.Structure
{
    public class EnumObject : GObject
    {
        public EnumObject(string file, string code) : base (file, code)
        {
            objectType = ObjectType.EnumObject;
        }

        protected override string ParseName()
        {
            return Matcher.MatchEnumName(code);
        }
    }
}
