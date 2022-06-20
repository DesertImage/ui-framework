using System;
using System.Reflection;

namespace DesertImage.Extensions
{
    public static class ReflectionExtensions
    {
        public static FieldInfo GetPrivateField(this Type type, string fieldName)
        {
            var field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);

            if (field != null) return field;

            var baseType = type.BaseType;

            return baseType == null ? null : GetPrivateField(baseType, fieldName);
        }

        public static FieldInfo GetPrivateField(this object obj, string fieldName)
        {
            return GetPrivateField(obj.GetType(), fieldName);
        }

        public static T GetPrivateFiled<T>(this object obj, string fieldName) where T : class
        {
            var fieldInfo = obj.GetPrivateField(fieldName);

            return fieldInfo.GetValue(obj) as T;
        }

        public static void SetPrivateFieldValue(this object obj, string fieldName, object value)
        {
            var field = GetPrivateField(obj, fieldName);

            if (field != null)
            {
                field.SetValue(obj, value);
                return;
            }

#if DEBUG
            UnityEngine.Debug.LogError($"[ReflectionExtensions] there is no field with name {fieldName}");
#endif
        }
    }
}