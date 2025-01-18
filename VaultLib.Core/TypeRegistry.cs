// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/23/2019 @ 8:51 PM.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Core
{
    /// <summary>
    ///     Provides a facility for mapping type names to actual types.
    /// </summary>
    public class TypeRegistry
    {
        private readonly Dictionary<string, Type> _typeDictionary = new();

        private readonly Dictionary<Type, ObjectActivator<object>> _activators = new();

        private readonly Dictionary<Type, Func<object, VaultReadContext, BinaryReader, object>> _readers = new();
        private readonly Dictionary<Type, Action<object, VaultWriteContext, BinaryWriter>> _writers = new();

        /// <summary>
        ///     Initializes the type registry. Registers some default types.
        /// </summary>
        public TypeRegistry()
        {
            RegisterAssemblyTypes(Assembly.GetAssembly(typeof(TypeRegistry)));

            RegisterPrimitive<bool>("EA::Reflection::Bool", r => r.ReadByte() != 0, (v, w) => w.Write(v));
            RegisterPrimitive<sbyte>("EA::Reflection::Int8", r => r.ReadSByte(), (v, w) => w.Write(v));
            RegisterPrimitive<byte>("EA::Reflection::UInt8", r => r.ReadByte(), (v, w) => w.Write(v));
            RegisterPrimitive<short>("EA::Reflection::Int16", r => r.ReadInt16(), (v, w) => w.Write(v));
            RegisterPrimitive<ushort>("EA::Reflection::UInt16", r => r.ReadUInt16(), (v, w) => w.Write(v));
            RegisterPrimitive<int>("EA::Reflection::Int32", r => r.ReadInt32(), (v, w) => w.Write(v));
            RegisterPrimitive<uint>("EA::Reflection::UInt32", r => r.ReadUInt32(), (v, w) => w.Write(v));
            RegisterPrimitive<long>("EA::Reflection::Int64", r => r.ReadInt64(), (v, w) => w.Write(v));
            RegisterPrimitive<ulong>("EA::Reflection::UInt64", r => r.ReadUInt64(), (v, w) => w.Write(v));
            RegisterPrimitive<float>("EA::Reflection::Float", r => r.ReadSingle(), (v, w) => w.Write(v));
        }

        /// <summary>
        ///     Registers a type with the type registry.
        /// </summary>
        /// <typeparam name="T">The actual type as defined in code.</typeparam>
        /// <param name="typeId">The text identifier for the type.</param>
        public void Register<T>(string typeId) where T : VltBaseType
        {
            RegisterVltBaseType(typeId, typeof(T));
        }

        private void RegisterVltBaseType(string typeId, Type type)
        {
            _typeDictionary[typeId] = type;
            _activators[type] = ReflectionUtils.GetActivator<object>(type.GetConstructor(new[]
            {
                typeof(VltClass), typeof(VltClassField), typeof(VltCollection)
            }));

            _readers[type] = (instance, context, reader) =>
            {
                var vltBaseType = (VltBaseType)instance;
                vltBaseType.Read(context, reader);
                return vltBaseType;
            };

            _writers[type] = (instance, context, writer) =>
            {
                var vltBaseType = (VltBaseType)instance;
                vltBaseType.Write(context, writer);
            };
        }

        public void RegisterPrimitive<T>(string typeId, Func<BinaryReader, T> reader, Action<T, BinaryWriter> writer)
            where T : struct, IConvertible
        {
            var type = typeof(T);
            RegisterPrimitive(typeId, reader, writer, type);
        }

        private void RegisterPrimitive<T>(string typeId, Func<BinaryReader, T> reader, Action<T, BinaryWriter> writer,
            Type type) where T : struct, IConvertible
        {
            _typeDictionary[typeId] = type;
            _activators[type] = _ => default(T);
            _readers[type] = (_, _, r) => reader(r);
            _writers[type] = (instance, _, w) => writer((T)instance, w);
        }

        /// <summary>
        ///     Registers all defined types in the given assembly.
        /// </summary>
        /// <param name="assembly">The assembly to load types from.</param>
        public void RegisterAssemblyTypes(Assembly assembly)
        {
            Debug.WriteLine("RegisterAssemblyTypes({0})", new object[] { assembly.FullName });

            foreach (var type in assembly.GetTypes())
            {
                if (type.IsGenericType || type.IsAbstract || type.IsNested ||
                    !type.DescendsFrom(typeof(VltBaseType)) && !type.IsEnum) continue;

                var typeInfoAttribute = type.GetCustomAttribute<VltTypeInfoAttribute>();

                if (typeInfoAttribute == null)
                {
                    Debug.WriteLine("WARN: skipping registering type {0} because it doesn't have VLTTypeInfo",
                        new object[] { type.FullName });
                    continue;
                }

                if (typeInfoAttribute.MappedTo != null)
                {
                    _typeDictionary[typeInfoAttribute.Name] = typeInfoAttribute.MappedTo;
                }
                else
                {
                    var finalType = type.IsEnum ? typeof(VltEnumType<>).MakeGenericType(type) : type;

                    if (finalType.GetCustomAttribute<PrimitiveInfoAttribute>() != null && finalType != typeof(Text))
                    {
                        Debug.WriteLine("MIGRATION: skipping type {0} derived from PrimitiveTypeBase",
                            new object[] { type.FullName });
                        continue;
                    }

                    RegisterVltBaseType(typeInfoAttribute.Name, finalType);
                }
            }
        }

        /// <summary>
        ///     Creates the appropriate instance type for the given field.
        /// </summary>
        /// <remarks>Returns a <see cref="VltArrayType" /> if the field is an array.</remarks>
        /// <param name="vltClass"></param>
        /// <param name="vltClassField"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        public object ConstructFieldValue(VltClass vltClass, VltClassField vltClassField,
            VltCollection collection)
        {
            var type = ResolveType(vltClassField.TypeName);

            if (vltClassField.IsArray)
                return new VltArrayType(vltClass, vltClassField, collection, type)
                    { ItemAlignment = vltClassField.Alignment };
            return ConstructTypeInstance(type, vltClass, vltClassField, collection);
        }

        public object ConstructTypeInstance(Type type, VltClass vltClass, VltClassField vltClassField,
            VltCollection collection)
        {
            var activator = _activators[type];

            return activator(vltClass, vltClassField, collection);
        }

        public object ReadFieldValue(VltClass vltClass, VltClassField vltClassField, VltCollection collection,
            VaultReadContext readContext, BinaryReader binaryReader)
        {
            var type = ResolveType(vltClassField.TypeName);
            if (vltClassField.IsArray)
            {
                var array = new VltArrayType(vltClass, vltClassField, collection, type)
                    { ItemAlignment = vltClassField.Alignment };
                array.Read(readContext, binaryReader);
                return array;
            }

            return ReadTypeInstance(vltClass, vltClassField, collection, readContext, binaryReader);
        }

        public object ReadTypeInstance(VltClass vltClass, VltClassField vltClassField, VltCollection collection,
            VaultReadContext readContext, BinaryReader binaryReader)
        {
            var type = ResolveType(vltClassField.TypeName);
            var init = ConstructTypeInstance(type, vltClass, vltClassField, collection);
            return _readers[type](init, readContext, binaryReader);
        }

        public void WriteFieldValue(VltClassField vltClassField,
            object instance,
            VaultWriteContext writeContext, BinaryWriter binaryWriter)
        {
            if (vltClassField.IsArray)
            {
                var array = (VltArrayType)instance;
                array.Write(writeContext, binaryWriter);
            }
            else
            {
                WriteTypeInstance(vltClassField, instance, writeContext, binaryWriter);
            }
        }

        public void WriteTypeInstance(VltClassField vltClassField,
            object instance,
            VaultWriteContext writeContext, BinaryWriter binaryWriter)
        {
            var type = ResolveType(vltClassField.TypeName);

            _writers[type](instance, writeContext, binaryWriter);
        }

        public Type ResolveType(string typeId)
        {
            if (_typeDictionary.TryGetValue(typeId, out var type))
                return type;

            throw new KeyNotFoundException($"Type '{typeId}' is not registered");
        }
    }
}