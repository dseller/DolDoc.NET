using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Antlr4.Runtime;
using DolDoc.Centaur.Internals;
using DolDoc.Centaur.Nodes;
using DolDoc.Centaur.Symbols;
using DolDoc.Shared;
using Math = DolDoc.Centaur.Internals.Math;

namespace DolDoc.Centaur
{
    public class CentaurEngine
    {
        private readonly ILogger logger;
        private readonly ICompilerContext rootContext;

        public CentaurEngine(ILogger logger)
        {
            this.logger = logger;
            SymbolTable = new SymbolTable();
            rootContext = new RootCompilerContext(logger, SymbolTable);
            LoadFunctions<Math>();
            LoadFunctions<Strings>();
            LoadFunctions<FileSystem>();
            LoadClass<CDirEntry>();
            LoadEnum<FUF_FLAG>();
            
            LoadFunctions<Chrono>();
            LoadClass<CDate>();
            LoadClass<CDateStruct>();
        }
        
        public SymbolTable SymbolTable { get; }

        public void LoadFunctions<T>()
        {
            var methods = typeof(T).GetMethods(BindingFlags.Public | BindingFlags.Static);
            foreach (var method in methods)
                SymbolTable.NewSymbol(new FunctionSymbol(method.Name, method.ReturnType,
                    method.GetParameters().Select(p => p.ParameterType).ToArray(), method));
        }

        public void LoadClass<T>()
        {
            SymbolTable.NewSymbol(new TypeSymbol(typeof(T).Name, typeof(T)));
        }

        public void LoadEnum<T>() where T : Enum
        {
            foreach (var value in Enum.GetValues(typeof(T)))
                SymbolTable.NewSymbol(new DefinitionSymbol(value.ToString(), typeof(int), (int) value));
        }
        
        public void Include(string name, string code)
        {
            var inputStream = new AntlrInputStream(code);
            var lexer = new centaurLexer(inputStream);

            var commonTokenStream = new CommonTokenStream(lexer);
            var parser = new centaurParser(commonTokenStream);
            var visitor = new CentaurVisitor(logger);
            
            var context = parser.start();
            var result = visitor.Visit(context) as DefinitionListNode;

            using (var ctx = rootContext.BeginSubContext(name))
            {
                var set = new List<ASTNode>(result.Definitions);

                var statementLists = set.OfType<StatementListNode>().ToList();
                var statementListIndices = statementLists.ToDictionary(s => s, s => set.IndexOf(s));

                var offset = 0;
                foreach (var statementList in set.OfType<StatementListNode>().ToList())
                {
                    // explode!
                    set.InsertRange(statementListIndices[statementList] + offset, statementList.Statements);
                    offset += statementList.Statements.Count - 1;
                    set.Remove(statementList);
                }
                
                foreach (var includeNode in set.OfType<IncludeNode>().ToList())
                {
                    Include(Path.GetFileNameWithoutExtension(includeNode.Path),  File.ReadAllText(includeNode.Path));
                    set.Remove(includeNode);
                }
                
                foreach (var structNode in set.OfType<ClassNode>().ToList())
                {
                    structNode.AddToSymbolTable(ctx);
                    set.Remove(structNode); 
                }

                foreach (var staticVar in set.OfType<DeclareVariableNode>().ToList())
                {
                    staticVar.DefineStaticVariable(ctx);
                    set.Remove(staticVar);
                }

                var funcs = new List<FunctionSymbol>();
                foreach (var fn in set.OfType<FunctionDefinitionNode>().ToList())
                {
                    funcs.Add(fn.GenerateBytecode(ctx));
                    set.Remove(fn);
                }
                
                // What remains must be constructed into a new function.
                var rootFn = new FunctionDefinitionNode("__TOPLEVELFUNCTION", "U0", new ParameterListNode(new List<ParameterNode>()), new ScopeNode(set.OfType<IBytecodeEmitter>().ToList()));
                rootFn.GenerateBytecode(ctx);

                var codeType = ctx.CodeBuilder.CreateType();
                foreach (var fn in funcs)
                    fn.CodeType = codeType;
                
                var tlf = codeType.GetMethod("__TOPLEVELFUNCTION");
                tlf.Invoke(null, Array.Empty<object>());
            }
        }

        public T Call<T>(string name, params object[] parameters)
        {
            var sym = SymbolTable.FindSymbol<FunctionSymbol>(name);
            var method = sym.CodeType?.GetMethod(name, BindingFlags.Public | BindingFlags.Static);
            return (T)method.Invoke(null, parameters);
        }
    }
}