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
        private static readonly char[] PathSeperator = { Path.DirectorySeparatorChar };

        private static string contentPath;
        private static string outputPath;

        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: ExportGGPK.exe <path to ggpk> <base output dir> [ggpk dir to extract]");
                return;
            }

            contentPath = args[0];
            outputPath = args[1];
            string data = "";
            if(args.Length == 3 )
                data = args[2];

            if (!File.Exists(contentPath))
            {
                Console.WriteLine(string.Format("Content pack file is not available at {0}", contentPath));
                return;
            }

            Container.Read(contentPath, Console.WriteLine);

            var gameData = GetDirectory(data);
            var dataItems = RecursiveFindByType(gameData);
            Console.WriteLine(string.Format("Found {0} data files!", dataItems.Count));
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
            var dirs = path.Split(PathSeperator, StringSplitOptions.RemoveEmptyEntries);
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

        private static List<FileRecord> RecursiveFindByType(DirectoryTreeNode currentNode, List<FileRecord> roller = null)
        {
            if (roller == null)
            {
                roller = new List<FileRecord>();
            }

            roller.AddRange(currentNode.Files);

            foreach (var subDir in currentNode.Children)
            {
                RecursiveFindByType(subDir, roller);
            }

            return roller;
        }
    }
}
