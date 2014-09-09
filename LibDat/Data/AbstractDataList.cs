using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibDat.Data
{
    public interface IDataList  {}

    public abstract class AbstractDataList<T> : AbstractData, IDataList
    {
        /// <summary>
        /// Number of elements in the list
        /// </summary>
        public int ListLength { get; protected set; }

        /// <summary>
        /// list of objects
        /// </summary>
        public List<T> Data { get; protected set; }

        protected AbstractDataList(int offset, int dataTableOffset, int listLength, BinaryReader inStream)
            : base(offset, dataTableOffset)
        {
            Data = new List<T>(listLength);
            ListLength = listLength;
            if (listLength == 0) return;

            inStream.BaseStream.Seek(offset + dataTableOffset, SeekOrigin.Begin);
            for (int i = 0; i < listLength; ++i)
            {
                ReadData(inStream);
            }
        }

        public override string ToString()
        {
            if (Data.Count == 0) return "";
            StringBuilder sb = new StringBuilder();
            foreach (var s in Data)
            {
                sb.Append(s.ToString()).Append(" ");
            }
            return sb.Remove(sb.Length - 1, 1).ToString();
        }

        public override void Save(BinaryWriter outStream)
        {
            //TODO
        }
    }
}
