using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using CoreLibraries.GameUtilities;
using CoreLibraries.IO;

namespace VaultLib.Core.Chunks
{
    public class VLTDependencyChunk : ChunkBase
    {
        public VLTDependencyChunk(List<string> dependencyNames)
        {
            DependencyNames = dependencyNames;
            Debug.Assert(dependencyNames.Count == 2, "dependencyNames.Count == 2");
        }

        public VLTDependencyChunk() { }

        public List<string> DependencyNames { get; }

        public override void Read(Vault vault, BinaryReader br)
        {
            var dependencyCount = br.ReadUInt32();
            var dependencyHashes = new List<uint>();
            var dependencyNames = new List<string>();

            for (int i = 0; i < dependencyCount; i++)
            {
                dependencyHashes.Add(br.ReadUInt32());
            }

            for (int i = 0; i < dependencyCount; i++)
            {
                br.ReadUInt32();
            }

            for (int i = 0; i < dependencyCount; i++)
            {
                dependencyNames.Add(NullTerminatedString.Read(br));
            }

            //for (int i = 0; i < dependencyCount; i++)
            //{
            //    Debug.WriteLine("dependencies[{0}]: {1} (0x{2:X8})", i, dependencyNames[i], dependencyHashes[i]);
            //}
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(DependencyNames.Count);

            foreach (var dependencyName in DependencyNames)
            {
                bw.Write(VLT32Hasher.Hash(dependencyName));
            }

            Dictionary<string, int> nameOffsets = new Dictionary<string, int>();
            int nameOffset = 0;

            foreach (var dependencyName in DependencyNames)
            {
                nameOffsets[dependencyName] = nameOffset;
                nameOffset += dependencyName.Length + 1;
            }

            foreach (var dependencyName in DependencyNames)
            {
                bw.Write(nameOffsets[dependencyName]);
            }

            foreach (var dependencyName in DependencyNames)
            {
                NullTerminatedString.Write(bw, dependencyName);
            }

            bw.AlignWriter(0x10);
        }

        public override uint ID => 0x4465704E;
        public override uint Size { get; set; }
        public override long Offset { get; set; }
    }
}
