using System;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.VinylsAttrib;

[VltTypeInfo("VinylsAttrib::DecalLayer")]
public class DecalLayer : VltBaseType
{
    public DecalLayer()
    {
        throw new NotImplementedException("VinylsAttrib::DecalLayer is not implemented");
    }

    public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
    {
        //
    }

    public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
    {
        //
    }
}