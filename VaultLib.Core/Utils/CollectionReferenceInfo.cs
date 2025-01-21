// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/13/2019 @ 10:24 AM.

using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Core.Utils;

public class CollectionReferenceInfo
{
    public CollectionReferenceInfo(VltBaseType source, VltCollection destination)
    {
        Source = source;
        Destination = destination;
    }

    public VltBaseType Source { get; }
    public VltCollection Destination { get; }
}