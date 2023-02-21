using System.Reflection;
using Antlr4.Runtime;
using DolDoc.Centaur;
using Lokad.ILPack;

namespace DolDoc.CentaurExample
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var input = File.ReadAllText("test.centaur");
            // var input = "struct MyStruct { int a; int b; } int get_42() { return 42; } int my_function() { int a = get_42(); int b = 2; return a + b; }";
            
            var inputStream = new AntlrInputStream(input);
            var lexer = new centaurLexer(inputStream);

            var commonTokenStream = new CommonTokenStream(lexer);
            var parser = new centaurParser(commonTokenStream);
            var visitor = new CentaurVisitor();

            var context = parser.start();
            visitor.Visit(context);
            var codeType = visitor.Save();

            var mi = codeType.GetMethod("my_function", BindingFlags.Public | BindingFlags.Static);
            var result = mi.Invoke(null, new object[] {});
            
            Console.WriteLine(result);
        }
    }
}