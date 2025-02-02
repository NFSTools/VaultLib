// This file is part of VaultLib.Core by heyitsleo.
// 
// Created: 10/31/2019 @ 3:57 PM.

using System;
using System.IO;
using CoreLibraries.IO;

namespace VaultLib.Core;

public class VaultReadWrapper : IDisposable
{
    public VaultReadWrapper(string vaultName, Stream binStream, Stream vltStream,
        ByteOrder byteOrder = ByteOrder.Little)
    {
        VaultName = vaultName;
        ByteOrder = byteOrder;
            
        BinStream = binStream;
        VltStream = vltStream;
    }

    public string VaultName { get; }

    public Stream BinStream { get; }
    public Stream VltStream { get; }

    public ByteOrder ByteOrder { get; }

    public void Dispose()
    {
        BinStream.Dispose();
        VltStream.Dispose();
    }
}