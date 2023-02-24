using System;
using System.Reflection.Emit;

namespace DolDoc.Centaur.Nodes
{
    public class MemberNode : ASTNode, IBytecodeEmitter, IWrite
    {
        private readonly IBytecodeEmitter target;
        private readonly string member;

        public MemberNode(IBytecodeEmitter target, string member)
        {
            this.target = target;
            this.member = member;
        }

        public void Emit(FunctionCompilerContext ctx)
        {
            target.Emit(ctx);
            var structType = target.Type(ctx);
            var field = structType.GetField(member);
            ctx.Generator.Emit(OpCodes.Ldfld, field);
        }

        public Type Type(ICompilerContext ctx)
        {
            var structType = target.Type(ctx);
            var field = structType.GetField(member);
            return field?.FieldType;
        }

        public void EmitWrite(FunctionCompilerContext ctx)
        {
            var structType = target.Type(ctx);
            var field = structType.GetField(member);
            
            var local = ctx.Generator.DeclareLocal(field.FieldType);
            ctx.Generator.Emit(OpCodes.Stloc, local);
            // ctx.Generator.Emit(OpCodes.Ldobj);
            target.Emit(ctx);
            ctx.Generator.Emit(OpCodes.Ldloc, local);
            ctx.Generator.Emit(OpCodes.Stfld, field);
        }
    }
}