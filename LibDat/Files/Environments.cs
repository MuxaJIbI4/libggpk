using System.IO;

namespace LibDat.Files
{
	public class Environments : BaseDat
	{
		[StringIndex]
		public int Index0 { get; set; }
		[StringIndex]
		public int Index1 { get; set; }
		public int Unknown0 { get; set; }
		[DataIndex]
		public int Data0 { get; set; }
		[StringIndex]
		public int Index2 { get; set; }
		[StringIndex]
		public int Index3 { get; set; }
		public int Unknown1 { get; set; }
		[StringIndex]
		public int Index4 { get; set; }
		[StringIndex]
		public int Index5 { get; set; }

		public Environments(BinaryReader inStream)
		{
			Index0 = inStream.ReadInt32();
			Index1 = inStream.ReadInt32();
			Unknown0 = inStream.ReadInt32();
			Data0 = inStream.ReadInt32();
			Index2 = inStream.ReadInt32();
			Index3 = inStream.ReadInt32();
			Unknown1 = inStream.ReadInt32();
			Index4 = inStream.ReadInt32();
			Index5 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Index0);
			outStream.Write(Index1);
			outStream.Write(Unknown0);
			outStream.Write(Data0);
			outStream.Write(Index2);
			outStream.Write(Index3);
			outStream.Write(Unknown1);
			outStream.Write(Index4);
			outStream.Write(Index5);
		}

		public override int GetSize()
		{
			return 0x24;
		}
	}
}