using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace LibDat.Data
{
	/// <summary>
	/// Represents a list of Int32 found in the resource section of a .dat file
	/// </summary>
	public class Int32List : AbstractData
	{
		/// <summary>
		/// Offset in the dat file with respect to the beginning of the data section
		/// </summary>
		public long Offset { get; private set; }
		/// <summary>
		/// Number of elements in the list
		/// </summary>
		public int ListLength { get; private set; }
		/// <summary>
		/// The Int32 list
		/// </summary>
		public List<Int32> Data { get; private set; }
		/// <summary>
		/// The replacement string. If this is set then it will replace the original string when it's saved.
		/// </summary>
		public string NewData { get; set; }
		/// <summary>
		/// Offset of the new string with respect to the beginning of the data section. This will be invalid until save is called.
		/// </summary>
		public long NewOffset;
		/// <summary>
		/// Offset of the data section in the .dat file (Starts with 0xbbbbbbbbbbbbbbbb)
		/// </summary>
		public long dataTableOffset;

		public Int32List(BinaryReader inStream, long offset, long dataTableOffset, int listLength)
		{
			this.Data = new List<Int32>(listLength);
			this.dataTableOffset = dataTableOffset;
			this.Offset = offset;
			this.ListLength = listLength;
			this.NewData = null;
			if (listLength == 0) return;

			inStream.BaseStream.Seek(offset + dataTableOffset, SeekOrigin.Begin);
			for (int i = 0; i < listLength; ++i)
			{
				ReadData(inStream);
			}
		}

		/// <summary>
		/// Reads the unicode string directly from the specified stream. Stream position is not preserved and will be at the end of the string upon successful read.
		/// </summary>
		/// <param name="inStream">Stream containing the unicode string</param>
		private void ReadData(BinaryReader inStream)
		{
			while (inStream.BaseStream.Position < inStream.BaseStream.Length)
			{
				Int32 u = inStream.ReadInt32();
				this.Data.Add(u);
				break;
			}
		}

		/// <summary>
		/// Saves the unicode string to the specified stream. If 'NewData' has been filled out then it will be written instead of the original data.
		/// </summary>
		/// <param name="outStream"></param>
		public override void Save(BinaryWriter outStream)
		{
			//TODO
		}

		public override string ToString()
		{
			if (Data.Count == 0) return "";
			StringBuilder sb = new StringBuilder();
			foreach (var s in Data)
			{
				sb.Append(s.ToString()).Append(" ");
			}
			return sb.Remove(sb.Length - 1, 1).ToString();
		}
	}
}