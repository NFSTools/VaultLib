using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(FETuningSlider))]
    public class FETuningSlider : VltBaseType
    {
        public FETuningSlider(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public FETuningSlider(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public RefSpec Ref { get; set; }
        public uint TitleHash { get; set; }
        public uint LeftHash { get; set; }
        public uint RightHash { get; set; }
        public uint HelpHash { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            Ref = new RefSpec(Class, Field, Collection);
            Ref.Read(context, fieldContext, br);
            TitleHash = br.ReadUInt32();
            LeftHash = br.ReadUInt32();
            RightHash = br.ReadUInt32();
            HelpHash = br.ReadUInt32();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            Ref.Write(context, fieldContext, bw);
            bw.Write(TitleHash);
            bw.Write(LeftHash);
            bw.Write(RightHash);
            bw.Write(HelpHash);
        }
    }
}