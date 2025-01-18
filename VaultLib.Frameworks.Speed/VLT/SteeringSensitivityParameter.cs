using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(SteeringSensitivityParameter))]
    public class SteeringSensitivityParameter : VltBaseType
    {
        public eSteeringCurveStyle CurveStyle { get; set; }
        public float CurvePower { get; set; }
        public float CurveMultiplierLowSpeed { get; set; }
        public float CurveMultiplierHighSpeed { get; set; }
        public float InnerDeadZone { get; set; }
        public float OuterDeadZone { get; set; }
        public ushort NumberOfSteps { get; set; }

        public SteeringSensitivityParameter(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public SteeringSensitivityParameter(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
        
        public override void Read(VaultReadContext context, BinaryReader br)
        {
            CurveStyle = br.ReadEnum<eSteeringCurveStyle>();
            CurvePower = br.ReadSingle();
            CurveMultiplierLowSpeed = br.ReadSingle();
            CurveMultiplierHighSpeed = br.ReadSingle();
            InnerDeadZone = br.ReadSingle();
            OuterDeadZone = br.ReadSingle();
            NumberOfSteps = br.ReadUInt16();

            br.AlignReader(4);
        }

        public override void Write(VaultWriteContext context, BinaryWriter bw)
        {
            bw.WriteEnum(CurveStyle);
            bw.Write(CurvePower);
            bw.Write(CurveMultiplierLowSpeed);
            bw.Write(CurveMultiplierHighSpeed);
            bw.Write(InnerDeadZone);
            bw.Write(OuterDeadZone);
            bw.Write(NumberOfSteps);

            bw.AlignWriter(4);
        }
    }
}