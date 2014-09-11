using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using LibDat.Data;
using System.Text.RegularExpressions;

namespace LibDat
{
    /// <summary>
    /// Parses and holds all information found in a specific .dat file.
    /// </summary>
    public class DatContainer
    {
        /// <summary>
        /// Name of the dat file (without .dat extension)
        /// </summary>
        public readonly string DatName;

        public DatRecordInfo RecordInfo { get; private set; }

        /// <summary>
        /// Offset of the data section in the .dat file (Starts with 0xbbbbbbbbbbbbbbbb)
        /// </summary>
        public int DataSectionOffset { get; private set; }

        /// <summary>
        /// Length of data section of .dat file
        /// </summary>
        public int DataSectionDataLength { get; private set; }

        /// <summary>
        /// Contains the entire unmodified data section of the .dat file
        /// </summary>
        private byte[] originalDataTable;

        /// <summary>
        /// List of properties for the specified dat type. See code in Files directory.
        /// </summary>
        public List<DatRecord> Records;

        /// <summary>
        /// Mapping of all known strings and other data found in the data section. 
        /// Key = offset with respect to beginning of data section.
        /// 
        /// </summary>
        public Dictionary<int, AbstractData> DataEntries = new Dictionary<int, AbstractData>();

        /// <summary>
        /// Parses the .dat file contents from inStream.
        /// </summary>
        /// <param name="inStream">Unicode binary reader containing ONLY the contents of a single .dat file and nothing more</param>
        /// <param name="fileName">Name of the dat file (with extension)</param>
        public DatContainer(Stream inStream, string fileName)
        {
            DatName = Path.GetFileNameWithoutExtension(fileName);
            RecordInfo = DatRecordInfoFactory.GetRecordInfo(DatName);
            using (var br = new BinaryReader(inStream, Encoding.Unicode))
            {
                Read(br);
            }
        }

        /// <summary>
        /// Parses the .dat file found at path 'fileName'
        /// </summary>
        /// <param name="filePath">Path of .dat file to parse</param>
        public DatContainer(string filePath)
        {
            DatName = Path.GetFileNameWithoutExtension(filePath);

            var fileBytes = File.ReadAllBytes(filePath);

            using (var ms = new MemoryStream(fileBytes))
            {
                using (var br = new BinaryReader(ms, System.Text.Encoding.Unicode))
                {
                    Read(br);
                }
            }
        }

