// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 7:27 PM.

using System;
using System.Buffers.Binary;
using System.Text;

namespace VaultLib.Core.Hashing;

/// <summary>
/// C# implementation of "lookup8" by Bob Jenkins.
/// </summary>
/// <remarks>Ported from the original C: https://burtleburtle.net/bob/c/lookup8.c</remarks>
public static class Vlt64Hasher
{
    private static void Mix64(ref ulong a, ref ulong b, ref ulong c)
    {
        a -= b;
        a -= c;
        a ^= (c >> 43);
        b -= c;
        b -= a;
        b ^= (a << 9);
        c -= a;
        c -= b;
        c ^= (b >> 8);
        a -= b;
        a -= c;
        a ^= (c >> 38);
        b -= c;
        b -= a;
        b ^= (a << 23);
        c -= a;
        c -= b;
        c ^= (b >> 5);
        a -= b;
        a -= c;
        a ^= (c >> 35);
        b -= c;
        b -= a;
        b ^= (a << 49);
        c -= a;
        c -= b;
        c ^= (b >> 11);
        a -= b;
        a -= c;
        a ^= (c >> 12);
        b -= c;
        b -= a;
        b ^= (a << 18);
        c -= a;
        c -= b;
        c ^= (b >> 22);
    }

    /// <summary>
    /// Produces a 64-bit hash of a string using the lookup8 algorithm.
    /// </summary>
    /// <param name="str">The string to hash.</param>
    /// <param name="initVal">The seed for the hash.</param>
    /// <param name="returnZeroForEmpty">Whether to return a value of 0 for empty strings.</param>
    /// <returns>The 64-bit lookup8 hash of <paramref name="str"/>.</returns>
    public static ulong Hash(string str, ulong initVal = 0xABCDEF0011223344, bool returnZeroForEmpty = true)
    {
        if (string.IsNullOrEmpty(str) && (str == null || returnZeroForEmpty))
            return 0;

        var stringBytes = Encoding.UTF8.GetBytes(str);
        ulong len = (ulong)stringBytes.Length;
        ulong a = initVal;
        ulong b = initVal;
        ulong c = 0x9e3779b97f4a7c13; // golden ratio

        var byteSpan = stringBytes.AsSpan();

        // handle most of the key
        while (len >= 24)
        {
            a += BinaryPrimitives.ReadUInt64LittleEndian(byteSpan);
            b += BinaryPrimitives.ReadUInt64LittleEndian(byteSpan[8..]);
            c += BinaryPrimitives.ReadUInt64LittleEndian(byteSpan[16..]);
            Mix64(ref a, ref b, ref c);
            byteSpan = byteSpan[24..];
            len -= 24;
        }

        // handle the last 23 bytes
        c += (ulong)stringBytes.Length;

        switch (len)
        {
            case 23:
                c += (ulong)byteSpan[22] << 56;
                goto case 22;
            case 22:
                c += (ulong)byteSpan[21] << 48;
                goto case 21;
            case 21:
                c += (ulong)byteSpan[20] << 40;
                goto case 20;
            case 20:
                c += (ulong)byteSpan[19] << 32;
                goto case 19;
            case 19:
                c += (ulong)byteSpan[18] << 24;
                goto case 18;
            case 18:
                c += (ulong)byteSpan[17] << 16;
                goto case 17;
            case 17:
                c += (ulong)byteSpan[16] << 8;
                goto case 16;
            // the first byte of c is reserved for the length
            case 16:
                b += (ulong)byteSpan[15] << 56;
                goto case 15;
            case 15:
                b += (ulong)byteSpan[14] << 48;
                goto case 14;
            case 14:
                b += (ulong)byteSpan[13] << 40;
                goto case 13;
            case 13:
                b += (ulong)byteSpan[12] << 32;
                goto case 12;
            case 12:
                b += (ulong)byteSpan[11] << 24;
                goto case 11;
            case 11:
                b += (ulong)byteSpan[10] << 16;
                goto case 10;
            case 10:
                b += (ulong)byteSpan[9] << 8;
                goto case 9;
            case 9:
                b += (ulong)byteSpan[8] << 0;
                goto case 8;
            case 8:
                a += (ulong)byteSpan[7] << 56;
                goto case 7;
            case 7:
                a += (ulong)byteSpan[6] << 48;
                goto case 6;
            case 6:
                a += (ulong)byteSpan[5] << 40;
                goto case 5;
            case 5:
                a += (ulong)byteSpan[4] << 32;
                goto case 4;
            case 4:
                a += (ulong)byteSpan[3] << 24;
                goto case 3;
            case 3:
                a += (ulong)byteSpan[2] << 16;
                goto case 2;
            case 2:
                a += (ulong)byteSpan[1] << 8;
                goto case 1;
            case 1:
                a += byteSpan[0];
                goto case 0;
            case 0:
                break;
        }

        Mix64(ref a, ref b, ref c);

        return c;
    }
}