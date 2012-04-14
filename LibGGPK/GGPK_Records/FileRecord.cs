using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LibGGPK
{
	public class FileRecord : BaseRecord
	{
		public const string Tag = "FILE";

		public byte[] Hash;
		public string Name;
		public long DataBegin;
		public long DataLength;
		public DirectoryTreeNode ContainingDirectory;

		public FileRecord(uint length, BinaryReader br)
		{
			Length = length;
			Read(br);
		}

		public override void Read(BinaryReader br)
		{
			int nameLength = br.ReadInt32();
			Hash = br.ReadBytes(32);

			Name = ASCIIEncoding.Unicode.GetString(br.ReadBytes(2 * (nameLength - 1)));
			br.ReadBytes(2); // Null terminator
			DataBegin = br.BaseStream.Position;
			DataLength = Length - (8 + nameLength * 2 + 32 + 4);

			br.BaseStream.Seek(DataLength, SeekOrigin.Current);
		}

		public override string ToString()
		{
			return Name;
		}

		public string ExtractTempFile(string ggpkPath)
		{
			string tempFileName = Path.GetTempFileName();

			ExtractFile(ggpkPath, tempFileName);

			return tempFileName;
		}

		public void ExtractFile(string ggpkPath, string outputPath)
		{
			byte[] fileData = ReadData(ggpkPath);
			File.WriteAllBytes(outputPath, fileData);
		}

		public void ExtractFileWithDirectoryStructure(string ggpkPath, string outputDirectory)
		{
			byte[] fileData = ReadData(ggpkPath);
			string completeOutputDirectory = outputDirectory + "\\" + GetDirectoryPath();

			Directory.CreateDirectory(completeOutputDirectory);
			File.WriteAllBytes(completeOutputDirectory + "\\" + Name, fileData);
		}

		public byte[] ReadData(string ggpkPath)
		{
			byte[] buffer = new byte[DataLength];

			using (var fs = File.Open(ggpkPath, FileMode.Open))
			{
				fs.Seek(DataBegin, SeekOrigin.Begin);
				fs.Read(buffer, 0, buffer.Length);
			}

			return buffer;
		}

		public enum DataFormat
		{
			Unknown,
			Image,
			Ascii,
			Unicode,
			CommaSeperatedValue,
			RichText,
			Sound,
		}

		private static Dictionary<string, DataFormat> knownFileFormats = new Dictionary<string, DataFormat>()
		{
			{".act", DataFormat.Unicode},
			{".amd", DataFormat.Unicode},
			{".ao", DataFormat.Unicode},
			{".aoc", DataFormat.Unicode},
			{".arl", DataFormat.Unicode},
			{".arm", DataFormat.Unicode},
			{".ast", DataFormat.Unknown},
			{".atlas", DataFormat.Unicode},
			{".cfg", DataFormat.Ascii},
			{".cht", DataFormat.Unicode},
			{".clt", DataFormat.Unicode},
			{".csv", DataFormat.CommaSeperatedValue},
			{".dat", DataFormat.Unknown},
			{".dct", DataFormat.Unicode},
			{".dds", DataFormat.Unknown},
			{".ddt", DataFormat.Unicode},
			{".dgr", DataFormat.Unicode},
			{".dlp", DataFormat.Unicode},
			{".ecf", DataFormat.Unicode},
			{".env", DataFormat.Unicode},
			{".epk", DataFormat.Unicode},
			{".et", DataFormat.Unicode},
			{".ffx", DataFormat.Unicode},
			{".fmt", DataFormat.Unknown},
			{".fx", DataFormat.Ascii},
			{".gft", DataFormat.Unicode},
			{".gt", DataFormat.Unicode},
			{".idl", DataFormat.Unicode},
			{".idt", DataFormat.Unicode},
			{".jpg", DataFormat.Image},
			{".mat", DataFormat.Unicode},
			{".mel", DataFormat.Ascii},
			{".mtd", DataFormat.Unicode},
			{".mtp", DataFormat.Unknown},
			{".ogg", DataFormat.Sound},
			{".ot", DataFormat.Unicode},
			{".otc", DataFormat.Unicode},
			{".pet", DataFormat.Unicode},
			{".png", DataFormat.Image},
			{".properties", DataFormat.Ascii},
			{".psg", DataFormat.Unknown},
			{".red", DataFormat.Unicode},
			{".rs", DataFormat.Unicode},
			{".rtf", DataFormat.RichText},
			{".slt", DataFormat.Ascii},
			{".sm", DataFormat.Unicode},
			{".smd", DataFormat.Unknown},
			{".tdt", DataFormat.Unknown},
			{".tgr", DataFormat.Unicode},
			{".tgt", DataFormat.Unknown},
			{".tmd", DataFormat.Unknown},
			{".tsi", DataFormat.Unicode},
			{".tst", DataFormat.Unicode},
			{".ttf", DataFormat.Unknown},
			{".txt", DataFormat.Unknown},
			{".ui", DataFormat.Unicode},
			{".xls", DataFormat.Unknown},
			{".xlsx", DataFormat.Unknown},
			{".xml", DataFormat.Unicode},
		};

		public DataFormat FileFormat 
		{
			get
			{
				return knownFileFormats[Path.GetExtension(Name).ToLower()];
			}
		}

		string directoryPath = null;
		public string GetDirectoryPath()
		{
			if(directoryPath != null)
				return directoryPath;

			Stack<string> pathQueue = new Stack<string>();
			StringBuilder sb = new StringBuilder();

			DirectoryTreeNode iter = ContainingDirectory;
			while (iter != null && iter.Name.Length > 0)
			{
				pathQueue.Push(iter.Name);
				iter = iter.Parent;
			}

			foreach (var item in pathQueue)
			{
				sb.Append(item + "\\");
			}

			directoryPath = sb.ToString();
			return directoryPath;
		}
	}
}
