using System;
using System.IO;

namespace LibDat.Files
{
	public class ShrineBuffs : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		public int Data0Length { get; set; }
		[Int32Index]
		public int Data0 { get; set; }
		public Int64 Unknown3 { get; set; }
		public Int64 Unknown4 { get; set; }

		public ShrineBuffs()
		{
			
		}
		public ShrineBuffs(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Data0Length = inStream.ReadInt32();
			Data0 = inStream.ReadInt32();
			Unknown3 = inStream.ReadInt64();
			Unknown4 = inStream.ReadInt64();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Data0Length);
			outStream.Write(Data0);
			outStream.Write(Unknown3);
			outStream.Write(Unknown4);
		}

		public override int GetSize()
		{
			return 0x1c;
		}
	}
}