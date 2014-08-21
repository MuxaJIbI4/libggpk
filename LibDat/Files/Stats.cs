using System;
using System.IO;

namespace LibDat.Files
{
	public class Stats : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		public bool Flag0 { get; set; }
		public bool Flag1 { get; set; }
		public bool Flag2 { get; set; }
		public int Unknown2 { get; set; }
		public bool Flag3 { get; set; }
		[UserStringIndex]
		public int Text { get; set; }
		public bool Flag5 { get; set; }
		public bool Flag6 { get; set; }
		public int StatKey1 { get; set; }
		public int StatKey2 { get; set; }
		public bool Flag7 { get; set; }

		public Stats(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Flag0 = inStream.ReadBoolean();
			Flag1 = inStream.ReadBoolean();
			Flag2 = inStream.ReadBoolean();
			Unknown2 = inStream.ReadInt32();
			Flag3 = inStream.ReadBoolean();
			Text = inStream.ReadInt32();
			Flag5 = inStream.ReadBoolean();
			Flag6 = inStream.ReadBoolean();
			StatKey1 = inStream.ReadInt32();
			StatKey2 = inStream.ReadInt32();
			Flag7 = inStream.ReadBoolean();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Flag0);
			outStream.Write(Flag1);
			outStream.Write(Flag2);
			outStream.Write(Unknown2);
			outStream.Write(Flag3);
			outStream.Write(Text);
			outStream.Write(Flag5);
			outStream.Write(Flag6);
			outStream.Write(StatKey1);
			outStream.Write(StatKey2);
			outStream.Write(Flag7);
		}

		public override int GetSize()
		{
			return 0x1B;
		}
	}
}