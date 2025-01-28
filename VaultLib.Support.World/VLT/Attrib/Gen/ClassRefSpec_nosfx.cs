// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/27/2019 @ 4:48 PM.

using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib.Gen;

namespace VaultLib.Support.World.VLT.Attrib.Gen;

[VltTypeInfo("Attrib::Gen::ClassRefSpec_nosfx")]
public class ClassRefSpec_nosfx : ClassRefSpec_template32
{
    public ClassRefSpec_nosfx() : base("nosfx")
    {
    }
}