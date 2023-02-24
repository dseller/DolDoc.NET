using System;

namespace DolDoc.Centaur.Nodes
{
    public class AssignNode : ASTNode, IBytecodeEmitter
    {
        private readonly IBytecodeEmitter source, target;

        public AssignNode(IBytecodeEmitter source, IBytecodeEmitter target)
        {
            this.source = source;
            this.target = target;
        }

        public void Emit(FunctionCompilerContext ctx)
        {
            if (target is not IWrite write)
                throw new Exception();
            
            // if (target is StructFieldAstNode sfn)
            //     sfn.target.EmitRead(generator);
            // if (source is StructFieldAstNode sfn2)
            //     sfn2.target.EmitRead(generator);
            source.Emit(ctx);
            write.EmitWrite(ctx);
        }

        public Type Type(ICompilerContext ctx) => null;
    }
}