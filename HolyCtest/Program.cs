using Antlr4.Runtime;
using DolDoc.HolyC.Grammar;
using System;

namespace HolyCtest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var input = "\"Hello World!\";";

            var inputStream = new AntlrInputStream(input);
            var lexer = new HolyCLexer(inputStream);

            var commonTokenStream = new CommonTokenStream(lexer);
            var parser = new HolyCParser(commonTokenStream);

            var visitor = new HolyCVisitor();


            var context = parser.compilationUnit();
            var result = visitor.Visit(context);
        }
    }
}
