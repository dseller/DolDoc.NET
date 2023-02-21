using System;

namespace DolDoc.Centaur.Nodes
{
    public class AssignNode : CodeNode
    {
        private readonly CodeNode source, target;

        public AssignNode(CodeNode source, CodeNode target)
        {
            this.source = source;
            this.target = target;
        }

        public override void Emit(LoggingILGenerator generator, SymbolTable symbolTable)
        {
            if (target is not IWrite write)
                throw new Exception();
            
            // if (target is StructFieldAstNode sfn)
            //     sfn.target.EmitRead(generator);
            // if (source is StructFieldAstNode sfn2)
            //     sfn2.target.EmitRead(generator);
            source.Emit(generator, symbolTable);
            write.EmitWrite(generator, symbolTable);
        }
    }
}