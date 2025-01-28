// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/13/2019 @ 10:24 AM.

using VaultLib.Core.Data;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;

namespace VaultLib.Core.Utils;

public class CollectionReferenceInfo<TKey> where TKey : struct, IKey<TKey>
{
    public CollectionReferenceInfo(VltBaseType<TKey> source, VltCollection<TKey> destination)
    {
        Source = source;
        Destination = destination;
    }

    public VltBaseType<TKey> Source { get; }
    public VltCollection<TKey> Destination { get; }
}