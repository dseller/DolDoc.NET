using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DolDoc.Centaur.Symbols;

namespace DolDoc.Centaur.Nodes
{
    public class FunctionDefinitionNode : DefinitionNode
    {
        private readonly string returnType;
        private readonly ParameterListNode parameters;
        private readonly ScopeNode body;

        public FunctionDefinitionNode(string name, string returnType, ParameterListNode parameters, ScopeNode body)
        {
            this.returnType = returnType;
            this.parameters = parameters;
            this.body = body;
            Name = name;
        }

        public override string Name { get; }

        public void GenerateBytecode(CompilerContext ctx)
        {
            ctx.Log.Debug($"=== Function {Name} ===");
            var type = ctx.SymbolTable.FindSymbol<TypeSymbol>(returnType)?.Type;

            var parameterTypes = new List<Type>();
            if (parameters != null)
            {
                foreach (var parameterNode in parameters.Parameters)
                {
                    var parameterType = ctx.SymbolTable.FindSymbol<TypeSymbol>(parameterNode.Type)?.Type;
                    if (parameterType == null)
                        throw new Exception();
                    parameterTypes.Add(parameterType);
                }
            }

            var builder = ctx.CodeBuilder.DefineMethod(Name,
                MethodAttributes.Public | MethodAttributes.Static,
                CallingConventions.Standard,
                type,
                parameterTypes.ToArray());

            var fnCtx = new FunctionCompilerContext(ctx, builder);
            ctx.SymbolTable.RootSymbols.Add(new FunctionSymbol(Name, type, parameterTypes.ToArray(), builder));
            ctx.SymbolTable.BeginScope();
            if (parameters != null)
            {
                int i = 0;
                foreach (var sym in parameters.Parameters)
                {
                    var parameterType = ctx.SymbolTable.FindSymbol<TypeSymbol>(sym.Type)?.Type;
                    ctx.SymbolTable.NewSymbol(new ParameterSymbol(sym.Name, parameterType, i++));
                }
            }
            body.Emit(fnCtx);
            

            ctx.SymbolTable.EndScope();

            ctx.Log.Debug($"=== Function END ===");
        }
    }
}