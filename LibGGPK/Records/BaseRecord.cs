using System.Collections.Generic;
using System.IO;

namespace LibGGPK.Records
{
    public abstract class BaseRecord
    {
        /// <summary>
        /// Length of the entire record in bytes
        /// </summary>
        public uint Length { get; protected set; }

        /// <summary>
        /// Offset in pack file where record begins
        /// </summary>
        public long RecordBegin { get; protected set; }

        public abstract void Read(BinaryReader br);

        public abstract void Write(BinaryWriter bw, Dictionary<long, long> changedOffsets);
    }
}
