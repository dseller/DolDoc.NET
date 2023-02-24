using System;
using System.Linq;
using System.Reflection;
using Antlr4.Runtime;
using DolDoc.Centaur.Nodes;
using DolDoc.Centaur.Symbols;
using DolDoc.Shared;

namespace DolDoc.Centaur
{
    public class CentaurEngine
    {
        private readonly ILogger logger;
        private Type codeType;

        public CentaurEngine(ILogger logger)
        {
            this.logger = logger;
            SymbolTable = new SymbolTable();
        }
        
        public SymbolTable SymbolTable { get; }

        public void LoadFunctions<T>()
        {
            var methods = typeof(T).GetMethods(BindingFlags.Public | BindingFlags.Static);
            foreach (var method in methods)
                SymbolTable.NewSymbol(new FunctionSymbol(method.Name, method.ReturnType,
                    method.GetParameters().Select(p => p.ParameterType).ToArray(), method));
        }

        public void Include(string code)
        {
            var inputStream = new AntlrInputStream(code);
            var lexer = new centaurLexer(inputStream);

            var commonTokenStream = new CommonTokenStream(lexer);
            var parser = new centaurParser(commonTokenStream);
            var visitor = new CentaurVisitor(logger);
            
            var context = parser.start();
            var result = visitor.Visit(context) as DefinitionListNode;
            
            var compilerContext = new CompilerContext(logger, SymbolTable);
            foreach (var structNode in result.Definitions.OfType<StructNode>())
                structNode.AddToSymbolTable(compilerContext);
            foreach (var staticVar in result.Definitions.OfType<DeclareVariableNode>())
                staticVar.DefineStaticVariable(compilerContext);
            foreach (var fn in result.Definitions.OfType<FunctionDefinitionNode>())
                fn.GenerateBytecode(compilerContext);
            
            codeType = compilerContext.CodeBuilder.CreateType();
        }

        public T Call<T>(string name, params object[] parameters)
        {
            var mi = codeType.GetMethod(name, BindingFlags.Public | BindingFlags.Static);
            return (T)mi?.Invoke(null, parameters);
        }
    }
}