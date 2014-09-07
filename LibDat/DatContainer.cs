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

        public readonly RecordInfo recordInfo;

        /// <summary>
        /// List of properties for the specified dat type. See code in Files directory.
        /// </summary>
        public List<Record> Records;

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

            // TODO
            // recordInfo = ??????

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
                Records = new List<Record>();
                return;
            }
            inStream.BaseStream.Seek(-8, SeekOrigin.Current);

            Records = new List<Record>(numberOfEntries);

            if (DatName.Length == 0)
            {
                throw new Exception("Missing dat parser for file " + DatName);
            }

            for (int i = 0; i < numberOfEntries; i++)
            {
                Records.Add(RecordFactory.Create(DatName, inStream));
            }

            if (inStream.ReadUInt64() != 0xBBbbBBbbBBbbBBbb)
            {
                throw new ApplicationException("Missing magic number after records");
            }

            DataTableBegin = inStream.BaseStream.Position - 8;
            inStream.BaseStream.Seek(-8, SeekOrigin.Current);
            originalDataTable = inStream.ReadBytes((int)(inStream.BaseStream.Length - inStream.BaseStream.Position));

            // Read all referenced string and data entries from the data following the entries (starting at magic number)
            foreach (var item in Records)
            {
                AddDataToTable(item, inStream);
            }
        }


        public object GetFieldValue(Record record, int index)
        {
            return record.GetField(index);
        }

		/// <summary>
		/// Finds all known references to strings and other data in the data section and adds them to the DataEntries. 
		/// Accomplished by reading the [StringIndex] and [DataIndex] attributes of our dat structure.
		/// </summary>
		/// <param name="entry">Dat parser created from parsing a single entry of the .dat file.</param>
		/// <param name="inStream">Stream containing contents of .dat file. Stream position not preserved.</param>
		private void AddDataToTable(Record entry, BinaryReader inStream)
		{
			var properties = entry.GetType().GetProperties();

			foreach (var prop in properties)
			{
				object[] customAttributes = prop.GetCustomAttributes(false);

				if (customAttributes.Length == 0)
					continue;

				int offset = (int)prop.GetValue(entry, null);
				if (DataEntries.ContainsKey(offset) && !DataEntries[offset].ToString().Equals(""))
				{
					continue;
				}

				if (customAttributes.Any(n => n is StringIndex))
				{
					DataEntries[offset] = new UnicodeString(inStream, offset, DataTableBegin, (customAttributes.Any(n => n is UserStringIndex)));
					//	Console.WriteLine("{0} -> {1}", offset, DataEntries[offset]);
				}
				else if (customAttributes.Any(n => n is DataIndex))
				{
					DataEntries[offset] = new UnkownData(inStream, offset, DataTableBegin);
				}
				else if (customAttributes.Any(n => n is UInt64Index))
				{
					var propLength = entry.GetType().GetProperties().Where(x => x.Name == prop.Name+"Length").FirstOrDefault();
					DataEntries[offset] = new UInt64List(inStream, offset, DataTableBegin, (int)(propLength.GetValue(entry, null)));
				}
				else if (customAttributes.Any(n => n is UInt32Index))
				{
					var propLength = entry.GetType().GetProperties().Where(x => x.Name == prop.Name + "Length").FirstOrDefault();
					DataEntries[offset] = new UInt32List(inStream, offset, DataTableBegin, (int)(propLength.GetValue(entry, null)));
				}
				else if (customAttributes.Any(n => n is Int32Index))
				{
					var propLength = entry.GetType().GetProperties().Where(x => x.Name == prop.Name + "Length").FirstOrDefault();
					DataEntries[offset] = new Int32List(inStream, offset, DataTableBegin, (int)(propLength.GetValue(entry, null)));
				}
			}
		}





		public byte[] GetBytes()
		{
			MemoryStream ms = new MemoryStream();
			Save(ms);

			return ms.ToArray();
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
				outStream.Write(new byte[recordInfo.Length * Records.Count]);
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
			foreach (var item in Records)
			{
				UpdateDataOffsets(item, changedOffsets);
				item.Save(outStream);
			}
		}

        /// <summary>
        /// Updates references to modified strings/data in the data section for the specified entry.
        /// </summary>
        /// <param name="entry">Entry being updated</param>
        /// <param name="updatedOffsets">Mapping of all changed offsets. Key = original offset, Value = new offset.</param>
        private void UpdateDataOffsets(Record entry, Dictionary<long, long> updatedOffsets)
        {
            var properties = entry.GetType().GetProperties();
            foreach (var prop in properties)
            {
                object[] customAttributes = prop.GetCustomAttributes(false);

                if (customAttributes.Length == 0)
                    continue;

                if (customAttributes.Any(n => n is UInt64Index) || customAttributes.Any(n => n is UInt32Index) ||
                    customAttributes.Any(n => n is Int32Index))
                    continue;

                int offset = (int)prop.GetValue(entry, null);
                if (updatedOffsets.ContainsKey(offset))
                {
                    //Console.WriteLine("Updating offset {0} for {1} (now {2})", offset, prop.Name, updatedOffsets[offset]);
                    prop.SetValue(entry, (int)updatedOffsets[offset], null);
                }
            }
        }

		/// <summary>
		/// Returns a CSV table with the contents of this dat container.
		/// </summary>
		/// <returns></returns>
		public string GetCSV()
		{
			const char seperator = ',';
			StringBuilder sb = new StringBuilder();

			bool displayedHeader = false;
			foreach (var item in Records)
			{
				var properties = item.GetType().GetProperties();

				if (!displayedHeader)
				{
					foreach (var propertyInfo in properties)
					{
						sb.AppendFormat("{0}{1}", propertyInfo.Name, seperator);
					}
					sb.Remove(sb.Length - 1, 1);
					sb.AppendLine();
					displayedHeader = true;
				}

				foreach (var propertyInfo in properties)
				{
					object fieldValue = propertyInfo.GetValue(item, null);
					object[] customAttributes = propertyInfo.GetCustomAttributes(false);

					if (customAttributes.Length > 0)
					{
						if (customAttributes.Any(n => n is UInt64Index) || customAttributes.Any(n => n is UInt32Index) || customAttributes.Any(n => n is Int32Index))
						{
							sb.AppendFormat("{0}{1}", fieldValue, seperator);
						}
						else if (customAttributes.Any(n => n is StringIndex))
						{
							sb.AppendFormat("\"{0}\"{1}", DataEntries[(int)fieldValue].ToString().Replace("\"", "\"\""), seperator);
						}
						else
						{
							sb.AppendFormat("{0}{1}", DataEntries[(int)fieldValue].ToString().Replace("\"", "\"\""), seperator);
						}
					}
					else
					{
						sb.AppendFormat("{0}{1}", fieldValue, seperator);
					}
				}
				sb.Remove(sb.Length - 1, 1);
				sb.AppendLine();
			}

			return sb.ToString();
		}


    }
}
