using System;
using System.Collections.Generic;

namespace DesertImage.Extensions
{
    public static class HashCodeTypeTool
    {
        private static readonly Dictionary<Type, int> HashCodes = new Dictionary<Type, int>();

        public static int GetCachedHashCode<T>()
        {
            var type = typeof(T);

            if (HashCodes.TryGetValue(type, out var hash)) return hash;
            
            hash = typeof(T).GetHashCode();
                
            HashCodes.Add(type, hash);

            return hash;
        }

        public static int GetCachedHashCode<T>(this T instance)
        {
            var type = instance.GetType();
            
            if(HashCodes.ContainsKey(type)) return HashCodes[type];
            
            var hash = type.GetHashCode();

            HashCodes.Add(type, hash);

            return hash;
        }
    }
}