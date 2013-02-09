using System.IO;

namespace LibDat.Files
{
	class QuestStates : BaseDat
	{
		public int Group;
		public int Unknown1;
		public int Unknown2;
		public int Unknown3;
		[DataIndex]
		public int Data0;
		public int Unknown4;
		public int Unknown5;
		[StringIndex]
		public int Text;
		public int Unknown6;
		public bool Flag0;
		public int Unknown7;
		[DataIndex]
		public int Data1;
		public int Unknown9;

		public QuestStates(BinaryReader inStream)
		{
			Group = inStream.ReadInt32();
			Unknown1 = inStream.ReadInt32();
			Unknown2 = inStream.ReadInt32();
			Unknown3 = inStream.ReadInt32();
			Data0 = inStream.ReadInt32();
			Unknown4 = inStream.ReadInt32();
			Unknown5 = inStream.ReadInt32();
			Text = inStream.ReadInt32();
			Unknown6 = inStream.ReadInt32();
			Flag0 = inStream.ReadBoolean();
			Unknown7 = inStream.ReadInt32();
			Data1 = inStream.ReadInt32();
			Unknown9 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Group);
			outStream.Write(Unknown1);
			outStream.Write(Unknown2);
			outStream.Write(Unknown3);
			outStream.Write(Data0);
			outStream.Write(Unknown4);
			outStream.Write(Unknown5);
			outStream.Write(Text);
			outStream.Write(Unknown6);
			outStream.Write(Flag0);
			outStream.Write(Unknown7);
			outStream.Write(Data1);
			outStream.Write(Unknown9);
		}

		public override int GetSize()
		{
			return 0x31;
		}
	}
}
