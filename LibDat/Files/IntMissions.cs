using System;
using System.IO;

namespace LibDat.Files
{
	public class IntMissions : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		public int Unknown1 { get; set; }
		public int Unknown2 { get; set; }
		public int Unknown3 { get; set; }
		public int Unknown4 { get; set; }
		public int Unknown5 { get; set; }
		public bool Flag0 { get; set; }
		public int Unknown6 { get; set; }
		public bool Flag1 { get; set; }
		public bool Flag2 { get; set; }
		public bool Flag3 { get; set; }
		public int Unknown8 { get; set; }
		public int Unknown9 { get; set; }
		public int Unknown10 { get; set; }
		public bool Flag4 { get; set; }
		public bool Flag5 { get; set; }
		public bool Flag6 { get; set; }
		public bool Flag7 { get; set; }
		public bool Flag8 { get; set; }
		public bool Flag9 { get; set; }
		public bool Flag10 { get; set; }
		public Int64 Unknown12 { get; set; }
		public int Unknown13 { get; set; }

		public IntMissions()
		{

		}

		public IntMissions(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Unknown1 = inStream.ReadInt32();
			Unknown2 = inStream.ReadInt32();
			Unknown3 = inStream.ReadInt32();
			Unknown4 = inStream.ReadInt32();
			Unknown5 = inStream.ReadInt32();
			Flag0 = inStream.ReadBoolean();
			Unknown6 = inStream.ReadInt32();
			Flag1 = inStream.ReadBoolean();
			Flag2 = inStream.ReadBoolean();
			Flag3 = inStream.ReadBoolean();
			Unknown8 = inStream.ReadInt32();
			Unknown9 = inStream.ReadInt32();
			Unknown10 = inStream.ReadInt32();
			Flag4 = inStream.ReadBoolean();
			Flag5 = inStream.ReadBoolean();
			Flag6 = inStream.ReadBoolean();
			Flag7 = inStream.ReadBoolean();
			Flag8 = inStream.ReadBoolean();
			Flag9 = inStream.ReadBoolean();
			Flag10 = inStream.ReadBoolean();
			Unknown12 = inStream.ReadInt64();
			Unknown13 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Unknown1);
			outStream.Write(Unknown2);
			outStream.Write(Unknown3);
			outStream.Write(Unknown4);
			outStream.Write(Unknown5);
			outStream.Write(Flag0);
			outStream.Write(Unknown6);
			outStream.Write(Flag1);
			outStream.Write(Flag2);
			outStream.Write(Flag3);
			outStream.Write(Unknown8);
			outStream.Write(Unknown9);
			outStream.Write(Unknown10);
			outStream.Write(Flag4);
			outStream.Write(Flag5);
			outStream.Write(Flag6);
			outStream.Write(Flag7);
			outStream.Write(Flag8);
			outStream.Write(Flag9);
			outStream.Write(Flag10);
			outStream.Write(Unknown12);
			outStream.Write(Unknown13);
		}

		public override int GetSize()
		{
			return 0x3F;
		}
	}
}