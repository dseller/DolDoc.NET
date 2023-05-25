using System;

namespace DolDoc.Centaur.Internals
{
    public class CQue
    {
        public CQue next, last;
    }
    
    public class Collections
    {
        public static void QueInit(CQue type)
        {
            type.next = null;
            type.last = type;
        }

        public static void QueIns(CQue entry, CQue pred)
        {
            
        }
    }
}