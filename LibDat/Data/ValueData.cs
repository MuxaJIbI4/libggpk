using System;
using System.IO;
using LibDat.Types;

namespace LibDat.Data
{
    /// <summary>
    /// represents simple value data (end node of any tree of pointer or list data)
    /// Extension methods BinaryWriter.Write<T>(object) and BinaryReader.Read<T>(object) are defined 
    /// in <c>RecordFactory</c> class.
    /// </summary>
    public class ValueData<T> : AbstractData
    {
        /// <summary>
        /// stored value
        /// </summary>
        public T Value { get; set; }

        public ValueData(BaseDataType type, int offset, BinaryReader reader)
            : base(type, offset)
        {
            Value = reader.Read<T>();
            Length = type.Width;
        }

        public override int Save(BinaryWriter writer)
        {
            var newOffset = (int)writer.BaseStream.Position;
            writer.Write<T>(Value);
            return newOffset;
        }

        public override string GetValueString()
        {
            return Value.ToString();
        }
    }

    public class Int32Data : ValueData<Int32>
    {
        public Int32Data(BaseDataType type, int offset, BinaryReader reader) : base(type, offset, reader) { }

        /// <summary>
        /// returns custom value for specific value of <c>Value</c> property
        /// </summary>
        public override string GetValueString()
        {
            // Int32 -16843010 : FEFE FEFE (hex)
            return Value == -16843010 ? "-1" : Value.ToString();
        }
    }

    public class Int64Data : ValueData<Int64>
    {
        public Int64Data(BaseDataType type, int offset, BinaryReader reader) : base(type, offset, reader) { }

        /// <summary>
        /// returns custom value for specific value of <c>Value</c> property
        /// </summary>
        public override string GetValueString()
        {
            // Int64 -72340172838076674: FEFE FEFE FEFE FEFE (hex)
            return Value == -72340172838076674 ? "-1" : Value.ToString();
        }
    }

    /// <summary>
    /// Represents a unicode string found in the data section of a .dat file
    /// </summary>
    public class StringData : ValueData<string>
    {
        /// <summary>
        /// The replacement string. If this is set then it will replace the original string when it's saved.
        /// </summary>
        public string NewValue { get; set; }

        public StringData(BaseDataType type, int offset, BinaryReader inStream)
            : base(type, offset, inStream)
        {
            NewValue = null;
            Length = 2 * Value.Length + 4;
        }

        /// <summary>
        /// Saves the unicode string to the specified stream. 
        /// If 'NewData' has been filled out then it will be written instead of the original data.
        /// </summary>
        /// <param name="outStream"></param>
        public override int Save(BinaryWriter outStream)
        {
            var newOffset = (int)outStream.BaseStream.Position;
            var dataToWrite = NewValue ?? Value;
            outStream.Write<string>(dataToWrite);
            return newOffset;
        }

        public override string GetValueString()
        {
            return NewValue ?? Value;
        }
    }
}
