using System.IO;

namespace LibGGPK.Records
{
    public abstract class BaseRecord
    {
        /// <summary>
        /// Length of the entire record in bytes
        /// </summary>
        protected uint Length { get; set; }

        /// <summary>
        /// Offset in pack file where record begins
        /// </summary>
        public long RecordBegin { get; protected set; }

        protected abstract void Read(BinaryReader br);
    }
}
