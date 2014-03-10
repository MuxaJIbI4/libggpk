using System;
using System.IO;

namespace LibDat.Files
{
	public class InvasionMonsterRestrictions : BaseDat
	{
		[StringIndex]
		public int Index0 { get; set; }
		public Int64 Unknown0 { get; set; }
		public Int64 Unknown1 { get; set; }
		public int Data0Length { get; set; }
		[Int32Index]
		public int Data0 { get; set; }

		public InvasionMonsterRestrictions(BinaryReader inStream)
		{
			Index0 = inStream.ReadInt32();
			Unknown0 = inStream.ReadInt64();
			Unknown1 = inStream.ReadInt64();
			Data0Length = inStream.ReadInt32();
			Data0 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Index0);
			outStream.Write(Unknown0);
			outStream.Write(Unknown1);
			outStream.Write(Data0Length);
			outStream.Write(Data0);
		}

		public override int GetSize()
		{
			return 0x1C;
		}
	}
}