using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LibGGPK
{
	public class GGPK
	{
		public Dictionary<long, BaseRecord> RecordOffsets;
		public DirectoryTreeNode DirectoryRoot;

		private const int EstimatedFileCount = 66000;

		public GGPK()
		{
			RecordOffsets = new Dictionary<long, BaseRecord>(EstimatedFileCount);
		}

		private void ReadRecordOfsets(string pathToGgpk, Action<string> output)
		{
			using (FileStream fs = File.Open(pathToGgpk, FileMode.Open))
			{
				BinaryReader br = new BinaryReader(fs);

				while (br.BaseStream.Position < br.BaseStream.Length)
				{
					long currentOffset = br.BaseStream.Position;
					BaseRecord record = RecordFactory.ReadRecord(br);
					RecordOffsets.Add(currentOffset, record);

					if (RecordOffsets.Count % (EstimatedFileCount / 10) == 0)
					{
						output( String.Format("{0:00.00}%\n", (100*RecordOffsets.Count) / (float)EstimatedFileCount));
					}
				}
			}
		}

		private void CreateDirectoryTree(Action<string> output)
		{
			DirectoryRoot = DirectoryTreeMaker.BuildDirectoryTree(RecordOffsets);
		}

		public void Read(string pathToGgpk, Action<string> output)
		{
			output("Parsing GGPK...\n");
			ReadRecordOfsets(pathToGgpk, output);
			output("\n");
			output("Building directory tree...\n");
			CreateDirectoryTree(output);
		}
	}
}