        /// <summary>
        /// Reads the .dat frile from the specified stream
        /// </summary>
        /// <param name="inStream">Stream containing contents of .dat file</param>
        private void Read(BinaryReader inStream)
        {
            // check that record format is defined
            if (RecordInfo == null)
                throw new Exception("Missing dat parser for file " + DatName);

            var numberOfEntries = inStream.ReadInt32();
            if (inStream.ReadUInt64() == 0xBBbbBBbbBBbbBBbb)
            {
                Records = new List<DatRecord>();
                return;
            }
            inStream.BaseStream.Seek(-8, SeekOrigin.Current);

            // find record_length;
            var length = findRecordLength(inStream, numberOfEntries);
            if ((numberOfEntries > 0) && length != RecordInfo.Length)
                throw new Exception("Found record length = " + length + " not equal length defined in XML: " + RecordInfo.Length);

            // read records
            Records = new List<DatRecord>(numberOfEntries);
            for (var i = 0; i < numberOfEntries; i++)
            {
                Records.Add(new DatRecord(RecordInfo, inStream));
            }

            // check magic number
            if (inStream.ReadUInt64() != 0xBBbbBBbbBBbbBBbb)
                throw new ApplicationException("Missing magic number after records");

            // read data section
            inStream.BaseStream.Seek(-8, SeekOrigin.Current);
            DataSectionOffset = (int)inStream.BaseStream.Position;
            DataSectionDataLength = (int)(inStream.BaseStream.Length) - DataSectionOffset - 8;
            originalDataTable = inStream.ReadBytes((int)(inStream.BaseStream.Length - inStream.BaseStream.Position));

            // Read all referenced string and data entries from the data following the entries (starting at magic number)
            if (!RecordInfo.HasPointers)
                return;

            var dr = new DataReader(DataEntries, DataSectionOffset);
            foreach (var r in Records)
            {
                try
                {
                    dr.ReadRecordData(inStream, r);
                }
                catch (Exception e)
                {
                    // for debugging
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }

        private int findRecordLength(BinaryReader inStream, int numberOfEntries)
        {
            if (numberOfEntries == 0)
                return 0;

            inStream.BaseStream.Seek(4, SeekOrigin.Begin);
            var StringLength = inStream.BaseStream.Length;
            var record_length = 0;
            for (var i = 0; inStream.BaseStream.Position <= StringLength - 8; i++)
            {
                var ul = inStream.ReadUInt64();
                if (ul == 0xBBbbBBbbBBbbBBbb)
                {
                    record_length = i;
                    break;
                }
                else
                {
                    inStream.BaseStream.Seek(-8 + numberOfEntries, SeekOrigin.Current);
                    Console.WriteLine(inStream.BaseStream.Position);
                }
            }
            if (record_length == 0)
                throw new Exception("Couldn't find record_length");

            inStream.BaseStream.Seek(4, SeekOrigin.Begin);
            return record_length;
        }

        /// <summary>
        /// Saves parsed data to specified path
        /// </summary>
        /// <param name="filePath">Path to write contents to</param>
        public void Save(string filePath)
        {
            using (var outStream = File.Open(filePath, FileMode.Create))
            {
                Save(outStream);
            }
        }

        public byte[] SaveAsBytes()
        {
            var ms = new MemoryStream();
            Save(ms);

            return ms.ToArray();
        }

        /// <summary>
        /// Saves parsed data to specified stream
        /// </summary>
        /// <param name="outStream">Stream to write contents to</param>
        public void Save(Stream rawOutStream)
        {
            // Mapping of the new string and data offsets
            var changedOffsets = new Dictionary<int, int>();

            var outStream = new BinaryWriter(rawOutStream, System.Text.Encoding.Unicode);
            outStream.Write(Records.Count);

            if (Records.Count > 0)
            {
                // Pretty ugly way to zero out the for sizeof(Entry) * EntryCount bytes
                outStream.Write(new byte[RecordInfo.Length * Records.Count]);
            }

            var newStartOfDataSection = (int)outStream.BaseStream.Position;
            outStream.Write(originalDataTable);

            foreach (var item in DataEntries)
            {
                if (item.Value is UnicodeString)
                {
                    var str = item.Value as UnicodeString;
                    if (!string.IsNullOrWhiteSpace(str.NewData))
                    {
                        str.Save(outStream);
                        changedOffsets[str.Offset] = str.NewOffset;
                    }
                }
            }

            // Go back to the beginning and write the real entries
            outStream.BaseStream.Seek(4, SeekOrigin.Begin);

            // Now we must go through each StringIndex and DataIndex and update the index then save it
            foreach (var r in Records)
            {
                UpdateDataOffsets(r, changedOffsets);
                r.Save(outStream);
            }
        }

        /// <summary>
        /// Updates references to modified strings/data in the data section for the specified entry.
        /// </summary>
        /// <param name="entry">Entry being updated</param>
        /// <param name="updatedOffsets">Mapping of all changed offsets. Key = original offset, Value = new offset.</param>
        private void UpdateDataOffsets(DatRecord record, Dictionary<int, int> updatedOffsets)
        {
            var fields = RecordInfo.Fields;

            if (!RecordInfo.HasPointers)
            {
                return;
            }

            foreach (var field in fields)
            {
                if (!field.IsPointer || !field.IsString())
                    continue;

                var offset = (int)record.GetFieldValue(field);
                if (updatedOffsets.ContainsKey(offset))
                {
                    //Console.WriteLine("Updating offset {0} for {1} (now {2})", offset, prop.Name, updatedOffsets[offset]);
                    record.SetFieldValue(field, (int)updatedOffsets[offset]);
                }
            }
        }

        /// <summary>
        /// Returns a CSV table with the contents of this dat container.
        /// </summary>
        /// <returns></returns>
        public string GetCSV()
        {
            const char separator = ',';
            var sb = new StringBuilder();
            var fields = RecordInfo.Fields;

            // add header
            sb.AppendFormat("Rows{0}", separator);
            foreach (var field in fields)
            {
                sb.AppendFormat("{0}{1}", field.Description, separator);
            }
            sb.Remove(sb.Length - 1, 1);
            sb.AppendLine();

            // add records
            foreach (var record in Records)
            {
                // row index
                sb.AppendFormat("{0}{1}", Records.IndexOf(record), separator);

                // add fields
                foreach (var field in fields)
                {
                    var fieldValue = record.GetFieldValue(field);
                    sb.AppendFormat("{0}{1}", GetCSVString(field, fieldValue), separator);
                }

                // finish line
                sb.Remove(sb.Length - 1, 1);
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public string GetCSVString(DatRecordFieldInfo field, object fieldValue)
        {
            string str = null;
            if (field.IsPointer)
            {
                switch (field.PointerTypeString)
                {
                    case PointerTypes.UInt64Index:
                    case PointerTypes.UInt32Index:
                    case PointerTypes.Int32Index:
                        str = DataEntries[(int)fieldValue].ToString(); break;
                    case PointerTypes.StringIndex:
                    case PointerTypes.UserStringIndex:
                        str = '"' + DataEntries[(int)fieldValue].ToString().Replace("\"", "\"\"") + '"';
                        break;
                    case PointerTypes.DataIndex:
                        str = DataEntries[(int)fieldValue].ToString().Replace("\"", "\"\""); break;
                }
            }
            if (str == null)
            {
                var tmp = fieldValue.ToString();
                str = (Regex.IsMatch(tmp, ",") ? '"' + tmp.Replace("\"", "\"\"") + '"' : tmp);
            }
            return str;
        }
    }
}
