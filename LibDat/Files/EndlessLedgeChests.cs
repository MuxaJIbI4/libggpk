using System;
using System.IO;

namespace LibDat.Files
{
	public class EndlessLedgeChests : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		public Int64 Unknown1 { get; set; }
		public int Data0Length { get; set; }
		[DataIndex]
		public int Data0 { get; set; }
		[StringIndex]
		public int Unknown4 { get; set; }

		public EndlessLedgeChests()
		{
			
		}
		public EndlessLedgeChests(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Unknown1 = inStream.ReadInt64();
            Data0Length = inStream.ReadInt32();
            Data0 = inStream.ReadInt32();
			Unknown4 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Unknown1);
            outStream.Write(Data0Length);
            outStream.Write(Data0);
			outStream.Write(Unknown4);
		}

		public override int GetSize()
		{
			return 0x18;
		}
	}
}