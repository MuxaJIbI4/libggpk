using System;
using System.IO;

namespace LibDat.Data
{
    public sealed class PointerData : AbstractData
    {
        /// <summary>
        /// contains offset to data section entry (Int32 data at this instance <c>Offset</c>)
        /// </summary>
        public int PointerOffset { get; private set; }

        public AbstractData Data { get; set; }

        public PointerData(int offset, int dataTableOffset, BinaryReader inStream)
            : base(offset, dataTableOffset)
        {
            Offset = offset;
            inStream.BaseStream.Seek(offset + dataTableOffset, SeekOrigin.Begin);
            ReadData(inStream);
            Length = 4;
        }

        protected override void ReadData(BinaryReader inStream)
        {
            PointerOffset = inStream.ReadInt32();
        }

        public override void Save(BinaryWriter outStream)
        {
            // TODO write Data instead of offset to Data
            // TODO: look for changed offset of data section entry which initially was at PointerOffset
            throw new NotImplementedException();
            outStream.Write(PointerOffset);
        }

        public override string GetValueString()
        {
            return Data == null ? "[Error: Pointed Data Not Initialized]" : Data.GetValueString();
        }
    }
}
