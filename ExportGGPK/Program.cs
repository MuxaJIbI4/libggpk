using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibGGPK;
using LibGGPK.Records;

namespace ExportGGPK
{
    class Program
    {
        private static readonly GrindingGearsPackageContainer Container = new GrindingGearsPackageContainer();
        private static readonly Dictionary<string, List<FileRecord>> Data = new Dictionary<string, List<FileRecord>>();

        private static string contentPath;
        private static string outputPath;

        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: ExportGGPK.exe <path to ggpk> <base output dir>");
                return;
            }

            contentPath = args[0];
            outputPath = args[1];

            if (!File.Exists(contentPath))
            {
                Console.WriteLine($"Content pack file is not available at ${contentPath}");
                return;
            }

            Container.Read(contentPath, Console.WriteLine);

            var art = GetDirectory(@"Art\2DItems");
            var gameData = GetDirectory("Data");

            var artItems = RecursiveFindByType(FileRecord.DataFormat.TextureDds, art);
            var dataItems = RecursiveFindByType(FileRecord.DataFormat.Dat, gameData);

            Console.WriteLine($"Found {artItems.Count} textures!");
            Console.WriteLine($"Found {dataItems.Count} data files!");

            Extract(artItems);
            Extract(dataItems);
        }

        private static void Extract(IEnumerable<FileRecord> items)
        {
            foreach (var item in items)
            {
                item.ExtractFileWithDirectoryStructure(contentPath, outputPath);
            }
        }

        private static DirectoryTreeNode GetDirectory(string path)
        {
            var dirs = path.Split(Path.DirectorySeparatorChar);
            var currDir = Container.DirectoryRoot;

            foreach (var dir in dirs)
            {
                currDir = WalkNode(currDir, dir);
            }

            return currDir;
        }

        private static DirectoryTreeNode WalkNode(DirectoryTreeNode start, string subDirectory)
        {
            return start.Children.Find((val) => val.Name == subDirectory);
        }

        public static IEnumerable<FileRecord> GetFiles(string subDirectory)
        {
            var dir = WalkNode(Container.DirectoryRoot, subDirectory);
            return dir == null ? new List<FileRecord>() : dir.Files;
        }

        public static IEnumerable<FileRecord> FilterByType(FileRecord.DataFormat format, List<FileRecord> records)
        {
            return records.Where((record) => record.FileFormat.Equals(format));
        }

        private static List<FileRecord> RecursiveFindByType(FileRecord.DataFormat type, DirectoryTreeNode currentNode, List<FileRecord> roller = null)
        {
            if (roller == null)
            {
                roller = new List<FileRecord>();
            }

            roller.AddRange(FilterByType(type, currentNode.Files));

            foreach (var subDir in currentNode.Children)
            {
                RecursiveFindByType(type, subDir, roller);
            }

            return roller;
        }
    }
}
