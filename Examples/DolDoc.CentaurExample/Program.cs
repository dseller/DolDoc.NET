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
        private static ILogger logger;
        
        public static void Write(string text) => Console.Write(text);

        public static string ToBinary(long value) => Convert.ToString(value, 2);

        public static void LogInfo(string text) => logger?.Information(text);

        public static string Str(object obj) => obj.ToString(); //obj?.ToString() ?? "null";
        
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .MinimumLevel.Debug()
                .CreateLogger();
            Program.logger = Log.Logger;

            var input = File.ReadAllText("test.centaur");
            // var input = "struct MyStruct { int a; int b; } int get_42() { return 42; } int my_function() { int a = get_42(); int b = 2; return a + b; }";
            var inputStream = new AntlrInputStream(input);
            var lexer = new centaurLexer(inputStream);

            var logger = new SeriLogger(Log.Logger);
            var commonTokenStream = new CommonTokenStream(lexer);
            var parser = new centaurParser(commonTokenStream);
            var visitor = new CentaurVisitor(logger);
            var symbolTable = new SymbolTable();
            symbolTable.RegisterFunction("Print", typeof(Program).GetMethod("Write", BindingFlags.Public | BindingFlags.Static, new[] { typeof(string) }));
            symbolTable.RegisterFunction("Str", typeof(Program).GetMethod("Str", BindingFlags.Public | BindingFlags.Static, new[] { typeof(object) }));
            symbolTable.RegisterFunction("LogInfo", typeof(Program).GetMethod("LogInfo", BindingFlags.Public | BindingFlags.Static, new[] { typeof(string) }));
            symbolTable.RegisterFunction("ToBinary", typeof(Program).GetMethod("ToBinary", BindingFlags.Public | BindingFlags.Static, new[] { typeof(long) }));
            

            var context = parser.start();
            var result = visitor.Visit(context) as DefinitionListNode;


            var compilerContext = new CompilerContext(logger, symbolTable);
            foreach (var staticVar in result.Definitions.OfType<DeclareVariableNode>())
                staticVar.DefineStaticVariable(compilerContext);
            foreach (var fn in result.Definitions.OfType<FunctionDefinitionNode>())
                fn.GenerateBytecode(compilerContext);

            var codeType = compilerContext.CodeBuilder.CreateType();

            var mi = codeType.GetMethod("my_function", BindingFlags.Public | BindingFlags.Static);
            var x = mi.Invoke(null, new object[] {});
            
            // Console.WriteLine(x);
            logger.Info("Result: {result}", x);
        }
    }
}