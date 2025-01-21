// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/20/2019 @ 12:08 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT;

[VltTypeInfo(nameof(FEHintsData))]
public class FEHintsData : VltBaseType, IReferencesStrings
{
    public uint SubjectHALId { get; set; }
    public uint TextHALId { get; set; }
    public string Picture { get; set; } = string.Empty;

    public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
    {
        SubjectHALId = br.ReadUInt32();
        TextHALId = br.ReadUInt32();
        Picture = context.ReadString(br);
    }

    public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
    {
        bw.Write(SubjectHALId);
        bw.Write(TextHALId);
        context.WriteString(Picture, fieldContext, bw);
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
        return new[] { Picture };
    }
}