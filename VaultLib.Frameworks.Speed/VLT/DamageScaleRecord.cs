// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 12:35 AM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(DamageScaleRecord))]
    public class DamageScaleRecord : VltBaseType
    {
        public float VisualScale { get; set; }
        public float HitPointScale { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            VisualScale = br.ReadSingle();
            HitPointScale = br.ReadSingle();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.Write(VisualScale);
            bw.Write(HitPointScale);
        }

        public DamageScaleRecord(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public DamageScaleRecord(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}