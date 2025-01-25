// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/14/2019 @ 2:22 PM.

using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace VaultLib.Core.Utils;

public static class BinaryExtensions
{
    public static uint ReadPointer(this BinaryReader br, [CallerFilePath] string callerFilePath = "",
        [CallerLineNumber] int callerLineNumber = 0)
    {
        var position = br.BaseStream.Position;
        var pointer = br.ReadUInt32();
#if DEBUG
        if (pointer != 0)
        {
            // Debug.WriteLine(
            //     $"ReadPointer: {callerFilePath}:{callerLineNumber} - stream @ 0x{position:X} - ptr 0x{pointer:X}");
        }
#endif
        return pointer;
    }

    public static void SafeAlignReader(this BinaryReader br, int boundary)
    {
        if (br.BaseStream.Position % boundary == 0L)
            return;
        var pos = br.BaseStream.Position;
        var numBytes = boundary - br.BaseStream.Position % boundary;
#if DEBUG
        var bytes = new byte[numBytes];
        if (br.Read(bytes, 0, bytes.Length) != bytes.Length)
        {
            throw new EndOfStreamException();
        }

        for (var i = 0; i < numBytes; i++)
        {
            if (bytes[i] == 0) continue;
            var byteOffset = pos + i;
            throw new InvalidDataException($"Data detected in byte {i} of padding (offset: 0x{byteOffset:X})");
        }
#else
        br.BaseStream.Position += numBytes;
#endif
    }
}