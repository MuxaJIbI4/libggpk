using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LibGGPK
{
	/// <summary>
	/// A free record represents space in the pack file that has been marked as deleted. It's much cheaper to just
	/// mark areas as free and append data to the end of the pack file than it is to rebuild the entire pack file just
	/// to remove a piece of data.
	/// </summary>
	public class FreeRecord : BaseRecord
	{
		public const string Tag = "FREE";

		/// <summary>
		/// Offset in pack file where the raw data begins
		/// </summary>
		public long DataBegin;
		/// <summary>
		/// Length of the raw data
		/// </summary>
		public long DataLength;

		public FreeRecord(uint length, BinaryReader br)
		{
			RecordBegin = br.BaseStream.Position - 8;
			Length = length;
			Read(br);
		}

		/// <summary>
		/// Reads the FREE record entry from the specified stream
		/// </summary>
		/// <param name="br">Stream pointing at a FREE record</param>
		public override void Read(BinaryReader br)
		{
			base.Read(br);

			DataBegin = br.BaseStream.Position;
			DataLength = Length - 8;
			br.BaseStream.Seek(Length - 8, SeekOrigin.Current);
		}

		/// <summary>
		/// Reads the data this record contains
		/// </summary>
		/// <param name="ggpkPath">Path of pack file that contains this record</param>
		/// <returns>Garbage data contained in this record</returns>
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

		public override string ToString()
		{
			return Tag;
		}
	}
}
