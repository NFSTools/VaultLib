using System.IO;
using VaultLib.Core;
using VaultLib.Core.Exports;

namespace BurnoutConsole
{
    public class CustomExport : BaseExport
    {
        public override void Read(Vault vault, BinaryReader br)
        {
            //
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(0);
        }

        public override ulong GetExportID()
        {
            return 0x10011001;
        }

        public override string GetTypeId()
        {
            return "MyCustomExport";
        }
    }
}