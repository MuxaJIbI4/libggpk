using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace LibDat.Data
{
    /// <summary>
    /// Represents a list of Int32 found in the resource section of a .dat file
    /// </summary>
    public class Int32List : AbstractDataList<Int32>
    {
        public Int32List(long offset, long dataTableOffset, int listLength, BinaryReader inStream)
            : base(offset, dataTableOffset, listLength, inStream) { }

        public override void ReadData(BinaryReader inStream)
        {
            while (inStream.BaseStream.Position < inStream.BaseStream.Length)
            {
                Int32 u = inStream.ReadInt32();
                this.Data.Add(u);
                break;
            }
        }
    }
}