using System;
using System.IO;

namespace LibDat.Files
{
	public class UniqueSets : BaseDat
	{
		[StringIndex]
		public int Index0 { get; set; }
		public int Unknown0 { get; set; }
		public Int64 Unknown1 { get; set; }
		public int Unknown2 { get; set; }

		public UniqueSets(BinaryReader inStream)
		{
			Index0 = inStream.ReadInt32();
			Unknown0 = inStream.ReadInt32();
			Unknown1 = inStream.ReadInt64();
			Unknown2 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Index0);
			outStream.Write(Unknown0);
			outStream.Write(Unknown1);
			outStream.Write(Unknown2);
		}

		public override int GetSize()
		{
			return 0x14;
		}
	}
}