using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibGGPK
{
	internal static class DirectoryTreeMaker
	{
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
			};

			foreach (var item in currentDirectory.RecordOffsets)
			{
				BuildDirectoryTree(item, root, recordOffsets);
			}

			return root;
		}

		internal static void BuildDirectoryTree(long fileOffset, DirectoryTreeNode root, Dictionary<long, BaseRecord> recordOffsets)
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
					BuildDirectoryTree(item.Offset, child, recordOffsets);
				}
			}
			else if (recordOffsets[fileOffset] is FileRecord)
			{
				FileRecord currentFile = recordOffsets[fileOffset] as FileRecord;
				currentFile.ContainingDirectory = root;
				root.Files.Add(currentFile);
			}
		}

	}
}
