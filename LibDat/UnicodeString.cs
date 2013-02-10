using System.ComponentModel;
using System.IO;
using System.Text;

namespace LibDat
{
	/// <summary>
	/// Represents a unicode string found in the data section of a .dat file
	/// </summary>
	public class UnicodeString : BaseData
	{
		/// <summary>
		/// Offset in the dat file with respect to the beginning of the data section
		/// </summary>
		public int Offset { get; private set; }
		/// <summary>
		/// The string
		/// </summary>
		public string Data { get; private set; }
		/// <summary>
		/// The replacement string. If this is set then it will replace the original string when it's saved.
		/// </summary>
		public string NewData { get; set; }
		/// <summary>
		/// Offset of the new string with respect to the beginning of the data section. This will be invalid until save is called.
		/// </summary>
		public int NewOffset;
		/// <summary>
		/// Offset of the data section in the .dat file (Starts with 0xbbbbbbbbbbbbbbbb)
		/// </summary>
		private readonly int dataTableOffset;

		
		public UnicodeString(BinaryReader inStream, int offset, int dataTableOffset)
		{
			this.dataTableOffset = dataTableOffset;
			this.Offset = offset;
			this.NewData = null;

			inStream.BaseStream.Seek(offset + dataTableOffset, SeekOrigin.Begin);
			ReadData(inStream);
		}

		/// <summary>
		/// Reads the unicode string directly from the specified stream. Stream position is not preserved and will be at the end of the string upon successful read.
		/// </summary>
		/// <param name="inStream">Stream containing the unicode string</param>
		private void ReadData(BinaryReader inStream)
		{
			StringBuilder sb = new StringBuilder();

			while (inStream.BaseStream.Position < inStream.BaseStream.Length)
			{
				char ch = inStream.ReadChar();
				if (ch == 0)
				{
					ch = inStream.ReadChar();
					break;
				}

				sb.Append(ch);
			}

			this.Data = sb.ToString();
		}

		/// <summary>
		/// Saves the unicode string to the specified stream. If 'NewData' has been filled out then it will be written instead of the original data.
		/// </summary>
		/// <param name="outStream"></param>
		public override void Save(BinaryWriter outStream)
		{
			this.NewOffset = (int)(outStream.BaseStream.Position - dataTableOffset);
			string dataToWrite = NewData ?? Data;

			for (int i = 0; i < dataToWrite.Length; i++)
			{
				outStream.Write(dataToWrite[i]);
			}
			outStream.Write((int)0);
		}

		public override string ToString()
		{
			return Data;
		}
	}
}