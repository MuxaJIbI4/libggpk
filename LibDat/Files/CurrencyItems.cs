using System;
using System.IO;

namespace LibDat.Files
{
	public class CurrencyItems : BaseDat
	{
		public Int64 Unknown0 { get; set; }
		public int Stacks { get; set; }
		public int Unknown2 { get; set; }
		[StringIndex]
		public int Action { get; set; }
		[UserStringIndex]
		public int Directions { get; set; }
		public Int64 Unknown3 { get; set; }
		[UserStringIndex]
		public int Description { get; set; }
		public Int64 Unknown4 { get; set; }
		public bool Flag1 { get; set; }
		[UserStringIndex]
		public int CosmeticTypeName { get; set; }
		public Int64 Unknown5 { get; set; }
		public int Unknown14 { get; set; }
		public int Unknown15 { get; set; }
		public int Unknown16 { get; set; }
		public int Unknown17 { get; set; }

		public CurrencyItems(BinaryReader inStream)
		{
			Unknown0 = inStream.ReadInt64();
			Stacks = inStream.ReadInt32();
			Unknown2 = inStream.ReadInt32();
			Action = inStream.ReadInt32();
			Directions = inStream.ReadInt32();
			Unknown3 = inStream.ReadInt64();
			Description = inStream.ReadInt32();
			Unknown4 = inStream.ReadInt64();
			Flag1 = inStream.ReadBoolean();
			CosmeticTypeName = inStream.ReadInt32();
			Unknown5 = inStream.ReadInt64();
			Unknown14 = inStream.ReadInt32();
			Unknown15 = inStream.ReadInt32();
			Unknown16 = inStream.ReadInt32();
			Unknown17 = inStream.ReadInt32();

		}
		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Unknown0);
			outStream.Write(Stacks);
			outStream.Write(Unknown2);
			outStream.Write(Action);
			outStream.Write(Directions);
			outStream.Write(Unknown3);
			outStream.Write(Description);
			outStream.Write(Unknown4);
			outStream.Write(Flag1);
			outStream.Write(CosmeticTypeName);
			outStream.Write(Unknown5);
			outStream.Write(Unknown14);
			outStream.Write(Unknown15);
			outStream.Write(Unknown16);
			outStream.Write(Unknown17);
		}

		public override int GetSize()
		{
			return 0x49;
		}
	}
}
