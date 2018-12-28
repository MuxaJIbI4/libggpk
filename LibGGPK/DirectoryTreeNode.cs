using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using LibGGPK.Records;

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
        private string _directoryPath;

        /// <summary>
        /// Traverses a directory tree in PostOrder (Preform directory action, then traverse children) 
        /// and preforms actions on the files and directories
        /// </summary>
        /// <param name="root">Root of directory tree</param>
        /// <param name="directoryAction">Action to preform on each directory</param>
        /// <param name="fileAction">Action to preform on each file</param>
        public static void TraverseTreePreorder(
            DirectoryTreeNode root, 
            Action<DirectoryTreeNode> directoryAction, 
            Action<FileRecord> fileAction)
        {
            foreach (var childDirectory in root.Children)
            {
                if (directoryAction != null)
                {
                    directoryAction(childDirectory);
                }
                TraverseTreePreorder(childDirectory, directoryAction, fileAction);
            }

            if (fileAction == null) return;
            foreach (var file in root.Files)
            {
                fileAction(file);
            }
        }

        /// <summary>
        /// Traverses a directory tree in PostOrder (Traverse children, then preform directory action) 
        /// and preforms actions on the files and directories
        /// </summary>
        /// <param name="root">Root of directory tree</param>
        /// <param name="directoryAction">Action to preform on each directory</param>
        /// <param name="fileAction">Action to preform on each file</param>
        public static void TraverseTreePostorder(
            DirectoryTreeNode root, 
            Action<DirectoryTreeNode> directoryAction, 
            Action<FileRecord> fileAction)
        {
            foreach (var childDirectory in root.Children)
            {
                TraverseTreePostorder(childDirectory, directoryAction, fileAction);
                if (directoryAction != null)
                {
                    directoryAction(childDirectory);
                }
            }

            if (fileAction == null) return;
            foreach (var file in root.Files)
            {
                fileAction(file);
            }
        }

        /// <summary>
        /// Gets the absolute directory of this node
        /// </summary>
        /// <returns>Absolute directory of this node</returns>
        public string GetDirectoryPath()
        {
            if (_directoryPath != null)
                return _directoryPath;

            var pathQueue = new Stack<string>();
            var sb = new StringBuilder();

            // Traverse the directory tree until we hit the root node, pushing all
            //  encountered directory names onto the stack
            var iter = this;
            while (iter != null && iter.Name.Length > 0)
            {
                pathQueue.Push(iter.Name);
                iter = iter.Parent;
            }

            foreach (var item in pathQueue)
            {
                sb.Append(item + Path.DirectorySeparatorChar);
            }

            _directoryPath = sb.ToString();
            return _directoryPath;
        }

        public override string ToString()
        {
            return Name;
        }

        public int CompareTo(object obj)
        {
            if (!(obj is DirectoryTreeNode))
                throw new Exception("Can only compare DirectoryTreeNodes");

            return String.Compare(Name, (obj as DirectoryTreeNode).Name, StringComparison.Ordinal);
        }

        public void RemoveFile(FileRecord file)
        {
            if (!Files.Contains(file))
                throw new Exception("Tried to removed file than not belong to this directory: " + Name);

            // remove from DirectoryTreeNode
            Files.Remove(file);

            // remove from DirectoryRecord
            var entry = Record.Entries.FirstOrDefault(n => n.Offset == file.RecordBegin);
            Record.Entries.Remove(entry);
        }

        public void RemoveDirectory(DirectoryTreeNode dir)
        {
            if (!Children.Contains(dir))
                throw new Exception("Tried to removed directory than not belong to this directory: " + Name);

            // remove from DirectoryTreeNode
            Children.Remove(dir);

            // remove from DirectoryRecord
            var entry = Record.Entries.FirstOrDefault(n => n.Offset == dir.Record.RecordBegin);
            Record.Entries.Remove(entry);
        }
    };


}
