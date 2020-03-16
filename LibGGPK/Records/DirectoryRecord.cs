using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LibGGPK.Records
{
    /// <summary>
    /// Represents a directory in the pack file. Each directory contains a list of records of Files and Directories that
    /// exist in this directory.
    /// </summary>
    public sealed class DirectoryRecord : BaseRecord
    {
        public struct DirectoryEntry
        {
            /// <summary>
            /// Murmur2 hash of lowercase entry name
            /// </summary>
            public uint EntryNameHash;
            /// <summary>
            /// Offset in pack file where the record begins
            /// </summary>
            public long Offset;
        }

        public const string Tag = "PDIR";

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
        public List<DirectoryEntry> Entries;
        /// <summary>
        /// Offset in pack file where entries list begins. This is only here because it makes rewriting the entries list easier.
        /// </summary>
        public long EntriesBegin;

        public DirectoryRecord(uint length, BinaryReader br)
        {
            RecordBegin = br.BaseStream.Position - 8;
            Length = length;
            Entries = new List<DirectoryEntry>();
            Read(br); 
        }

        public DirectoryRecord(long recordBegin, uint length, byte[] hash, string name, long entriesBegin, List<DirectoryEntry> entries)
        {
            RecordBegin = recordBegin;
            Length = length;
            Hash = hash;
            Name = name;
            EntriesBegin = entriesBegin;
            Entries = entries;
        }

        /// <summary>
        /// Reads the PDIR record entry from the specified stream
        /// </summary>
        /// <param name="br">Stream pointing at a PDIR record</param>
        public override void Read(BinaryReader br)
        {
            var nameLength = br.ReadInt32();
            var totalEntries = br.ReadInt32();

            Hash = br.ReadBytes(32);
            Name = Encoding.Unicode.GetString(br.ReadBytes(2 * (nameLength - 1)));
            br.BaseStream.Seek(2, SeekOrigin.Current); // Null terminator

            EntriesBegin = br.BaseStream.Position;
            for (var i = 0; i < totalEntries; i++)
            {
                Entries.Add(new DirectoryEntry
                {
                    EntryNameHash = br.ReadUInt32(),
                    Offset = br.ReadInt64(),
                });
            }
        }

        public override void Write(BinaryWriter bw, Dictionary<long, long> changedOffsets)
        {
            var currentOffset = bw.BaseStream.Position;
            if (currentOffset != RecordBegin)
                changedOffsets[RecordBegin] = currentOffset;

            bw.Write(Length);
            bw.Write(Encoding.ASCII.GetBytes(Tag));
            bw.Write(Name.Length + 1);
            bw.Write(Entries.Count);
            bw.Write(Hash);
            bw.Write(Encoding.Unicode.GetBytes(Name));
            bw.Write((short)0);

            foreach (var entry in Entries)
            {
                bw.Write(entry.EntryNameHash);
                var offset = entry.Offset;
                bw.Write(changedOffsets.ContainsKey(offset) ? changedOffsets[offset] : offset);
            }
        }

        /// <summary>
        /// Updates the location of an entry in this directory.
        /// </summary>
        /// <param name="ggpkPath">Path of pack file that contains this record</param>
        /// <param name="nameHash">Previous name hash of entry</param>
        /// <param name="newEntryOffset">New offset of entry</param>
        public void UpdateOffset(string ggpkPath, uint nameHash, long newEntryOffset)
        {
            var entry = Entries.FirstOrDefault(e => e.EntryNameHash == nameHash);
            if (entry.Offset == 0 )
                throw new ApplicationException("Entry not found!");

            using (var ggpkFileStream = File.Open(ggpkPath, FileMode.Open))
            {
                // Jump to the location of 'Entries' in the ggpk file and 
                // change the entry for 'previousEntryOffset' to 'newEntryOffset'
                ggpkFileStream.Seek(EntriesBegin + 12 * Entries.IndexOf(entry) + 4, SeekOrigin.Begin);
                var bw = new BinaryWriter(ggpkFileStream);
                bw.Write(newEntryOffset);
                entry.Offset = newEntryOffset;
            }
        }

        public uint GetNameHash()
        {
            return Murmur.Hash2(Name);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
