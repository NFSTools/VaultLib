// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 11:56 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT.RenderReflect
{
    [VLTTypeInfo("RenderReflect::cDepthStencilStateAttribDefinition")]
    public class cDepthStencilStateAttribDefinition : VLTBaseType, IReferencesStrings
    {
        public string DebugName { get; set; }
        public byte[] Data { get; set; }
        private Text _debugNameText;

        public override void Read(Vault vault, BinaryReader br)
        {
            _debugNameText = new Text(Class, Field, Collection);
            _debugNameText.Read(vault, br);
            Data = br.ReadBytes(76);
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            _debugNameText.Write(vault, bw);
            bw.Write(Data);
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            _debugNameText.ReadPointerData(vault,br);
            DebugName = _debugNameText.Value;
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            _debugNameText.WritePointerData(vault,bw);
        }

        public void AddPointers(Vault vault)
        {
            _debugNameText.AddPointers(vault);
        }

        public IEnumerable<string> GetStrings()
        {
            return _debugNameText.GetStrings();
        }

        public cDepthStencilStateAttribDefinition(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public cDepthStencilStateAttribDefinition(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}