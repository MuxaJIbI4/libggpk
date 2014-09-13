using System;
using System.IO;
using LibDat.Types;

namespace LibDat.Data
{
    /// <summary>
    /// for C# value types
    /// </summary>
    public class ValueData<T> : AbstractData
    {
        /// <summary>
        /// stored value
        /// </summary>
        public T Value { get; set; }

        public ValueData(DataType type, int offset, BinaryReader reader)
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
        public Int32Data(DataType type, int offset, BinaryReader reader) : base(type, offset, reader)
        {
            // Int32 -16843010 : FEFE FEFE (hex)
            if (Value == -16843010) Value = -1;
        }
    }

    public class Int64Data : ValueData<Int64>
    {
        public Int64Data(DataType type, int offset, BinaryReader reader) : base(type, offset, reader)
        {
            // Int64 -72340172838076674: FEFE FEFE FEFE FEFE (hex)
            if (Value == -72340172838076674) Value = -1;
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

        public StringData(DataType type, int offset, BinaryReader inStream)
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
