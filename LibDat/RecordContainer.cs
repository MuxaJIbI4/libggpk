using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LibDat
{
	public class DatContainer<T> where T : BaseDat
	{
		public Dictionary<int, BaseData> DataEntries = new Dictionary<int, BaseData>();
		public List<T> Entries;
		public int DataTableBegin;

		private byte[] originalDataTable;


		public DatContainer(BinaryReader inStream)
		{
			Read(inStream);
		}

		public DatContainer(string fileName)
		{
			byte[] fileBytes = File.ReadAllBytes(fileName);

			using (MemoryStream ms = new MemoryStream(fileBytes))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					Read(br);
				}
			}
		}

		private void AddDataToTable(T entry, BinaryReader inStream)
		{
			var properties = typeof(T).GetProperties();
			foreach (var prop in properties)
			{
				object[] customAttributes = prop.GetCustomAttributes(false);

				if (customAttributes.Length == 0)
					continue;

				int offset = (int)prop.GetValue(entry, null);
				if (DataEntries.ContainsKey(offset))
				{
					continue;
				}

				if (customAttributes[0] is StringIndex)
				{
					DataEntries[offset] = new UnicodeString(inStream, offset, DataTableBegin);
				}
				else if (customAttributes[0] is DataIndex)
				{
					DataEntries[offset] = new UnkownData(inStream, offset, DataTableBegin);
				}
			}
		}

		private void UpdateDataOffsets(T entry, Dictionary<int, int> updatedOffsets)
		{
			var properties = typeof(T).GetProperties();
			foreach (var prop in properties)
			{
				object[] customAttributes = prop.GetCustomAttributes(false);

				if (customAttributes.Length == 0)
					continue;

				int offset = (int)prop.GetValue(entry, null);
				if (updatedOffsets.ContainsKey(offset))
				{
					Console.WriteLine("Updating offset {0} for {1} (now {2})", offset, prop.Name, updatedOffsets[offset]);
					prop.SetValue(entry, updatedOffsets[offset], null);
				}
			}
		}

		private void Read(BinaryReader inStream)
		{
			int numberOfEntries = inStream.ReadInt32();
			Entries = new List<T>(numberOfEntries);

			for (int i = 0; i < numberOfEntries; i++)
			{
				// TODO: Skip reflection if it's running slow (compiled lambda?)
				T newEntry = (T)Activator.CreateInstance(typeof(T), new object[] { inStream });
				Entries.Add(newEntry);
			}

			if (inStream.ReadUInt64() != 0xBBbbBBbbBBbbBBbb)
			{
				throw new ApplicationException("Missing magic number after records");
			}

			DataTableBegin = (int)(inStream.BaseStream.Position - 8);
			inStream.BaseStream.Seek(-8, SeekOrigin.Current);
			originalDataTable = inStream.ReadBytes((int)(inStream.BaseStream.Length - inStream.BaseStream.Position));

			// Read all referenced string and data entries from the data following the entries (starting at magic number)
			foreach (var item in Entries)
			{
				AddDataToTable(item, inStream);
			}
		}

		public void Save(string fileName)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				try
				{
					using (BinaryWriter outStream = new BinaryWriter(ms))
					{
						Save(outStream);
					}
				}
				catch (Exception)
				{
					throw;
				}

				File.WriteAllBytes(fileName, ms.GetBuffer());
			}
		}

		public void Save(BinaryWriter outStream)
		{
			// Mapping of the new string and data offsets
			Dictionary<int, int> changedOffsets = new Dictionary<int, int>();

			outStream.Write(Entries.Count);

			if (Entries.Count > 0)
			{
				// Pretty ugly way to zero out the for sizeof(Entry) * EntryCount bytes
				outStream.Write(new byte[(Entries[0] as BaseDat).GetSize() * Entries.Count]);
			}

			int newStartOfDataSection = (int)outStream.BaseStream.Position;
			outStream.Write(originalDataTable);

			foreach (var item in DataEntries)
			{
				if (!(item.Value is UnicodeString))
					continue;

				UnicodeString str = item.Value as UnicodeString;
				if (!string.IsNullOrWhiteSpace(str.NewData))
				{
					str.Save(outStream);
					changedOffsets[str.Offset] = str.NewOffset;
				}
			}

			// Go back to the beginning and write the real entries
			outStream.BaseStream.Seek(4, SeekOrigin.Begin);

			// Now we must go through each StringIndex and DataIndex and update the index then save it
			foreach (var item in Entries)
			{
				UpdateDataOffsets(item, changedOffsets);
				item.Save(outStream);
			}
		}
	}
}
