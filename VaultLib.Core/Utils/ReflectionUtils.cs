// This file is part of VaultLib.Core by heyitsleo.
// 
// Created: 11/27/2019 @ 12:05 PM.

using System;

namespace VaultLib.Core.Utils
{
    public static class ReflectionUtils
    {
        public static bool DescendsFrom(this Type type, Type parentType)
        {
            Type tmpType = type;

            while (tmpType != null)
            {
                tmpType = tmpType.BaseType;

                if (tmpType == parentType)
                    return true;
            }

            return false;
        }
    }
}