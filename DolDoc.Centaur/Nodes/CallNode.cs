using System;
using System.Diagnostics;
using System.Reflection.Emit;

namespace DolDoc.Centaur.Nodes
{
    public class CallNode : CodeNode
    {
        private readonly string name;

        public CallNode(string name)
        {
            this.name = name;
        }

        public override void Emit(LoggingILGenerator generator, SymbolTable symbolTable)
        {
            var sym = symbolTable.FindSymbol(name);
            Debug.Assert(sym != null);
            Debug.Assert(sym.Target == SymbolTarget.Function);
            Debug.Assert(sym.ParameterTypes == null || sym.ParameterTypes.Length  == 0);

            // if (sym.ParameterTypes != null && sym.ParameterTypes.Length > 0)
            // {
            //     foreach (var child in context.args.children)
            //     {
            //         var x = Visit(child) as DataAstNode;
            //         x?.EmitRead(generator);
            //     }
            //     generator.EmitCall(OpCodes.Call, mi, sym.ParameterTypes);                
            // }
            // else
            
            generator.Emit(OpCodes.Call, sym.Method);  
        }
    }
}