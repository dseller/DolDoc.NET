using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DolDoc.Centaur.Symbols;

namespace DolDoc.Centaur
{
    public class SymbolTable
    {
        private List<Symbol> CurrentSymbolTable => symbols.FirstOrDefault();
        private readonly Stack<List<Symbol>> symbols;
        public List<Symbol> RootSymbols { get; }

        public SymbolTable()
        {
            symbols = new Stack<List<Symbol>>();
            RootSymbols = new List<Symbol>();
            symbols.Push(RootSymbols);
            
            RootSymbols.AddRange(new[]
            {
                new TypeSymbol("U0", typeof(void)),
                new TypeSymbol("I64", typeof(long)),
                new TypeSymbol("U64", typeof(ulong)),
                new TypeSymbol("I32", typeof(int)),
                new TypeSymbol("U32", typeof(uint)),
                new TypeSymbol("U8", typeof(byte)),
                new TypeSymbol("S8", typeof(sbyte)),
                new TypeSymbol("bool", typeof(bool)),
                new TypeSymbol("string", typeof(string)),
                new TypeSymbol("object", typeof(object))
            });
        }

        public bool IsRootScope => CurrentSymbolTable == RootSymbols;

        public void Clear()
        {
            symbols.Clear();  
            symbols.Push(RootSymbols);
        } 
        
        public Symbol FindSymbol(string name)
        {
            foreach (var scope in symbols)
            {
                var symbol = scope.Find(s => s.Name == name);
                if (symbol != default)
                    return symbol;
            }

            return null;
        }

        public T FindSymbol<T>(string name) where T : class => FindSymbol(name) as T;
        
        public T NewSymbol<T>(T symbol) where T : Symbol
        {
            CurrentSymbolTable.Add(symbol);
            return symbol;
        }

        public void BeginScope()
        {
            symbols.Push(new());
        }

        public void EndScope()
        {
            symbols.Pop();
        }
        
        public void RegisterFunction(string name, MethodInfo m)
        {
            RootSymbols.Add(new FunctionSymbol(name, m.ReturnType, m.GetParameters().Select(p => p.ParameterType).ToArray(), m));
        }
    }
}