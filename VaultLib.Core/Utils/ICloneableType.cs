// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/05/2019 @ 5:50 PM.

using VaultLib.Core.Types;

namespace VaultLib.Core.Utils
{
    public interface ICloneableType
    {
        VLTBaseType CloneSelf();
    }
}