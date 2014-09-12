using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LibDat.Data
{
    public interface IDataList  {}

    public abstract class AbstractDataList<T> : AbstractData, IDataList
    {
        /// <summary>
        /// Number of elements in the list
        /// </summary>
        public int ListLength { get; private set; }

        /// <summary>
        /// list of objects
        /// </summary>
        protected List<T> Data { get; private set; }

        protected AbstractDataList(int offset, int dataTableOffset, int listLength, BinaryReader inStream)
            : base(offset, dataTableOffset)
        {
            Data = new List<T>(listLength);
            ListLength = listLength;
            if (listLength == 0)
                listLength = 1; // ignore zero length integer array???

            inStream.BaseStream.Seek(offset + dataTableOffset, SeekOrigin.Begin);
            for (var i = 0; i < listLength; ++i)
            {
                ReadData(inStream);
            }
        }

        public override string GetValueString()
        {
            return String.Join(" ", Data.ToArray());
        }

        public override void Save(BinaryWriter outStream)
        {
            //TODO
            throw new NotImplementedException();
        }
    }
}
