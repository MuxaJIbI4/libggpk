using System;
using System.IO;

namespace LibDat.Files
{
	public class DexIntMissionGuardMods : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		public Int64 Unknown1 { get; set; }
		public int Unknown3 { get; set; }
		public bool Flag0 { get; set; } // Not sure if the flag is before or after Unknown4...
		public int Unknown4 { get; set; }
		public int Unknown5 { get; set; }


		public DexIntMissionGuardMods()
		{

		}

		public DexIntMissionGuardMods(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Unknown1 = inStream.ReadInt64();
			Unknown3 = inStream.ReadInt32();
			Flag0 = inStream.ReadBoolean();
			Unknown4 = inStream.ReadInt32();
			Unknown5 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Unknown1);
			outStream.Write(Unknown3);
			outStream.Write(Flag0);
			outStream.Write(Unknown4);
			outStream.Write(Unknown5);
		}

		public override int GetSize()
		{
			return 0x19;
		}
	}
}