using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LibGGPK
{
	/// <summary>
	/// Represents a directory in the pack file. Each directory contains a list of records of Files and Directories that
	/// exist in this directory.
	/// </summary>
	public sealed class DirectoryRecord : BaseRecord
	{
		public const string Tag = "PDIR";

		public struct DirectoryEntry
		{
			/// <summary>
			/// Murmur2 hash of lowercase entry name
			/// </summary>
			public int EntryNameHash;
			/// <summary>
			/// Offset in pack file where the record begins
			/// </summary>
			public long Offset;
		}

		/// <summary>
		/// SHA256 hash of ... something
		/// </summary>
		public byte[] Hash;
		/// <summary>
		/// Name of directory
		/// </summary>
		public string Name;
		/// <summary>
		/// Records this directory contains. Each entry is an offset in the pack file of the record.
		/// </summary>
		public DirectoryEntry[] Entries;
		/// <summary>
		/// Offset in pack file where entries list begins. This is only here because it makes rewriting the entries list easier.
		/// </summary>
		public long EntriesBegin;

		public DirectoryRecord(uint length, BinaryReader br)
		{
			RecordBegin = br.BaseStream.Position - 8;
			Length = length;
			Read(br);
		}

		/// <summary>
		/// Reads the PDIR record entry from the specified stream
		/// </summary>
		/// <param name="br">Stream pointing at a PDIR record</param>
		public override void Read(BinaryReader br)
		{
			int nameLength = br.ReadInt32();
			int totalEntries = br.ReadInt32();

			Hash = br.ReadBytes(32);
			Name = Encoding.Unicode.GetString(br.ReadBytes(2 * (nameLength - 1)));
			br.BaseStream.Seek(2, SeekOrigin.Current); // Null terminator

			EntriesBegin = br.BaseStream.Position;
			Entries = new DirectoryEntry[totalEntries];
			for (int i = 0; i < totalEntries; i++)
			{
				Entries[i] = new DirectoryEntry()
				{
					EntryNameHash = br.ReadInt32(),
					Offset = br.ReadInt64(),
				};
			}
		}

		/// <summary>
		/// Updates the location of an entry in this directory.
		/// </summary>
		/// <param name="ggpkPath">Path of pack file that contains this record</param>
		/// <param name="previousEntryOffset">Previous offset of entry</param>
		/// <param name="newEntryOffset">New offset of entry</param>
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
			{
				throw new ApplicationException("Entry not found!");
			}

			using (FileStream ggpkFileStream = File.Open(ggpkPath, FileMode.Open))
			{
				// Jump to the location of 'Entries' in the ggpk file and change the entry for 'previousEntryOffset'
				//  to 'newEntryOffset'
				ggpkFileStream.Seek(EntriesBegin + 12 * entryIndex + 4, SeekOrigin.Begin);
				BinaryWriter bw = new BinaryWriter(ggpkFileStream);
				bw.Write(newEntryOffset);
				Entries[entryIndex].Offset = newEntryOffset;
			}
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
