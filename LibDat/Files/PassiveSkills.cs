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
		public int Unknown2 { get; set; }
		public int Unknown3 { get; set; }
		public int Unknown4 { get; set; }
		public int Unknown5 { get; set; }
		public int Unknown6 { get; set; }
		public int Unknown7 { get; set; }
		public int Unknown8 { get; set; }
		[UserStringIndex]
		public int Index2 { get; set; }
		public int Unknown10 { get; set; }
		public int Unknown11 { get; set; }
		public bool IsKeystone { get; set; }
		public bool IsNotable { get; set; }
		public int Unknown12 { get; set; }
		public bool IsJustIcon { get; set; }
		public Int64 Unknown13 { get; set; }

		public PassiveSkills(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Icon = inStream.ReadInt32();
			Unknown2 = inStream.ReadInt32();
			Unknown3 = inStream.ReadInt32();
			Unknown4 = inStream.ReadInt32();
			Unknown5 = inStream.ReadInt32();
			Unknown6 = inStream.ReadInt32();
			Unknown7 = inStream.ReadInt32();
			Unknown8 = inStream.ReadInt32();
			Index2 = inStream.ReadInt32();
			Unknown10 = inStream.ReadInt32();
			Unknown11 = inStream.ReadInt32();
			IsKeystone = inStream.ReadBoolean();
			IsNotable = inStream.ReadBoolean();
			Unknown12 = inStream.ReadInt32();
			IsJustIcon = inStream.ReadBoolean();
			Unknown13 = inStream.ReadInt64();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Icon);
			outStream.Write(Unknown2);
			outStream.Write(Unknown3);
			outStream.Write(Unknown4);
			outStream.Write(Unknown5);
			outStream.Write(Unknown6);
			outStream.Write(Unknown7);
			outStream.Write(Unknown8);
			outStream.Write(Index2);
			outStream.Write(Unknown10);
			outStream.Write(Unknown11);
			outStream.Write(IsKeystone);
			outStream.Write(IsNotable);
			outStream.Write(Unknown12);
			outStream.Write(IsJustIcon);
			outStream.Write(Unknown13);
		}

		public override int GetSize()
		{
			return 0x3F;
		}
	}
}