using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibGGPK
{
	public class DirectoryTreeNode
	{
		public DirectoryTreeNode Parent;
		public List<DirectoryTreeNode> Children;
		public List<FileRecord> Files;
		public string Name;
		public DirectoryRecord Record;

		public override string ToString()
		{
			return Name;
		}

		enum TraversalOrder
		{
			Preorder,
			Postorder,
		}

		public static void TraverseTreePreorder(DirectoryTreeNode root, Action<DirectoryTreeNode> directoryAction, Action<FileRecord> fileAction)
		{
			foreach (var childDirectory in root.Children)
			{
				if (directoryAction != null)
				{
					directoryAction(childDirectory);
				}
				TraverseTreePreorder(childDirectory, directoryAction, fileAction);
			}

			if (fileAction != null)
			{
				foreach (var file in root.Files)
				{
					fileAction(file);
				}
			}
		}

		public static void TraverseTreePostorder(DirectoryTreeNode root, Action<DirectoryTreeNode> directoryAction, Action<FileRecord> fileAction)
		{
			foreach (var childDirectory in root.Children)
			{
				TraverseTreePreorder(childDirectory, directoryAction, fileAction);
				if (directoryAction != null)
				{
					directoryAction(childDirectory);
				}
			}

			if (fileAction != null)
			{
				foreach (var file in root.Files)
				{
					fileAction(file);
				}
			}
		}
	};


}
