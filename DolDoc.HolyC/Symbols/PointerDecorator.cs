//using HolyScript.Compiler.Symbols;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using WebAssembly;
//using WebAssembly.Instructions;

//namespace DolDoc.HolyC.Symbols
//{
//    internal class PointerDecorator : Symbol
//    {
//        private readonly Symbol inner;

//        internal PointerDecorator(Symbol inner)
//            : base(inner.Name, inner.Type, true)
//        {
//            this.inner = inner;
//        }

//        public override Instruction[] EmitLoad()
//        {


//            throw new NotImplementedException();
//        }

//        public override Instruction[] EmitStore()
//        {
//            var result = new List<Instruction>();
//            result.Add(new LocalSet(0));
//            result.AddRange(inner.EmitLoad());
//            result.Add(new Int32Store());
//            return result.ToArray();
//        }
            
            
//        //    => new[]
//        //{
//        //    new LocalSet(0),
//        //    inner.EmitLoad()
//        //}
//    }
//}
