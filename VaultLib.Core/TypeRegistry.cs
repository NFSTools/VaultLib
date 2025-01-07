// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/23/2019 @ 8:51 PM.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
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

        private readonly Dictionary<Type, ObjectActivator<VLTBaseType>> _activators = new();

        /// <summary>
        ///     Initializes the type registry. Registers some default types.
        /// </summary>
        public TypeRegistry()
        {
            RegisterAssemblyTypes(Assembly.GetAssembly(typeof(TypeRegistry)));
        }

        /// <summary>
        ///     Registers a type with the type registry.
        /// </summary>
        /// <typeparam name="T">The actual type as defined in code.</typeparam>
        /// <param name="typeId">The text identifier for the type.</param>
        public void Register<T>(string typeId) where T : VLTBaseType
        {
            RegisterType(typeId, typeof(T));
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
                    !type.DescendsFrom(typeof(VLTBaseType)) && !type.IsEnum) continue;

                var typeInfoAttribute = type.GetCustomAttribute<VLTTypeInfoAttribute>();

                if (typeInfoAttribute == null)
                {
                    Debug.WriteLine("WARN: skipping registering type {0} because it doesn't have VLTTypeInfo",
                        new object[] { type.FullName });
                    continue;
                }

                var finalType = type.IsEnum ? typeof(VLTEnumType<>).MakeGenericType(type) : type;

                RegisterType(typeInfoAttribute.Name, finalType);
            }
        }

        /// <summary>
        ///     Creates the appropriate instance type for the given field.
        /// </summary>
        /// <remarks>Returns a <see cref="VLTArrayType" /> if the field is an array.</remarks>
        /// <param name="vltClass"></param>
        /// <param name="vltClassField"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        public VLTBaseType CreateInstance(VltClass vltClass, VltClassField vltClassField,
            VltCollection collection)
        {
            var type = ResolveType(vltClassField.TypeName);
            VLTBaseType instance;

            if (vltClassField.IsArray)
                instance = new VLTArrayType(vltClass, vltClassField, collection, type)
                    { ItemAlignment = vltClassField.Alignment };
            else
                instance = ConstructInstance(type, vltClass, vltClassField, collection);

            return instance;
        }

        public VLTBaseType ConstructInstance(Type type, VltClass vltClass, VltClassField vltClassField,
            VltCollection collection)
        {
            if (!_activators.TryGetValue(type, out var activator))
            {
                activator = ReflectionUtils.GetActivator<VLTBaseType>(type.GetConstructor(new[]
                {
                    typeof(VltClass), typeof(VltClassField), typeof(VltCollection)
                }));

                _activators[type] = activator;
            }

            return activator(vltClass, vltClassField, collection);
        }

        private void RegisterType(string typeId, Type type)
        {
            _typeDictionary[typeId] = type;
        }

        public Type ResolveType(string typeId)
        {
            if (_typeDictionary.TryGetValue(typeId, out var type))
                return type;

            throw new KeyNotFoundException($"Type '{typeId}' is not registered");
        }
    }
}