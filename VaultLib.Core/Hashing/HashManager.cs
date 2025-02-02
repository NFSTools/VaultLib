// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/24/2019 @ 3:56 PM.

using System.Collections.Generic;
using System.IO;

namespace VaultLib.Core.Hashing;

public static class HashManager
{
    private static readonly Dictionary<uint, string> VltHashDictionary = new Dictionary<uint, string>();
    private static readonly Dictionary<ulong, string> Vlt64HashDictionary = new Dictionary<ulong, string>();

    public static void LoadDictionary(string file)
    {
        foreach (var line in File.ReadLines(file)) AddVlt(line);
    }

    public static void AddVlt(string str)
    {
        VltHashDictionary[Vlt32Hasher.Hash(str)] = str;
        Vlt64HashDictionary[Vlt64Hasher.Hash(str)] = str;
    }

    public static string? ResolveVlt(uint hash)
    {
        return VltHashDictionary.GetValueOrDefault(hash);
    }

    public static string? ResolveVlt(ulong hash)
    {
        return Vlt64HashDictionary.GetValueOrDefault(hash);
    }
}