using System;
using System.IO;

namespace LibDat.Files
{
	public class StrDexIntMissionExtraRequirement : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		public int Unknown1 { get; set; }
		public int Unknown2 { get; set; }
		public int Unknown3 { get; set; }
		public int Unknown4 { get; set; }
		public bool Flag0 { get; set; }
		public bool Flag1 { get; set; }
		public bool Flag2 { get; set; }
		public Int64 Unknown5 { get; set; }
		public int Unknown7 { get; set; }
		public int Unknown8 { get; set; }

		public StrDexIntMissionExtraRequirement()
		{

		}

		public StrDexIntMissionExtraRequirement(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Unknown1 = inStream.ReadInt32();
			Unknown2 = inStream.ReadInt32();
			Unknown3 = inStream.ReadInt32();
			Unknown4 = inStream.ReadInt32();
			Flag0 = inStream.ReadBoolean();
			Flag1 = inStream.ReadBoolean();
			Flag2 = inStream.ReadBoolean();
			Unknown5 = inStream.ReadInt64();
			Unknown7 = inStream.ReadInt32();
			Unknown8 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Unknown1);
			outStream.Write(Unknown2);
			outStream.Write(Unknown3);
			outStream.Write(Unknown4);
			outStream.Write(Flag0);
			outStream.Write(Flag1);
			outStream.Write(Flag2);
			outStream.Write(Unknown5);
			outStream.Write(Unknown7);
			outStream.Write(Unknown8);
		}

		public override int GetSize()
		{
			return 0x27;
		}
	}
}