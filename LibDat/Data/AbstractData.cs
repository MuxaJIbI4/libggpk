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
        /// Writes to <c>writer</c> pointer to itself
        /// </summary>
        /// <param name="writer"></param>
        public virtual void WritePointer(BinaryWriter writer)
        {
            writer.Write(Offset);
        }

        public virtual void WritePointerOffset(BinaryWriter writer, int NewOffset)
        {
            Offset = NewOffset;
            writer.Write(Offset);
        }

        /// <summary>
        /// returns visual representation of data 
        /// </summary>
        public abstract string GetValueString();
    }
}