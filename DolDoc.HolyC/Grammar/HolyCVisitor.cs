using Antlr4.Runtime.Misc;
using DolDoc.HolyC.Grammar;
using HolyScript.Compiler.Symbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebAssembly;
using WebAssembly.Instructions;

namespace HolyScript.Compiler
{
    public class HolyCVisitor : HolyCBaseVisitor<object>
    {
        private const int PrintFn = 0;
        private const int AssertFn = 1;
        private const int LogIntFn = 2;

        private readonly HolyCParser parser;
        private Stack<SymbolTable> symbolTables;
        private int dataOffset;
        private Dictionary<string, uint> methods;
        private FunctionBody functionBody, topLevelBody;
        private List<WebAssemblyValueType> parameters;

        public HolyCVisitor(HolyCParser parser)
        {
            methods = new Dictionary<string, uint>();
            topLevelBody = new FunctionBody();
            // Local #0 is accumulator local... 
            topLevelBody.Locals.Add(new Local() { Type = WebAssemblyValueType.Int32, Count = 1 });

            Module = new Module();
            Module.Imports = new List<Import>();
            Module.Imports.Add(new Import.Function("io", "log", PrintFn));
            Module.Imports.Add(new Import.Function("io", "assert", AssertFn));
            Module.Imports.Add(new Import.Function("io", "logInt", LogIntFn));
            Module.Imports.Add(new Import.Memory("core", "mem", new Memory(10, null)));

            // PrintFn
            Module.Types.Add(new WebAssemblyType()
            {
                Parameters = new List<WebAssemblyValueType> { WebAssemblyValueType.Int32 },
                Returns = new List<WebAssemblyValueType>()
            });

            // AssertFn
            Module.Types.Add(new WebAssemblyType
            {
                Parameters = new List<WebAssemblyValueType> { WebAssemblyValueType.Int32, WebAssemblyValueType.Int32 },
                Returns = new List<WebAssemblyValueType>()
            });

            // LogIntFn
            Module.Types.Add(new WebAssemblyType()
            {
                Parameters = new List<WebAssemblyValueType> { WebAssemblyValueType.Int32 },
                Returns = new List<WebAssemblyValueType>()
            });

            symbolTables = new Stack<SymbolTable>();
            GlobalSymbolTable = new SymbolTable();
            symbolTables.Push(GlobalSymbolTable);
            this.parser = parser;
        }

        public Module Module { get; }

        public SymbolTable GlobalSymbolTable { get; }

        public SymbolTable CurrentSymbolTable => symbolTables.Peek();

        private FunctionBody Body => functionBody ?? topLevelBody;

        public int Finalize()
        {
            if (!(topLevelBody.Code.Last() is Return))
            {
                // Add a return 0
                topLevelBody.Code.Add(new Int32Constant(0));
                topLevelBody.Code.Add(new Return());
            }

            topLevelBody.Code.Add(new End());

            Module.Types.Add(new WebAssemblyType
            {
                Parameters = new List<WebAssemblyValueType>(),
                Returns = new List<WebAssemblyValueType> { WebAssemblyValueType.Int32 }
            });

            Module.Codes.Add(topLevelBody);
            Module.Functions.Add(new Function((uint)(Module.Types.Count - 1)));
            Module.Exports.Add(new Export("___main", (uint)(Module.Types.Count - 1)));
            return Module.Exports.Count - 1;
        }

        //public override object VisitInclude([NotNull] HolyCParser.IncludeContext context)
        //{
        //    Console.WriteLine("including {0}", context.path.Text.Replace("\"", string.Empty));

        //    return base.VisitInclude(context);
        //}

        //public override object VisitInclude([NotNull] HolyCParser.IncludeContext context)
        //{
        //    Console.WriteLine("include: {0}", context.children[0].ToString());
        //    return base.VisitInclude(context);
        //}

        public override object VisitAssign([NotNull] HolyCParser.AssignContext context)
        {
            string name;
            bool isPtrAssignment = false;
            if (context.dst is HolyCParser.PointerExprContext ptrCtx)
            {
                name = ptrCtx.sym.GetText();
                isPtrAssignment = true;
            }
            else
                name = context.dst.GetText();
            var symbol = CurrentSymbolTable.Get(name);

            if (isPtrAssignment)
            {
                // Load the address on the stack
                Body.Code.Add(symbol.EmitLoad());
                Visit(context.src);
                Body.Code.Add(new Int32Store());
            }
            else
            {
                Visit(context.src);
                Body.Code.Add(symbol.EmitStore());
            }

            return null;
        }

