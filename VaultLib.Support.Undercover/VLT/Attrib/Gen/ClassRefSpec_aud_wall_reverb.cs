// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/27/2019 @ 4:45 PM.

using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib.Gen;

namespace VaultLib.Support.Undercover.VLT.Attrib.Gen
{
    [VLTTypeInfo("Attrib::Gen::ClassRefSpec_aud_wall_reverb")]
    public class ClassRefSpec_aud_wall_reverb : ClassRefSpec_template
    {
        public ClassRefSpec_aud_wall_reverb(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection, "aud_wall_reverb")
        {
        }

        public ClassRefSpec_aud_wall_reverb(VltClass @class, VltClassField field) : base(@class, field, "aud_wall_reverb")
        {
        }
    }
}