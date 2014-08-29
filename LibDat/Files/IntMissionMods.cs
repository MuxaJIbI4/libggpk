using System;
using System.IO;

namespace LibDat.Files
{
	public class IntMissionMods : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		public int Unknown1 { get; set; }
		public int Unknown2 { get; set; }
		public int Unknown3 { get; set; }
		public int Unknown4 { get; set; }
		public int Unknown5 { get; set; }
		public Int64 Unknown6 { get; set; }
		public Int64 Unknown8 { get; set; }
		public int Unknown10 { get; set; }
		public int Unknown11 { get; set; }

		public IntMissionMods()
		{

		}

		public IntMissionMods(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Unknown1 = inStream.ReadInt32();
			Unknown2 = inStream.ReadInt32();
			Unknown3 = inStream.ReadInt32();
			Unknown4 = inStream.ReadInt32();
			Unknown5 = inStream.ReadInt32();
			Unknown6 = inStream.ReadInt64();
			Unknown8 = inStream.ReadInt64();
			Unknown10 = inStream.ReadInt32();
			Unknown11 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Unknown1);
			outStream.Write(Unknown2);
			outStream.Write(Unknown3);
			outStream.Write(Unknown4);
			outStream.Write(Unknown5);
			outStream.Write(Unknown6);
			outStream.Write(Unknown8);
			outStream.Write(Unknown10);
			outStream.Write(Unknown11);
		}

		public override int GetSize()
		{
			return 0x30;
		}
	}
}