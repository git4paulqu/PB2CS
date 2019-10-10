namespace gen.Source.Structure
{
    public class ClassObject : GObject
    {
        public ClassObject(string file, string code) : base(file, code)
        {
            objectType = ObjectType.ClassObject;
        }

        protected override string ParseName()
        {
            return Matcher.MatchClassName(code);
        }
    }
}
