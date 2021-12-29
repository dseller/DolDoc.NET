using Antlr4.Runtime.Misc;
using Sigil.NonGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DolDoc.HolyC.Grammar
{
    public class HolyCVisitor : HolyCBaseVisitor<object>
    {
        private ILGenerator ilGenerator;
        private Dictionary<string, DynamicMethod> methods;
        // private Dictionary<string, int> variables;
        private Dictionary<string, LocalBuilder> variables;

        public DynamicMethod GetMethod(string name) => methods[name];

        public HolyCVisitor()
        {
            methods = new Dictionary<string, DynamicMethod>();
        }

        public override object VisitInclude([NotNull] HolyCParser.IncludeContext context)
        {
            Console.WriteLine("include: {0}", context.children[0].ToString());
            return base.VisitInclude(context);
        }

        public override object VisitIdent([NotNull] HolyCParser.IdentContext context)
        {
            var name = context.children[0].GetText();
            var variable = variables[name];

            ilGenerator.Emit(OpCodes.Ldloc, variable);

            return null;
        }

        public override object VisitAdd([NotNull] HolyCParser.AddContext context)
        {
            Visit(context.children[0]);
            Visit(context.children[2]);
            ilGenerator.Emit(OpCodes.Add);
            return null;
        }

        public override object VisitSub([NotNull] HolyCParser.SubContext context)
        {
            Visit(context.children[0]);
            Visit(context.children[2]);
            ilGenerator.Emit(OpCodes.Sub);
            return null;
        }

        public override object VisitMul([NotNull] HolyCParser.MulContext context)
        {
            Visit(context.children[0]);
            Visit(context.children[2]);
            ilGenerator.Emit(OpCodes.Mul);
            return null;
        }

        public override object VisitDiv([NotNull] HolyCParser.DivContext context)
        {
            Visit(context.children[0]);
            Visit(context.children[2]);
            ilGenerator.Emit(OpCodes.Div);
            return null;
        }

        public override object VisitPrintStatement([NotNull] HolyCParser.PrintStatementContext context)
        {
            if (ilGenerator == null)
                return null;

            var str = context.children[0].GetText();
            ilGenerator.EmitWriteLine(str.Substring(1, str.Length - 2));

            return null;
        }

        public override object VisitDirectDeclarator([NotNull] HolyCParser.DirectDeclaratorContext context)
        {
            return base.VisitDirectDeclarator(context);
        }

        public override object VisitCall([NotNull] HolyCParser.CallContext context)
        {
            MethodInfo mi;
            var name = context.children[0].GetText();
            if (methods.TryGetValue(name, out var method))
                mi = method;
            else
                mi = typeof(Runtime).GetMethod(name);

            ilGenerator.EmitWriteLine("doing call");
            ilGenerator.Emit(OpCodes.Call, mi);
            
            return null;
        }

        public override object VisitDeclaration([NotNull] HolyCParser.DeclarationContext context)
        {
            string name = null;
            var type = context.children[0].GetChild(0).GetText();
            if (context.ChildCount == 2)
                name = context.children[0].GetChild(1).GetText();
            else if (context.ChildCount == 3)
                name = context.children[1].GetChild(0).GetChild(0).GetText();

            if (variables.ContainsKey(name))
                throw new ApplicationException($"Duplicate variable name {name}");

            var variable = ilGenerator.DeclareLocal(GetClrType(type));
            variables.Add(name, variable);

            // Visit InitDeclarator
            if (context.ChildCount == 3)
                Visit(context.children[1]);

            return null;
        }

        public override object VisitInitDeclarator([NotNull] HolyCParser.InitDeclaratorContext context)
        {
            var variable = context.children[0].GetText();
            Visit(context.children[2]);

            ilGenerator.Emit(OpCodes.Stloc, variables[variable]);
            
            return null;
        }

        public override object VisitConstant([NotNull] HolyCParser.ConstantContext context)
        {
            // ilGenerator.Emit(OpCodes.st)
            if (int.TryParse(context.children[0].GetText(), out var intValue))
                ilGenerator.Emit(OpCodes.Ldc_I4, intValue);
            else
                ilGenerator.Emit(OpCodes.Ldstr, context.children[0].GetText());

            return base.VisitConstant(context);
        }

        public override object VisitString([NotNull] HolyCParser.StringContext context)
        {
            ilGenerator.Emit(OpCodes.Ldstr, context.children[0].GetText());

            // return base.VisitString(context);
            return null;
        }

        public override object VisitProto([NotNull] HolyCParser.ProtoContext context)
        {
            return base.VisitProto(context);
        }

        public override object VisitConstantExpression([NotNull] HolyCParser.ConstantExpressionContext context)
        {
            return base.VisitConstantExpression(context);
        }

        public override object VisitReturn([NotNull] HolyCParser.ReturnContext context)
        {
            Visit(context.children[1]);
            return null;
        }

        //public override object VisitExpression([NotNull] HolyCParser.ExpressionContext context)
        //{
        //    return base.VisitExpression(context);
        //}

        public override object VisitFunctionDefinition([NotNull] HolyCParser.FunctionDefinitionContext context)
        {
            var type = context.children[0].GetText();
            var name = context.children[1].GetChild(0).GetChild(0).GetText();
            var compound = context.children[2];

            var clrType = GetClrType(type);

            var method = new DynamicMethod(name, clrType, null, typeof(Runtime));
            variables = new Dictionary<string, LocalBuilder>();
            ilGenerator = method.GetILGenerator();

            Visit(compound);
            ilGenerator.Emit(OpCodes.Ret);

            Console.WriteLine("Function '{0}' -> {1}", name, type);

            methods.Add(name, method);
            return null;
        }

        private Type GetClrType(string type)
        {
            return type switch
            {
                "U0" => typeof(void),
                "I32" => typeof(int),
                _ => throw new NotSupportedException("Unsupported " + type)
            };
        }
    }
}
