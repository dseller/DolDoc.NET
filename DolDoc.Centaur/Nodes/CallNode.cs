using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Emit;
using DolDoc.Centaur.Symbols;

namespace DolDoc.Centaur.Nodes
{
    public class CallNode : ASTNode, IBytecodeEmitter
    {
        private readonly string name;
        private readonly List<IBytecodeEmitter> arguments;

        public CallNode(string name, List<IBytecodeEmitter> arguments)
        {
            this.name = name;
            this.arguments = arguments;
        }

        public void Emit(FunctionCompilerContext ctx)
        {
            var sym = ctx.SymbolTable.FindSymbol<FunctionSymbol>(name);
            Debug.Assert(sym != null);

            if (sym.Parameters != null && sym.Parameters.Length > 0)
            {
                var i = 0;
                foreach (var arg in arguments)
                {
                    arg.Emit(ctx);
                    if (sym.Parameters[i] == typeof(object) && arg.Type != null && arg.Type.IsPrimitive)
                        ctx.Generator.Emit(OpCodes.Box, arg.Type);
                    i++;
                }

                ctx.Generator.EmitCall(OpCodes.Call, sym.MethodInfo, sym.Parameters);                
            }
            else
                ctx.Generator.Emit(OpCodes.Call, sym.MethodInfo);  
        }

        public Type Type => null; // TODO
    }
}