using System;
using System.IO;

namespace LibDat.Files
{
	public class Maps : BaseDat
	{
		public Int64 Unknown0 { get; set; }
		public Int64 Unknown1 { get; set; }
		public Int64 Unknown2 { get; set; }
		public Int64 Unknown3 { get; set; }

		public Maps(BinaryReader inStream)
		{
			Unknown0 = inStream.ReadInt64();
			Unknown1 = inStream.ReadInt64();
			Unknown2 = inStream.ReadInt64();
			Unknown3 = inStream.ReadInt64();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Unknown0);
			outStream.Write(Unknown1);
			outStream.Write(Unknown2);
			outStream.Write(Unknown3);
		}

		public override int GetSize()
		{
			return 0x20;
		}
	}
}