using System.IO;

namespace LibDat
{
	public class UnkownData : BaseData
	{
		public int Offset { get; set; }
		public int Data { get; set; }

		public UnkownData(BinaryReader inStream, int offset, int dataTableOffset)
		{
			this.Offset = offset;

			inStream.BaseStream.Seek(offset + dataTableOffset, SeekOrigin.Begin);
			ReadData(inStream);
		}

		private void ReadData(BinaryReader inStream)
		{
			this.Data = inStream.ReadInt32();
		}

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