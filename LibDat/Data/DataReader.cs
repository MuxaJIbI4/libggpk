using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibDat.Data
{
    // this class help to read data from data section
    public class DataReader
    {
        private int DataTableBegin;
        private Dictionary<int, AbstractData> DataEntries;

        public DataReader(Dictionary<int, AbstractData> entries, int dataTableBegin)
        {
            if (entries == null)
                throw new Exception("Entries can't be null");
            DataEntries = entries;
            DataTableBegin = dataTableBegin;
        }

        /// <summary>
        /// Finds all known references to strings and other data in the data section and adds them to the DataEntries. 
        /// Accomplished by reading the [StringIndex] and [DataIndex] attributes of our dat structure.
        /// </summary>
        /// <param name="entry">Dat parser created from parsing a single entry of the .dat file.</param>
        /// <param name="inStream">Stream containing contents of .dat file. Stream position not preserved.</param>
        public void ReadRecordData(BinaryReader inStream, DatRecord record)
        {
            var fields = record.RecordInfo.Fields;
            foreach (var field in fields)
            {
                if (!field.IsPointer)
                    continue;

                int offset = (int)(record.GetFieldValue(field));

                if (DataEntries.ContainsKey(offset) && (!DataEntries[offset].ToString().Equals("")))
                    continue;

                // find length of array to read at given offset
                if (field.IsPointer && (field.PointerType is IDataList)) // data is a list
                {
                    DatRecordFieldInfo fieldLength = fields
                        .Where(x => x.Description == field.Description + "_Length")
                        .FirstOrDefault();
                    if (fieldLength == null)
                        throw new Exception("Couldn't find length field for field: " + field.Description);
                    // TODO check that fieldLength isn't pointer field

                    int length = (int)(record.GetFieldValue(fieldLength));

                    if (length == 0)
                    {
                        // possibilities:
                        // 1. wrong record format
                        // 2. this offset is a start of string instead of array (ex: FlaskTypes.dat)
                        // TODO: get statistics for this case, when and whehre it happens
                        // TODO: create better approach for handling reference to length of array at pointer offset
                        // TODO: 1) add it to XML definitions? (<pointer>UInt32Index</pointer><length_field id='name of field' /> )
                        DataEntries[offset] = new UnicodeString(offset, DataTableBegin, inStream, false);
                    }
                    else
                    {
                        DataEntries[offset] = (AbstractData)Activator.CreateInstance(field.PointerType, new object[] { offset, DataTableBegin, length, inStream });
                    }
                }
                else // data not a list
                {
                    switch (field.PointerTypeString)
                    {
                        case PointerTypes.StringIndex:
                        case PointerTypes.IndirectStringIndex:
                            DataEntries[offset] = new UnicodeString(offset, DataTableBegin, inStream, false); break;
                        case PointerTypes.UserStringIndex:
                            DataEntries[offset] = new UnicodeString(offset, DataTableBegin, inStream, true); break;
                        case PointerTypes.DataIndex:
                            DataEntries[offset] = new UnknownData(offset, DataTableBegin, inStream); break;
                    }
                }
            }
        }
    }
}
