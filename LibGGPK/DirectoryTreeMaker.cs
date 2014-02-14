using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibGGPK
{
	/// <summary>
	/// Helper class to generate directory trees
	/// </summary>
	internal static class DirectoryTreeMaker
	{
		/// <summary>
		/// Builds a directory tree by traversing the list of record headers found in the pack file
		/// </summary>
		/// <param name="recordOffsets">Map of record offsets and headers to create directory tree from</param>
		/// <returns>Root node of directory tree</returns>
		internal static DirectoryTreeNode BuildDirectoryTree(Dictionary<long, BaseRecord> recordOffsets)
		{
			// This offset is a directory, add it as a child of root and process all of it's entries
			GGPKRecord currentDirectory = recordOffsets[0] as GGPKRecord;

			DirectoryTreeNode root = new DirectoryTreeNode()
			{
				Children = new List<DirectoryTreeNode>(),
				Files = new List<FileRecord>(),
				Name = "ROOT",
				Parent = null,
				Record = null,
			};

			// First level only contains a empty string name directory record and a free record
			foreach (var Offset in currentDirectory.RecordOffsets)
			{
				BuildDirectoryTree(Offset, root, recordOffsets);
			}

			return root;
		}

		/// <summary>
		/// Recursivly creates a directory tree by traversing PDIR records. Adds FILE records to the current directory
		/// tree node. Recursivly traverses PDIR records and adds them to the current directory tree node's children.
		/// </summary>
		/// <param name="fileOffset">Offset of this record being traversed</param>
		/// <param name="root">Parent node</param>
		/// <param name="recordOffsets">Map of record offsets and headers to create directory tree from</param>
		private static void BuildDirectoryTree(long fileOffset, DirectoryTreeNode root, Dictionary<long, BaseRecord> recordOffsets)
		{
			if(!recordOffsets.ContainsKey(fileOffset))
			{
				return;
			}

			if(recordOffsets[fileOffset] is DirectoryRecord)
			{
				// This offset is a directory, add it as a child of root and process all of it's entries
				DirectoryRecord currentDirectory = recordOffsets[fileOffset] as DirectoryRecord;
				DirectoryTreeNode child = new DirectoryTreeNode()
				{
					Name = currentDirectory.Name,
					Parent = root,
					Children = new List<DirectoryTreeNode>(),
					Files = new List<FileRecord>(),
					Record = currentDirectory,
				};

				root.Children.Add(child);

				foreach (var item in currentDirectory.Entries)
				{
					BuildDirectoryTree(item, child, recordOffsets);
				}
			}
		}

		/// <summary>
		/// Recursivly creates a directory tree by traversing PDIR records. Adds FILE records to the current directory
		/// tree node. Recursivly traverses PDIR records and adds them to the current directory tree node's children.
		/// </summary>
		/// <param name="directoryEntry">Directory Entry</param>
		/// <param name="root">Parent node</param>
		/// <param name="recordOffsets">Map of record offsets and headers to create directory tree from</param>
		private static void BuildDirectoryTree(DirectoryRecord.DirectoryEntry directoryEntry, DirectoryTreeNode root, Dictionary<long, BaseRecord> recordOffsets)
		{
			if (!recordOffsets.ContainsKey(directoryEntry.Offset))
			{
				return;
			}

			if (recordOffsets[directoryEntry.Offset] is DirectoryRecord)
			{
				// This offset is a directory, add it as a child of root and process all of it's entries
				DirectoryRecord currentDirectory = recordOffsets[directoryEntry.Offset] as DirectoryRecord;
				currentDirectory.EntryNameHash = directoryEntry.EntryNameHash;
				DirectoryTreeNode child = new DirectoryTreeNode()
				{
					Name = currentDirectory.Name,
					Parent = root,
					Children = new List<DirectoryTreeNode>(),
					Files = new List<FileRecord>(),
					Record = currentDirectory,
				};

				root.Children.Add(child);

				foreach (var entry in currentDirectory.Entries)
				{
					BuildDirectoryTree(entry, child, recordOffsets);
				}
			}
			else if (recordOffsets[directoryEntry.Offset] is FileRecord)
			{
				FileRecord currentFile = recordOffsets[directoryEntry.Offset] as FileRecord;
				// Skip empty .dat Files under Data/
				if (root.Name.Equals("Data") && currentFile.DataLength == 12)
					return;
				currentFile.ContainingDirectory = root;
				currentFile.EntryNameHash = directoryEntry.EntryNameHash;
				root.Files.Add(currentFile);
			}
		}
	}
}
