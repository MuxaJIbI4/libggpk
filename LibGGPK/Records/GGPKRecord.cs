using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibGGPK.Records
{
    /// <summary>
    /// GGPK record is the very first record and exists at the very beginning of the pack file.
    /// It must have excatly 2 entries - One goes to the root directory and the other to a FREE record.
    /// </summary>
    public sealed class GgpkRecord : BaseRecord
    {
        public const string Tag = "GGPK";

        /// <summary>
        /// List record offsets this record contains. It must have exactly 2 entries.
        /// </summary>
        public long[] RecordOffsets;

        public GgpkRecord(uint length)
        {
            Length = length;
            RecordOffsets = new long[2];
        }

        public GgpkRecord(uint length, BinaryReader br)
        {
            RecordBegin = br.BaseStream.Position - 8;
            Length = length;
            Read(br);
        }

        public GgpkRecord(long recordBegin, uint length, long[] recordOffsets)
        {
            RecordBegin = recordBegin;
            Length = length;
            RecordOffsets = recordOffsets;
        }

        /// <summary>
        /// Reads the GGPK record entry from the specified stream
        /// </summary>
        /// <param name="br">Stream pointing at a GGPK record</param>
        public override void Read(BinaryReader br)
        {
            var totalRecordOffsets = br.ReadInt32();
            RecordOffsets = new long[totalRecordOffsets];

            for (var i = 0; i < totalRecordOffsets; i++)
            {
                RecordOffsets[i] = br.ReadInt64();
            }
        }

        public override void Write(BinaryWriter bw, Dictionary<long, long> changedOffsets)
        {
            bw.Write(Length);                           // 28
            bw.Write(Encoding.ASCII.GetBytes(Tag));     // GGPK
            bw.Write(RecordOffsets.Length);             // 2

            var offset = RecordOffsets[0];
            bw.Write(changedOffsets.ContainsKey(offset) ? changedOffsets[offset] : offset);
            offset = RecordOffsets[1];
            bw.Write(changedOffsets.ContainsKey(offset) ? changedOffsets[offset] : offset);
        }

        public override string ToString()
        {
            return Tag;
        }
    }

}
