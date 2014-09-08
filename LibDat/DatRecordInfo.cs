using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;


namespace LibDat
{
    // contains info about single record in *.dat file
    public class DatRecordInfo
    {
        /// <summary>
        /// Represents the number of bytes this record will read or write to the DAT file
        /// </summary>
        /// <returns>Number of bytes this record will take in its native format</returns>
        public int Length { get; private set; }

        // record fields
        private List<DatRecordFieldInfo> fields;
        public ReadOnlyCollection<DatRecordFieldInfo> Fields
        {
            get { return fields.AsReadOnly(); }
            set { }
        }

        // returns true if record has fields which contain offset to data section of .dat
        public bool HasPointers { get; private set; }

        public DatRecordInfo(int length, List<DatRecordFieldInfo> fields)
        {
            Length = length;
            this.fields = fields;
            HasPointers = fields.Any(x => x.HasPointer);
        }
    }
}
