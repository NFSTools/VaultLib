// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/28/2019 @ 3:50 PM.

using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib.Gen;

namespace VaultLib.Support.World.VLT.Attrib.Gen
{
    [VLTTypeInfo("Attrib::Gen::ClassRefSpec_timeofdaylighting")]
    public class ClassRefSpec_timeofdaylighting : ClassRefSpec_template
    {
        public ClassRefSpec_timeofdaylighting(VLTClass @class, VLTClassField field, VLTCollection collection) : base(@class, field, collection, "timeofdaylighting")
        {

        }

        public ClassRefSpec_timeofdaylighting(VLTClass @class, VLTClassField field) : base(@class, field, "timeofdaylighting")
        {

        }
    }
}