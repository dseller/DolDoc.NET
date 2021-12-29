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
                    ""Returning 111"";
                    return 111;
                }

                U8 *MemTest()
                {
                    U8 *data = MAlloc(32);

                    return data;
                }

                I32 Chained()
                {
                    I32 i = 1337;
                    I32 x = Test;
                    I32 v;
                    i++;
                    // i--;
                    Kak;
                    v = 1;

                    U8 *what = MemTest;
                    U8 *sss = ""Wha333t"";
                    Print(sss);

                    return x + 10 + (v * 1000) + 50 - i * 2 / 4;
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
