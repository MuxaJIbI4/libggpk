using System.IO;

namespace LibDat.Data
{
    /// <summary>
    /// Represents unknown data  found in the data section of a .dat file. None of this is tested and is probably incorrect.
    /// </summary>
    public sealed class UnknownData : AbstractData
    {
        /// <summary>
        /// The unknown data
        /// </summary>
        private int Data { get; set; }

        public UnknownData(int offset, int dataTableOffset, BinaryReader inStream)
            : base(offset, dataTableOffset)
        {
            inStream.BaseStream.Seek(offset + dataTableOffset, SeekOrigin.Begin);
            ReadData(inStream);

            Length = 4;
        }

        protected override void ReadData(BinaryReader inStream)
        {
            Data = inStream.ReadInt32();
        }

        /// <summary>
        /// Saves the data to the specified stream.
        /// </summary>
        /// <param name="outStream"></param>
        public override void Save(BinaryWriter outStream)
        {
            outStream.Write(Data);
        }

        public override string GetValueString()
        {
            return Data.ToString();
        }
    }
}