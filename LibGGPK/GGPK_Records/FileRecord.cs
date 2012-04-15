using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LibGGPK
{
	/// <summary>
	/// A file record represents a file entry in the pack file.
	/// </summary>
	public class FileRecord : BaseRecord
	{
		public const string Tag = "FILE";

		/// <summary>
		/// SHA256 hash of this file's data
		/// </summary>
		public byte[] Hash;
		/// <summary>
		/// File name
		/// </summary>
		public string Name;
		/// <summary>
		/// Offset in pack file where the raw data begins
		/// </summary>
		public long DataBegin;
		/// <summary>
		/// Length of the raw file data
		/// </summary>
		public long DataLength;
		/// <summary>
		/// Directory this file resides in
		/// </summary>
		public DirectoryTreeNode ContainingDirectory;
		/// <summary>
		/// Cached directory path so we don't need to recalculate it
		/// </summary>
		private string directoryPath = null;

		/// <summary>
		/// Types of data a file can contain
		/// </summary>
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

		public FileRecord(uint length, BinaryReader br)
		{
			RecordBegin = br.BaseStream.Position - 8;
			Length = length;
			Read(br);
		}

		/// <summary>
		/// Reads the FILE record entry from the specified stream
		/// </summary>
		/// <param name="br">Stream pointing at a FILE record</param>
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

		/// <summary>
		/// Extracts this file to a temporary file, path of this temporary file is returned
		/// </summary>
		/// <param name="ggpkPath">Path of pack file that contains this record</param>
		/// <returns>Path of temporary file containing extracted data</returns>
		public string ExtractTempFile(string ggpkPath)
		{
			string tempFileName = Path.GetTempFileName();

			ExtractFile(ggpkPath, tempFileName);

			return tempFileName;
		}

		/// <summary>
		/// Extracts this file to the specified path
		/// </summary>
		/// <param name="ggpkPath">Path of pack file that contains this record</param>
		/// <param name="outputPath">Path to extract this file to</param>
		public void ExtractFile(string ggpkPath, string outputPath)
		{
			byte[] fileData = ReadData(ggpkPath);
			File.WriteAllBytes(outputPath, fileData);
		}

		/// <summary>
		/// Extracts this file to the specified directory
		/// </summary>
		/// <param name="ggpkPath">Path of pack file that contains this record</param>
		/// <param name="outputDirectory">Directory to extract this file to</param>
		public void ExtractFileWithDirectoryStructure(string ggpkPath, string outputDirectory)
		{
			byte[] fileData = ReadData(ggpkPath);
			string completeOutputDirectory = outputDirectory + "\\" + GetDirectoryPath();

			Directory.CreateDirectory(completeOutputDirectory);
			File.WriteAllBytes(completeOutputDirectory + "\\" + Name, fileData);
		}

		/// <summary>
		/// Reads this file's data from the specified pack file
		/// </summary>
		/// <param name="ggpkPath">Path of pack file that contains this record</param>
		/// <returns>Raw file data</returns>
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

		/// <summary>
		/// Format of data contained in this file
		/// </summary>
		public DataFormat FileFormat 
		{
			get
			{
				return knownFileFormats[Path.GetExtension(Name).ToLower()];
			}
		}

		/// <summary>
		/// Gets the absolute directory of this file
		/// </summary>
		/// <returns>Absolute directory of this file</returns>
		public string GetDirectoryPath()
		{
			if(directoryPath != null)
				return directoryPath;

			Stack<string> pathQueue = new Stack<string>();
			StringBuilder sb = new StringBuilder();

			// Traverse the directory tree until we hit the root node, pushing all
			//  encountered directory names onto the stack
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

		/// <summary>
		/// Replaces the contents of this file with the data from the specified file on disk.
		/// </summary>
		/// <param name="ggpkPath">Path of pack file that contains this record</param>
		/// <param name="replacmentPath">Path to file containing replacement data</param>
		public void ReplaceContents(string ggpkPath, string replacmentPath)
		{
			byte[] replacmentData = File.ReadAllBytes(replacmentPath);
			long previousRecordBegin = RecordBegin;

			using (FileStream ggpkFileStream = File.Open(ggpkPath, FileMode.Open))
			{
				byte[] freeRecordTag = ASCIIEncoding.ASCII.GetBytes("FREE");
				byte[] previousDataHeader = new byte[Length - DataLength];


				// Backup the previous record header
				ggpkFileStream.Seek(RecordBegin, SeekOrigin.Begin);
				ggpkFileStream.Read(previousDataHeader, 0, previousDataHeader.Length);

				// Mark previous data as FREE
				ggpkFileStream.Seek(RecordBegin+4, SeekOrigin.Begin);
				ggpkFileStream.Write(freeRecordTag, 0, 4);

				// Write our new file length to the header
				using (MemoryStream ms = new MemoryStream(previousDataHeader))
				{
					using (BinaryWriter bw = new BinaryWriter(ms))
					{
						bw.Write((UInt32)((Length - DataLength) + replacmentData.Length));
					}
				}

				// Append header to the bottom of the GGPK file, followed by the new file contents
				ggpkFileStream.Seek(0, SeekOrigin.End);
				RecordBegin = ggpkFileStream.Position;
				ggpkFileStream.Write(previousDataHeader, 0, previousDataHeader.Length);
				ggpkFileStream.Write(replacmentData, 0, replacmentData.Length);
			}

			ContainingDirectory.Record.UpdateOffset(ggpkPath, previousRecordBegin, RecordBegin);
		}


		#region uglystuff
		/// <summary>
		/// A quick and dirty mapping of what type of data is contained in each file type
		/// </summary>
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
		#endregion


		public override string ToString()
		{
			return Name;
		}
	}
}
