using System;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.VinylsAttrib;

[VltTypeInfo("VinylsAttrib::DecalLayer")]
public class DecalLayer: VltBaseType<VaultLib.Core.DataInterfaces.Key32>
{
    public DecalLayer()
    {
        throw new NotImplementedException("VinylsAttrib::DecalLayer is not implemented");
    }

    public override void Read(VaultReadContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        //
    }

    public override void Write(VaultWriteContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        //
    }
}