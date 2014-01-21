using System.IO;

namespace LibDat.Files
{
	public class ChestClusters : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		public int Data0Length { get; set; }
		[UInt64Index]
		public int Data0 { get; set; }
		public int Data1Length { get; set; }
		[UInt32Index]
		public int Data1 { get; set; }
		public int Unknown3 { get; set; }
		public int Unknown4 { get; set; }
		public int Unknown5 { get; set; }

		public ChestClusters(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Data0Length = inStream.ReadInt32();
			Data0 = inStream.ReadInt32();
			Data1Length = inStream.ReadInt32();
			Data1 = inStream.ReadInt32();
			Unknown3 = inStream.ReadInt32();
			Unknown4 = inStream.ReadInt32();
			Unknown5 = inStream.ReadInt32();
		}
		public override void Save(System.IO.BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Data0Length);
			outStream.Write(Data0);
			outStream.Write(Data1Length);
			outStream.Write(Data1);
			outStream.Write(Unknown3);
			outStream.Write(Unknown4);
			outStream.Write(Unknown5);
		}

		public override int GetSize()
		{
			return 0x20;
		}
	}
}
