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
        private int Index { get; set; }

        private readonly List<FieldData> _fieldsData;
        public ReadOnlyCollection<FieldData> FieldsData
        {
            get { return _fieldsData.AsReadOnly(); }
        }

        public RecordData(RecordInfo ri, BinaryReader inStream, int index)
        {
            RecordInfo = ri;
            Index = index;
            _fieldsData = new List<FieldData>();

            // Seek to start of record
            inStream.BaseStream.Seek(4 + ri.Length*index, SeekOrigin.Begin);
            var startOffset = (int)inStream.BaseStream.Position;
            foreach (var fi in RecordInfo.Fields)
            {
                try
                {
                    inStream.BaseStream.Seek(startOffset + fi.RecordOffset, SeekOrigin.Begin);
                    var fieldData = new FieldData(fi, inStream, RecordInfo.x64);
                    _fieldsData.Add(fieldData);
                }
                catch (Exception e)
                {
                    var error = String.Format(
                        "Error: Row ID = {0} Field Id={1}, Field Type Name = {2},"  +
                        "\n Message:{3}\n Stacktrace: {4}",
                        Index, fi.Id, fi.FieldType.Name, e.Message, e.StackTrace);
                    Console.WriteLine(error);
                    throw new Exception(error);
                }
            }
        }
    }
}