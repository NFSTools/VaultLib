using System.Collections.Generic;
using System.Linq;
using VaultLib.Core.Types;

namespace VaultLib.Core.Utils;

public static class CloneHelper
{
    public static T[] CloneComplex<T>(this T[] array) where T : IComplexType
    {
        return array.Select(t => (T)t.Clone()).ToArray();
    }

    public static T[] CloneSimple<T>(this T[] array) where T : unmanaged
    {
        return (T[])array.Clone();
    }

    public static List<T> CloneComplex<T>(this List<T> list) where T : IComplexType
    {
        return list.Select(t => (T)t.Clone()).ToList();
    }
}