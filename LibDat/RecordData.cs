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
        public RecordInfo RecordInfo { get; private set; }

        private List<FieldData> _fieldsData;
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

        // return value of field given by its RecordFieldInfo object
        public object GetFieldValue(FieldInfo field)
        {
            var index = RecordInfo.Fields.IndexOf(field);
            if (index == -1)
                throw new Exception("Searched record for field that doesn't exist in it");
            return GetFieldValue(index);
        }

        // return value of field given by its index starting from 0
        public object GetFieldValue(int index)
        {
            if (index < 0 || index >= _fieldsCount)
                throw new Exception("Field's index out of bounds: " + index + " not in [0," + _fieldsCount + "]");
            return _fieldsData[index].Value;
        }

        /// <summary>
        /// Updates references to modified strings/data in the data section for the specified entry.
        /// </summary>
        public void UpdateDataOffsets(Dictionary<int, int> updatedOffsets)
        {
            var fieldInfos = RecordInfo.Fields;
            if (!RecordInfo.HasPointers)
                return;

            foreach (var fieldInfo in fieldInfos)
            {
                if (!fieldInfo.IsPointer || !fieldInfo.IsString())
                    continue;

                var offset = (int)GetFieldValue(fieldInfo);
                if (updatedOffsets.ContainsKey(offset))
                {
                    //Console.WriteLine("Updating offset {0} for {1} (now {2})", offset, prop.Name, updatedOffsets[offset]);
                    SetFieldValue(fieldInfo, updatedOffsets[offset]);
                }
            }
        }

        // saved value of field given by its RecordFieldInfo object
        public void SetFieldValue(FieldInfo field, object value)
        {
            var index = RecordInfo.Fields.IndexOf(field);
            if (index == -1)
                throw new Exception("Searched record for field that doesn't exist in it");
            SetFieldValue(index, value);
        }

        // saved value of field given by its RecordFieldInfo object
        public void SetFieldValue(int index, object value)
        {
            if (index < 0 || index >= _fieldsCount)
                throw new Exception("Field's index out of bounds: " + index + " not in [0," + _fieldsCount + "]");
            var fieldInfo = RecordInfo.Fields[index];
            var width = fieldInfo.FieldType.Width;
            var error = false;
            switch (width)
            {
                case 1: if (!(value is bool || value is byte))  error = true; break;
                case 2: if (!(value is short))                  error = true; break;
                case 4: if (!(value is int))                    error = true; break;
                case 8: if (!(value is Int64))                  error = true; break;
            }
            if (error)
                throw new Exception("Can't save value of type " + value.GetType()
                    + " into field of type " + fieldInfo.FieldType.Name);
            _fieldsData[index].Value = value;
        }
    }
}
