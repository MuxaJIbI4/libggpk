using System;
using System.IO;

namespace LibDat.Files
{
	public class MonsterPacks : BaseDat
	{
		[StringIndex]
		public int Unknown0 { get; set; }
		public int Unknown1 { get; set; }
		public int Unknown2 { get; set; }
		public int Unknown3 { get; set; }
		public int Unknown4 { get; set; }
		public int Unknown5 { get; set; }
		public int Unknown6 { get; set; }
		public int Unknown7 { get; set; }
		public int Data0Length { get; set; }
		[UInt64Index]
		public int Data0 { get; set; }
		public bool Flag0 { get; set; }
		public int Unknown11 { get; set; }
		public int Data1Length { get; set; }
		[UInt32Index]
		public int Data1 { get; set; }
		public int Data2Length { get; set; }
		[UInt64Index]
		public int Data2 { get; set; }

		public MonsterPacks(BinaryReader inStream)
		{
			Unknown0 = inStream.ReadInt32();
			Unknown1 = inStream.ReadInt32();
			Unknown2 = inStream.ReadInt32();
			Unknown3 = inStream.ReadInt32();
			Unknown4 = inStream.ReadInt32();
			Unknown5 = inStream.ReadInt32();
			Unknown6 = inStream.ReadInt32();
			Unknown7 = inStream.ReadInt32();
			Data0Length = inStream.ReadInt32();
			Data0 = inStream.ReadInt32();
			Flag0 = inStream.ReadBoolean();
			Unknown11 = inStream.ReadInt32();
			Data1Length = inStream.ReadInt32();
			Data1 = inStream.ReadInt32();
			Data2Length = inStream.ReadInt32();
			Data2 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Unknown0);
			outStream.Write(Unknown1);
			outStream.Write(Unknown2);
			outStream.Write(Unknown3);
			outStream.Write(Unknown4);
			outStream.Write(Unknown5);
			outStream.Write(Unknown6);
			outStream.Write(Unknown7);
			outStream.Write(Data0Length);
			outStream.Write(Data0);
			outStream.Write(Flag0);
			outStream.Write(Unknown11);
			outStream.Write(Data1Length);
			outStream.Write(Data1);
			outStream.Write(Data2Length);
			outStream.Write(Data2);
		}

		public override int GetSize()
		{
			return 0x3D;
		}
	}
}