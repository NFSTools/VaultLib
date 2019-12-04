// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/27/2019 @ 4:46 PM.

using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib.Gen;

namespace VaultLib.Support.World.VLT.Attrib.Gen
{
    [VLTTypeInfo("Attrib::Gen::ClassRefSpec_aud_world_reverb")]
    public class ClassRefSpec_aud_world_reverb : ClassRefSpec_template
    {
        public ClassRefSpec_aud_world_reverb(VLTClass @class, VLTClassField field, VLTCollection collection) : base(@class, field, collection, "aud_world_reverb")
        {

        }

        public ClassRefSpec_aud_world_reverb(VLTClass @class, VLTClassField field) : base(@class, field, "aud_world_reverb")
        {

        }
    }
}