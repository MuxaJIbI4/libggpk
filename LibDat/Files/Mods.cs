using System;
using System.IO;

namespace LibDat.Files
{
	public class Mods : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		public int Unknown0 { get; set; }
		// Generalized mod group
		//		group 45 = increased attack speed
		//		group 80 = local increased attack speed
		//public int Group { get; set; } // Removed as of latest patch
		public int Level { get; set; }
		public Int64 Stat1Group { get; set; }
		public Int64 Stat2Group { get; set; }
		public Int64 Stat3Group { get; set; }
		public Int64 Stat4Group { get; set; }
		// 1 - Item
		// 2 - Flask
		// 3 - Monster
		// 4 - Chest
		// 5 - Map
		public int Domain { get; set; }
		[UserStringIndex]
		public int Name { get; set; }
		// 1 - Prefix
		// 2 - Suffix
		// 3 - Cannot be generated
		public int GenerationType { get; set; }
		[StringIndex]
		public int CorrectGroup { get; set; }
		public int Stat1Min { get; set; }
		public int Stat1Max { get; set; }
		public int Stat2Min { get; set; }
		public int Stat2Max { get; set; }
		public int Stat3Min { get; set; }
		public int Stat3Max { get; set; }
		public int Stat4Min { get; set; }
		public int Stat4Max { get; set; }
		public int Data0Length { get; set; }
		[UInt64Index]
		public int Data0 { get; set; }
		public int Data1Length { get; set; }
		[UInt32Index]
		public int Data1 { get; set; }
		public Int64 Unknown27 { get; set; }
		public int Unknown28 { get; set; }
		public int Data2Length { get; set; }
		[UInt64Index]
		public int Data2 { get; set; }
		public Int64 Unknown32 { get; set; }
		public int Data3Length { get; set; }
		[UInt32Index]
		public int Data3 { get; set; }
		public int Data4Length { get; set; }
		[UInt32Index]
		public int Data4 { get; set; }
		[StringIndex]
		public int Metadata { get; set; }
		public Int64 Unknown39 { get; set; }
		public Int64 Unknown41 { get; set; }
		public Int64 Unknown43 { get; set; }
		public int Unknown45 { get; set; }
		public int Unknown46 { get; set; }
		public Int64 Unknown47 { get; set; }

		public Mods(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Unknown0 = inStream.ReadInt32();
			Level = inStream.ReadInt32();
			Stat1Group = inStream.ReadInt64();
			Stat2Group = inStream.ReadInt64();
			Stat3Group = inStream.ReadInt64();
			Stat4Group = inStream.ReadInt64();
			Domain = inStream.ReadInt32();
			Name = inStream.ReadInt32();
			GenerationType = inStream.ReadInt32();
			CorrectGroup = inStream.ReadInt32();
			Stat1Min = inStream.ReadInt32();
			Stat1Max = inStream.ReadInt32();
			Stat2Min = inStream.ReadInt32();
			Stat2Max = inStream.ReadInt32();
			Stat3Min = inStream.ReadInt32();
			Stat3Max = inStream.ReadInt32();
			Stat4Min = inStream.ReadInt32();
			Stat4Max = inStream.ReadInt32();
			Data0Length = inStream.ReadInt32();
			Data0 = inStream.ReadInt32();
			Data1Length = inStream.ReadInt32();
			Data1 = inStream.ReadInt32();
			Unknown27 = inStream.ReadInt64();
			Unknown28 = inStream.ReadInt32();
			Data2Length = inStream.ReadInt32();
			Data2 = inStream.ReadInt32();
			Unknown32 = inStream.ReadInt64();
			Data3Length = inStream.ReadInt32();
			Data3 = inStream.ReadInt32();
			Data4Length = inStream.ReadInt32();
			Data4 = inStream.ReadInt32();
			Metadata = inStream.ReadInt32();
			Unknown39 = inStream.ReadInt64();
			Unknown41 = inStream.ReadInt64();
			Unknown43 = inStream.ReadInt64();
			Unknown45 = inStream.ReadInt32();
			Unknown46 = inStream.ReadInt32();
			Unknown47 = inStream.ReadInt64();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Unknown0);
			outStream.Write(Level);
			outStream.Write(Stat1Group);
			outStream.Write(Stat2Group);
			outStream.Write(Stat3Group);
			outStream.Write(Stat4Group);
			outStream.Write(Domain);
			outStream.Write(Name);
			outStream.Write(GenerationType);
			outStream.Write(CorrectGroup);
			outStream.Write(Stat1Min);
			outStream.Write(Stat1Max);
			outStream.Write(Stat2Min);
			outStream.Write(Stat2Max);
			outStream.Write(Stat3Min);
			outStream.Write(Stat3Max);
			outStream.Write(Stat4Min);
			outStream.Write(Stat4Max);
			outStream.Write(Data0Length);
			outStream.Write(Data0);
			outStream.Write(Data1Length);
			outStream.Write(Data1);
			outStream.Write(Unknown27);
			outStream.Write(Unknown28);
			outStream.Write(Data2Length);
			outStream.Write(Data2);
			outStream.Write(Unknown32);
			outStream.Write(Data3Length);
			outStream.Write(Data3);
			outStream.Write(Data4Length);
			outStream.Write(Data4);
			outStream.Write(Metadata);
			outStream.Write(Unknown39);
			outStream.Write(Unknown41);
			outStream.Write(Unknown43);
			outStream.Write(Unknown45);
			outStream.Write(Unknown46);
			outStream.Write(Unknown47);
		}

		public override int GetSize()
		{
			return 0xC4;
		}
	}
}
