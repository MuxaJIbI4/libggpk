using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LibDat
{
    // contains info about single record in *.dat file
    public class RecordInfo
    {
        /// <summary>
        /// Name of .dat file which this record definition belongs to
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Represents the number of bytes this record will read or write to the DAT file
        /// </summary>
        /// <returns>Number of bytes this record will take in its native format</returns>
        public int Length { get; private set; }



        // record fields
        private readonly List<FieldInfo> _fields;
        public ReadOnlyCollection<FieldInfo> Fields
        {
            get { return _fields.AsReadOnly(); }
        }

        // returns true if record has fields which contain offset to data section of .dat
        public bool HasPointers { get; private set; }

        public RecordInfo(string id, int length = 0, List<FieldInfo> fields = null)
        {
            Id = id;
            Length = length;
            _fields = fields ?? new List<FieldInfo>();
            HasPointers = _fields.Any(x => x.IsPointer);
        }
    }
}
