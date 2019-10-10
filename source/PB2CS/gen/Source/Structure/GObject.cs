using System;
using System.Collections.Generic;

namespace gen.Source.Structure
{
    public abstract class GObject
    {
        public GObject(string file, string code)
        {
            this.file = file;
            this.code = code;
            this.name = string.Empty;
            tuples = new List<Tuple>();
        }

        public bool Parse()
        {
            if (string.IsNullOrEmpty(code))
            {
                return false;
            }

            try
            {
                name = ParseName();
                PareTuple();
            }
            catch (Exception e)
            {
                Utility.Logger.Log("Pare object error, file is {0}, name is {1}, error:{2}", file, name, e.Message.ToString());
                throw e;
            }
            return true;
        }

        protected abstract string ParseName();

        private void PareTuple()
        {
            List<string> tupleString = Matcher.MatchProperty(code);
            bool isClass = IsClass;
            foreach (string item in tupleString)
            {
                Tuple tuple = new Tuple(item);
                if (!tuple.Parse(isClass))
                {
                    continue;
                }

                TryAddTuple(tuple);
                if (!repeated && tuple.repeated)
                {
                    this.repeated = true;
                }
            }
        }

        private void TryAddTuple(Tuple tuple)
        {
            foreach (var item in tuples)
            {
                if (item.name == tuple.name)
                {
                    throw new Exception(string.Format("filed name: {0} can not be equal.", tuple.name));
                }
                if (IsClass && item.index == tuple.index)
                {
                    throw new Exception(string.Format("index: {0} can not be equal, filed 1: {1}, filed 1: {2}.", tuple.index, item.name, tuple.name));
                }
            }
            tuples.Add(tuple);
        }

        public bool IsClass {
            get {
                return objectType == ObjectType.ClassObject;
            }
        }

        public string file { get; private set; }
        public string name { get; protected set; }
        public bool repeated { get; private set; }
        public List<Tuple> tuples { get; }
        public ObjectType objectType { get; protected set; }

        protected string code;
    }

    public enum ObjectType
    {
        Unknown,
        ClassObject,
        EnumObject,
    }
}
