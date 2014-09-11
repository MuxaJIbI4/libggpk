using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LibDat.Data
{
    // this class help to read data from data section
    public class DataReader
    {
        private readonly int _dataTableBegin;
        private readonly Dictionary<int, AbstractData> _dataEntries;

        public DataReader(Dictionary<int, AbstractData> entries, int dataTableBegin)
        {
            if (entries == null)
                throw new Exception("Entries can't be null");
            _dataEntries = entries;
            _dataTableBegin = dataTableBegin;
        }

        /// <summary>
        /// Finds all known references to strings and other data in the data section and adds them to the DataEntries. 
        /// Accomplished by reading the [StringIndex] and [DataIndex] attributes of our dat structure.
        /// </summary>
        /// <param name="inStream">Stream containing contents of .dat file. Stream position not preserved.</param>
        /// <param name="record">Record with fields pointing to data</param>
        public void ReadRecordData(BinaryReader inStream, DatRecord record)
        {
            var fields = record.RecordInfo.Fields;
            foreach (var field in fields)
            {
                if (!field.IsPointer)
                    continue;

                var offset = (int)(record.GetFieldValue(field));
                if (_dataEntries.ContainsKey(offset) && (!_dataEntries[offset].ToString().Equals("")))
                    continue;

                // find length of array to read at given offset
                if (typeof(IDataList).IsAssignableFrom(field.PointerType)) // data is a list
                {
                    ReadList(inStream, record, field);
                }
                else // data not a list
                {
                    switch (field.PointerTypeString)
                    {
                        case PointerTypes.StringIndex:
                        case PointerTypes.IndirectStringIndex:
                            ReadString(inStream, record, field, false); break;
                        case PointerTypes.UserStringIndex:
                            ReadString(inStream, record, field, true); break;
                        case PointerTypes.DataIndex:
                            ReadUnknownData(inStream, record, field); break;
                    }
                }
            }
        }

        /// <summary>
        /// Reads list of data from inStream at offset written into given field of record
        /// </summary>
        /// <param name="inStream">stream to read</param>
        /// <param name="record">currently processed record</param>
        /// <param name="field">currently processed field's info</param>
        private void ReadList(BinaryReader inStream, DatRecord record, DatRecordFieldInfo field)
        {
            var fieldLength = record.RecordInfo.Fields.FirstOrDefault(x => x.Description == field.Description + "_Length");
            if (fieldLength == null)
                throw new Exception("Couldn't find length field for list field: " + field.Description);
            var length = (int)(record.GetFieldValue(fieldLength));
            var offset = (int)(record.GetFieldValue(field));
            if (length != 0)
            {
                _dataEntries[offset] = (AbstractData)Activator.CreateInstance(
                    field.PointerType,
                    new object[] {offset, _dataTableBegin, length, inStream});
                return;
            }

            // length == 0 possibilities:
            // 1. wrong record format
            // 2. this offset is a start of string instead of array (ex: FlaskTypes.dat)
            // TODO: get statistics for this case, when and whehre it happens
            // TODO: create better approach for handling reference to length of array at pointer offset
            // TODO: 1) add it to XML definitions? (<pointer>UInt32Index</pointer><length_field id='name of field' /> )
            // TODO: this string also can be pointer to string
            ReadString(inStream, record, field, false);
        }

        /// <summary>
        /// Reads string from inStream at offset written into given field of record
        /// </summary>
        /// <param name="inStream"></param>
        /// <param name="record"></param>
        /// <param name="field"></param>
        /// <param name="isUserString"></param>
        private void ReadString(BinaryReader inStream, DatRecord record, DatRecordFieldInfo field, bool isUserString)
        {
            var offset = (int)(record.GetFieldValue(field));
            var fieldLength = record.RecordInfo.Fields.FirstOrDefault(x => x.Description == field.Description + "_Prefix");
            if (fieldLength != null) // this field can contain pointer to pointers to string
            {
                var isPointerToPointer = (int)(record.GetFieldValue(fieldLength)) == 1;
                if (isPointerToPointer)
                {
                    var data = new PointerData(offset, _dataTableBegin, inStream);
                    _dataEntries[offset] = data;
                    var newOffset = data.PointerOffset;
                    var pointerData = new UnicodeString(newOffset, _dataTableBegin, inStream, isUserString);
                    _dataEntries[newOffset] = pointerData;
                    data.Data = pointerData;
                    return;
                }
            }

            // simple string pointer
            _dataEntries[offset] = new UnicodeString(offset, _dataTableBegin, inStream, isUserString);
        }

        private void ReadUnknownData(BinaryReader inStream, DatRecord record, DatRecordFieldInfo field)
        {
            var offset = (int)(record.GetFieldValue(field));
            _dataEntries[offset] = new UnknownData(offset, _dataTableBegin, inStream);
        }

    }
}
