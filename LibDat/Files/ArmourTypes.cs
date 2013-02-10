using System;
using System.IO;

namespace LibDat.Files
{
	public class ArmourTypes : BaseDat
	{
		public Int64 Unkown0 { get; set; }

		public ArmourTypes(BinaryReader inStream)
		{
			Unkown0 = inStream.ReadInt64();
		}

		public override void Save(System.IO.BinaryWriter outStream)
		{
			outStream.Write(Unkown0);
		}

		public override int GetSize()
		{
			return 0x08;
		}
	}
}
