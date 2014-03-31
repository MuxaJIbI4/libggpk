using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace LibGGPK
{
	/// <summary>
	/// A file record represents a file entry in the pack file.
	/// </summary>
	public sealed class FileRecord : BaseRecord, IComparable
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
		/// Murmur2 hash of lowercase entry name
		/// </summary>
		public int EntryNameHash;

		/// <summary>
		/// Types of data a file can contain
		/// </summary>
		public enum DataFormat
		{
			Unknown,
			Image,
			Ascii,
			Unicode,
			RichText,
			Sound,
			Dat,
			TextureDDS,
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

			Name = Encoding.Unicode.GetString(br.ReadBytes(2 * (nameLength - 1)));
			br.BaseStream.Seek(2, SeekOrigin.Current); // Null terminator
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
			string tempFileName = Path.GetTempFileName() ;
			string modifiedTempFileName = tempFileName + Path.GetExtension(Name);

			File.Move(tempFileName, modifiedTempFileName);
			ExtractFile(ggpkPath, modifiedTempFileName);

			return modifiedTempFileName;
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
			string completeOutputDirectory = outputDirectory + Path.DirectorySeparatorChar + GetDirectoryPath();

			Directory.CreateDirectory(completeOutputDirectory);
			File.WriteAllBytes(completeOutputDirectory + Path.DirectorySeparatorChar + Name, fileData);
		}

		/// <summary>
		/// Reads this file's data from the specified pack file
		/// </summary>
		/// <param name="ggpkPath">Path of pack file that contains this record</param>
		/// <returns>Raw file data</returns>
		public byte[] ReadData(string ggpkPath)
		{
			byte[] buffer = new byte[DataLength];

			using (var fs = File.Open(ggpkPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
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
				return KnownFileFormats[Path.GetExtension(Name).ToLower()];
			}
		}

		/// <summary>
		/// Gets the absolute directory of this file
		/// </summary>
		/// <returns>Absolute directory of this file</returns>
		public string GetDirectoryPath()
		{
			return ContainingDirectory.GetDirectoryPath();
		}

		/// <summary>
		/// Marks the current record as being FREE space and adds it to the FREE list
		/// </summary>
		/// <param name="ggpkFileStream">Stream for GGPK file</param>
		/// <param name="freeRecordRoot">Root of FREE records</param>
		private void MarkAsFree(FileStream ggpkFileStream, LinkedList<FreeRecord> freeRecordRoot)
		{
			byte[] nextFreeRecordOffset = BitConverter.GetBytes((long)0);
			byte[] freeRecordTag = Encoding.ASCII.GetBytes("FREE");

			// Mark previous data as FREE
			ggpkFileStream.Seek(RecordBegin + 4, SeekOrigin.Begin);
			ggpkFileStream.Write(freeRecordTag, 0, 4);
			ggpkFileStream.Write(nextFreeRecordOffset, 0, nextFreeRecordOffset.Length);

			// Update last FREE record to point to our new FREE record
			ggpkFileStream.Seek(freeRecordRoot.Last.Value.RecordBegin + 8, SeekOrigin.Begin);
			ggpkFileStream.Write(BitConverter.GetBytes(RecordBegin), 0, 8);
		}

		public void ReplaceContents(string ggpkPath, byte[] replacmentData, LinkedList<FreeRecord> freeRecordRoot)
		{
			using (FileStream ggpkFileStream = File.Open(ggpkPath, FileMode.Open))
			{
				MarkAsFree(ggpkFileStream, freeRecordRoot);

				ggpkFileStream.Seek(0, SeekOrigin.End);

				// Update and write new record data to end of GGPK file
				RecordBegin = ggpkFileStream.Position;
				Length = (uint)(Length - DataLength + replacmentData.Length);

				// Generate the new Hash
				SHA256 sha256 = SHA256Managed.Create();
				Hash = sha256.ComputeHash(replacmentData);

				BinaryWriter bw = new BinaryWriter(ggpkFileStream);
				bw.Write(Length);
				bw.Write(Tag.ToCharArray(0, 4));
				bw.Write(Name.Length + 1);
				bw.Write(Hash);
				bw.Write(Encoding.Unicode.GetBytes(Name + "\0"));

				DataBegin = bw.BaseStream.Position;
				DataLength = replacmentData.Length;

				bw.Write(replacmentData);
			}

			ContainingDirectory.Record.UpdateOffset(ggpkPath, EntryNameHash, RecordBegin);
		}

		/// <summary>
		/// Replaces the contents of this file with the data from the specified file on disk.
		/// </summary>
		/// <param name="ggpkPath">Path of pack file that contains this record</param>
		/// <param name="replacmentPath">Path to file containing replacement data</param>
		/// <param name="freeRecordRoot">Root of the Free record list</param>
		public void ReplaceContents(string ggpkPath, string replacmentPath, LinkedList<FreeRecord> freeRecordRoot)
		{
			ReplaceContents(ggpkPath, File.ReadAllBytes(replacmentPath), freeRecordRoot);
		}


		#region uglystuff
		/// <summary>
		/// A quick and dirty mapping of what type of data is contained in each file type
		/// </summary>
		private static readonly Dictionary<string, DataFormat> KnownFileFormats = new Dictionary<string, DataFormat>()
		{
			{"", DataFormat.Unknown},
			{".act", DataFormat.Unicode},
			{".ais", DataFormat.Unicode},
			{".amd", DataFormat.Unicode}, // Animated Meta Data
			{".ao", DataFormat.Unicode}, // Animated Object
			{".aoc", DataFormat.Unicode}, // Animated Object Controller
			{".arl", DataFormat.Unicode},
			{".arm", DataFormat.Unicode}, // Rooms
			{".ast", DataFormat.Unknown}, // Skeleton
			{".atlas", DataFormat.Unicode},
			{".bat", DataFormat.Unknown},
			{".cfg", DataFormat.Ascii},
			{".cht", DataFormat.Unicode}, // ChestData
			{".clt", DataFormat.Unicode},
			{".csv", DataFormat.Ascii},
			{".dat", DataFormat.Dat},
			{".dct", DataFormat.Unicode}, // Decals
			{".dds", DataFormat.TextureDDS},
			{".ddt", DataFormat.Unicode}, // Doodads
			{".dgr", DataFormat.Unicode},
			{".dlp", DataFormat.Unicode},
			{".ecf", DataFormat.Unicode},
			{".env", DataFormat.Unicode}, // Environment
			{".epk", DataFormat.Unicode},
			{".et", DataFormat.Unicode},
			{".ffx", DataFormat.Unicode}, // FFX Render
			{".fmt", DataFormat.Unknown},
			{".fx", DataFormat.Ascii}, // Shader
			{".gft", DataFormat.Unicode},
			{".gt", DataFormat.Unicode}, // Ground Types
			{".idl", DataFormat.Unicode},
			{".idt", DataFormat.Unicode},
			{".jpg", DataFormat.Image},
			{".mat", DataFormat.Unicode}, // Materials
			{".mel", DataFormat.Ascii}, // Maya Embedded Language
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
			{".rs", DataFormat.Unicode}, // Room Set
			{".rtf", DataFormat.RichText},
			{".slt", DataFormat.Ascii},
			{".sm", DataFormat.Unicode}, // Skin Mesh
			{".smd", DataFormat.Unknown}, // Skin Mesh Data
			{".tdt", DataFormat.Unknown},
			{".tgm", DataFormat.Unknown},
			{".tgr", DataFormat.Unicode},
			{".tgt", DataFormat.Unicode},
			{".tmd", DataFormat.Unknown},
			{".tsi", DataFormat.Unicode},
			{".tst", DataFormat.Unicode},
			{".ttf", DataFormat.Unknown}, // Font
			{".txt", DataFormat.Unicode},
			{".ui", DataFormat.Unicode}, // User Interface
			{".xls", DataFormat.Unknown},
			{".xlsx", DataFormat.Unknown},
			{".xml", DataFormat.Unicode},
		};
		#endregion


		public override string ToString()
		{
			return Name;
		}

		public int CompareTo(object obj)
		{
			if (!(obj is FileRecord))
				throw new NotImplementedException("Can only compare FileRecords");

			return Name.CompareTo((obj as FileRecord).Name);
		}
	}
}
