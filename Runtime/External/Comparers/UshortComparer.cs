using System.Collections.Generic;

namespace External
{
    public class UshortComparer : IEqualityComparer<ushort>
    {
        public bool Equals(ushort x, ushort y)
        {
            return x == y;
        }

        public int GetHashCode(ushort obj)
        {
            return obj;
        }
    }
}