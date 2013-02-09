using System.IO;
using System.Text;

namespace LibDat
{
	public class UnicodeString : BaseData
	{
		public string Data { get; set; }


		public UnicodeString(string data)
		{
			this.Data = data;
		}
		public UnicodeString(BinaryReader inStream)
		{
			ReadData(inStream);
		}
		public UnicodeString(BinaryReader inStream, int offset)
		{
			inStream.BaseStream.Seek(offset, SeekOrigin.Begin);
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

			Data = sb.ToString();
		}

		public override void Save(BinaryWriter outStream)
		{
			foreach (char ch in Data)
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