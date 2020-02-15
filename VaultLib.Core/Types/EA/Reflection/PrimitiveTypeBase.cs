// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/04/2019 @ 7:07 PM.

using System;
using VaultLib.Core.Data;

namespace VaultLib.Core.Types.EA.Reflection
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PrimitiveInfoAttribute : Attribute
    {
        public PrimitiveInfoAttribute(Type primitiveType)
        {
            PrimitiveType = primitiveType;
        }

        public Type PrimitiveType { get; }
    }

    public abstract class PrimitiveTypeBase : VLTBaseType
    {
        protected PrimitiveTypeBase()
        {
        }

        protected PrimitiveTypeBase(VltClass @class, VltClassField field, VltCollection collection) : base(@class,
            field, collection)
        {
        }

        protected PrimitiveTypeBase(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public abstract IConvertible GetValue();
        public abstract void SetValue(IConvertible value);
    }
}