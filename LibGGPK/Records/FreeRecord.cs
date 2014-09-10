using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LibGGPK.Records
{
    /// <summary>
    /// A free record represents space in the pack file that has been marked as deleted. It's much cheaper to just
    /// mark areas as free and append data to the end of the pack file than it is to rebuild the entire pack file just
    /// to remove a piece of data.
    /// </summary>
    public sealed class FreeRecord : BaseRecord
    {
        public const string Tag = "FREE";

        /// <summary>
        /// Offset of next FREE record
        /// </summary>
        public long NextFreeOffset;

        public FreeRecord(uint length, long recordBegin, long nextFreeOffset)
        {
            Length = length;
            RecordBegin = recordBegin;
            NextFreeOffset = nextFreeOffset;
        }

        public FreeRecord(uint length, BinaryReader br)
        {
            RecordBegin = br.BaseStream.Position - 8;
            Length = length;
            Read(br);
        }

        /// <summary>
        /// Reads the FREE record entry from the specified stream
        /// </summary>
        /// <param name="br">Stream pointing at a FREE record</param>
        public override void Read(BinaryReader br)
        {
            NextFreeOffset = br.ReadInt64();
            br.BaseStream.Seek(Length - 16, SeekOrigin.Current);
        }

        public override void Write(BinaryWriter bw, Dictionary<long, long> changedOffsets)
        {
            bw.Write(Length);                       // 16
            bw.Write(Encoding.ASCII.GetBytes(Tag)); // FREE
            bw.Write(NextFreeOffset);

            // IMPORTANT: ignore next FREE Record offset value
        }

        public override string ToString()
        {
            return Tag;
        }
    }
}
