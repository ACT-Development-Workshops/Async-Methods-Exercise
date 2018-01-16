using System;
using System.Collections.Generic;

namespace TutsUniversity.Infrastructure.Reflection
{
    public static class TypeExtensions
    {
        public static IEnumerable<Type> AllCustomClassesInHierarchy(this Type type)
        {
            var currentType = type;
            while (currentType != typeof(object))
            {
                yield return currentType;
                currentType = currentType.BaseType;
            }
        }
    }
}