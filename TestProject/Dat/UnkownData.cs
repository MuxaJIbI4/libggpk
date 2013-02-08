using System.IO;

namespace TestProject.Dat
{
	public class UnkownData : BaseData
	{
		public int Data { get; set; }

		public UnkownData(int data)
		{
			this.Data = data;
		}
		public UnkownData(BinaryReader inStream)
		{
			ReadData(inStream);
		}
		public UnkownData(BinaryReader inStream, int offset)
		{
			inStream.BaseStream.Seek(offset, SeekOrigin.Begin);
			ReadData(inStream);
		}

		private void ReadData(BinaryReader inStream)
		{
			Data = inStream.ReadInt32();
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