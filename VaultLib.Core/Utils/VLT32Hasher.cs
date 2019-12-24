using System.Globalization;

namespace VaultLib.Core.Utils
{
    public static class VLT32Hasher
    {
        public static uint Hash(string k, uint init = 0xABCDEF00, bool returnZeroForEmpty = true)
        {
            if (string.IsNullOrEmpty(k) && (k == null || returnZeroForEmpty))
                return 0;
            if (k.StartsWith("0x") && uint.TryParse(k.Substring(2), NumberStyles.AllowHexSpecifier, CultureInfo.CurrentCulture, out uint u))
                return u;

            int koffs = 0;
            int len = k.Length;
            uint a = 0x9e3779b9;
            uint b = a;
            uint c = init;

            while (len >= 12)
            {
                a += (uint)k[0 + koffs] + ((uint)k[1 + koffs] << 8) + ((uint)k[2 + koffs] << 16) + ((uint)k[3 + koffs] << 24);
                b += (uint)k[4 + koffs] + ((uint)k[5 + koffs] << 8) + ((uint)k[6 + koffs] << 16) + ((uint)k[7 + koffs] << 24);
                c += (uint)k[8 + koffs] + ((uint)k[9 + koffs] << 8) + ((uint)k[10 + koffs] << 16) + ((uint)k[11 + koffs] << 24);

                a -= b; a -= c; a ^= (c >> 13);
                b -= c; b -= a; b ^= (a << 8);
                c -= a; c -= b; c ^= (b >> 13);
                a -= b; a -= c; a ^= (c >> 12);
                b -= c; b -= a; b ^= (a << 16);
                c -= a; c -= b; c ^= (b >> 5);
                a -= b; a -= c; a ^= (c >> 3);
                b -= c; b -= a; b ^= (a << 10);
                c -= a; c -= b; c ^= (b >> 15);

                koffs += 12;
                len -= 12;
            }

            c += (uint)k.Length;

            switch (len)
            {
                case 11:
                    c += (uint)k[10 + koffs] << 24;
                    goto case 10;
                case 10:
                    c += (uint)k[9 + koffs] << 16;
                    goto case 9;
                case 9:
                    c += (uint)k[8 + koffs] << 8;
                    goto case 8;
                case 8:
                    b += (uint)k[7 + koffs] << 24;
                    goto case 7;
                case 7:
                    b += (uint)k[6 + koffs] << 16;
                    goto case 6;
                case 6:
                    b += (uint)k[5 + koffs] << 8;
                    goto case 5;
                case 5:
                    b += (uint)k[4 + koffs];
                    goto case 4;
                case 4:
                    a += (uint)k[3 + koffs] << 24;
                    goto case 3;
                case 3:
                    a += (uint)k[2 + koffs] << 16;
                    goto case 2;
                case 2:
                    a += (uint)k[1 + koffs] << 8;
                    goto case 1;
                case 1:
                    a += (uint)k[0 + koffs];
                    break;
            }

            a -= b; a -= c; a ^= (c >> 13);
            b -= c; b -= a; b ^= (a << 8);
            c -= a; c -= b; c ^= (b >> 13);
            a -= b; a -= c; a ^= (c >> 12);
            b -= c; b -= a; b ^= (a << 16);
            c -= a; c -= b; c ^= (b >> 5);
            a -= b; a -= c; a ^= (c >> 3);
            b -= c; b -= a; b ^= (a << 10);
            c -= a; c -= b; c ^= (b >> 15);

            return c;
        }
    }
}