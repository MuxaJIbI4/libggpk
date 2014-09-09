using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace LibDat.Data
{
    /// <summary>
    /// Represents a list of UInt64 found in the resource section of a .dat file
    /// </summary>
    public class UInt64List : AbstractDataList<UInt64>
    {
        public UInt64List(int offset, int dataTableOffset, int listLength, BinaryReader inStream)
            : base(offset, dataTableOffset, listLength, inStream) { }

        public override void ReadData(BinaryReader inStream)
        {
            while (inStream.BaseStream.Position < inStream.BaseStream.Length)
            {
                UInt64 u = inStream.ReadUInt64();
                Data.Add(u);
                break;
            }
        }
    }
}