using System.IO;

namespace LibDat.Data
{
    sealed class PointerData : AbstractData
    {
        /// <summary>
        /// The unknown data
        /// </summary>
        public int PointerOffset { get; private set; }

        public object Data { get; set; }

        public PointerData(int offset, int dataTableOffset, BinaryReader inStream)
            : base(offset, dataTableOffset)
        {
            inStream.BaseStream.Seek(offset + dataTableOffset, SeekOrigin.Begin);
            ReadData(inStream);
        }

        protected override void ReadData(BinaryReader inStream)
        {
            PointerOffset = inStream.ReadInt32();
        }

        public override void Save(BinaryWriter outStream)
        {
            // TODO write Data instead of offset to Data
            outStream.Write(PointerOffset);
        }

        public override string ToString()
        {
            return Data.ToString();
        }
    }
}
