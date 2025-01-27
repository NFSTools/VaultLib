// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/20/2019 @ 9:22 PM.

using System;

namespace VaultLib.Core.Types;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Struct)]
public class VltTypeInfoAttribute : Attribute
{
    public VltTypeInfoAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
        
    public Type MappedTo { get; set; }
}