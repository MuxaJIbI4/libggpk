using System.IO;

namespace LibDat.Files
{
	public class Environments : BaseDat
	{
		[StringIndex]
		public int Index0 { get; set; }
		[StringIndex]
		public int Index1 { get; set; }
		public int Data0Length { get; set; }
		[UInt64Index]
		public int Data0 { get; set; }
		[StringIndex]
		public int Index2 { get; set; }
		[StringIndex]
		public int Index3 { get; set; }
		public int Data1Length { get; set; }
		[UInt64Index]
		public int Data1 { get; set; }
		[StringIndex]
		public int Index5 { get; set; }

		public Environments(BinaryReader inStream)
		{
			Index0 = inStream.ReadInt32();
			Index1 = inStream.ReadInt32();
			Data0Length = inStream.ReadInt32();
			Data0 = inStream.ReadInt32();
			Index2 = inStream.ReadInt32();
			Index3 = inStream.ReadInt32();
			Data1Length = inStream.ReadInt32();
			Data1 = inStream.ReadInt32();
			Index5 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Index0);
			outStream.Write(Index1);
			outStream.Write(Data0Length);
			outStream.Write(Data0);
			outStream.Write(Index2);
			outStream.Write(Index3);
			outStream.Write(Data1Length);
			outStream.Write(Data1);
			outStream.Write(Index5);
		}

		public override int GetSize()
		{
			return 0x24;
		}
	}
}