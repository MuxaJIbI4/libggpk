using System.IO;

namespace LibDat.Data
{
    public abstract class AbstractData
    {
        /// <summary>
        /// Offset in the dat file with respect to the beginning of the data section
        /// </summary>
        public long Offset { get; protected set; }

        /// <summary>
        /// Offset of the data section in the .dat file (Starts with 0xbbbbbbbbbbbbbbbb)
        /// </summary>
        public long DataTableOffset {get; protected set; }

        protected AbstractData()
        {
        }

        protected AbstractData(long offset, long dataTableOffset)
        {
            DataTableOffset = dataTableOffset;
            Offset = offset;
        }

        /// <summary>
        /// Reads the data directly from the specified stream. 
        /// Stream position is not preserved and will be at the end of the data upon successful read.
        /// </summary>
        /// <param name="inStream">Stream containing the unicode string</param>
        public abstract void ReadData(BinaryReader inStream);

        /// <summary>
        /// Save this data to the specified stream. Stream position is not preserved.
        /// </summary>
        /// <param name="outStream">Stream to write contents to</param>
        public abstract void Save(BinaryWriter outStream);
    }
}