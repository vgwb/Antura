using System;

namespace Replacement
{
    [Serializable]
    public class ReplacementPair<T1, T2>
    {
        public T1 from;
        public T2 to;
    }
}
