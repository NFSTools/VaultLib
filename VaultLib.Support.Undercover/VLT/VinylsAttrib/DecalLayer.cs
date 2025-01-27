using System;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.VinylsAttrib;

[VltTypeInfo("VinylsAttrib::DecalLayer")]
public class DecalLayer: VltBaseType<uint>
{
    public DecalLayer()
    {
        throw new NotImplementedException("VinylsAttrib::DecalLayer is not implemented");
    }

    public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        //
    }

    public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
        //
    }
}