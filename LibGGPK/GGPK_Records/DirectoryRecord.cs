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

		public DirectoryRecord(uint length, BinaryReader br)
		{
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
	}
}
