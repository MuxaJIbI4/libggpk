using System.ComponentModel;
using System.IO;
using System.Text;

namespace LibDat
{
	public class UnicodeString : BaseData
	{
		public int Offset { get; private set; }
		public string Data { get; private set; }
		public string NewData { get; set; }
		public int NewOffset;
		private readonly int dataTableOffset;

		
		public UnicodeString(BinaryReader inStream, int offset, int dataTableOffset)
		{
			this.dataTableOffset = dataTableOffset;
			this.Offset = offset;
			this.NewData = null;

			inStream.BaseStream.Seek(offset + dataTableOffset, SeekOrigin.Begin);
			ReadData(inStream);
		}

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
				inStream.ReadChar();

				sb.Append(ch);
			}

			this.Data = sb.ToString();
		}

		public override void Save(BinaryWriter outStream)
		{
			this.NewOffset = (int)(outStream.BaseStream.Position - dataTableOffset);
			string dataToWrite = NewData ?? Data;

			foreach (char ch in dataToWrite)
			{
				outStream.Write(ch);
				outStream.Write((byte)0);
			}
			outStream.Write((int)0);
		}

		public override string ToString()
		{
			return Data;
		}
	}
}