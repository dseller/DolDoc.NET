using System.Reflection;
using Antlr4.Runtime;
using DolDoc.Centaur;
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

            var commonTokenStream = new CommonTokenStream(lexer);
            var parser = new centaurParser(commonTokenStream);
            var visitor = new CentaurVisitor(new SeriLogger(Log.Logger));
            visitor.RegisterFunction("print", typeof(Program).GetMethod("Write", BindingFlags.Public | BindingFlags.Static, new[] { typeof(string) }));
            visitor.RegisterFunction("str", typeof(Program).GetMethod("Str", BindingFlags.Public | BindingFlags.Static, new[] { typeof(object) }));

            var context = parser.start();
            visitor.Visit(context);
            var codeType = visitor.Save();

            var mi = codeType.GetMethod("my_function", BindingFlags.Public | BindingFlags.Static);
            var result = mi.Invoke(null, new object[] {});
            
            Console.WriteLine(result);
        }
    }
}