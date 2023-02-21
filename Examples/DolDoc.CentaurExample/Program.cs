using System.Reflection;
using Antlr4.Runtime;
using DolDoc.Centaur;
using DolDoc.Centaur.Nodes;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace DolDoc.CentaurExample
{
    public static class Program
    {
        public static void Write(string text) => Console.Write(text);

        public static string Str(object obj) => obj?.ToString() ?? "null";
        
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .MinimumLevel.Debug()
                .CreateLogger();

            var input = File.ReadAllText("test.centaur");
            // var input = "struct MyStruct { int a; int b; } int get_42() { return 42; } int my_function() { int a = get_42(); int b = 2; return a + b; }";
            
            var inputStream = new AntlrInputStream(input);
            var lexer = new centaurLexer(inputStream);

            var logger = new SeriLogger(Log.Logger);
            var commonTokenStream = new CommonTokenStream(lexer);
            var parser = new centaurParser(commonTokenStream);
            var visitor = new CentaurVisitor(logger);
            var symbolTable = new SymbolTable();
            symbolTable.RegisterFunction("print", typeof(Program).GetMethod("Write", BindingFlags.Public | BindingFlags.Static, new[] { typeof(string) }));
            symbolTable.RegisterFunction("str", typeof(Program).GetMethod("Str", BindingFlags.Public | BindingFlags.Static, new[] { typeof(object) }));

            var context = parser.start();
            var result = visitor.Visit(context) as DefinitionListNode;
            foreach (var fn in result.Definitions.OfType<FunctionDefinitionNode>())
                fn.GenerateBytecode(logger, symbolTable, visitor.codeBuilder);

            var codeType = visitor.codeBuilder.CreateType();

            var mi = codeType.GetMethod("my_function", BindingFlags.Public | BindingFlags.Static);
            var x = mi.Invoke(null, new object[] {});
            
            Console.WriteLine(x);
        }
    }
}