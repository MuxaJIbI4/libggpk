using System;
using System.IO;

namespace LibDat.Files
{
	public class QuestStaticRewards : BaseDat
	{
		public int Unknown0 { get; set; }
		public int Unknown1 { get; set; }
		public Int64 Unknown2 { get; set; }
		public int Unknown3 { get; set; }
		public Int64 Unknown4 { get; set; }
		public Int64 Unknown5 { get; set; }

		public QuestStaticRewards(BinaryReader inStream)
		{
			Unknown0 = inStream.ReadInt32();
			Unknown1 = inStream.ReadInt32();
			Unknown2 = inStream.ReadInt64();
			Unknown3 = inStream.ReadInt32();
			Unknown4 = inStream.ReadInt64();
			Unknown5 = inStream.ReadInt64();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Unknown0);
			outStream.Write(Unknown1);
			outStream.Write(Unknown2);
			outStream.Write(Unknown3);
			outStream.Write(Unknown4);
			outStream.Write(Unknown5);
		}

		public override int GetSize()
		{
			return 0x24;
		}
	}
}