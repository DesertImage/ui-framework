using System.Collections;
using System.Collections.Generic;

namespace DesertImage.ECS
{
    public class MatchersComparer : IEqualityComparer<IMatcher>, IEqualityComparer
    {
        public bool Equals(IMatcher x, IMatcher y)
        {
            if (x == null || y == null) return false;

            return x.Equals(y);
        }

        public int GetHashCode(IMatcher obj)
        {
            return obj.GetHashCode();
        }

        public bool Equals(object x, object y)
        {
            if (!(x is IMatcher xMatcher)) return false;
            if (!(y is IMatcher yMatcher)) return false;

            return xMatcher.Equals(yMatcher);
        }

        public int GetHashCode(object obj)
        {
            return obj.GetHashCode();
        }
    }
}