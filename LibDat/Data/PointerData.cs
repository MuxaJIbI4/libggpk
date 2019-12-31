using System;
using System.Collections.Generic;
using System.IO;
using LibDat.Types;

namespace LibDat.Data
{
    /// <summary>
    /// Value f
    /// </summary>
    public class PointerData : AbstractData
    {
        /// <summary>
        /// referenced data
        /// </summary>
        public AbstractData RefData { get; private set; }
        
        /// <summary>
        /// referenced data's type
        /// </summary>
        public BaseDataType RefType { get; private set; }

        public PointerData(PointerDataType dataType, BinaryReader reader, Dictionary<string, object> options)
            : base(dataType)
        {
            if (!options.ContainsKey("offset"))
                throw new Exception("Wrong parameters for reading ListData");

            RefType = dataType.RefType;
            Length = RefType.PointerWidth;

            // moving to start of pointer so that RefType can read it's own options from that point
            Offset = (int)options["offset"];
            reader.BaseStream.Seek(DatContainer.DataSectionOffset + Offset, SeekOrigin.Begin);

            // only RefType knows how to read it's parameters
            var refParams = RefType.ReadPointer(reader);

            var listDataType = RefType as ListDataType;
            if (listDataType != null)
                RefData = (Length == 8) ? TypeFactory.CreateData(listDataType, reader, refParams) : TypeFactory64.CreateData(listDataType, reader, refParams); //8 or 16
            else
                RefData = (Length == 4) ? TypeFactory.CreateData(RefType, reader, refParams) : TypeFactory64.CreateData(RefType, reader, refParams); //4 or 8

            DatContainer.DataPointers[Offset] = this;
        }

        public override string GetValueString()
        {
            return RefData.GetValueString();
        }
    }
}