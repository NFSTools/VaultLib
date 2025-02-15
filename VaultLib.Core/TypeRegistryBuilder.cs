using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using CoreLibraries.IO;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.DB;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib.Types;
using VaultLib.Core.Utils;
using BinaryExtensions = VaultLib.Core.Utils.BinaryExtensions;

namespace VaultLib.Core;

public class TypeRegistryBuilder<TKey> where TKey : struct, IKey<TKey>
{
    private readonly Dictionary<(TKey, TKey), Type> _fieldOverrides = new();
    private readonly Dictionary<TKey, Type> _typeDictionary = new();

    private readonly Dictionary<Type, ObjectActivator<object>> _activators = new();

    private readonly Dictionary<Type, TypeReader<TKey>>
        _readers = new();

    private readonly Dictionary<Type, TypeWriter<TKey>>
        _writers = new();

    public ByteOrder ByteOrder { get; }

    public TypeRegistryBuilder(ByteOrder byteOrder)
    {
        ByteOrder = byteOrder;

        RegisterAssemblyTypes(typeof(TypeRegistry<>).Assembly);

        RegisterPrimitive<bool>("EA::Reflection::Bool", r =>
            {
                var b = r.ReadByte();
                Debug.Assert(b is 0 or 1);
                return b == 1;
            },
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

        AddType("EA::Reflection::Text", typeof(string));
        _activators[typeof(string)] = _ => string.Empty;
        _readers[typeof(string)] = (_, ctx, _, br) => ctx.ReadString(br);
        _writers[typeof(string)] = (s, ctx, fieldCtx, bw) => ctx.WriteString((string)s, fieldCtx, bw);

        RegisterPrimitive("DUMMY_VltKey32", Key32.Read, (v, w) => v.Write(w));
        RegisterPrimitive("DUMMY_VltKey64", Key64.Read, (v, w) => v.Write(w));
        RegisterPrimitive("DUMMY_BinKey32", BinKey32.Read, (v, w) => v.Write(w));
        RegisterPrimitive("DUMMY_BinKey64", BinKey64.Read, (v, w) => v.Write(w));

        RegisterStruct<Vector2>("Attrib::Types::Vector2");
        RegisterStruct<Vector3>("Attrib::Types::Vector3");
        RegisterStruct<Vector4>("Attrib::Types::Vector4");
        RegisterStruct<Matrix>("Attrib::Types::Matrix");
    }

    public void Map<TDest>(string typeId)
    {
        var destType = typeof(TDest);
        if (!_activators.ContainsKey(destType))
            throw new KeyNotFoundException($"Type {destType} has not been registered");
        AddType(typeId, destType);
    }

    /// <summary>
    ///     Registers a type with the type registry.
    /// </summary>
    /// <typeparam name="T">The actual type as defined in code.</typeparam>
    /// <param name="typeId">The text identifier for the type.</param>
    public void Register<T>(string typeId) where T : VltBaseType<TKey>, new()
    {
        RegisterVltBaseType(typeId, typeof(T));
    }

    public void AddFieldOverride<T>(string className, string fieldName) where T : new()
    {
        AddFieldOverride<T>(TKey.FromString(className), TKey.FromString(fieldName));
    }

    public void AddFieldOverride<T>(TKey classKey, TKey fieldKey) where T : new()
    {
        AddFieldOverride(classKey, fieldKey, typeof(T));
    }

    public void RegisterStruct<T>(string typeId) where T : unmanaged
    {
        RegisterStruct(typeId, typeof(T));
    }

    public void RegisterPrimitive<T>(string typeId, Func<BinaryReader, T> reader,
        Action<T, BinaryWriter> writer)
        where T : unmanaged
    {
        var type = typeof(T);
        RegisterPrimitive(typeId, reader, writer, type);
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
                Debug.WriteLine("DEBUG: skipping registering type {0} because it doesn't have VLTTypeInfo",
                    new object[] { type.FullName });
                continue;
            }

            if (type.IsGenericType || type.IsAbstract || type.IsNested)
            {
                Debug.WriteLine("DEBUG: skipping registering type {0} because it's either generic, abstract, or nested",
                    new object[] { type.FullName });
                continue;
            }

            if (typeInfoAttribute.MappedTo != null)
            {
                AddType(typeInfoAttribute.Name, typeInfoAttribute.MappedTo);
            }
            else if (type.IsEnum)
            {
                AddType(typeInfoAttribute.Name, type);
                var defaultValue = Activator.CreateInstance(type);
                _activators[type] = _ => defaultValue;

                var reader = CreateEnumReader(type);
                var writer = CreateEnumWriter(type);

                _readers[type] = (_, _, _, r) => reader(r);
                _writers[type] = (instance, _, _, w) => writer(instance, w);
            }
            else if (type.IsValueType)
            {
                if (!IsUnmanagedType(type))
                    throw new Exception($"Can't register managed struct: {type}");
                RegisterStruct(typeInfoAttribute.Name, type);
            }
            else if (type.DescendsFrom(typeof(VltBaseType<TKey>)))
            {
                RegisterVltBaseType(typeInfoAttribute.Name, type);
            }
        }
    }

    public TypeRegistry<TKey> Build(Database<TKey> database)
    {
        return new TypeRegistry<TKey>(
            database,
            new ReadOnlyDictionary<(TKey, TKey), Type>(_fieldOverrides),
            new ReadOnlyDictionary<TKey, Type>(_typeDictionary),
            new ReadOnlyDictionary<Type, ObjectActivator<object>>(_activators),
            new ReadOnlyDictionary<Type, TypeReader<TKey>>(_readers),
            new ReadOnlyDictionary<Type, TypeWriter<TKey>>(_writers));
    }

    private void AddType(string typeName, Type type)
    {
        _typeDictionary[TKey.FromString(typeName)] = type;
    }

    private void AddFieldOverride(TKey classKey, TKey fieldKey, Type type)
    {
        _fieldOverrides[(classKey, fieldKey)] = type;
    }

    private void RegisterVltBaseType(string typeId, Type type)
    {
        var constructorInfo = type.GetConstructor(Type.EmptyTypes);
        if (constructorInfo == null)
            throw new MissingMethodException(
                $"Could not find zero-parameter constructor for type {type} (registered as {typeId})");

        AddType(typeId, type);
        _activators[type] = ReflectionUtils.GetActivator<object>(constructorInfo);

        _readers[type] = (instance, context, fieldContext, reader) =>
        {
            var vltBaseType = (VltBaseType<TKey>)instance;
            vltBaseType.Read(context, fieldContext, reader);
            return vltBaseType;
        };

        _writers[type] = (instance, context, fieldContext, writer) =>
        {
            var vltBaseType = (VltBaseType<TKey>)instance;
            vltBaseType.Write(context, fieldContext, writer);
        };
    }

    private void RegisterStruct(string typeId, Type type)
    {
        // if (ByteOrder == ByteOrder.Big)
        {
            if (!typeof(IComplexType).IsAssignableFrom(type))
            {
                throw new Exception(
                    $"Struct type {type} doesn't implement IComplexType, which is necessary for big-endian operations");
            }
        }

        if (!_typeDictionary.ContainsValue(type))
        {
            _activators[type] = _ => Activator.CreateInstance(type)!;
            var readerProxy = CreateStructReaderProxy(type);
            var writerProxy = CreateStructWriterProxy(type);

            if (ByteOrder == ByteOrder.Big)
            {
                _readers[type] = (init, context, fieldContext, br) =>
                {
                    var value = readerProxy(init, context, fieldContext, br);
                    ((IComplexType)value).EndianSwap();
                    return value;
                };

                _writers[type] = (value, context, fieldContext, writer) =>
                {
                    // StructWriter writes raw bytes, so we need to do an in-place change
                    ((IComplexType)value).EndianSwap();
                    writerProxy(value, context, fieldContext, writer);
                    // Client might want to use the data after writing it, so we need to be nice
                    // and undo the previous endian swap
                    ((IComplexType)value).EndianSwap();
                };
            }
            else
            {
                _readers[type] = readerProxy;
                _writers[type] = writerProxy;
            }
        }

        AddType(typeId, type);
    }

    private static TypeReader<TKey> CreateStructReaderProxy(Type structType)
    {
        var paramInitValue = Expression.Parameter(typeof(object), "init");
        var paramContext = Expression.Parameter(typeof(VaultReadContext<TKey>), "context");
        var paramFieldContext = Expression.Parameter(typeof(FieldReadWriteContext<TKey>), "fieldContext");
        var paramBinaryReader = Expression.Parameter(typeof(BinaryReader), "br");

        var body = Expression.Block(
            typeof(object),
            Expression.Convert(Expression.Call(
                    typeof(TypeRegistryBuilder<TKey>), nameof(StructReader), new[] { structType }, paramBinaryReader),
                typeof(object))
        );

        return Expression.Lambda<TypeReader<TKey>>(body, paramInitValue, paramContext, paramFieldContext,
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

    private static TypeWriter<TKey> CreateStructWriterProxy(Type structType)
    {
        var paramValue = Expression.Parameter(typeof(object), "value");
        var paramContext = Expression.Parameter(typeof(VaultWriteContext<TKey>), "context");
        var paramFieldContext = Expression.Parameter(typeof(FieldReadWriteContext<TKey>), "fieldContext");
        var paramBinaryWriter = Expression.Parameter(typeof(BinaryWriter), "bw");

        var body = Expression.Call(
            typeof(TypeRegistryBuilder<TKey>), nameof(StructWriter), new[] { structType },
            Expression.Convert(paramValue, structType), paramBinaryWriter);

        return Expression.Lambda<TypeWriter<TKey>>(body, paramValue, paramContext, paramFieldContext,
            paramBinaryWriter).Compile();
    }

    private static void StructWriter<T>(T value, BinaryWriter writer) where T : unmanaged
    {
        var size = Marshal.SizeOf<T>();
        Span<byte> bytes = stackalloc byte[size];
        MemoryMarshal.Write(bytes, ref value);
        writer.Write(bytes);
    }

    private void RegisterPrimitive<T>(string typeId, Func<BinaryReader, T> reader, Action<T, BinaryWriter> writer,
        Type type) where T : unmanaged
    {
        AddType(typeId, type);
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
}

internal delegate object TypeReader<TKey>(object init, VaultReadContext<TKey> context,
    FieldReadWriteContext<TKey> fieldContext,
    BinaryReader br) where TKey : struct, IKey<TKey>;

internal delegate void TypeWriter<TKey>(object value, VaultWriteContext<TKey> context,
    FieldReadWriteContext<TKey> fieldContext,
    BinaryWriter bw) where TKey : struct, IKey<TKey>;