using System;
using System.IO;

namespace LibDat.Files
{
	public class CurrencyItems : BaseDat
	{
		public Int64 Unknown0 { get; set; }
		public int Unknown1 { get; set; }
		public int Unknown2 { get; set; }
		[StringIndex]
		public int Action { get; set; }
		[StringIndex]
		public int Directions { get; set; }
		public Int64 Unknown3 { get; set; }
		[StringIndex]
		public int Description { get; set; }

		public CurrencyItems(BinaryReader inStream)
		{
			Unknown0 = inStream.ReadInt64();
			Unknown1 = inStream.ReadInt32();
			Unknown2 = inStream.ReadInt32();
			Action = inStream.ReadInt32();
			Directions = inStream.ReadInt32();
			Unknown3 = inStream.ReadInt64();
			Description = inStream.ReadInt32();
		}
		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Unknown0);
			outStream.Write(Unknown1);
			outStream.Write(Unknown2);
			outStream.Write(Action);
			outStream.Write(Directions);
			outStream.Write(Unknown3);
			outStream.Write(Description);
		}

		public override int GetSize()
		{
			return 0x24;
		}
	}
}
