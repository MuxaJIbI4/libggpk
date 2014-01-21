using System.IO;

namespace LibDat.Files
{
	public class ActiveSkills : BaseDat
	{
		[StringIndex]
		public int NameIndex { get; set; }
		[UserStringIndex]
		public int DisplayedName { get; set; }
		[UserStringIndex]
		public int Description { get; set; }
		[StringIndex]
		public int Index3 { get; set; }
		[StringIndex]
		public int IconPath { get; set; }
		public int Data0Length { get; set; }
		[DataIndex]
		public int Data0 { get; set; }
		public int Unknown1 { get; set; }
		public int Data1Length { get; set; }
		[DataIndex]
		public int Data1 { get; set; }
		public int Data2Length { get; set; }
		[DataIndex]
		public int Data2 { get; set; }
		[StringIndex]
		public int WebsiteDescription { get; set; }
		[StringIndex]
		public int WebsiteImage { get; set; }
		public bool Flag0 { get; set; }
		public int Unknown4 { get; set; }
		public bool Flag1 { get; set; }
		public int Unknown5 { get; set; }
		[StringIndex]
		public int Metadata { get; set; }

		public ActiveSkills(BinaryReader inStream)
		{
			NameIndex = inStream.ReadInt32();
			DisplayedName = inStream.ReadInt32();
			Description = inStream.ReadInt32();
			Index3 = inStream.ReadInt32();
			IconPath = inStream.ReadInt32();
			Data0Length = inStream.ReadInt32();
			Data0 = inStream.ReadInt32();
			Unknown1 = inStream.ReadInt32();
			Data1Length = inStream.ReadInt32();
			Data1 = inStream.ReadInt32();
			Data2Length = inStream.ReadInt32();
			Data2 = inStream.ReadInt32();
			WebsiteDescription = inStream.ReadInt32();
			WebsiteImage = inStream.ReadInt32();
			Flag0 = inStream.ReadBoolean();
			Unknown4 = inStream.ReadInt32();
			Flag1 = inStream.ReadBoolean();
			Unknown5 = inStream.ReadInt32();
			Metadata = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(NameIndex);
			outStream.Write(DisplayedName);
			outStream.Write(Description);
			outStream.Write(Index3);
			outStream.Write(IconPath);
			outStream.Write(Data0Length);
			outStream.Write(Data0);
			outStream.Write(Unknown1);
			outStream.Write(Data1Length);
			outStream.Write(Data1);
			outStream.Write(Data2Length);
			outStream.Write(Data2);
			outStream.Write(WebsiteDescription);
			outStream.Write(WebsiteImage);
			outStream.Write(Flag0);
			outStream.Write(Unknown4);
			outStream.Write(Flag1);
			outStream.Write(Unknown5);
			outStream.Write(Metadata);
		}

		public override int GetSize()
		{
			return 0x46;
		}
	}
}
