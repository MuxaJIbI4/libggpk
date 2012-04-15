using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LibGGPK
{
	public class DirectoryRecord : BaseRecord
	{
		public const string Tag = "PDIR";

		public struct DirectoryEntry
		{
			public int Unknown;
			public long Offset;
		}

		public byte[] Hash;
		public string Name;
		public DirectoryEntry[] Entries;
		public long EntriesBegin;

		public DirectoryRecord(uint length, BinaryReader br)
		{
			RecordBegin = br.BaseStream.Position - 8;
			Length = length;
			Read(br);
		}

		public override void Read(BinaryReader br)
		{
			int nameLength = br.ReadInt32();
			int totalEntries = br.ReadInt32();

			Hash = br.ReadBytes(32);
			Name = ASCIIEncoding.Unicode.GetString(br.ReadBytes(2 * (nameLength - 1)));
			br.ReadBytes(2); // Null terminator

			EntriesBegin = br.BaseStream.Position;
			Entries = new DirectoryEntry[totalEntries];
			for (int i = 0; i < totalEntries; i++)
			{
				Entries[i] = new DirectoryEntry()
				{
					Unknown = br.ReadInt32(),
					Offset = br.ReadInt64(),
				};
			}
		}

		public override string ToString()
		{
			return Name;
		}

		public void UpdateOffset(string ggpkPath, long previousEntryOffset, long newEntryOffset)
		{
			int entryIndex = -1;

			for (int i = 0; i < Entries.Length; i++)
			{
				if (Entries[i].Offset == previousEntryOffset)
				{
					entryIndex = i;
					break;
				}
			}

			if (entryIndex == -1)
				throw new Exception("Entry not found!");

			using (FileStream ggpkFileStream = File.Open(ggpkPath, FileMode.Open))
			{
				// See DirectoryEntry struct, seeks the offset portion of the specified entry
				ggpkFileStream.Seek(EntriesBegin + 12*entryIndex + 4, SeekOrigin.Begin);
				BinaryWriter bw = new BinaryWriter(ggpkFileStream);
				bw.Write(newEntryOffset);
			}
		}
	}
}
