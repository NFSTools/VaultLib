using System.IO;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Types.Attrib;

public abstract class BaseBlob<TKey> : VltBaseType<TKey>, IVltPointerObject<TKey> where TKey : struct, IKey<TKey>
{
    public byte[]? Data { get; set; }

    private int Length { get; set; }

    private uint _dataOffset;
    private long _dataPtrDst;

    private long _dataPtrSrc;

    public override void Read(VaultReadContext<TKey> context, FieldReadWriteContext<TKey> fieldContext, BinaryReader br)
    {
        Length = br.ReadInt32();

        if (Length < 0)
        {
            throw new InvalidDataException("Blob length cannot be less than 0");
        }

        _dataOffset = br.ReadPointer();
    }

    public override void Write(VaultWriteContext<TKey> context, FieldReadWriteContext<TKey> fieldContext,
        BinaryWriter bw)
    {
        if (Data != null)
        {
            PrepareData();
            bw.Write(GetDataLength());
        }
        else
        {
            bw.Write(0);
        }

        _dataPtrSrc = bw.BaseStream.Position;
        bw.Write(0);
    }

    public void ReadPointerData(VaultReadContext<TKey> context, FieldReadWriteContext<TKey> fieldContext, BinaryReader br)
    {
        if (_dataOffset != 0)
        {
            br.BaseStream.Position = _dataOffset;
            Data = ReadData(br);
        }
    }

    public void WritePointerData(VaultWriteContext<TKey> context, FieldReadWriteContext<TKey> fieldContext, BinaryWriter bw)
    {
        if (Data != null)
        {
            _dataPtrDst = bw.BaseStream.Position;
            WriteData(bw);
        }
    }

    public void AddPointers(VaultWriteContext<TKey> context, FieldReadWriteContext<TKey> fieldContext)
    {
        context.AddPointer(_dataPtrSrc, _dataPtrDst, false);
    }

    protected abstract void PrepareData();
    protected abstract int GetDataLength();

    protected abstract void WriteData(BinaryWriter bw);

    protected virtual byte[] ReadData(BinaryReader br)
    {
        byte[] bytes = br.ReadBytes(Length);

        if (bytes.Length != Length)
        {
            throw new InvalidDataException($"Expected {Length} bytes but got {bytes.Length}");
        }

        return bytes;
    }
}