        public override object VisitIdent([NotNull] HolyCParser.IdentContext context)
        {
            Console.WriteLine(context.ToStringTree(parser));
            var name = context.children[0].GetText();

            if (CurrentSymbolTable.Contains(name))
            {
                var sym = CurrentSymbolTable.Get(name);
                //if (sym.IsPointer)
                //{
                //    Body.Code.Add(sym.EmitLoad());
                //    Body.Code.Add(new Int32Load());
                //}
                //else
                    Body.Code.Add(sym.EmitLoad());
            }
            else if (methods.ContainsKey(name))
                Body.Code.Add(new Call(methods[name]));

            return null;
        }

        public override object VisitAdd([NotNull] HolyCParser.AddContext context)
        {
            Visit(context.children[0]);
            Visit(context.children[2]);
            // ilGenerator.Emit(OpCodes.Add);
            Body.Code.Add(new Int32Add());
            return null;
        }

        public override object VisitSub([NotNull] HolyCParser.SubContext context)
        {
            Visit(context.children[0]);
            Visit(context.children[2]);
            Body.Code.Add(new Int32Subtract());
            return null;
        }

        public override object VisitMul([NotNull] HolyCParser.MulContext context)
        {
            Visit(context.children[0]);
            Visit(context.children[2]);
            Body.Code.Add(new Int32Multiply());
            return null;
        }

        public override object VisitDiv([NotNull] HolyCParser.DivContext context)
        {
            Visit(context.children[0]);
            Visit(context.children[2]);
            Body.Code.Add(new Int32DivideSigned());
            return null;
        }

        public override object VisitShl([NotNull] HolyCParser.ShlContext context)
        {
            Visit(context.children[0]);
            Visit(context.children[2]);
            Body.Code.Add(new Int32ShiftLeft());
            return null;
        }

        public override object VisitShr([NotNull] HolyCParser.ShrContext context)
        {
            Visit(context.children[0]);
            Visit(context.children[2]);
            Body.Code.Add(new Int32ShiftRightSigned());
            return null;
        }

        public override object VisitEq([NotNull] HolyCParser.EqContext context)
        {
            Visit(context.children[0]);
            Visit(context.children[2]);
            Body.Code.Add(new Int32Equal());
            return null;
        }

        public override object VisitNeq([NotNull] HolyCParser.NeqContext context)
        {
            Visit(context.children[0]);
            Visit(context.children[2]);
            Body.Code.Add(new Int32NotEqual());
            return null;
        }

        public override object VisitLt([NotNull] HolyCParser.LtContext context)
        {
            Visit(context.children[0]);
            Visit(context.children[2]);
            Body.Code.Add(new Int32LessThanSigned());
            return null;
        }

        public override object VisitLte([NotNull] HolyCParser.LteContext context)
        {
            Visit(context.children[0]);
            Visit(context.children[2]);
            Body.Code.Add(new Int64LessThanOrEqualSigned());
            return null;
        }

        public override object VisitGt([NotNull] HolyCParser.GtContext context)
        {
            Visit(context.children[0]);
            Visit(context.children[2]);
            Body.Code.Add(new Int32GreaterThanSigned());
            return null;
        }

        public override object VisitGte([NotNull] HolyCParser.GteContext context)
        {
            Visit(context.children[0]);
            Visit(context.children[2]);
            Body.Code.Add(new Int32GreaterThanOrEqualSigned());
            return null;
        }

        public override object VisitAnd([NotNull] HolyCParser.AndContext context)
        {
            Visit(context.children[0]);
            Visit(context.children[2]);
            Body.Code.Add(new Int32And());
            return null;
        }

        private int WriteString(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            var data = new Data();
            data.InitializerExpression = new List<Instruction>()
            {
                new Int32Constant(dataOffset),
                new End()
            };
            //data.RawData = bytes;
            // data.RawData.AddRange(bytes);
            foreach (var b in bytes)
                data.RawData.Add(b);
            data.RawData.Add(0x00);

            Module.Data.Add(data);

            var res = dataOffset;
            dataOffset += bytes.Length + 1;
            return res;
        }

        public override object VisitPrintStatement([NotNull] HolyCParser.PrintStatementContext context)
        {
            var str = context.children[0].GetText();
            var offset = WriteString(str.Substring(1, str.Length - 2));

            Body.Code.Add(new Int32Constant(offset));
            Body.Code.Add(new Call(PrintFn));

            return null;
        }

        //public override object VisitDirectDeclarator([NotNull] HolyCParser.DirectDeclaratorContext context)
        //{
        //    return base.VisitDirectDeclarator(context);
        //}

