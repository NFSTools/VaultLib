// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/27/2019 @ 4:48 PM.

using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib.Gen;

namespace VaultLib.Support.World.VLT.Attrib.Gen
{
    [VLTTypeInfo("Attrib::Gen::ClassRefSpec_fe_flashers")]
    public class ClassRefSpec_fe_flashers : ClassRefSpec_template
    {
        public ClassRefSpec_fe_flashers(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection, "fe_flashers")
        {

        }

        public ClassRefSpec_fe_flashers(VltClass @class, VltClassField field) : base(@class, field, "fe_flashers")
        {

        }
    }
}