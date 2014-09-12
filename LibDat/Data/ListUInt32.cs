using System;
using System.IO;

namespace LibDat.Data
{
    /// <summary>
    /// Represents a list of UInt32 found in the resource section of a .dat file
    /// </summary>
    public class ListUInt32 : AbstractDataList<UInt32>
    {
        public ListUInt32(int offset, int dataTableOffset, int listLength, BinaryReader inStream)
            : base(offset, dataTableOffset, listLength, inStream) { }

        protected override void ReadData(BinaryReader inStream)
        {
            while (inStream.BaseStream.Position < inStream.BaseStream.Length)
            {
                var u = inStream.ReadUInt32();
                Data.Add(u);
                break;
            }
        }
    }
}