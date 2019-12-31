using System;
using System.Collections.Generic;
using System.IO;
using LibDat.Types;

namespace LibDat.Data
{
    /// <summary>
    /// represents simple value data (end node of any tree of pointer or list data)
    /// </summary>
    public class ValueData64<T> : AbstractData
    {
        /// <summary>
        /// stored value
        /// </summary>
        public T Value { get; private set; }

        public ValueData64(BaseDataType type, BinaryReader reader, Dictionary<string, object> options)
            : base(type)
        {
            if (!options.ContainsKey("offset"))
                throw new Exception("Wrong parameters for reading ListData");

            // moving to start of value data
            Offset = (int)options["offset"];
            reader.BaseStream.Seek(DatContainer.DataSectionOffset + Offset, SeekOrigin.Begin);

            Value = reader.Read64<T>();
            Length = type.Width;
        }

        public override string GetValueString()
        {
            return Value.ToString();
        }
    }

    public class Int32Data64 : ValueData64<Int32>
    {
        public Int32Data64(BaseDataType type, BinaryReader reader, Dictionary<string, object> options)
            : base(type, reader, options) { }

        /// <summary>
        /// returns custom value for specific value of <c>Value</c> property
        /// </summary>
        public override string GetValueString()
        {
            // Int32 -16843010 : FEFE FEFE (hex)
            return Value == -16843010 ? "-1" : Value.ToString();
        }
    }

    public class Int64Data64 : ValueData64<Int64>
    {
        public Int64Data64(BaseDataType type, BinaryReader reader, Dictionary<string, object> options)
            : base(type, reader, options) { }

        /// <summary>
        /// returns custom value for specific value of <c>Value</c> property
        /// </summary>
        public override string GetValueString()
        {
            // Int64 -72340172838076674: FEFE FEFE FEFE FEFE (hex)
            return Value == -72340172838076674 ? "-1" : Value.ToString();
        }
    }
}