using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LibGGPK
{
    /// <summary>
    /// Handles parsing the GGPK pack file.
    /// </summary>
    public class GrindingGearsPackageContainer
    {
        /// <summary>
        /// Map of every records offsets in the pack file
        /// TODO make it immutable to other classes
        /// </summary>
        public Dictionary<long, BaseRecord> RecordOffsets { get; private set; }
        /// <summary>
        /// Root of the directory tree
        /// </summary>
        public DirectoryTreeNode DirectoryRoot;
        /// <summary>
        /// Root of the Free record list
        /// </summary>
        public LinkedList<FreeRecord> FreeRoot;
        /// <summary>
        /// An estimation of the number of records in the Contents.GGPK file. This is only
        /// used to inform the users of the parsing progress.
        /// </summary>
        private const int EstimatedFileCount = 175000;

        public bool IsReadOnly { get { return _isReadOnly; } }
        private bool _isReadOnly;
        private List<FileRecord> _files;
        private List<DirectoryRecord> _directories;
        private List<FreeRecord> _freeRecords;

        public GrindingGearsPackageContainer()
        {
            RecordOffsets = new Dictionary<long, BaseRecord>(EstimatedFileCount);
            _files = new List<FileRecord>();
            _directories = new List<DirectoryRecord>();
            _freeRecords = new List<FreeRecord>();
        }

        /// <summary>
        /// Iterates through the pack file and records the offsets and headers of each record found
        /// </summary>
        /// <param name="pathToGgpk">Path to pack file to read</param>
        /// <param name="output">Output function</param>
        private void ReadRecordOffsets(string pathToGgpk, Action<string> output)
        {
            var previousPercentComplete = 0.0f;

            using (var fs = OpenFile(pathToGgpk, out _isReadOnly))
            {
                var br = new BinaryReader(fs);
                var streamLength = br.BaseStream.Length;

                while (br.BaseStream.Position < streamLength)
                {
                    var currentOffset = br.BaseStream.Position;
                    var record = RecordFactory.ReadRecord(br);
                    RecordOffsets.Add(currentOffset, record);

                    var percentComplete = currentOffset / (float)streamLength;
                    if (percentComplete - previousPercentComplete >= 0.10f)
                    {
                        if (output != null)
                            output(String.Format("\t{0:00.00}%{1}", 100.0 * percentComplete, Environment.NewLine));
                        previousPercentComplete = percentComplete;
                    }
                }
                if (output != null)
                {
                    var percentReady = 100.0f * br.BaseStream.Position / br.BaseStream.Length;
                    output(String.Format("\t{0:00.00}%{1}", percentReady, Environment.NewLine));
                }
            }
        }

        private static FileStream OpenFile(string path, out bool isReadOnly)
        {
            isReadOnly = true;
            try
            {
                var ret = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                isReadOnly = false;
                return ret;
            }
            catch (IOException)
            {
                // File can't be written to, since it's being used (either by the program, or by the game itself)
                return File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }
        }

        /// <summary>
        /// Parses the GGPK pack file and builds a directory tree from it.
        /// </summary>
        /// <param name="pathToGgpk">Path to pack file to read</param>
        /// <param name="output">Output function</param>
        public void Read(string pathToGgpk, Action<string> output)
        {
            if (output != null)
            {
                output("Parsing GGPK..." + Environment.NewLine);
                output("Searching file for records:" + Environment.NewLine);
            }

            ReadRecordOffsets(pathToGgpk, output);

            if (output != null)
            {
                output(Environment.NewLine);
                output("Building directory tree..." + Environment.NewLine);
            }

            CreateDirectoryTree(output);
        }

        /// <summary>
        /// Creates a directory tree using the parsed record headers and offsets
        /// </summary>
        /// <param name="output">Output function</param>
        private void CreateDirectoryTree(Action<string> output)
        {
            DirectoryRoot = BuildDirectoryTree();
            output(String.Format("Found {0} directories and {1} files", _directories.Count, _files.Count) + Environment.NewLine);
            FreeRoot = BuildFreeList();
            output(String.Format("Found {0} FREE records", _freeRecords.Count) + Environment.NewLine);
        }

        #region DirectoryTreeMaker

        /// <summary>
        /// Builds a directory tree by traversing the list of record headers found in the pack file
        /// </summary>
        /// <returns>Root node of directory tree</returns>
        private DirectoryTreeNode BuildDirectoryTree()
        {
            var ggpkRecord = RecordOffsets[0] as GGPKRecord;
            if (ggpkRecord == null)
                throw new Exception("First record isn't GGPK record");

            // First level only contains a empty string name directory record and a free record
            var rootDirectoryOffset = ggpkRecord.RecordOffsets
                .Where(RecordOffsets.ContainsKey).FirstOrDefault(o => RecordOffsets[o] is DirectoryRecord);
            if (rootDirectoryOffset == 0) // default value
                throw new Exception("Couldn't find root directory offset");

            var firstDirectory = RecordOffsets[rootDirectoryOffset] as DirectoryRecord;
            if (firstDirectory == null)
                throw new Exception("Couldn't find root directory record");

            var root = new DirectoryTreeNode
            {
                Children = new List<DirectoryTreeNode>(),
                Files = new List<FileRecord>(),
                Name = "ROOT",
                Parent = null,
                Record = null,
            };

            // start building files tree
            foreach (var item in firstDirectory.Entries)
            {
                BuildDirectoryTree(item, root);
            }
            return root;
        }

        /// <summary>
        /// Recursivly creates a directory tree by traversing PDIR records. Adds FILE records to the current directory
        /// tree node. Recursivly traverses PDIR records and adds them to the current directory tree node's children.
        /// </summary>
        /// <param name="directoryEntry">Directory Entry</param>
        /// <param name="root">Parent node</param>
        private void BuildDirectoryTree(DirectoryRecord.DirectoryEntry directoryEntry, DirectoryTreeNode root)
        {
            if (!RecordOffsets.ContainsKey(directoryEntry.Offset))
                return; // TODO throw error

            if (RecordOffsets[directoryEntry.Offset] is DirectoryRecord)
            {
                // This offset is a directory, add it as a child of root and process all of it's entries
                var currentDirectory = RecordOffsets[directoryEntry.Offset] as DirectoryRecord;
                _directories.Add(currentDirectory);
                var child = new DirectoryTreeNode
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
                    BuildDirectoryTree(entry, child);
                }
            }
            else if (RecordOffsets[directoryEntry.Offset] is FileRecord)
            {
                var currentFile = RecordOffsets[directoryEntry.Offset] as FileRecord;
                _files.Add(currentFile);
                currentFile.ContainingDirectory = root;
                root.Files.Add(currentFile);
            }
        }

        #endregion

        #region FreeListMaker

        /// <summary>
        /// Builds a linked list of FREE records by traversing FREE records in GGPK file
        /// </summary>
        /// <returns>Linked list containing list of FREE records</returns>
        private LinkedList<FreeRecord> BuildFreeList()
        {
            var ggpkRecord = RecordOffsets[0] as GGPKRecord;
            if (ggpkRecord == null)
                throw new Exception("First record isn't GGPK record");

            // First level only contains a empty string name directory record and a free record
            var firstFreeRecordOffset = ggpkRecord.RecordOffsets
                .Where(RecordOffsets.ContainsKey).FirstOrDefault(o => RecordOffsets[o] is FreeRecord);
            if (firstFreeRecordOffset == 0) // default value
                throw new Exception("Couldn't find first FREE record offset");

            var currentFreeRecord = RecordOffsets[firstFreeRecordOffset] as FreeRecord;
            if (currentFreeRecord == null)
                throw new Exception("Couldn't find first FREE record");

            var freeList = new LinkedList<FreeRecord>();
            while (true)
            {
                freeList.AddLast(currentFreeRecord);
                _freeRecords.Add(currentFreeRecord);
                var nextFreeOFfset = currentFreeRecord.NextFreeOffset;

                if (nextFreeOFfset == 0)
                    break;

                if (!RecordOffsets.ContainsKey(nextFreeOFfset))
                    throw new Exception("Failed to find next FREE record in map of record offsets");

                currentFreeRecord = RecordOffsets[currentFreeRecord.NextFreeOffset] as FreeRecord;

                if (currentFreeRecord == null)
                    throw new Exception("Found a record that wasn't a FREE record while looking for next FREE record");
            }

            return freeList;
        }
        #endregion
    }
}
