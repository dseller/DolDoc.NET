using WebAssembly;
using WebAssembly.Instructions;

namespace HolyScript.Compiler.Symbols
{
    public class GlobalVariable : Symbol
    {
        public GlobalVariable(Module module, string name, SymbolType type, bool isPointer) : base(name, type, isPointer)
        {
            GlobalIdx = (uint)module.Globals.Count;
        }

        public uint GlobalIdx { get; }

        public Global AsGlobal => new Global
        {
            ContentType = HolyCVisitor.GetWasmType(Type.ToString()).Value,
            IsMutable = true,
            InitializerExpression = new Instruction[]
            {
                new Int32Constant(GlobalIdx * 8),
                new End()
            }
        };

        public override Instruction EmitLoad() => new GlobalGet(GlobalIdx);

        public override Instruction EmitStore() => new GlobalSet(GlobalIdx);
    }
}
