using System;
using System.IO;
using LibDat.Types;

namespace LibDat.Data
{
    public abstract class AbstractData
    {
        /// <summary>
        /// Offset in the dat file with respect to the beginning of the data section
        /// </summary>
        public int Offset { get; protected set; }

        /// <summary>
        /// Contains offset to value AbstractData in data section (only for pointer type fields).
        /// If AbstractData at <c>Offset</c> isn't PointerData then <c>>ValueOffset = Offset</c>
        /// </summary>
//        public int ValueOffset { get; protected set; }

        /// <summary>
        /// length of data (in bytes)
        /// </summary>
        public int Length { get; protected set; }

        public DataType Type { get; private set; }

        protected AbstractData(DataType type, int offset)
        {
            Offset = offset;
            Type = type;
        }

        /// <summary>
        /// Save this data to the specified stream. Stream position is not preserved.
        /// </summary>
        // <param name="outStream">Stream to write contents to</param>
        /// <returns>offset where data was writter</returns>
        public abstract int Save(BinaryWriter outStream);

        /// <summary>
        /// returns string representation of data this data section entry contain
        /// (in case it points to another data section entry it returns 
        /// result of <c>GetValueString()</c> call on that data
        /// </summary>
        /// <returns></returns>
        public abstract string GetValueString();

        [Obsolete]
        public new string ToString()
        {
            // TODO: remove later
            throw new NotImplementedException();
        }
    }
}