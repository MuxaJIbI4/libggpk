using System;
using System.IO;

namespace LibDat.Files
{
	public class MonsterPackEntries : BaseDat
	{
		[StringIndex]
		public int Index0 { get; set; }
		public Int64 Unknown0 { get; set; }
		public bool Flag0 { get; set; }
		public int Unknown1 { get; set; }
		public Int64 Unknown2 { get; set; }

		public MonsterPackEntries(BinaryReader inStream)
		{
			Index0 = inStream.ReadInt32();
			Unknown0 = inStream.ReadInt64();
			Flag0 = inStream.ReadBoolean();
			Unknown1 = inStream.ReadInt32();
			Unknown2 = inStream.ReadInt64();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Index0);
			outStream.Write(Unknown0);
			outStream.Write(Flag0);
			outStream.Write(Unknown1);
			outStream.Write(Unknown2);
		}

		public override int GetSize()
		{
			return 0x19;
		}
	}
}