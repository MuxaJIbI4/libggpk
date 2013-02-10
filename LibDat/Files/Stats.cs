using System;
using System.IO;

namespace LibDat.Files
{
	public class Stats : BaseDat
	{
		[StringIndex]
		public int Index0 { get; set; }
		public bool Flag0 { get; set; }
		public bool Flag1 { get; set; }
		public bool Flag2 { get; set; }
		public int Unknown2 { get; set; }
		public bool Flag3 { get; set; }
		[StringIndex]
		public int Index1 { get; set; }
		public bool Flag4 { get; set; }
		public bool Flag5 { get; set; }
		public bool Flag6 { get; set; }
		public Int64 Unknown4 { get; set; }
		public bool Flag7 { get; set; }

		public Stats(BinaryReader inStream)
		{
			Index0 = inStream.ReadInt32();
			Flag0 = inStream.ReadBoolean();
			Flag1 = inStream.ReadBoolean();
			Flag2 = inStream.ReadBoolean();
			Unknown2 = inStream.ReadInt32();
			Flag3 = inStream.ReadBoolean();
			Index1 = inStream.ReadInt32();
			Flag4 = inStream.ReadBoolean();
			Flag5 = inStream.ReadBoolean();
			Flag6 = inStream.ReadBoolean();
			Unknown4 = inStream.ReadInt64();
			Flag7 = inStream.ReadBoolean();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Index0);
			outStream.Write(Flag0);
			outStream.Write(Flag1);
			outStream.Write(Flag2);
			outStream.Write(Unknown2);
			outStream.Write(Flag3);
			outStream.Write(Index1);
			outStream.Write(Flag4);
			outStream.Write(Flag5);
			outStream.Write(Flag6);
			outStream.Write(Unknown4);
			outStream.Write(Flag7);
		}

		public override int GetSize()
		{
			return 0x1C;
		}
	}
}