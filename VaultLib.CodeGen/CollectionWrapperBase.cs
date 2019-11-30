// This file is part of VaultCLI by heyitsleo.
// 
// Created: 11/27/2019 @ 11:10 AM.

using System;
using System.Collections.Generic;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Abstractions;
using VaultLib.Core.Types.EA.Reflection;

namespace VaultLib.CodeGen
{
    public abstract class CollectionWrapperBase
    {
        protected CollectionWrapperBase(VLTCollection collection)
        {
            Collection = collection;
        }

        protected VLTCollection Collection { get; }

        public bool HasField(string key)
        {
            VLTClassField field = this.Collection.Class.GetField(key);

            return Collection.DataRow.ContainsKey(field.Key);
        }

        protected T GetValue<T>(string key)
        {
            VLTClassField field = this.Collection.Class.GetField(key);

            if (Collection.DataRow.TryGetValue(field.Key, out VLTBaseType dataRowValue))
            {
                return convertItem<T>(dataRowValue);
            }

            return default(T);
        }

        protected List<T> GetArray<T>(string key)
        {
            VLTClassField field = this.Collection.Class.GetField(key);

            if (!field.IsArray)
                throw new ArgumentException($"field '{key}' is not an array");

            if (Collection.DataRow.TryGetValue(field.Key, out VLTBaseType dataRowValue))
            {
                VLTArrayType vltArray = (VLTArrayType) dataRowValue;
                List<T> list = new List<T>();

                foreach (var item in vltArray.Items)
                {
                    list.Add(convertItem<T>(item));
                }

                return list;
            }

            return null;
        }

        private T convertItem<T>(VLTBaseType item)
        {
            Type paramType = typeof(T);

            switch (true)
            {
                case true when paramType.IsPrimitive:
                    return (T)((PrimitiveTypeBase)item).GetValue();
                case true when paramType == typeof(string):
                    return (T)Convert.ChangeType(((Text)item).Value, typeof(T));
                case true when paramType == typeof(BaseRefSpec):
                    return createRefSpecWrapper<T>((BaseRefSpec) item);
                default:
                    return (T)Convert.ChangeType(item, typeof(T));
            }
        }

        private T createRefSpecWrapper<T>(BaseRefSpec item)
        {
            return (T) (object) new RefSpecWrapper(Collection.Class.Database.FindClass(item.ClassKey),
                Collection.Class.Database.RowManager.FindCollectionByName(item.ClassKey, item.CollectionKey));
        }
    }
}