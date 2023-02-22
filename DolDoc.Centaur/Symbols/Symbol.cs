using System;

namespace DolDoc.Centaur.Symbols
{
    public abstract class Symbol
    {
        protected Symbol(string name, Type type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; private set; }
        
        public Type Type { get; private set; }

        public abstract void EmitGet(FunctionCompilerContext ctx);
        
        public abstract void EmitSet(FunctionCompilerContext ctx);
        //string Name, Type Type, int Index, SymbolTarget Target, Type[] ParameterTypes = null, MethodInfo Method = null
    }
}