using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.ProStreet.VLT
{
    [VLTTypeInfo(nameof(CCarDamageLevels))]
    public class CCarDamageLevels : VLTBaseType
    {
        public CCarDamageLevels(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public CCarDamageLevels(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public float Speed0 { get; set; }
        public float Speed1 { get; set; }
        public float Speed2 { get; set; }
        public float Influence { get; set; }
        public float LightCrack { get; set; }
        public float LightShatter { get; set; }
        public float WindowCrack { get; set; }
        public float WindowShatter { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Speed0 = br.ReadSingle();
            Speed1 = br.ReadSingle();
            Speed2 = br.ReadSingle();
            Influence = br.ReadSingle();
            LightCrack = br.ReadSingle();
            LightShatter = br.ReadSingle();
            WindowCrack = br.ReadSingle();
            WindowShatter = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(Speed0);
            bw.Write(Speed1);
            bw.Write(Speed2);
            bw.Write(Influence);
            bw.Write(LightCrack);
            bw.Write(LightShatter);
            bw.Write(WindowCrack);
            bw.Write(WindowShatter);
        }
    }
}