using System;
using System.IO;

namespace LibDat.Files
{
	public class Dances : BaseDat
	{
		public Int64 Unknown0 { get; set; }
		public Int64 Unknown1 { get; set; }

		public Dances(BinaryReader inStream)
		{
			Unknown0 = inStream.ReadInt64();
			Unknown1 = inStream.ReadInt64();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Unknown0);
			outStream.Write(Unknown1);
		}

		public override int GetSize()
		{
			return 0x10;
		}
	}
}