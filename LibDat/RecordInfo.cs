using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LibDat
{
    /// <summary>
    /// contains inforamtion about format of record of .dat file
    /// </summary>
    public class RecordInfo
    {
        /// <summary>
        /// Whether the extension is .dat64
        /// </summary>
        public bool x64;

        /// <summary>
        /// Returns Name (without extension) of .dat file record of which this record describes
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Contains number of bytes this record will read or write to the .dat file
        /// </summary>
        /// <returns>Number of bytes this record will take in its native format</returns>
        public int Length { get; set; }

        private readonly List<FieldInfo> _fields;
        public ReadOnlyCollection<FieldInfo> Fields
        {
            get { return _fields.AsReadOnly(); }
        }

        /// <summary>
        /// returns true if record has fields of pointer type
        /// </summary>
        public bool HasPointers { get; private set; }

        public RecordInfo(string fileName, int length = 0, List<FieldInfo> fields = null, bool dat64=false)
        {
            x64 = dat64;
            FileName = fileName;
            Length = length;
            _fields = fields ?? new List<FieldInfo>();
            HasPointers = _fields.Any(x => x.IsPointer);
        }
    }
}