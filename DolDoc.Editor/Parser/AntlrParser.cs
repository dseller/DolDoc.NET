using DolDoc.Editor.Core;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using DolDoc.Editor.Entries;
using System.Linq;
using static DolDoc.Editor.Parser.DolDocParser;

namespace DolDoc.Editor.Parser
{
    public class AntlrParser : DolDocParserBaseVisitor<object>, IDolDocParser
    {
        public IEnumerable<DocumentEntry> Parse(string input)
        {
            var inputStream = new AntlrInputStream(input);
            var lexer = new DolDocLexer(inputStream);

            var commonTokenStream = new CommonTokenStream(lexer);
            var parser = new DolDocParser(commonTokenStream);

            var context = parser.start();
            var result = Visit(context);

            return result as IEnumerable<DocumentEntry>;
        }

        public override object VisitStart([NotNull] StartContext context)
        {
            var result = new List<DocumentEntry>();
            foreach (var child in context.children)
            {
                var v = Visit(child);

                if (v is DocumentEntry entry)
                    result.Add(entry);
                else if (v is IEnumerable<DocumentEntry> entries)
                    result.AddRange(entries);
            }

            return result.AsEnumerable();
        }

        public override object VisitChunk_list([NotNull] Chunk_listContext context)
        {
            var result = new List<DocumentEntry>();
            foreach (var child in context.children)
                if (Visit(child) is DocumentEntry entry)
                    result.Add(entry);
            return result.AsEnumerable();
        }

        public override object VisitDoldoc_command([NotNull] Doldoc_commandContext context)
        {
            var command = context.cmd.Text;
            var entry = EntryFactory.Create(command, context.flags == null ? new List<Flag>() : (IList<Flag>)Visit(context.flags), context.args == null ? new List<Argument>() : (IList<Argument>)Visit(context.args));

            return entry;
        }

        public override object VisitArgument_list([NotNull] Argument_listContext context)
        {
            var result = new List<Argument>();
            foreach (var child in context.children)
                if (child is ArgumentContext || child is Named_argumentContext)
                    result.Add((Argument)Visit(child));
            return result;
        }

        public override object VisitFlag_list([NotNull] Flag_listContext context)
        {
            var result = new List<Flag>();
            foreach (var child in context.children)
                if (child is FlagContext)
                    result.Add((Flag)Visit(child));
            return result;
        }

        public override object VisitFlag([NotNull] FlagContext context) => new Flag(context.a.Text == "+", context.value.Text);

        public override object VisitString([NotNull] StringContext context) => context.GetText().Substring(1, context.GetText().Length - 2);

        public override object VisitColor([NotNull] ColorContext context) => context.GetText();

        public override object VisitInteger([NotNull] IntegerContext context) => context.GetText();

        public override object VisitArgument([NotNull] ArgumentContext context) => new Argument(null, Visit(context.children[0]).ToString());

        public override object VisitNamed_argument([NotNull] Named_argumentContext context) => new Argument(context.key.Text, ((Argument)Visit(context.value)).Value);

        public override object VisitContent([NotNull] ContentContext context)
        {
            return new Text(new List<Flag>(), new List<Argument>
            {
                new Argument(null, context.GetText())
            });
        }
    }
}
