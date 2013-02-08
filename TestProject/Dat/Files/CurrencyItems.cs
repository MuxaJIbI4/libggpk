using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestProject.Dat.Files
{
	class CurrencyItems : BaseDat
	{
		public Int64 Unknown0;
		public int Unknown1;
		public int Unknown2;
		[StringIndex]
		public int Action;
		[StringIndex]
		public int Directions;
		public Int64 Unknown3;
		[StringIndex]
		public int Description;

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
