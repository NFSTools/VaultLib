// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/27/2019 @ 4:47 PM.

using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib.Gen;

namespace VaultLib.Support.World.VLT.Attrib.Gen
{
    [VLTTypeInfo("Attrib::Gen::ClassRefSpec_emittergroup")]
    public class ClassRefSpec_emittergroup : ClassRefSpec_template
    {
        public ClassRefSpec_emittergroup(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection, "emittergroup")
        {

        }

        public ClassRefSpec_emittergroup(VltClass @class, VltClassField field) : base(@class, field, "emittergroup")
        {

        }
    }
}