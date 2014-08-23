using System.IO;

namespace LibDat.Files
{
	public class Quest : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		public int Act { get; set; }
		[UserStringIndex]
		public int Title { get; set; }
		public int Unknown3 { get; set; }
		[StringIndex]
		public int QuestIcon { get; set; }
		public int Unknown5 { get; set; }
		public bool Flag0 { get; set; }
		public int Unknown6 { get; set; }
		public bool Flag1 { get; set; }


		public Quest(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Act = inStream.ReadInt32();
			Title = inStream.ReadInt32();
			Unknown3 = inStream.ReadInt32();
			QuestIcon = inStream.ReadInt32();
			Unknown5 = inStream.ReadInt32();
			Flag0 = inStream.ReadBoolean();
			Unknown6 = inStream.ReadInt32();
			Flag1 = inStream.ReadBoolean();

		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Act);
			outStream.Write(Title);
			outStream.Write(Unknown3);
			outStream.Write(QuestIcon);
			outStream.Write(Unknown5);
			outStream.Write(Flag0);
			outStream.Write(Unknown6);
			outStream.Write(Flag1);
		}

		public override int GetSize()
		{
			return 0x1E;
		}
	}
}