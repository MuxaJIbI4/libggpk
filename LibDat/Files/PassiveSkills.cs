using System;
using System.IO;

namespace LibDat.Files
{
	public class PassiveSkills : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		[StringIndex]
		public int Icon { get; set; }
		public int Data0Length { get; set; }
		public int Data0 { get; set; }
		public int Stat1 { get; set; }
		public int Stat2 { get; set; }
		public int Stat3 { get; set; }
		public int Stat4 { get; set; }
		public int Unknown8 { get; set; }
		[UserStringIndex]
		public int Name { get; set; }
		public int Data1Length { get; set; }
		public int Data1 { get; set; }
		public bool IsKeystone { get; set; }
		public bool IsNotable { get; set; }
		[StringIndex]
		public int FlavourText { get; set; }
		public bool IsJustIcon { get; set; }
		public Int64 Unknown13 { get; set; }

		public PassiveSkills(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Icon = inStream.ReadInt32();
			Data0Length = inStream.ReadInt32();
			Data0 = inStream.ReadInt32();
			Stat1 = inStream.ReadInt32();
			Stat2 = inStream.ReadInt32();
			Stat3 = inStream.ReadInt32();
			Stat4 = inStream.ReadInt32();
			Unknown8 = inStream.ReadInt32();
			Name = inStream.ReadInt32();
			Data1Length = inStream.ReadInt32();
			Data1 = inStream.ReadInt32();
			IsKeystone = inStream.ReadBoolean();
			IsNotable = inStream.ReadBoolean();
			FlavourText = inStream.ReadInt32();
			IsJustIcon = inStream.ReadBoolean();
			Unknown13 = inStream.ReadInt64();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Icon);
			outStream.Write(Data0Length);
			outStream.Write(Data0);
			outStream.Write(Stat1);
			outStream.Write(Stat2);
			outStream.Write(Stat3);
			outStream.Write(Stat4);
			outStream.Write(Unknown8);
			outStream.Write(Name);
			outStream.Write(Data1Length);
			outStream.Write(Data1);
			outStream.Write(IsKeystone);
			outStream.Write(IsNotable);
			outStream.Write(FlavourText);
			outStream.Write(IsJustIcon);
			outStream.Write(Unknown13);
		}

		public override int GetSize()
		{
			return 0x3F;
		}
	}
}