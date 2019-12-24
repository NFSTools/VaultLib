// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 7:27 PM.

using System.Globalization;
using System.Text;

namespace VaultLib.Core.Utils
{
    public static class VLT64Hasher
    {
        public static ulong Hash(string str, ulong init = 0xABCDEF0011223344, bool returnZeroForEmpty = true)
        {
            if (string.IsNullOrEmpty(str) && (str == null || returnZeroForEmpty))
                return 0;
            if (str.StartsWith("0x") && ulong.TryParse(str.Substring(2), NumberStyles.AllowHexSpecifier, CultureInfo.CurrentCulture, out ulong u))
                return u;
            byte[] data = Encoding.ASCII.GetBytes(str);
            var bytesProcessed = (uint)str.Length;
            var mixVar1 = (ulong)bytesProcessed;
            var manipulatedPrime = 0x9e3779b97f4a7c13;
            ulong mixVar2;
            var manipulatedInit = init;
            int dataPos = 0;

            if (mixVar1 < 0x18)
            {
                manipulatedPrime = 0x9e3779b97f4a7c13;
                manipulatedInit = init;
            }
            else
            {
                do
                {
                    var lVar5 = manipulatedInit + data[dataPos + 8] + ((ulong)data[dataPos + 0xf] << 0x38) +
                                  (ulong)data[dataPos + 0xb] * 0x1000000 + ((ulong)data[dataPos + 0xd] << 0x28) +
                                  (ulong)data[dataPos + 9] * 0x100 + (ulong)data[dataPos + 10] * 0x10000 +
                                  ((ulong)data[dataPos + 0xc] << 0x20) + ((ulong)data[dataPos + 0xe] << 0x30);
                    manipulatedPrime = manipulatedPrime + data[dataPos + 0x10] + ((ulong)data[dataPos + 0x17] << 0x38) +
                                    (ulong)data[dataPos + 0x13] * 0x1000000 + ((ulong)data[dataPos + 0x15] << 0x28) +
                                    (ulong)data[dataPos + 0x11] * 0x100 + (ulong)data[dataPos + 0x12] * 0x10000 +
                                    ((ulong)data[dataPos + 0x14] << 0x20) + ((ulong)data[dataPos + 0x16] << 0x30);
                    manipulatedInit = init + data[dataPos + 0] + ((ulong)data[dataPos + 7] << 0x38) +
                                      (ulong)data[dataPos + 3] * 0x1000000 + ((ulong)data[dataPos + 5] << 0x28) +
                                      (ulong)data[dataPos + 1] * 0x100 + (ulong)data[dataPos + 2] * 0x10000 +
                                      ((ulong)data[dataPos + 4] << 0x20) + ((ulong)data[dataPos + 6] << 0x30) - lVar5
                            - manipulatedPrime ^ manipulatedPrime >> 0x2b;
                    init = lVar5 - manipulatedPrime - manipulatedInit ^ manipulatedInit << 9;
                    mixVar2 = manipulatedPrime - manipulatedInit - init ^ init >> 8;
                    manipulatedInit = manipulatedInit - init - mixVar2 ^ mixVar2 >> 0x26;
                    init = init - mixVar2 - manipulatedInit ^ manipulatedInit << 0x17;
                    mixVar2 = mixVar2 - manipulatedInit - init ^ init >> 5;
                    manipulatedInit = manipulatedInit - init - mixVar2 ^ mixVar2 >> 0x23;
                    manipulatedPrime = init - mixVar2 - manipulatedInit ^ manipulatedInit << 0x31;
                    mixVar2 = mixVar2 - manipulatedInit - manipulatedPrime ^ manipulatedPrime >> 0xb;
                    init = manipulatedInit - manipulatedPrime - mixVar2 ^ mixVar2 >> 0xc;
                    manipulatedInit = manipulatedPrime - mixVar2 - init ^ init << 0x12;
                    manipulatedPrime = mixVar2 - init - manipulatedInit ^ manipulatedInit >> 0x16;
                    dataPos += 0x18;
                    bytesProcessed -= 0x18;
                } while (0x17 < bytesProcessed);
            }
            manipulatedPrime += mixVar1 & 0xffffffff;
            switch (bytesProcessed)
            {
                case 0x17:
                    manipulatedPrime += (ulong)data[dataPos + 0x16] << 0x38;
                    goto case 0x16;
                case 0x16:
                    manipulatedPrime += (ulong)data[dataPos + 0x15] << 0x30;
                    goto case 0x15;
                case 0x15:
                    manipulatedPrime += (ulong)data[dataPos + 0x14] << 0x28;
                    goto case 0x14;
                case 0x14:
                    manipulatedPrime += (ulong)data[dataPos + 0x13] << 0x20;
                    goto case 0x13;
                case 0x13:
                    manipulatedPrime += (ulong)data[dataPos + 0x12] * 0x1000000;
                    goto case 0x12;
                case 0x12:
                    manipulatedPrime += (ulong)data[dataPos + 0x11] * 0x10000;
                    goto case 0x11;
                case 0x11:
                    manipulatedPrime += (ulong)data[dataPos + 0x10] * 0x100;
                    goto case 0x10;
                case 0x10:
                    manipulatedInit += (ulong)data[dataPos + 0xf] << 0x38;
                    goto case 0xf;
                case 0xf:
                    manipulatedInit += (ulong)data[dataPos + 0xe] << 0x30;
                    goto case 0xe;
                case 0xe:
                    manipulatedInit += (ulong)data[dataPos + 0xd] << 0x28;
                    goto case 0xd;
                case 0xd:
                    manipulatedInit += (ulong)data[dataPos + 0xc] << 0x20;
                    goto case 0xc;
                case 0xc:
                    manipulatedInit += (ulong)data[dataPos + 0xb] * 0x1000000;
                    goto case 0xb;
                case 0xb:
                    manipulatedInit += (ulong)data[dataPos + 10] * 0x10000;
                    goto case 0xa;
                case 10:
                    manipulatedInit += (ulong)data[dataPos + 9] * 0x100;
                    goto case 0x9;
                case 9:
                    manipulatedInit += data[dataPos + 8];
                    goto case 0x8;
                case 8:
                    init += (ulong)data[dataPos + 7] << 0x38;
                    goto case 0x7;
                case 7:
                    init += (ulong)data[dataPos + 6] << 0x30;
                    goto case 0x6;
                case 6:
                    init += (ulong)data[dataPos + 5] << 0x28;
                    goto case 0x5;
                case 5:
                    init += (ulong)data[dataPos + 4] << 0x20;
                    goto case 0x4;
                case 4:
                    init += (ulong)data[dataPos + 3] * 0x1000000;
                    goto case 0x3;
                case 3:
                    init += (ulong)data[dataPos + 2] * 0x10000;
                    goto case 0x2;
                case 2:
                    init += (ulong)data[dataPos + 1] * 0x100;
                    goto case 0x1;
                case 1:
                    init += data[dataPos];
                    goto default;
                default:
                    mixVar1 = init - manipulatedInit - manipulatedPrime ^ manipulatedPrime >> 0x2b;
                    init = manipulatedInit - manipulatedPrime - mixVar1 ^ mixVar1 << 9;
                    mixVar2 = manipulatedPrime - mixVar1 - init ^ init >> 8;
                    manipulatedInit = mixVar1 - init - mixVar2 ^ mixVar2 >> 0x26;
                    mixVar1 = init - mixVar2 - manipulatedInit ^ manipulatedInit << 0x17;
                    mixVar2 = mixVar2 - manipulatedInit - mixVar1 ^ mixVar1 >> 5;
                    manipulatedInit = manipulatedInit - mixVar1 - mixVar2 ^ mixVar2 >> 0x23;
                    mixVar1 = mixVar1 - mixVar2 - manipulatedInit ^ manipulatedInit << 0x31;
                    mixVar2 = mixVar2 - manipulatedInit - mixVar1 ^ mixVar1 >> 0xb;
                    manipulatedInit = manipulatedInit - mixVar1 - mixVar2 ^ mixVar2 >> 0xc;
                    mixVar1 = mixVar1 - mixVar2 - manipulatedInit ^ manipulatedInit << 0x12;
                    return mixVar2 - manipulatedInit - mixVar1 ^ mixVar1 >> 0x16;
            }
        }
    }
}