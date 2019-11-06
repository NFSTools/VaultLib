// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/04/2019 @ 8:17 PM.

using VaultLib.Core.Types;

namespace VaultLib.Core.Utils
{
    /// <summary>
    /// Workaround for the inability to use `is` with generic types
    /// </summary>
    public interface IDataArray
    {
        VLTBaseType[] GetItems();

        void SetItems(VLTBaseType[] items);
    }
}