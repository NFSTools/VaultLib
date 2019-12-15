using System.Diagnostics;
using System.IO;
using CoreLibraries.GameUtilities;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace TheRunConsole
{
    /// <summary>
    /// Maps a string to a 32-bit VLT hash.
    /// </summary>
    [VLTTypeInfo("fb::gp::StringHashMapper")]
    public class StringHashMapper : VLTBaseType, IPointerObject
    {
        private Text _strText;

        public string StringValue { get; set; }
        public uint StringHash { get; set; }

        public StringHashMapper(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public StringHashMapper(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public override void Read(Vault vault, BinaryReader br)
        {
            _strText = new Text(Class, Field, Collection);
            _strText.Read(vault, br);
            StringHash = br.ReadUInt32();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            _strText = new Text(Class, Field, Collection) { Value = StringValue };
            _strText.Write(vault, bw);
            bw.Write(VLT32Hasher.Hash(StringValue));
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            _strText.ReadPointerData(vault, br);
            StringValue = _strText.Value;

            //Debug.WriteLine("fb::gp::StringHashMapper: {0}/{1:X8}", StringValue, StringHash);
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            _strText.WritePointerData(vault, bw);
        }

        public void AddPointers(Vault vault)
        {
            _strText.AddPointers(vault);
        }
    }
}