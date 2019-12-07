// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/23/2019 @ 8:48 PM.

using System;

namespace VaultLib.Core.Data
{
    /// <summary>
    ///     A registered data type for VLT reading
    /// </summary>
    public class VLTDefinedType
    {
        /// <summary>
        ///     The actual type that should be instantiated.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        ///     The name of the type.
        /// </summary>
        public string Name { get; set; }
    }
}