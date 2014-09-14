using System;
using System.IO;
using LibDat.Types;

namespace LibDat.Data
{
    /// <summary>
    /// Base class for all types of data that can be met in .dat file
    /// </summary>
    public abstract class AbstractData
    {
        /// <summary>
        /// Offset in the dat file with respect to the beginning of the data section
        /// </summary>
        public int Offset { get; protected set; }

        /// <summary>
        /// data length in bytes
        /// </summary>
        public int Length { get; protected set; }

        /// <summary>
        /// corresponding type of data
        /// </summary>
        public BaseDataType Type { get; private set; }

        protected AbstractData(BaseDataType type)
        {
            Type = type;
        }

        /// <summary>
        /// Save this data to the specified stream. Stream position is not preserved.
        /// </summary>
        // <param name="outStream">Stream to write contents to</param>
        /// <returns>offset where data was written</returns>
        public abstract int Save(BinaryWriter outStream);

        /// <summary>
        /// returns visual representation of data including recursively dereferenced pointer and list data 
        /// </summary>
        /// <returns></returns>
        public abstract string GetValueString();

        [Obsolete]
        public new string ToString()
        {
            // TODO: GetValueString() should be used insted
            // TODO: return ToString() after all bufs will be fixed or call GetValueString() from ToString()
            throw new NotImplementedException();
        }
    }
}