using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using LibDat.Data;

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
        /// List of properties for the specified dat type. See code in Files directory.
        /// </summary>
        public List<DatRecord> Records;

        /// <summary>
        /// Offset of the data section in the .dat file (Starts with 0xbbbbbbbbbbbbbbbb)
        /// </summary>
        public long DataTableBegin;

        /// <summary>
        /// Contains the entire unmodified data section of the .dat file
        /// </summary>
        private byte[] originalDataTable;

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
            using (BinaryReader br = new BinaryReader(inStream, Encoding.Unicode))
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

            byte[] fileBytes = File.ReadAllBytes(filePath);

            using (MemoryStream ms = new MemoryStream(fileBytes))
            {
                using (BinaryReader br = new BinaryReader(ms, System.Text.Encoding.Unicode))
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
            int numberOfEntries = inStream.ReadInt32();
            if (inStream.ReadUInt64() == 0xBBbbBBbbBBbbBBbb)
            {
                Records = new List<DatRecord>();
                return;
            }
            inStream.BaseStream.Seek(-8, SeekOrigin.Current);

            Records = new List<DatRecord>(numberOfEntries);

            if (DatName.Length == 0)
            {
                throw new Exception("Missing dat parser for file " + DatName);
            }

            for (int i = 0; i < numberOfEntries; i++)
            {
                Records.Add(new DatRecord(RecordInfo, inStream));
            }

            if (inStream.ReadUInt64() != 0xBBbbBBbbBBbbBBbb)
            {
                throw new ApplicationException("Missing magic number after records");
            }

            DataTableBegin = inStream.BaseStream.Position - 8;
            inStream.BaseStream.Seek(-8, SeekOrigin.Current);
            originalDataTable = inStream.ReadBytes((int)(inStream.BaseStream.Length - inStream.BaseStream.Position));

            // Read all referenced string and data entries from the data following the entries (starting at magic number)
            foreach (var r in Records)
            {
                ReadRecordData(r, inStream);
            }
        }

        /// <summary>
        /// Finds all known references to strings and other data in the data section and adds them to the DataEntries. 
        /// Accomplished by reading the [StringIndex] and [DataIndex] attributes of our dat structure.
        /// </summary>
        /// <param name="entry">Dat parser created from parsing a single entry of the .dat file.</param>
        /// <param name="inStream">Stream containing contents of .dat file. Stream position not preserved.</param>
        private void ReadRecordData(DatRecord record, BinaryReader inStream)
        {
            var fields = RecordInfo.Fields;

            if (!RecordInfo.HasPointers)
            {
                return;
            }

            foreach (var field in fields)
            {
                if (!field.HasPointer)
                {
                    continue;
                }

                int offset = (int)record.GetFieldValue(field);

                if (DataEntries.ContainsKey(offset) && !DataEntries[offset].ToString().Equals(""))
                {
                    continue;
                }

                DatRecordFieldInfo fieldLength = null;
                int length;
                switch (field.PointerType)
                {
                    case PointerTypes.StringIndex:
                        DataEntries[offset] = new UnicodeString(offset, DataTableBegin, inStream, false); break;
                    case PointerTypes.IndirectStringIndex:
                    case PointerTypes.UserStringIndex:
                        DataEntries[offset] = new UnicodeString(offset, DataTableBegin, inStream, true); break;
                    case PointerTypes.DataIndex:
                        DataEntries[offset] = new UnknownData(offset, DataTableBegin, inStream); break;
                    // TODO: what if Length == 0 from array pointers ? then backtrack to StringIndex
                    // TODO: create better approach for handling reference to length of array at pointer offset
                    case PointerTypes.UInt64Index:
                        fieldLength = fields.Where(x => x.Description == field.Description + "_Length").FirstOrDefault();
                        if (fieldLength == null)
                            throw new Exception("Couldn't find length field for field: " + field.Description);
                        length = (int)(record.GetFieldValue(fieldLength));
                        if (length > 0)
                        {
                            DataEntries[offset] = new UInt64List(offset, DataTableBegin, length, inStream);
                        }
                        else
                        {
                            DataEntries[offset] = new UnicodeString(offset, DataTableBegin, inStream, false); break;
                        }
                        break;
                    case PointerTypes.UInt32Index:
                        fieldLength = fields.Where(x => x.Description == field.Description + "_Length").FirstOrDefault();
                        length = (int)(record.GetFieldValue(fieldLength));
                        if (length > 0)
                        {
                            DataEntries[offset] = new UInt32List(offset, DataTableBegin, length, inStream);
                        }
                        else
                        {
                            DataEntries[offset] = new UnicodeString(offset, DataTableBegin, inStream, false); break;
                        }
                        break;
                    case PointerTypes.Int32Index:
                        fieldLength = fields.Where(x => x.Description == field.Description + "_Length").FirstOrDefault();
                        if (fieldLength == null)
                            throw new Exception("Couldn't find length field for field: " + field.Description);
                        length = (int)(record.GetFieldValue(fieldLength));
                        if (length > 0)
                        {
                            DataEntries[offset] = new Int32List(offset, DataTableBegin, length, inStream);
                        }
                        else
                        {
                            DataEntries[offset] = new UnicodeString(offset, DataTableBegin, inStream, false); break;
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Saves parsed data to specified path
        /// </summary>
        /// <param name="filePath">Path to write contents to</param>
        public void Save(string filePath)
        {
            using (FileStream outStream = File.Open(filePath, FileMode.Create))
            {
                Save(outStream);
            }
        }

        /// <summary>
        /// Saves parsed data to specified stream
        /// </summary>
        /// <param name="outStream">Stream to write contents to</param>
        public void Save(Stream rawOutStream)
        {
            // Mapping of the new string and data offsets
            Dictionary<long, long> changedOffsets = new Dictionary<long, long>();

            BinaryWriter outStream = new BinaryWriter(rawOutStream, System.Text.Encoding.Unicode);
            outStream.Write(Records.Count);

            if (Records.Count > 0)
            {
                // Pretty ugly way to zero out the for sizeof(Entry) * EntryCount bytes
                outStream.Write(new byte[RecordInfo.Length * Records.Count]);
            }

            int newStartOfDataSection = (int)outStream.BaseStream.Position;
            outStream.Write(originalDataTable);

            foreach (var item in DataEntries)
            {
                if (item.Value is UnicodeString)
                {
                    UnicodeString str = item.Value as UnicodeString;
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
        private void UpdateDataOffsets(DatRecord record, Dictionary<long, long> updatedOffsets)
        {
            var fields = RecordInfo.Fields;

            if (!RecordInfo.HasPointers)
            {
                return;
            }

            foreach (var field in fields)
            {
                if (!field.HasPointer || !field.IsString() )
                    continue;

                int offset = (int)record.GetFieldValue(field);
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
            StringBuilder sb = new StringBuilder();
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
                foreach (var field in fields)
                {
                    sb.AppendFormat("{0}{1}", field.Index, separator);

                    object fieldValue = record.GetFieldValue(field);
                    if (!field.HasPointer)
                    {
                        sb.AppendFormat("{0}{1}", fieldValue, separator);
                        continue;
                    }

                    // has pointers
                    switch (field.PointerType)
                    {
                        case PointerTypes.UInt64Index:
                        case PointerTypes.UInt32Index:
                        case PointerTypes.Int32Index:
                            sb.AppendFormat("{0}{1}", fieldValue, separator); break;
                        case PointerTypes.StringIndex:
                        case PointerTypes.UserStringIndex:
                            sb.AppendFormat("\"{0}\"{1}", DataEntries[(int)fieldValue].ToString().Replace("\"", "\"\""), separator); break;
                        case PointerTypes.DataIndex:
                            sb.AppendFormat("{0}{1}", DataEntries[(int)fieldValue].ToString().Replace("\"", "\"\""), separator); break;
                    }
                }
                sb.Remove(sb.Length - 1, 1);
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public byte[] GetBytes()
        {
            MemoryStream ms = new MemoryStream();
            Save(ms);

            return ms.ToArray();
        }

    }
}
