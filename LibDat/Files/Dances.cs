using System;
using System.IO;

namespace LibDat.Files
{
	public class Dances : BaseDat
	{
		public Int64 ItemID { get; set; }
		public Int64 CharactersID { get; set; }

		public Dances(BinaryReader inStream)
		{
			ItemID = inStream.ReadInt64();
			CharactersID = inStream.ReadInt64();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(ItemID);
			outStream.Write(CharactersID);
		}

		public override int GetSize()
		{
			return 0x10;
		}
	}
}