// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/20/2019 @ 9:22 PM.

using System;

namespace VaultLib.Core.Types
{
    public class VLTTypeInfoAttribute : Attribute
    {
        public VLTTypeInfoAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}