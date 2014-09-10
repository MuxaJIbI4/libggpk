using System;
using System.Collections.Generic;
using System.IO;

namespace LibGGPK
{
    /// <summary>
    /// Handles parsing the GGPK pack file.
    /// </summary>
    public class GGPK
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

        public GGPK()
        {
            RecordOffsets = new Dictionary<long, BaseRecord>(EstimatedFileCount);
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
                        {
                            output(String.Format("{0:00.00}%{1}", 100.0 * percentComplete, Environment.NewLine));
                        }

                        previousPercentComplete = percentComplete;
                    }
                }
                if (output != null)
                {
                    output(String.Format("{0:00.00}%{1}", 100.0f * br.BaseStream.Position / (float)br.BaseStream.Length, Environment.NewLine));
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
            DirectoryRoot = DirectoryTreeMaker.BuildDirectoryTree(RecordOffsets);
            FreeRoot = FreeListMaker.BuildFreeList(RecordOffsets);
        }
    }
}
