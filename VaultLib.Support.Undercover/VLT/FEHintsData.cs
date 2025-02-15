// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/20/2019 @ 12:08 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT;

[VltTypeInfo(nameof(FEHintsData))]
public class FEHintsData: VltBaseType<Core.DataInterfaces.Key32>, IReferencesStrings
{
    public BinKey32 SubjectHALId { get; set; }
    public BinKey32 TextHALId { get; set; }
    public string Picture { get; set; } = string.Empty;

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context, FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        SubjectHALId = BinKey32.Read(br);
        TextHALId = BinKey32.Read(br);
        Picture = context.ReadString(br);
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context, FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        SubjectHALId.Write(bw);
        TextHALId.Write(bw);
        context.WriteString(Picture, fieldContext, bw);
    }

    public IEnumerable<string> GetStrings()
    {
        return new[] { Picture };
    }

    public override object Clone()
    {
        return MemberwiseClone();
    }
}