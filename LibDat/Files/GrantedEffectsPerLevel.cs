using System;
using System.IO;

namespace LibDat.Files
{
	public class GrantedEffectsPerLevel : BaseDat
	{
		public Int64 Unknown0 { get; set; }
		public int Unknown1 { get; set; }
		public int Data0Length { get; set; }
		[UInt64Index]
		public int Data0 { get; set; }
		public int Stat1 { get; set; }
		public int Stat2 { get; set; }
		public int Stat3 { get; set; }
		public int Stat4 { get; set; }
		public int Stat5 { get; set; }
		public int Stat6 { get; set; }
		public int Stat7 { get; set; }
		public int Stat8 { get; set; }
		public Int64 Unknown12 { get; set; }
		public int Unknown14 { get; set; }
		public int Unknown15 { get; set; }
		public int Unknown16 { get; set; }
		public int Unknown17 { get; set; }
		public int Data1Length { get; set; }
		[UInt64Index]
		public int Data1 { get; set; }
		public int Data2Length { get; set; }
		[UInt32Index]
		public int Data2 { get; set; }
		public int Unknown22 { get; set; }
		public int Unknown23 { get; set; }
		public int Unknown24 { get; set; }
		public int Unknown25 { get; set; }
		public int Unknown26 { get; set; }
		public int Unknown27 { get; set; }
		public int Data3Length { get; set; }
		[UInt64Index]
		public int Data3 { get; set; }
		public bool Flag0 { get; set; }

		public GrantedEffectsPerLevel(BinaryReader inStream)
		{
			Unknown0 = inStream.ReadInt64();
			Unknown1 = inStream.ReadInt32();
			Data0Length = inStream.ReadInt32();
			Data0 = inStream.ReadInt32();
			Stat1 = inStream.ReadInt32();
			Stat2 = inStream.ReadInt32();
			Stat3 = inStream.ReadInt32();
			Stat4 = inStream.ReadInt32();
			Stat5 = inStream.ReadInt32();
			Stat6 = inStream.ReadInt32();
			Stat7 = inStream.ReadInt32();
			Stat8 = inStream.ReadInt32();
			Unknown12 = inStream.ReadInt64();

			Unknown14 = inStream.ReadInt32();
			Unknown15 = inStream.ReadInt32();
			Unknown16 = inStream.ReadInt32();
			Unknown17 = inStream.ReadInt32();
			Data1Length = inStream.ReadInt32();
			Data1 = inStream.ReadInt32();
			Data2Length = inStream.ReadInt32();
			Data2 = inStream.ReadInt32();
			Unknown22 = inStream.ReadInt32();
			Unknown23 = inStream.ReadInt32();
			Unknown24 = inStream.ReadInt32();
			Unknown25 = inStream.ReadInt32();
			Unknown26 = inStream.ReadInt32();
			Unknown27 = inStream.ReadInt32();
			Data3Length = inStream.ReadInt32();
			Data3 = inStream.ReadInt32();
			Flag0 = inStream.ReadBoolean();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Unknown0);
			outStream.Write(Unknown1);
			outStream.Write(Data0Length);
			outStream.Write(Data0);
			outStream.Write(Stat1);
			outStream.Write(Stat2);
			outStream.Write(Stat3);
			outStream.Write(Stat4);
			outStream.Write(Stat5);
			outStream.Write(Stat6);
			outStream.Write(Stat7);
			outStream.Write(Stat8);
			outStream.Write(Unknown12);
			outStream.Write(Unknown14);
			outStream.Write(Unknown15);
			outStream.Write(Unknown16);
			outStream.Write(Unknown17);
			outStream.Write(Data1Length);
			outStream.Write(Data1);
			outStream.Write(Data2Length);
			outStream.Write(Data2);
			outStream.Write(Unknown22);
			outStream.Write(Unknown23);
			outStream.Write(Unknown24);
			outStream.Write(Unknown25);
			outStream.Write(Unknown26);
			outStream.Write(Unknown27);
			outStream.Write(Data3Length);
			outStream.Write(Data3);
			outStream.Write(Flag0);
		}

		public override int GetSize()
		{
			return 0x7D;
		}
	}
}