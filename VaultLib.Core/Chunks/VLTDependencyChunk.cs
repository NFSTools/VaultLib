using CoreLibraries.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace VaultLib.Core.Chunks
{
    public class VltDependencyChunk : ChunkBase
    {
        public VltDependencyChunk(List<string> dependencyNames)
        {
            DependencyNames = dependencyNames;
            Debug.Assert(dependencyNames.Count == 2, "dependencyNames.Count == 2");
        }

        public VltDependencyChunk()
        {
        }

        public List<string> DependencyNames { get; }

        public override uint Id => 0x4465704E;
        public override uint Size { get; set; }
        public override long Offset { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            //
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(DependencyNames.Count);

            if (vault.SaveContext.HashMode == VaultHashMode.Hash64)
                bw.Write(0);

            foreach (var dependencyName in DependencyNames) bw.Write(vault.SaveContext.StringHash(dependencyName));

            var nameOffsets = new Dictionary<string, int>();
            var nameOffset = 0;

            foreach (var dependencyName in DependencyNames)
            {
                nameOffsets[dependencyName] = nameOffset;
                nameOffset += dependencyName.Length + 1;
            }

            foreach (var dependencyName in DependencyNames) bw.Write(nameOffsets[dependencyName]);

            foreach (var dependencyName in DependencyNames) NullTerminatedString.Write(bw, dependencyName);

            bw.AlignWriter(0x10);
        }
    }
}