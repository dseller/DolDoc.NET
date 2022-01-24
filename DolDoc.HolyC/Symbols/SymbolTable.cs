using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolyScript.Compiler.Symbols
{
    public class SymbolTable
    {
        public Dictionary<string, Symbol> Symbols { get; }

        public SymbolTable()
        {
            Symbols = new Dictionary<string, Symbol>();
        }

        public void Add(Symbol sym)
        {
            Symbols.Add(sym.Name, sym);
        }

        public bool Contains(string name) => Symbols.ContainsKey(name);

        public Symbol Get(string name) => Symbols[name];

        public T Get<T>(string name) where T : Symbol => Symbols[name] as T;
    }
}
