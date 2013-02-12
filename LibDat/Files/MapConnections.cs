using System;
using System.IO;

namespace LibDat.Files
{
	public class MapConnections : BaseDat
	{
		public Int64 Unknown0 { get; set; }
		public Int64 Unknown1 { get; set; }
		public int Unknown2 { get; set; }
		[UserStringIndex]
		public int RestrictedAreaText { get; set; }
		public int Unknown3 { get; set; }
		public int Unknown4 { get; set; }
		public int Unknown5 { get; set; }

		public MapConnections(BinaryReader inStream)
		{
			Unknown0 = inStream.ReadInt64();
			Unknown1 = inStream.ReadInt64();
			Unknown2 = inStream.ReadInt32();
			RestrictedAreaText = inStream.ReadInt32();
			Unknown3 = inStream.ReadInt32();
			Unknown4 = inStream.ReadInt32();
			Unknown5 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Unknown0);
			outStream.Write(Unknown1);
			outStream.Write(Unknown2);
			outStream.Write(RestrictedAreaText);
			outStream.Write(Unknown3);
			outStream.Write(Unknown4);
			outStream.Write(Unknown5);
		}

		public override int GetSize()
		{
			return 0x24;
		}
	}
}