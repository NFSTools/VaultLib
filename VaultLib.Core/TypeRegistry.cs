// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/23/2019 @ 8:51 PM.

using CoreLibraries.GameUtilities;
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
    /// Provides a facility for mapping type names to actual types.
    /// Allows registration for specific games.
    /// </summary>
    public static class TypeRegistry
    {
        private static readonly Dictionary<string, Dictionary<string, Type>> TypeDictionary = new Dictionary<string, Dictionary<string, Type>>();
        private static readonly bool _initialized;

        private static readonly HashSet<string> UnknownTypes = new HashSet<string>();

        /// <summary>
        /// Initializes the type registry. Registers some default types.
        /// </summary>
        static TypeRegistry()
        {
            if (!_initialized)
            {
                RegisterAssemblyTypes(Assembly.GetAssembly(typeof(TypeRegistry)));

                _initialized = true;
            }
        }

        /// <summary>
        /// Registers a type with the type registry.
        /// </summary>
        /// <typeparam name="T">The actual type as defined in code.</typeparam>
        /// <param name="typeId">The text identifier for the type.</param>
        /// <param name="gameIds">The list of games that this type should be registered for.</param>
        public static void Register<T>(string typeId, params string[] gameIds) where T : VLTBaseType
        {
            List<string> games = gameIds.Length == 0
                ? new List<string>(GameIdHelper.GetIdList())
                : new List<string>(gameIds);

            foreach (var game in games)
            {
                Register<T>(game, typeId);
            }
        }

        /// <summary>
        /// Registers all defined types in the given assembly
        /// to the given game IDs.
        /// </summary>
        /// <param name="assembly">The assembly to load types from.</param>
        /// <param name="gameIds">The game IDs to register types under.</param>
        public static void RegisterAssemblyTypes(Assembly assembly, params string[] gameIds)
        {
            List<string> games = gameIds.Length == 0
                ? new List<string>(GameIdHelper.GetIdList())
                : new List<string>(gameIds);

            Debug.WriteLine("RegisterAssemblyTypes({0})", new object[] { assembly.FullName });

            foreach (var type in assembly.GetTypes())
            {
                if (type.IsGenericType || type.IsAbstract || type.IsNested ||
                    (!type.DescendsFrom(typeof(VLTBaseType)) && !type.IsEnum)) continue;

                VLTTypeInfoAttribute typeInfoAttribute = type.GetCustomAttribute<VLTTypeInfoAttribute>();

                if (typeInfoAttribute == null)
                {
                    Debug.WriteLine("WARN: skipping registering type {0} because it doesn't have VLTTypeInfo",
                        new object[] { type.FullName });
                    continue;
                }

                foreach (var gameId in games)
                {
                    Type finalType = type.IsEnum ? typeof(VLTEnumType<>).MakeGenericType(type) : type;

                    RegisterType(gameId, typeInfoAttribute.Name, finalType);
                }
            }
        }

        /// <summary>
        /// Creates the appropriate instance type for the given field.
        /// </summary>
        /// <remarks>Returns a <see cref="VLTArrayType"/> if the field is an array.</remarks>
        /// <param name="gameId"></param>
        /// <param name="vltClass"></param>
        /// <param name="vltClassField"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static VLTBaseType CreateInstance(string gameId, VLTClass vltClass, VLTClassField vltClassField,
            VLTCollection collection)
        {
            if (!_initialized)
            {
                throw new InvalidOperationException("TypeRegistry has not been initialized!");
            }

            Type type = ResolveType(gameId, vltClassField.TypeName);
            VLTBaseType instance;

            if (vltClassField.IsArray)
            {
                instance = new VLTArrayType { ItemType = type, ItemAlignment = vltClassField.Alignment };
            }
            else
            {
                instance = (VLTBaseType)Activator.CreateInstance(type);
            }

            if (instance is VLTUnknown unknown)
            {
                unknown.Size = vltClassField.Size;
            }

            instance.Collection = collection;
            instance.Class = vltClass;
            instance.Field = vltClassField;

            return instance;
        }

        /// <summary>
        /// For debugging purposes - prints the name of every UNRESOLVED type.
        /// </summary>
        public static void ListUnknownTypes()
        {
            Debug.WriteLine("Unknown types:");
            foreach (var type in UnknownTypes)
            {
                Debug.WriteLine("\t{0}", new object[] { type });
            }
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
            {
                return type;
            }

            UnknownTypes.Add(typeId);

            return typeof(VLTUnknown);
        }
    }
}