        public override object VisitCall([NotNull] HolyCParser.CallContext context)
        {
            uint? index = null;
            var name = context.children[0].GetText();
            if (methods.TryGetValue(name, out var i))
                index = i;
            else
            {
                if (name == "Print")
                    index = PrintFn;
                else if (name == "Assert")
                {
                    index = AssertFn;
                    var offset = WriteString(context.children[2].GetText());
                    Body.Code.Add(new Int32Constant(offset));
                }
                else if (name == "LogInt")
                    index = LogIntFn;
            }

            if (context.ChildCount > 3)
                Visit(context.children[2]);

            if (index != null)
                Body.Code.Add(new Call(index.Value));
            return null;
        }

        //public override object VisitDereference([NotNull] HolyCParser.DereferenceContext context)
        //{
        //    Console.WriteLine(context.ToStringTree(parser));
        //    return base.VisitDereference(context);
        //}

        public override object VisitDeclaration([NotNull] HolyCParser.DeclarationContext context)
        {
            // string name = null;
            // Console.WriteLine(context.ToStringTree(parser));
            string type = context.ts.type.GetText();

            // Console.WriteLine(type);

            foreach (var declarator in context.declarators._declarators)
            {
                var name = declarator.d.dd.id?.Text ?? declarator.d.dd.dd.id?.Text;
                int arraySize;
                bool isArray;
                if (declarator.d.dd.size != null)
                {
                    isArray = true;
                    int.TryParse(declarator.d.dd.size.Text, out arraySize);
                }

                bool isPointer = declarator.d.ptr != null;

                // var type = context.children[0].GetChild(0).GetText();
                //if (context.ChildCount == 2)
                //    name = context.children[0].GetChild(1).GetText();
                //else if (context.ChildCount == 3)
                //    name = context.children[1].GetChild(0).GetChild(0).GetText();

                if (CurrentSymbolTable.Contains(name))
                    throw new ApplicationException($"Duplicate variable name {name}");

                if (functionBody == null)
                {
                    // Global scope
                    var g = new GlobalVariable(Module, name, Enum.Parse<SymbolType>(type), isPointer);
                    GlobalSymbolTable.Add(g);
                    Module.Globals.Add(g.AsGlobal);
                }
                else
                {
                    var v = new LocalVariable(Body, parameters.Count, name, Enum.Parse<SymbolType>(type), isPointer);
                    CurrentSymbolTable.Add(v);
                    Body.Locals.Add(v.AsLocal);
                }


                // var variable = ilGenerator.DeclareLocal(GetClrType(type));
                // variables.Add(name, variable);

                // Visit InitDeclarator
                Visit(declarator);
            }




            return null;
        }

        public override object VisitIf([NotNull] HolyCParser.IfContext context)
        {
            Visit(context.cond);

            //var iif = new If();
            Body.Code.Add(new If(BlockType.Int32));
            Visit(context.yes);
            if (context.no != null)
            {
                Body.Code.Add(new Else());
                Visit(context.no);
            }
            Body.Code.Add(new End());

            return null;
        }

        public override object VisitWhile([NotNull] HolyCParser.WhileContext context)
        {
            Visit(context.cond);
            Body.Code.Add(new If());
            Body.Code.Add(new Loop());
            Visit(context.st);
            Visit(context.cond);
            Body.Code.Add(new BranchIf(0));
            Body.Code.Add(new End());
            Body.Code.Add(new End());
            return null;
        }

        public override object VisitDefine([NotNull] HolyCParser.DefineContext context)
        {
            GlobalSymbolTable.Add(new Define(context.key.Text, int.Parse(context.value.Text)));
            return null;
        }

        public override object VisitParameterDeclaration([NotNull] HolyCParser.ParameterDeclarationContext context)
        {
            var type = context.children[0].GetText();
            var name = context.d.dd.id.Text;

            var parameter = new Parameter(parameters, name, Enum.Parse<SymbolType>(type), context.d.ptr != null);
            CurrentSymbolTable.Add(parameter);
            parameters.Add(GetWasmType(parameter.Type.ToString()).Value);

            return base.VisitParameterDeclaration(context);
        }

        //public override object VisitArgumentExpressionList([NotNull] HolyCParser.ArgumentExpressionListContext context)
        //{
        //    return base.VisitArgumentExpressionList(context);
        //}

        public override object VisitInitDeclarator([NotNull] HolyCParser.InitDeclaratorContext context)
        {
            if (context.i == null)
                return null;

            var variable = context.d.dd.id.Text;
            Visit(context.i);

            var sym = CurrentSymbolTable.Get(variable);
            Body.Code.Add(sym.EmitStore());

            return null;
        }

