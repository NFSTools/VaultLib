// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/27/2019 @ 4:47 PM.

using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib.Gen;

namespace VaultLib.Support.World.VLT.Attrib.Gen
{
    [VLTTypeInfo("Attrib::Gen::ClassRefSpec_coplights_flashpattern")]
    public class ClassRefSpec_coplights_flashpattern : ClassRefSpec_template
    {
        public ClassRefSpec_coplights_flashpattern(VLTClass @class, VLTClassField field, VLTCollection collection) : base(@class, field, collection, "coplights_flashpattern")
        {

        }

        public ClassRefSpec_coplights_flashpattern(VLTClass @class, VLTClassField field) : base(@class, field, "coplights_flashpattern")
        {

        }
    }
}