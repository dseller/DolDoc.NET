using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using DolDoc.Shared;

namespace DolDoc.Centaur
{
    public class LoggingILGenerator
    {
        private readonly ILogger logger;
        private readonly ILGenerator generator;

        public LoggingILGenerator(ILogger logger, ILGenerator generator)
        {
            this.logger = logger;
            this.generator = generator;
        }

        public LocalBuilder DeclareLocal(Type localType)
        {
            var res = generator.DeclareLocal(localType);
            logger.Debug(".decl {name} -> {idx}", localType.Name, res.LocalIndex);
            return res;
        }

        public void Emit(OpCode opcode)
        {
            generator.Emit(opcode);
            logger.Debug("{opcode}", opcode);
        }

        public void Emit(OpCode opcode, byte arg)
        {
            generator.Emit(opcode, arg);
            logger.Debug($"{opcode}\t{arg}");
        }

        public void Emit(OpCode opcode, sbyte arg)
        {
            generator.Emit(opcode, arg);
            logger.Debug($"{opcode}\t{arg}");
        }

        public void Emit(OpCode opcode, short arg)
        {
            generator.Emit(opcode, arg);
            logger.Debug("{opcode}\t{arg}", opcode, arg);
        }

        public void Emit(OpCode opcode, int arg)
        {
            generator.Emit(opcode, arg);
            logger.Debug("{opcode}\t{arg}", opcode, arg);
        }

        public void Emit(OpCode opcode, MethodInfo meth)
        {
            generator.Emit(opcode, meth);
            logger.Debug("{opcode}\t{name}", opcode, meth.Name);
        }

        public void EmitCall(OpCode opcode, MethodInfo methodInfo, Type[] optionalParameterTypes)
        {
            generator.EmitCall(opcode, methodInfo, optionalParameterTypes);
            logger.Debug($"{opcode}\t{methodInfo.Name}\t{string.Join(',', optionalParameterTypes.Select(p => p.ToString()))}");
        }

        public void Emit(OpCode opcode, SignatureHelper signature)
        {
            generator.Emit(opcode, signature);
            logger.Debug($"{opcode}\t{signature}");
        }

        public void Emit(OpCode opcode, ConstructorInfo con)
        {
            generator.Emit(opcode, con);
            logger.Debug($"{opcode}\t{con.DeclaringType} ({con})");
        }

        public void Emit(OpCode opcode, Type cls)
        {
            generator.Emit(opcode, cls);
            logger.Debug($"{opcode}\t{cls}");
        }

        public void Emit(OpCode opcode, long arg)
        {
            generator.Emit(opcode, arg);
            logger.Debug("{opcode}\t{arg}", opcode, arg);
        }

        public void Emit(OpCode opcode, float arg)
        {
            generator.Emit(opcode, arg);
            logger.Debug($"{opcode}\t{arg}");
        }

        public void Emit(OpCode opcode, double arg)
        {
            generator.Emit(opcode, arg);
            logger.Debug($"{opcode}\t{arg}");
        }

        public void Emit(OpCode opcode, Label label)
        {
            generator.Emit(opcode, label);
            logger.Debug($"{opcode}\t{label}");
        }

        public void Emit(OpCode opcode, Label[] labels)
        {
            generator.Emit(opcode, labels);
            logger.Debug($"{opcode}\t{labels}");
        }

        public virtual void Emit(OpCode opcode, FieldInfo field)
        {
            generator.Emit(opcode, field);
            logger.Debug("{opcode}\t{fieldName}", opcode, field.Name);
        }

        public void Emit(OpCode opcode, string str)
        {
            generator.Emit(opcode, str);
            logger.Debug($"{opcode}\t{{str}}", str);
        }

        public void Emit(OpCode opcode, LocalBuilder local)
        {
            generator.Emit(opcode, local);
            logger.Debug($"{opcode}\t{local}");
        }
    }
}