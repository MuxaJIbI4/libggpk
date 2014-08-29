using System;
using System.IO;

namespace LibDat.Files
{
	public class DexIntMissions : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		public int Unknown1 { get; set; }
		public bool Flag0 { get; set; }
		public bool Flag1 { get; set; }
		public bool Flag2 { get; set; }
		public bool Flag3 { get; set; }
		public int Unknown3 { get; set; }
		public Int64 Unknown4 { get; set; }
		public int Unknown6 { get; set; }
		public int Unknown7 { get; set; }
		public bool Flag4 { get; set; }
		public bool Flag5 { get; set; }
		public int Unknown8 { get; set; }
		public int Unknown9 { get; set; }
		public bool Flag6 { get; set; }
		public int Unknown10 { get; set; }

		public DexIntMissions()
		{

		}

		public DexIntMissions(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Unknown1 = inStream.ReadInt32();
			Flag0 = inStream.ReadBoolean();
			Flag1 = inStream.ReadBoolean();
			Flag2 = inStream.ReadBoolean();
			Flag3 = inStream.ReadBoolean();
			Unknown3 = inStream.ReadInt32();
			Unknown4 = inStream.ReadInt64();
			Unknown6 = inStream.ReadInt32();
			Unknown7 = inStream.ReadInt32();
			Flag4 = inStream.ReadBoolean();
			Flag5 = inStream.ReadBoolean();
			Unknown8 = inStream.ReadInt32();
			Unknown9 = inStream.ReadInt32();
			Flag6 = inStream.ReadBoolean();
			Unknown10 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Unknown1);
			outStream.Write(Flag0);
			outStream.Write(Flag1);
			outStream.Write(Flag2);
			outStream.Write(Flag3);
			outStream.Write(Unknown3);
			outStream.Write(Unknown4);
			outStream.Write(Unknown6);
			outStream.Write(Unknown7);
			outStream.Write(Flag4);
			outStream.Write(Flag5);
			outStream.Write(Unknown8);
			outStream.Write(Unknown9);
			outStream.Write(Flag6);
			outStream.Write(Unknown10);
		}

		public override int GetSize()
		{
			return 0x2F;
		}
	}
}