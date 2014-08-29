using System;
using System.IO;

namespace LibDat.Files
{
	public class BeyondDemons : BaseDat
	{
		public int Unknown0 { get; set; }
		public int Unknown1 { get; set; }
		public bool Flag0 { get; set; }
		public bool Flag1 { get; set; }

		public BeyondDemons()
		{

		}

		public BeyondDemons(BinaryReader inStream)
		{
			Unknown0 = inStream.ReadInt32();
			Unknown1 = inStream.ReadInt32();
			Flag0 = inStream.ReadBoolean();
			Flag1 = inStream.ReadBoolean();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Unknown0);
			outStream.Write(Unknown1);
			outStream.Write(Flag0);
			outStream.Write(Flag1);
		}

		public override int GetSize()
		{
			return 0xA;
		}
	}
}