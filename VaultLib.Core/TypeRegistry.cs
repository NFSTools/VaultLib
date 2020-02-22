// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/23/2019 @ 8:51 PM.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Core
{
    /// <summary>
    ///     Provides a facility for mapping type names to actual types.
    ///     Allows registration for specific games.
    /// </summary>
    public static class TypeRegistry
    {
        private static readonly Dictionary<string, Dictionary<string, Type>> TypeDictionary =
            new Dictionary<string, Dictionary<string, Type>>();

        private static readonly Dictionary<Type, ObjectActivator<VLTBaseType>> Activators =
            new Dictionary<Type, ObjectActivator<VLTBaseType>>();

        /// <summary>
        ///     Initializes the type registry. Registers some default types.
        /// </summary>
        static TypeRegistry()
        {
            RegisterAssemblyTypes(Assembly.GetAssembly(typeof(TypeRegistry)));
        }

        /// <summary>
        ///     Registers a type with the type registry.
        /// </summary>
        /// <typeparam name="T">The actual type as defined in code.</typeparam>
        /// <param name="typeId">The text identifier for the type.</param>
        /// <param name="gameIds">The list of games that this type should be registered for.</param>
        public static void Register<T>(string typeId, params string[] gameIds) where T : VLTBaseType
        {
            var games = gameIds.Length == 0
                ? new List<string>(new[] { "GLOBAL" })
                : new List<string>(gameIds);

            foreach (var game in games) Register<T>(game, typeId);
        }

        /// <summary>
        ///     Registers all defined types in the given assembly
        ///     to the given game IDs.
        /// </summary>
        /// <param name="assembly">The assembly to load types from.</param>
        /// <param name="gameIds">The game IDs to register types under.</param>
        public static void RegisterAssemblyTypes(Assembly assembly, params string[] gameIds)
        {
            var games = gameIds.Length == 0
                ? new List<string>(new[] { "GLOBAL" })
                : new List<string>(gameIds);

            Debug.WriteLine("RegisterAssemblyTypes({0})", new object[] { assembly.FullName });

            foreach (var type in assembly.GetTypes())
            {
                if (type.IsGenericType || type.IsAbstract || type.IsNested ||
                    !type.DescendsFrom(typeof(VLTBaseType)) && !type.IsEnum) continue;

                var typeInfoAttribute = type.GetCustomAttribute<VLTTypeInfoAttribute>();

                if (typeInfoAttribute == null)
                {
                    Debug.WriteLine("WARN: skipping registering type {0} because it doesn't have VLTTypeInfo",
                        new object[] { type.FullName });
                    continue;
                }

                foreach (var gameId in games)
                {
                    var finalType = type.IsEnum ? typeof(VLTEnumType<>).MakeGenericType(type) : type;

                    RegisterType(gameId, typeInfoAttribute.Name, finalType);
                }
            }
        }

        /// <summary>
        ///     Creates the appropriate instance type for the given field.
        /// </summary>
        /// <remarks>Returns a <see cref="VLTArrayType" /> if the field is an array.</remarks>
        /// <param name="gameId"></param>
        /// <param name="vltClass"></param>
        /// <param name="vltClassField"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static VLTBaseType CreateInstance(string gameId, VltClass vltClass, VltClassField vltClassField,
            VltCollection collection)
        {
            var type = ResolveType(gameId, vltClassField.TypeName);
            VLTBaseType instance;

            if (vltClassField.IsArray)
                instance = new VLTArrayType(vltClass, vltClassField, collection, type)
                { ItemAlignment = vltClassField.Alignment };
            else
                instance = ConstructInstance(type, vltClass, vltClassField, collection);

            return instance;
        }

        public static VLTBaseType ConstructInstance(Type type, VltClass vltClass, VltClassField vltClassField,
            VltCollection collection)
        {
            if (!Activators.TryGetValue(type, out var activator))
            {
                activator = ReflectionUtils.GetActivator<VLTBaseType>(type.GetConstructor(new[]
                {
                    typeof(VltClass), typeof(VltClassField), typeof(VltCollection)
                }));

                Activators[type] = activator;
            }

            return activator(vltClass, vltClassField, collection);
        }

        public static IReadOnlyDictionary<string, ReadOnlyDictionary<string, Type>> GetTypeDictionary()
        {
            return new ReadOnlyDictionary<string, ReadOnlyDictionary<string, Type>>(
                TypeDictionary.ToDictionary(v => v.Key, v => new ReadOnlyDictionary<string, Type>(v.Value)));
        }

        private static void Register<T>(string gameId, string typeId) where T : VLTBaseType
        {
            RegisterType(gameId, typeId, typeof(T));
        }

        private static void RegisterType(string gameId, string typeId, Type type)
        {
            if (!TypeDictionary.ContainsKey(gameId))
                TypeDictionary[gameId] = new Dictionary<string, Type>();
            TypeDictionary[gameId][typeId] = type;
        }

        public static Type ResolveType(string gameId, string typeId)
        {
            if (TypeDictionary.TryGetValue(gameId, out var typeDict)
                && typeDict.TryGetValue(typeId, out var type))
                return type;
            if (gameId != "GLOBAL")
                return ResolveType("GLOBAL", typeId);

            throw new KeyNotFoundException($"Type '{typeId}' is not registered for game {gameId}");
        }
    }
}