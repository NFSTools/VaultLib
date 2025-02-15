using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT;

[VltTypeInfo(nameof(FEQuickUpgrade))]
public class FEQuickUpgrade : VltBaseType<Core.DataInterfaces.Key32>, IReferencesStrings,
    IVltPointerObject<Key32>
{
    public float Cost { get; set; }
    public float Tier1_Cost { get; set; }
    public float Tier2_Cost { get; set; }
    public float Tier3_Cost { get; set; }
    public float Tier4_Cost { get; set; }
    public string OfferID { get; set; } = string.Empty;
    public List<FEQuickUpgradeEntry> Entries { get; set; }

    private byte _packageLength;
    private uint _ptrPackages;
    private long _ptrPackagesSrc;
    private long _ptrPackagesDst;

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        _ptrPackages = br.ReadUInt32();
        Cost = br.ReadSingle();
        Tier1_Cost = br.ReadSingle();
        Tier2_Cost = br.ReadSingle();
        Tier3_Cost = br.ReadSingle();
        Tier4_Cost = br.ReadSingle();
        OfferID = context.ReadString(br);
        _packageLength = br.ReadByte();
        br.SafeAlignReader(4);
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        _ptrPackagesSrc = bw.BaseStream.Position;
        bw.Write(0);
        bw.Write(Cost);
        bw.Write(Tier1_Cost);
        bw.Write(Tier2_Cost);
        bw.Write(Tier3_Cost);
        bw.Write(Tier4_Cost);
        context.WriteString(OfferID, fieldContext, bw);
        bw.Write((byte)Entries.Count);
        bw.AlignWriter(4);
    }

    public void ReadPointerData(VaultReadContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        br.BaseStream.Position = _ptrPackages;

        Entries = new List<FEQuickUpgradeEntry>();

        for (int i = 0; i < _packageLength; i++)
        {
            FEQuickUpgradeEntry entry = new FEQuickUpgradeEntry();
            entry.Read(context, fieldContext, br);

            Entries.Add(entry);
        }
    }

    public void WritePointerData(VaultWriteContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        _ptrPackagesDst = bw.BaseStream.Position;

        foreach (var entry in Entries)
        {
            entry.Write(context, fieldContext, bw);
        }
    }

    public void AddPointers(VaultWriteContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext)
    {
        Debug.Assert(_ptrPackagesSrc != 0 && _ptrPackagesDst != 0);
        context.AddPointer(_ptrPackagesSrc, _ptrPackagesDst, false);
    }

    public IEnumerable<string> GetStrings()
    {
        return new[] { OfferID };
    }

    public override object Clone()
    {
        return new FEQuickUpgrade
        {
            Cost = Cost,
            Tier1_Cost = Tier1_Cost,
            Tier2_Cost = Tier2_Cost,
            Tier3_Cost = Tier3_Cost,
            Tier4_Cost = Tier4_Cost,
            OfferID = OfferID,
            Entries = Entries.Select(o => (FEQuickUpgradeEntry)o.Clone()).ToList(),
        };
    }
}