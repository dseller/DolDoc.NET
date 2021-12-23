using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolDoc.HolyC.Grammar
{
    public class HolyCVisitor : HolyCBaseVisitor<object>
    {
        public override object VisitPrintStatement([NotNull] HolyCParser.PrintStatementContext context)
        {
            Console.WriteLine(context.children[0].ToString());

            return null;
        }

        public override object VisitDeclaration([NotNull] HolyCParser.DeclarationContext context)
        {
            return base.VisitDeclaration(context);
        }
    }
}
