using System;
using System.IO;

namespace LibDat.Files
{
	public class ShieldTypes : BaseDat
	{
		public Int64 ItemKey { get; set; }
		public int Block { get; set; }

		public ShieldTypes(BinaryReader inStream)
		{
			ItemKey = inStream.ReadInt64();
			Block = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(ItemKey);
			outStream.Write(Block);
		}

		public override int GetSize()
		{
			return 0x0C;
		}
	}
}