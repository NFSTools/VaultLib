// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 11:12 PM.

using System.Collections.Generic;

namespace VaultLib.Core.Utils
{
    public interface IReferencesStrings : IPointerObject
    {
        IEnumerable<string> GetStrings();
    }
}