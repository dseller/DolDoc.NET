using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DolDoc.Centaur.Symbols;

namespace DolDoc.Centaur
{
    public enum SymbolTarget
    {
        Variable = 0,
        Function = 1,
        Parameter = 2
    }
    
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
                new TypeSymbol("int", typeof(uint)),
                new TypeSymbol("byte", typeof(byte)),
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