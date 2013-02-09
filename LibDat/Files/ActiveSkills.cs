using System.IO;

namespace LibDat.Files
{
	class ActiveSkills : BaseDat
	{
		[StringIndex]
		public int NameIndex;
		[StringIndex]
		public int DisplayedName;
		[StringIndex]
		public int Description;
		[StringIndex]
		public int Index3;
		[StringIndex]
		public int IconPath;
		public int Unkown0;
		[DataIndex]
		public int Data0;
		public int Unknown1;
		public int Unknown2;
		[DataIndex]
		public int Data1;
		public int Unknown3;
		[DataIndex]
		public int Data2;
		[StringIndex]
		public int ExtraDescription;
		[StringIndex]
		public int ExamplePath;
		public bool Flag0;
		public int Unknown4;
		public bool Flag1;
		public int Unknown5;
		[DataIndex] 
		public int Data3;

		public ActiveSkills(BinaryReader inStream)
		{
			NameIndex = inStream.ReadInt32();
			DisplayedName = inStream.ReadInt32();
			Description = inStream.ReadInt32();
			Index3 = inStream.ReadInt32();
			IconPath = inStream.ReadInt32();
			Unkown0 = inStream.ReadInt32();
			Data0 = inStream.ReadInt32();
			Unknown1 = inStream.ReadInt32();
			Unknown2 = inStream.ReadInt32();
			Data1 = inStream.ReadInt32();
			Unknown3 = inStream.ReadInt32();
			Data2 = inStream.ReadInt32();
			ExtraDescription = inStream.ReadInt32();
			ExamplePath = inStream.ReadInt32();
			Flag0 = inStream.ReadBoolean();
			Unknown4 = inStream.ReadInt32();
			Flag1 = inStream.ReadBoolean();
			Unknown5 = inStream.ReadInt32();
			Data3 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(NameIndex);
			outStream.Write(DisplayedName);
			outStream.Write(Description);
			outStream.Write(Index3);
			outStream.Write(IconPath);
			outStream.Write(Unkown0);
			outStream.Write(Data0);
			outStream.Write(Unknown1);
			outStream.Write(Unknown2);
			outStream.Write(Data1);
			outStream.Write(Unknown3);
			outStream.Write(Data2);
			outStream.Write(ExtraDescription);
			outStream.Write(ExamplePath);
			outStream.Write(Flag0);
			outStream.Write(Unknown4);
			outStream.Write(Flag1);
			outStream.Write(Unknown5);
			outStream.Write(Data3);
		}

		public override int GetSize()
		{
			return 0x46;
		}
	}
}
