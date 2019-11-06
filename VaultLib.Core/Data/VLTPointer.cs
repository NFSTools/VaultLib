// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/24/2019 @ 5:45 PM.

using System.Collections.Generic;

namespace VaultLib.Core.Data
{
    public enum VLTPointerType
    {
        Bin,
        Vlt
    }

    public class VLTPointer
    {
        public uint FixUpOffset { get; set; }

        public uint Destination { get; set; }

        public VLTPointerType Type { get; set; }

        public bool Tracked { get; set; }

        public override string ToString()
        {
            return $"{Type}+{FixUpOffset:X} -> {Destination:X}";
        }

        private sealed class FixUpOffsetDestinationTypeEqualityComparer : IEqualityComparer<VLTPointer>
        {
            public bool Equals(VLTPointer x, VLTPointer y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.FixUpOffset == y.FixUpOffset && x.Destination == y.Destination && x.Type == y.Type;
            }

            public int GetHashCode(VLTPointer obj)
            {
                unchecked
                {
                    var hashCode = (int) obj.FixUpOffset;
                    hashCode = (hashCode * 397) ^ (int) obj.Destination;
                    hashCode = (hashCode * 397) ^ (int) obj.Type;
                    return hashCode;
                }
            }
        }

        public static IEqualityComparer<VLTPointer> FixUpOffsetDestinationTypeComparer { get; } = new FixUpOffsetDestinationTypeEqualityComparer();
    }
}