        public override object VisitInc([NotNull] HolyCParser.IncContext context)
        {
            var name = context.children[0].GetText();
            var sym = CurrentSymbolTable.Get(name);

            Body.Code.Add(sym.EmitLoad());
            Body.Code.Add(new Int32Constant(1));
            Body.Code.Add(new Int32Add());
            Body.Code.Add(sym.EmitStore());

            return null;
        }

        public override object VisitDec([NotNull] HolyCParser.DecContext context)
        {
            var name = context.children[0].GetText();
            var sym = CurrentSymbolTable.Get(name);

            Body.Code.Add(sym.EmitLoad());
            Body.Code.Add(new Int32Constant(1));
            Body.Code.Add(new Int32Subtract());
            Body.Code.Add(sym.EmitStore());

            return null;
        }

        public override object VisitConstant([NotNull] HolyCParser.ConstantContext context)
        {
            // ilGenerator.Emit(OpCodes.st)
            if (int.TryParse(context.children[0].GetText(), out var intValue))
                Body.Code.Add(new Int32Constant(intValue));
            else
            {
                var text = context.children[0].GetText();
                Body.Code.Add(new Int32Constant(WriteString(text)));
            }

            //else
            //    ilGenerator.Emit(OpCodes.Ldstr, context.children[0].GetText());

            // return base.VisitConstant(context);
            return null;
        }

        public override object VisitReturn([NotNull] HolyCParser.ReturnContext context)
        {
            base.VisitReturn(context);
            Body.Code.Add(new Return());
            //return base.;
            return null;
        }

        public override object VisitString([NotNull] HolyCParser.StringContext context)
        {
            var str = context.children[0].GetText();
            Body.Code.Add(new Int32Constant(WriteString(str.Substring(1, str.Length - 2))));
            return null;
        }

        public override object VisitFunctionDefinition([NotNull] HolyCParser.FunctionDefinitionContext context)
        {
            // Console.WriteLine(context.children[0].ToStringTree(parser));
            //Console.WriteLine(context.children[1].GetChild(0).GetChild(2).ToStringTree(parser));


            var type = context.children[0].GetText();
            var name = context.children[1].GetChild(0).GetChild(0).GetText();
            var compound = context.children[2];

            var wasmResultType = GetWasmType(type);


            functionBody = new FunctionBody();
            parameters = new List<WebAssemblyValueType>();
            Body.Code = new List<Instruction>();
            // Local #0 is accumulator local... 
            Body.Locals.Add(new Local() { Type = WebAssemblyValueType.Int32, Count = 1 });

            //if (context.children[1].ChildCount > 1)
            if (context.children[1].GetChild(0).ChildCount >= 3)
                Visit(context.children[1].GetChild(0).GetChild(2));
            Visit(compound);

            Module.Types.Add(new WebAssemblyType
            {
                Parameters = parameters,
                Returns = wasmResultType == null ? new List<WebAssemblyValueType>() : new List<WebAssemblyValueType> { wasmResultType.Value }
            });

            Module.Functions.Add(new Function((uint)(Module.Types.Count - 1)));
            Body.Code.Add(new End());

            Module.Codes.Add(functionBody);
            functionBody = null;

            Module.Exports.Add(new Export(name, (uint)(Module.Types.Count - 1)));

            Console.WriteLine("🚂 Function '{0}' -> {1}", name, type);

            methods.Add(name, (uint)(Module.Types.Count - 1));
            return null;
        }

        public override object VisitCompilationUnit([NotNull] HolyCParser.CompilationUnitContext context)
        {
            //Console.WriteLine(context.ToStringTree(parser));
            return base.VisitCompilationUnit(context);
        }

        public static WebAssemblyValueType? GetWasmType(string type)
        {
            return type switch
            {
                "U0" => null,
                "I8" => WebAssemblyValueType.Int32,
                "U8" => WebAssemblyValueType.Int32,
                // "U8*" => typeof(byte[]),
                "I16" => WebAssemblyValueType.Int32,
                "U16" => WebAssemblyValueType.Int32,
                "I32" => WebAssemblyValueType.Int32,
                "U32" => WebAssemblyValueType.Int32,
                "I64" => WebAssemblyValueType.Int64,
                "U64" => WebAssemblyValueType.Int64,
                "F64" => WebAssemblyValueType.Float64,
                // "Str" => typeof(string),
                _ => null
            };
        }
    }
}
