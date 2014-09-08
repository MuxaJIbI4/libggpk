using System.IO;

namespace LibDat.Data
{
	/// <summary>
	/// Represents unknown data  found in the data section of a .dat file. None of this is tested and is probably incorrect.
	/// </summary>
	public class UnknownData : AbstractData
	{
		/// <summary>
		/// Offset in the dat file with respect to the beginning of the data section
		/// </summary>
		public long Offset { get; set; }
		/// <summary>
		/// The unknown data
		/// </summary>
		public int Data { get; set; }

		public UnknownData(BinaryReader inStream, long offset, long dataTableOffset)
		{
			this.Offset = offset;

			inStream.BaseStream.Seek(offset + dataTableOffset, SeekOrigin.Begin);
			ReadData(inStream);
		}

		/// <summary>
		/// Reads the data directly from the specified stream. Stream position is not preserved and will be at the end of the data upon successful read.
		/// </summary>
		/// <param name="inStream">Stream containing the unicode string</param>
		private void ReadData(BinaryReader inStream)
		{
			this.Data = inStream.ReadInt32();
		}

		/// <summary>
		/// Saves the data to the specified stream.
		/// </summary>
		/// <param name="outStream"></param>
		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Data);
		}

		public override string ToString()
		{
			return Data.ToString();
		}
	}
}