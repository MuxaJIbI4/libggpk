using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace LibDat
{
    /// <summary>
    /// Contains list of <c>FieldData</c> read from single record in .dat file
    /// </summary>
    public class RecordData
    {
        private RecordInfo RecordInfo { get; set; }

        private readonly List<FieldData> _fieldsData;
        public ReadOnlyCollection<FieldData> FieldsData
        {
            get { return _fieldsData.AsReadOnly(); }
        }

        private readonly int _fieldsCount;

        public RecordData(RecordInfo ri, BinaryReader inStream)
        {
            RecordInfo = ri;
            _fieldsData = new List<FieldData>();
            _fieldsCount = ri.Fields.Count;

            Read(inStream);
        }

        private void Read(BinaryReader inStream)
        {
            foreach (var fi in RecordInfo.Fields)
            {
                _fieldsData.Add(new FieldData(fi, fi.FieldType.Reader(inStream)));
            }
        }

        /// <summary>
        /// Save this record to the specified stream. Stream position is not preserved.
        /// </summary>
        /// <param name="outStream">Stream to write contents to</param>
        public void Save(BinaryWriter outStream)
        {
            for (var i = 0; i < _fieldsCount; i++)
            {
                // TODO This saves fields data as it were at the beginning
                RecordInfo.Fields[i].FieldType.Writer(outStream, _fieldsData[i].Value);
            }
        }

        /// <summary>
        /// Updates references to modified strings/data in the data section for the specified entry.
        /// </summary>
        public void UpdateDataOffsets(Dictionary<int, int> updatedOffsets)
        {
            if (!RecordInfo.HasPointers)
                return;

            foreach (var fieldData in FieldsData)
            {
                if (!fieldData.FieldInfo.IsPointer || !fieldData.FieldInfo.IsString())
                    continue;

                var offset = fieldData.Offset;
                if (updatedOffsets.ContainsKey(offset))
                    fieldData.Value = updatedOffsets[offset];
            }
        }
    }
}
