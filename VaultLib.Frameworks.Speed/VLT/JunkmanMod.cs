// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 11:38 AM.

using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(JunkmanMod))]
    public struct JunkmanMod
    {
        public uint ClassKey;
        public uint DefinitionKey;
        public float ScaleF;
        public float ScaleR;
    }
}