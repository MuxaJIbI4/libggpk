using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LibGGPK
{
	/// <summary>
	/// Node for building directory trees
	/// </summary>
	public class DirectoryTreeNode : IComparable
	{
		/// <summary>
		/// Parent directory tree node
		/// </summary>
		public DirectoryTreeNode Parent;
		/// <summary>
		/// Child directory tree nodes
		/// </summary>
		public List<DirectoryTreeNode> Children;
		/// <summary>
		/// File records contained in this directory
		/// </summary>
		public List<FileRecord> Files;
		/// <summary>
		/// Directory name
		/// </summary>
		public string Name;
		/// <summary>
		/// PDIR record this directory tree node is for
		/// </summary>
		public DirectoryRecord Record;
		/// <summary>
		/// Cached directory path so we don't need to recalculate it
		/// </summary>
		private string directoryPath = null;

		/// <summary>
		/// Traverses a directory tree in PostOrder (Preform directory action, then traverse children) and preforms actions on the files and directories
		/// </summary>
		/// <param name="root">Root of directory tree</param>
		/// <param name="directoryAction">Action to preform on each directory</param>
		/// <param name="fileAction">Action to preform on each file</param>
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

		/// <summary>
		/// Traverses a directory tree in PostOrder (Traverse children, then preform directory action) and preforms actions on the files and directories
		/// </summary>
		/// <param name="root">Root of directory tree</param>
		/// <param name="directoryAction">Action to preform on each directory</param>
		/// <param name="fileAction">Action to preform on each file</param>
		public static void TraverseTreePostorder(DirectoryTreeNode root, Action<DirectoryTreeNode> directoryAction, Action<FileRecord> fileAction)
		{
			foreach (var childDirectory in root.Children)
			{
				TraverseTreePostorder(childDirectory, directoryAction, fileAction);
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

		/// <summary>
		/// Gets the absolute directory of this node
		/// </summary>
		/// <returns>Absolute directory of this node</returns>
		public string GetDirectoryPath()
		{
			if (directoryPath != null)
				return directoryPath;

			Stack<string> pathQueue = new Stack<string>();
			StringBuilder sb = new StringBuilder();

			// Traverse the directory tree until we hit the root node, pushing all
			//  encountered directory names onto the stack
			DirectoryTreeNode iter = this;
			while (iter != null && iter.Name.Length > 0)
			{
				pathQueue.Push(iter.Name);
				iter = iter.Parent;
			}

			foreach (var item in pathQueue)
			{
				sb.Append(item + Path.DirectorySeparatorChar);
			}

			directoryPath = sb.ToString();
			return directoryPath;
		}

		public override string ToString()
		{
			return Name;
		}


		public int CompareTo(object obj)
		{
			if (!(obj is DirectoryTreeNode))
				throw new NotImplementedException("Can only compare DirectoryTreeNodes");

			return Name.CompareTo((obj as DirectoryTreeNode).Name);
		}
	};


}
