using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using DolDoc.Centaur.Symbols;
using DolDoc.Shared;

namespace DolDoc.Centaur
{
    public interface ICompilerContext : IDisposable
    {
        ILogger Log { get; }
        SymbolTable SymbolTable { get; }
        TypeBuilder CodeBuilder { get; }
        AssemblyBuilder AssemblyBuilder { get; }
        ModuleBuilder ModuleBuilder { get; }
        Assembly Assembly { get; }
        
        ICompilerContext BeginSubContext(string name);
        void LeaveSubContext();
    }

    public class RootCompilerContext : ICompilerContext
    {
        internal readonly Stack<ICompilerContext> compilerContexts;

        public RootCompilerContext(ILogger logger, SymbolTable symbolTable)
        {
            Log = logger;
            SymbolTable = symbolTable;
            compilerContexts = new Stack<ICompilerContext>();
            AssemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("CentaurDomain"), AssemblyBuilderAccess.RunAndCollect);
            ModuleBuilder = AssemblyBuilder.DefineDynamicModule("CentaurDomain");
        }
        
        public ILogger Log { get; }
        public SymbolTable SymbolTable { get; }
        public TypeBuilder CodeBuilder => compilerContexts.Peek()?.CodeBuilder;
        // public AssemblyBuilder AssemblyBuilder => compilerContexts.Peek()?.AssemblyBuilder;
        // public ModuleBuilder ModuleBuilder => compilerContexts.Peek()?.ModuleBuilder;
        public AssemblyBuilder AssemblyBuilder { get; }
        public ModuleBuilder ModuleBuilder { get; }
        public Assembly Assembly => compilerContexts.Peek()?.Assembly;
        
        public ICompilerContext BeginSubContext(string name)
        {
            var ctx = new NestedCompilerContext(name, Log, SymbolTable, this);
            Log.Info("Begin SubContext for {name}", name);
            compilerContexts.Push(ctx);
            return ctx;
        }

        public void LeaveSubContext()
        {
            Log.Info("Leave SubContext");
            compilerContexts.Pop();
        } 

        public void Dispose()
        {
        }
    }
    
    public class NestedCompilerContext : ICompilerContext
    {
        private readonly ICompilerContext root;

        public NestedCompilerContext(string name, ILogger logger, SymbolTable symbolTable, ICompilerContext root)
        {
            this.root = root;
            Log = logger;
            SymbolTable = symbolTable;
            // AssemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(name), AssemblyBuilderAccess.RunAndCollect);
            // ModuleBuilder = AssemblyBuilder.DefineDynamicModule(name);
            AssemblyBuilder = root.AssemblyBuilder;
            ModuleBuilder = root.ModuleBuilder;
            CodeBuilder = ModuleBuilder.DefineType($"gen_{name}");
        }
        
        public virtual ILogger Log { get; }
        
        public virtual SymbolTable SymbolTable { get; }
        
        public virtual TypeBuilder CodeBuilder { get; private set; }
        
        public virtual AssemblyBuilder AssemblyBuilder { get; private set; }
        
        public virtual ModuleBuilder ModuleBuilder { get; private set; }
        
        public virtual Assembly Assembly => AssemblyBuilder;
        
        public ICompilerContext BeginSubContext(string name) => root.BeginSubContext(name);
        
        public void LeaveSubContext() => root.LeaveSubContext();

        public void Dispose() => LeaveSubContext();
    }

    public class FunctionCompilerContext : ICompilerContext
    {
        private readonly ICompilerContext parent;
        private readonly MethodBuilder methodBuilder;

        public FunctionCompilerContext(ICompilerContext parent, MethodBuilder methodBuilder)
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

        public ICompilerContext BeginSubContext(string name) => parent.BeginSubContext(name);
        public void LeaveSubContext() => parent.LeaveSubContext();

        public void Dispose()
        {
        }
    }
}