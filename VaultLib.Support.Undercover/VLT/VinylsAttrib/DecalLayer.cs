using System;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.VinylsAttrib;

[VltTypeInfo("VinylsAttrib::DecalLayer")]
public class DecalLayer: VltBaseType<Core.DataInterfaces.Key32>
{
    public DecalLayer()
    {
        throw new NotImplementedException("VinylsAttrib::DecalLayer is not implemented");
    }

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context, FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        //
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context, FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        //
    }

    public override object Clone()
    {
        throw new NotImplementedException();
    }
}