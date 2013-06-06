using System;
using System.IO;

namespace LibDat.Files
{
	public class WorldAreas : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		[UserStringIndex]
		public int Name { get; set; }
		public int Act { get; set; }
		public bool IsTown { get; set; }
		public bool HasWaypoint { get; set; }
		public int Unknown3 { get; set; }
		public int Unknown4 { get; set; }
		public Int64 MonsterLevel { get; set; }
		public bool Flag2 { get; set; }
		public int Unknown6 { get; set; }
		public int Unknown7 { get; set; }
		[StringIndex]
		public int LoadingScreenA { get; set; }
		public int Unknown9 { get; set; }
		public int Unknown10 { get; set; }
		[StringIndex]
		public int LoadingScreenB { get; set; }
		public int Unknown12 { get; set; }
		public int Unknown13 { get; set; }
		public int Unknown14 { get; set; }
		public int Unknown15 { get; set; }
		public int Unknown16 { get; set; }
		public int Unknown17 { get; set; }
		public int Unknown18 { get; set; }
		public int Unknown19 { get; set; }
		public int Unknown20 { get; set; }
		public int Unknown21 { get; set; }
		public int Unknown22 { get; set; }
		public int Unknown23 { get; set; }
		public int Unknown24 { get; set; }
		public int Unknown25 { get; set; }
		public int Unknown26 { get; set; }
		public int Unknown27 { get; set; }
		public int Unknown28 { get; set; }
		public bool IsPVP { get; set; }
		public int Unknown29 { get; set; }
		public int Unknown30 { get; set; }
		public int Unknown31 { get; set; }
		public int Unknown32 { get; set; }
		public int Unknown33 { get; set; }
		public bool IsMap { get; set; }
		public Int64 Unknown34 { get; set; }

		public WorldAreas(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Name = inStream.ReadInt32();
			Act = inStream.ReadInt32();
			IsTown = inStream.ReadBoolean();
			HasWaypoint = inStream.ReadBoolean();
			Unknown3 = inStream.ReadInt32();
			Unknown4 = inStream.ReadInt32();
			MonsterLevel = inStream.ReadInt64();
			Flag2 = inStream.ReadBoolean();
			Unknown6 = inStream.ReadInt32();
			Unknown7 = inStream.ReadInt32();
			LoadingScreenA = inStream.ReadInt32();
			Unknown9 = inStream.ReadInt32();
			Unknown10 = inStream.ReadInt32();
			LoadingScreenB = inStream.ReadInt32();
			Unknown12 = inStream.ReadInt32();
			Unknown13 = inStream.ReadInt32();
			Unknown14 = inStream.ReadInt32();
			Unknown15 = inStream.ReadInt32();
			Unknown16 = inStream.ReadInt32();
			Unknown17 = inStream.ReadInt32();
			Unknown18 = inStream.ReadInt32();
			Unknown19 = inStream.ReadInt32();
			Unknown20 = inStream.ReadInt32();
			Unknown21 = inStream.ReadInt32();
			Unknown22 = inStream.ReadInt32();
			Unknown23 = inStream.ReadInt32();
			Unknown24 = inStream.ReadInt32();
			Unknown25 = inStream.ReadInt32();
			Unknown26 = inStream.ReadInt32();
			Unknown27 = inStream.ReadInt32();
			Unknown28 = inStream.ReadInt32();
			IsPVP = inStream.ReadBoolean();
			Unknown29 = inStream.ReadInt32();
			Unknown30 = inStream.ReadInt32();
			Unknown31 = inStream.ReadInt32();
			Unknown32 = inStream.ReadInt32();
			Unknown33 = inStream.ReadInt32();
			IsMap = inStream.ReadBoolean();
			Unknown34 = inStream.ReadInt64();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Name);
			outStream.Write(Act);
			outStream.Write(IsTown);
			outStream.Write(HasWaypoint);
			outStream.Write(Unknown3);
			outStream.Write(Unknown4);
			outStream.Write(MonsterLevel);
			outStream.Write(Flag2);
			outStream.Write(Unknown6);
			outStream.Write(Unknown7);
			outStream.Write(LoadingScreenA);
			outStream.Write(Unknown9);
			outStream.Write(Unknown10);
			outStream.Write(LoadingScreenB);
			outStream.Write(Unknown12);
			outStream.Write(Unknown13);
			outStream.Write(Unknown14);
			outStream.Write(Unknown15);
			outStream.Write(Unknown16);
			outStream.Write(Unknown17);
			outStream.Write(Unknown18);
			outStream.Write(Unknown19);
			outStream.Write(Unknown20);
			outStream.Write(Unknown21);
			outStream.Write(Unknown22);
			outStream.Write(Unknown23);
			outStream.Write(Unknown24);
			outStream.Write(Unknown25);
			outStream.Write(Unknown26);
			outStream.Write(Unknown27);
			outStream.Write(Unknown28);
			outStream.Write(IsPVP);
			outStream.Write(Unknown29);
			outStream.Write(Unknown30);
			outStream.Write(Unknown31);
			outStream.Write(Unknown32);
			outStream.Write(Unknown33);
			outStream.Write(IsMap);
			outStream.Write(Unknown34);
		}

		public override int GetSize()
		{
			return 0x99;
		}
	}
}