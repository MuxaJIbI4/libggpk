using System;
using System.IO;

namespace LibDat.Data
{
    /// <summary>
    /// Represents a list of Int32 found in the resource section of a .dat file
    /// </summary>
    public class ListInt32 : AbstractDataList<Int32>
    {
        public ListInt32(int offset, int dataTableOffset, int listLength, BinaryReader inStream)
            : base(offset, dataTableOffset, listLength, inStream) { }

        protected override void ReadData(BinaryReader inStream)
        {
            while (inStream.BaseStream.Position < inStream.BaseStream.Length)
            {
                var u = inStream.ReadInt32();
                Data.Add(u);
                break;
            }
        }
    }
}