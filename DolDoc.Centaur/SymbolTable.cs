using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
        public record Symbol(string Name, Type Type, int Index, SymbolTarget Target, Type[] ParameterTypes = null, MethodInfo Method = null);
        private List<Symbol> CurrentSymbolTable => symbols.LastOrDefault();
        private readonly Stack<List<Symbol>> symbols;
        public List<Symbol> RootSymbols { get; }

        public SymbolTable()
        {
            symbols = new Stack<List<Symbol>>();
            RootSymbols = new List<Symbol>();
            symbols.Push(RootSymbols);
        }

        public bool IsRootScope => CurrentSymbolTable == RootSymbols;

        public void Clear()
        {
            symbols.Clear();  
            symbols.Push(RootSymbols);
        } 
        
        public Symbol FindSymbol(string name)
        {
            foreach (var scope in symbols.Reverse())
            {
                var symbol = scope.Find(s => s.Name == name);
                if (symbol != default)
                    return symbol;
            }

            return null;
        }

        public void NewSymbol(Symbol symbol)
        {
            CurrentSymbolTable.Add(symbol);
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
            RootSymbols.Add(new Symbol(name, m.ReturnType, 0, SymbolTarget.Function, m.GetParameters().Select(p => p.ParameterType).ToArray(), m));
        }
    }
}