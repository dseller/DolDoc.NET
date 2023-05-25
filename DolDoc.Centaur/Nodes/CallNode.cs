using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
            if (sym == null)
                throw new Exception($"Could not find function {name}");

            if (sym.Parameters != null && sym.Parameters.Length > 0)
            {
                var i = 0;
                foreach (var arg in arguments)
                {
                    arg.Emit(ctx);
                    // ctx.Log.Info("Parameter {idx}: {type}", i, sym.Parameters[i]);
                    if (sym.Parameters[i] == typeof(object) && arg.Type(ctx) != null && arg.Type(ctx).IsPrimitive)
                        ctx.Generator.Emit(OpCodes.Box, arg.Type(ctx));
                    i++;
                }

                ctx.Generator.EmitCall(OpCodes.Call, sym.MethodInfo, sym.Parameters);                
            }
            else
                ctx.Generator.Emit(OpCodes.Call, sym.MethodInfo);
        }

        public Type Type(ICompilerContext ctx) => ctx.SymbolTable.FindSymbol<FunctionSymbol>(name)?.Type;
    }
}