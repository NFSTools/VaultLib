// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 5:21 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(FEPartCamera))]
    public class FEPartCamera : VltBaseType, IReferencesStrings
    {
        public string SlotName { get; set; } = string.Empty;
        public RefSpec Camera { get; set; } = new();

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            SlotName = context.ReadString(br);
            Camera.Read(context, fieldContext, br);
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            context.WriteString(SlotName, fieldContext, bw);
            Camera.Write(context, fieldContext, bw);
        }

        public void ReadPointerData(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
        }

        public void WritePointerData(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
        }

        public void AddPointers(VaultWriteContext context, FieldReadWriteContext fieldContext)
        {
        }

        public IEnumerable<string> GetStrings()
        {
            return new[] { SlotName };
        }
    }
}