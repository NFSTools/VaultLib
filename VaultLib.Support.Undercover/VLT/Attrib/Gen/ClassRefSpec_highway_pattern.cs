// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/27/2019 @ 4:45 PM.

using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib.Gen;

namespace VaultLib.Support.Undercover.VLT.Attrib.Gen
{
    [VLTTypeInfo("Attrib::Gen::ClassRefSpec_highway_pattern")]
    public class ClassRefSpec_highway_pattern : ClassRefSpec_template
    {
        public ClassRefSpec_highway_pattern(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection, "highway_pattern")
        {
        }

        public ClassRefSpec_highway_pattern(VltClass @class, VltClassField field) : base(@class, field, "highway_pattern")
        {
        }
    }
}