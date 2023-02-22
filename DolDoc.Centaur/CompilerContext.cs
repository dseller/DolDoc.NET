using System.Reflection;
using System.Reflection.Emit;
using DolDoc.Centaur.Symbols;
using DolDoc.Shared;

namespace DolDoc.Centaur
{
    public interface ICompilerContext
    {
        ILogger Log { get; }
        SymbolTable SymbolTable { get; }
        TypeBuilder CodeBuilder { get; }
        AssemblyBuilder AssemblyBuilder { get; }
        ModuleBuilder ModuleBuilder { get; }
        Assembly Assembly { get; }
    }

    public class CompilerContext : ICompilerContext
    {
        public CompilerContext(ILogger logger, SymbolTable symbolTable)
        {
            Log = logger;
            SymbolTable = symbolTable;
            AssemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("test"), AssemblyBuilderAccess.RunAndCollect);
            ModuleBuilder = AssemblyBuilder.DefineDynamicModule("test");
            CodeBuilder = ModuleBuilder.DefineType("gen_code");
        }
        
        public virtual ILogger Log { get; }
        
        public virtual SymbolTable SymbolTable { get; }
        
        public virtual TypeBuilder CodeBuilder { get; }
        
        public virtual AssemblyBuilder AssemblyBuilder { get; }
        
        public virtual ModuleBuilder ModuleBuilder { get; }
        
        public virtual Assembly Assembly => AssemblyBuilder;
    }

    public class FunctionCompilerContext : ICompilerContext
    {
        private readonly CompilerContext parent;
        private readonly MethodBuilder methodBuilder;

        public FunctionCompilerContext(CompilerContext parent, MethodBuilder methodBuilder)
        {
            this.parent = parent;
            this.methodBuilder = methodBuilder;
            Generator = new LoggingILGenerator(parent.Log, methodBuilder.GetILGenerator());
        }
        
        public LoggingILGenerator Generator { get; }

        public ILogger Log => parent.Log;

        public ModuleBuilder ModuleBuilder => parent.ModuleBuilder;

        public SymbolTable SymbolTable => parent.SymbolTable;

        public AssemblyBuilder AssemblyBuilder => parent.AssemblyBuilder;

        public Assembly Assembly => parent.Assembly;

        public TypeBuilder CodeBuilder => parent.CodeBuilder;
    }
}