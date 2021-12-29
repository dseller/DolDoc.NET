using Antlr4.Runtime;
using DolDoc.HolyC.Grammar;
using System;

namespace HolyCtest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var input = @"
                I32 Test(I8 value)
                {
                    return 111;
                }

                I32 Chained()
                {
                    I32 i = 1337;
                    I32 x = Test();
                    Kak();

                    return x + i - 10 + 50 - i * 2 / 4;
                }

                // Test();
                // U8 *str = ""Test"";
            ";

            var inputStream = new AntlrInputStream(input);
            var lexer = new HolyCLexer(inputStream);

            var commonTokenStream = new CommonTokenStream(lexer);
            var parser = new HolyCParser(commonTokenStream);

            var visitor = new HolyCVisitor();


            var context = parser.compilationUnit();
            visitor.Visit(context);

            var result = visitor.GetMethod("Chained").Invoke(null, null);
            Console.WriteLine(result);
        }
    }
}
