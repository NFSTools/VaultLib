// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/27/2019 @ 4:48 PM.

using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib.Gen;

namespace VaultLib.Support.World.VLT.Attrib.Gen
{
    [VLTTypeInfo("Attrib::Gen::ClassRefSpec_nosfx")]
    public class ClassRefSpec_nosfx : ClassRefSpec_template
    {
        public ClassRefSpec_nosfx(VLTClass @class, VLTClassField field, VLTCollection collection) : base(@class, field, collection, "nosfx")
        {

        }

        public ClassRefSpec_nosfx(VLTClass @class, VLTClassField field) : base(@class, field, "nosfx")
        {

        }
    }
}