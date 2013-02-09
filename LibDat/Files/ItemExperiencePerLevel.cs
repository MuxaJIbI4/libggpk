using System;
using System.IO;

namespace LibDat.Files
{
	public class ItemExperiencePerLevel : BaseDat
	{
		public Int64 Unknown0 { get; set; }
		public int Unknown1 { get; set; }
		public int Unknown2 { get; set; }

		public ItemExperiencePerLevel(BinaryReader inStream)
		{
			Unknown0 = inStream.ReadInt64();
			Unknown1 = inStream.ReadInt32();
			Unknown2 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Unknown0);
			outStream.Write(Unknown1);
			outStream.Write(Unknown2);
		}

		public override int GetSize()
		{
			return 0x10;
		}
	}
}