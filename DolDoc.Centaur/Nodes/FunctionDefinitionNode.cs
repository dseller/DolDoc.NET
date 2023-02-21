using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using DolDoc.Shared;

namespace DolDoc.Centaur.Nodes
{
    public class FunctionDefinitionNode : DefinitionNode
    {
        private readonly Type returnType;
        private readonly List<SymbolTable.Symbol> parameters;
        private readonly ScopeNode body;

        public FunctionDefinitionNode(string name, Type returnType, List<SymbolTable.Symbol> parameters, ScopeNode body)
        {
            this.returnType = returnType;
            this.parameters = parameters;
            this.body = body;
            Name = name;
        }

        public override string Name { get; }

        public void GenerateBytecode(ILogger logger, SymbolTable symbolTable, TypeBuilder codeBuilder)
        {
            logger.Debug($"=== Function {Name} ===");
            var builder = codeBuilder.DefineMethod(Name, 
                MethodAttributes.Public | MethodAttributes.Static, 
                CallingConventions.Standard, 
                returnType, 
                parameters == null ? Array.Empty<Type>() : parameters.Select(s => s.Type).ToArray());
            var generator = new LoggingILGenerator(logger, builder.GetILGenerator());

            symbolTable.RootSymbols.Add(new SymbolTable.Symbol(Name, returnType, 0, SymbolTarget.Function, parameters?.Select(s => s.Type).ToArray(), builder));
            symbolTable.BeginScope();
            body.Emit(generator, symbolTable);
            // if (parameters != null)
            //     foreach (var sym in parameters.Symbols)
            //         symbolTable.NewSymbol(sym);
            symbolTable.EndScope();

            logger.Debug($"=== Function END ===");
        }
    }
}