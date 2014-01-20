using System;
using System.IO;

namespace LibDat.Files
{
	public class QuestRewards : BaseDat
	{
		public Int64 QuestKey { get; set; }
		public int Unknown2 { get; set; }
		public int Unknown3 { get; set; }
		public Int64 Unknown4 { get; set; }
		public Int64 ItemKey { get; set; }

		public int Unknown7 { get; set; }
		public int Unknown8 { get; set; }
		public int Unknown9 { get; set; }
		[StringIndex]
		public int SocketGems { get; set; }
		public Int64 Unknown11 { get; set; }

		public QuestRewards(BinaryReader inStream)
		{
			QuestKey = inStream.ReadInt64();
			Unknown2 = inStream.ReadInt32();
			Unknown3 = inStream.ReadInt32();
			Unknown4 = inStream.ReadInt64();
			ItemKey = inStream.ReadInt64();
			Unknown7 = inStream.ReadInt32();
			Unknown8 = inStream.ReadInt32();
			Unknown9 = inStream.ReadInt32();
			SocketGems = inStream.ReadInt32();
			Unknown11 = inStream.ReadInt64();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(QuestKey);
			outStream.Write(Unknown2);
			outStream.Write(Unknown3);
			outStream.Write(Unknown4);
			outStream.Write(ItemKey);
			outStream.Write(Unknown7);
			outStream.Write(Unknown8);
			outStream.Write(Unknown9);
			outStream.Write(SocketGems);
			outStream.Write(Unknown11);
		}

		public override int GetSize()
		{
			return 0x38;
		}
	}
}