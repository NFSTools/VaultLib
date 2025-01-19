// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/23/2019 @ 8:51 PM.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
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

        private delegate object TypeReader(object init, VaultReadContext context, FieldReadWriteContext fieldContext,
            BinaryReader br);

        private delegate void TypeWriter(object value, VaultWriteContext context, FieldReadWriteContext fieldContext,
            BinaryWriter bw);

        private readonly Dictionary<Type, TypeReader>
            _readers = new();

        private readonly Dictionary<Type, TypeWriter>
            _writers = new();

        /// <summary>
        ///     Initializes the type registry. Registers some default types.
        /// </summary>
        public TypeRegistry()
        {
            RegisterAssemblyTypes(Assembly.GetAssembly(typeof(TypeRegistry)));

            RegisterPrimitive<bool>("EA::Reflection::Bool", r => r.ReadByte() != 0,
                (v, w) => w.Write(v ? (byte)1 : (byte)0));
            RegisterPrimitive<sbyte>("EA::Reflection::Int8", r => r.ReadSByte(), (v, w) => w.Write(v));
            RegisterPrimitive<byte>("EA::Reflection::UInt8", r => r.ReadByte(), (v, w) => w.Write(v));
            RegisterPrimitive<short>("EA::Reflection::Int16", r => r.ReadInt16(), (v, w) => w.Write(v));
            RegisterPrimitive<ushort>("EA::Reflection::UInt16", r => r.ReadUInt16(), (v, w) => w.Write(v));
            RegisterPrimitive<int>("EA::Reflection::Int32", r => r.ReadInt32(), (v, w) => w.Write(v));
            RegisterPrimitive<uint>("EA::Reflection::UInt32", r => r.ReadUInt32(), (v, w) => w.Write(v));
            RegisterPrimitive<long>("EA::Reflection::Int64", r => r.ReadInt64(), (v, w) => w.Write(v));
            RegisterPrimitive<ulong>("EA::Reflection::UInt64", r => r.ReadUInt64(), (v, w) => w.Write(v));
            RegisterPrimitive<float>("EA::Reflection::Float", r => r.ReadSingle(), (v, w) => w.Write(v));

            _typeDictionary["EA::Reflection::Text"] = typeof(string);
            _activators[typeof(string)] = _ => null;
            _readers[typeof(string)] = (_, ctx, _, br) => ctx.ReadString(br);
            _writers[typeof(string)] = (s, ctx, fieldCtx, bw) => ctx.WriteString((string)s, fieldCtx, bw);
        }

        public void Map<TDest>(string typeId)
        {
            var destType = typeof(TDest);
            if (!_activators.ContainsKey(destType))
                throw new KeyNotFoundException($"Type {destType} has not been registered");
            _typeDictionary[typeId] = destType;
        }

        public bool IsConstructorRegistered<T>() => IsConstructorRegistered(typeof(T));

        public bool IsConstructorRegistered(Type type)
        {
            return _activators.ContainsKey(type);
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
            var constructorInfo = type.GetConstructor(Type.EmptyTypes);
            if (constructorInfo == null)
                throw new MissingMethodException(
                    $"Could not find zero-parameter constructor for type {type} (registered as {typeId})");

            _typeDictionary[typeId] = type;
            _activators[type] = ReflectionUtils.GetActivator<object>(constructorInfo);

            _readers[type] = (instance, context, fieldContext, reader) =>
            {
                var vltBaseType = (VltBaseType)instance;
                vltBaseType.Read(context, fieldContext, reader);
                return vltBaseType;
            };

            _writers[type] = (instance, context, fieldContext, writer) =>
            {
                var vltBaseType = (VltBaseType)instance;
                vltBaseType.Write(context, fieldContext, writer);
            };
        }

        public void RegisterStruct<T>(string typeId) where T : unmanaged
        {
            RegisterStruct(typeId, typeof(T));
        }

        private void RegisterStruct(string typeId, Type type)
        {
            if (!_typeDictionary.ContainsValue(type))
            {
                _activators[type] = _ => Activator.CreateInstance(type);
                _readers[type] = CreateStructReaderProxy(type);
                _writers[type] = CreateStructWriterProxy(type);
            }

            _typeDictionary[typeId] = type;
        }

        private static TypeReader CreateStructReaderProxy(Type structType)
        {
            var paramInitValue = Expression.Parameter(typeof(object), "init");
            var paramContext = Expression.Parameter(typeof(VaultReadContext), "context");
            var paramFieldContext = Expression.Parameter(typeof(FieldReadWriteContext), "fieldContext");
            var paramBinaryReader = Expression.Parameter(typeof(BinaryReader), "br");

            var body = Expression.Block(
                typeof(object),
                Expression.Convert(Expression.Call(
                        typeof(TypeRegistry), nameof(StructReader), new[] { structType }, paramBinaryReader),
                    typeof(object))
            );

            return Expression.Lambda<TypeReader>(body, paramInitValue, paramContext, paramFieldContext,
                paramBinaryReader).Compile();
        }

        private static T StructReader<T>(BinaryReader reader) where T : unmanaged
        {
            var type = typeof(T);
            var size = Marshal.SizeOf<T>();
            Span<byte> bytes = stackalloc byte[size];
            if (reader.Read(bytes) != size)
            {
                throw new EndOfStreamException($"Failed to read {size} bytes for unmanaged type {type}");
            }

            return MemoryMarshal.Read<T>(bytes);
        }

        private static TypeWriter CreateStructWriterProxy(Type structType)
        {
            var paramValue = Expression.Parameter(typeof(object), "value");
            var paramContext = Expression.Parameter(typeof(VaultWriteContext), "context");
            var paramFieldContext = Expression.Parameter(typeof(FieldReadWriteContext), "fieldContext");
            var paramBinaryWriter = Expression.Parameter(typeof(BinaryWriter), "bw");

            var body = Expression.Call(
                typeof(TypeRegistry), nameof(StructWriter), new[] { structType },
                Expression.Convert(paramValue, structType), paramBinaryWriter);

            return Expression.Lambda<TypeWriter>(body, paramValue, paramContext, paramFieldContext,
                paramBinaryWriter).Compile();
        }

        private static void StructWriter<T>(T value, BinaryWriter writer) where T : unmanaged
        {
            var size = Marshal.SizeOf<T>();
            Span<byte> bytes = stackalloc byte[size];
            MemoryMarshal.Write(bytes, ref value);
            writer.Write(bytes);
        }

        public void RegisterPrimitive<T>(string typeId, Func<BinaryReader, T> reader,
            Action<T, BinaryWriter> writer)
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
            _readers[type] = (_, _, _, r) => reader(r);
            _writers[type] = (instance, _, _, w) => writer((T)instance, w);
        }

        private static Func<BinaryReader, object> CreateEnumReader(Type enumType)
        {
            var underlyingType = Enum.GetUnderlyingType(enumType);

            if (underlyingType == typeof(uint))
                return r => Enum.ToObject(enumType, r.ReadUInt32());
            if (underlyingType == typeof(int))
                return r => Enum.ToObject(enumType, r.ReadInt32());
            if (underlyingType == typeof(ushort))
                return r => Enum.ToObject(enumType, r.ReadUInt16());
            if (underlyingType == typeof(short))
                return r => Enum.ToObject(enumType, r.ReadInt16());
            throw new InvalidOperationException($"Unsupported enum underlying type: {underlyingType.FullName}");
        }

        private static Action<object, BinaryWriter> CreateEnumWriter(Type enumType)
        {
            var underlyingType = Enum.GetUnderlyingType(enumType);

            if (underlyingType == typeof(uint))
                return (v, w) => w.Write((uint)v);
            if (underlyingType == typeof(int))
                return (v, w) => w.Write((int)v);
            if (underlyingType == typeof(ushort))
                return (v, w) => w.Write((ushort)v);
            if (underlyingType == typeof(short))
                return (v, w) => w.Write((short)v);
            throw new InvalidOperationException($"Unsupported enum underlying type: {underlyingType.FullName}");
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
                var typeInfoAttribute = type.GetCustomAttribute<VltTypeInfoAttribute>();

                if (typeInfoAttribute == null)
                {
                    Debug.WriteLine("WARN: skipping registering type {0} because it doesn't have VLTTypeInfo",
                        new object[] { type.FullName });
                    continue;
                }

                if (type.IsGenericType || type.IsAbstract || type.IsNested)
                {
                    continue;
                }

                if (typeInfoAttribute.MappedTo != null)
                {
                    _typeDictionary[typeInfoAttribute.Name] = typeInfoAttribute.MappedTo;
                }
                else if (type.IsEnum)
                {
                    _typeDictionary[typeInfoAttribute.Name] = type;
                    var defaultValue = Activator.CreateInstance(type);
                    _activators[type] = _ => defaultValue;

                    var reader = CreateEnumReader(type);
                    var writer = CreateEnumWriter(type);

                    _readers[type] = (_, _, _, r) => reader(r);
                    _writers[type] = (instance, _, _, w) => writer(instance, w);
                }
                else if (type.IsValueType && IsUnmanagedType(type))
                {
                    RegisterStruct(typeInfoAttribute.Name, type);
                }
                else if (type.DescendsFrom(typeof(VltBaseType)))
                {
                    RegisterVltBaseType(typeInfoAttribute.Name, type);
                }
            }
        }

        private static bool IsUnmanagedType(Type type)
        {
            /*
            A type is an unmanaged type if it's any of the following types:

sbyte, byte, short, ushort, int, uint, long, ulong, nint, nuint, char, float, double, decimal, or bool
Any enum type
Any pointer type
A tuple whose members are all of an unmanaged type
Any user-defined struct type that contains fields of unmanaged types only.
             */
            if (type.IsPrimitive || type.IsEnum || type.IsPointer)
            {
                return true;
            }

            if (type.IsGenericType || !type.IsValueType)
            {
                return false;
            }

            return type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .All(f => IsUnmanagedType(f.FieldType));
        }

        public object ConstructTypeInstance(Type type, VltClassField vltClassField)
        {
            Debug.Assert(type == ResolveType(vltClassField.TypeName));

            return _activators[type]();
        }

        public object ReadFieldValue(VaultReadContext readContext, FieldReadWriteContext fieldContext,
            BinaryReader binaryReader)
        {
            var vltClassField = fieldContext.Field;
            var type = ResolveType(vltClassField.TypeName);
            if (vltClassField.IsArray)
            {
                var array = new VltArrayType(vltClassField, type);
                array.Read(readContext, fieldContext, binaryReader);
                return array;
            }

            return ReadTypeInstance(readContext, fieldContext, binaryReader);
        }

        public object ReadTypeInstance(VaultReadContext readContext, FieldReadWriteContext fieldContext,
            BinaryReader binaryReader)
        {
            var vltClassField = fieldContext.Field;
            var type = ResolveType(vltClassField.TypeName);
            var init = ConstructTypeInstance(type, vltClassField);
            return _readers[type](init, readContext, fieldContext, binaryReader);
        }

        public void WriteFieldValue(object instance,
            VaultWriteContext writeContext, FieldReadWriteContext fieldContext, BinaryWriter binaryWriter)
        {
            var vltClassField = fieldContext.Field;
            if (vltClassField.IsArray)
            {
                var array = (VltArrayType)instance;
                array.Write(writeContext, fieldContext, binaryWriter);
            }
            else
            {
                WriteTypeInstance(vltClassField, instance, writeContext, fieldContext, binaryWriter);
            }
        }

        public void WriteTypeInstance(VltClassField vltClassField,
            object instance,
            VaultWriteContext writeContext, FieldReadWriteContext fieldContext, BinaryWriter binaryWriter)
        {
            var type = ResolveType(vltClassField.TypeName);

            _writers[type](instance, writeContext, fieldContext, binaryWriter);
        }

        public Type ResolveType(string typeId)
        {
            if (_typeDictionary.TryGetValue(typeId, out var type))
                return type;

            throw new KeyNotFoundException($"Type '{typeId}' is not registered");
        }
    }
}