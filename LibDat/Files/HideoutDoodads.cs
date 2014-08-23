using System;
using System.IO;

namespace LibDat.Files
{
	public class HideoutDoodads : BaseDat
	{
		[StringIndex]
		public int Unknown0 { get; set; }
		public int Unknown1 { get; set; }
		public int Unknown2 { get; set; }
		public int AnimationStringPtr { get; set; } // Pointer to string
		public int Unknown4 { get; set; }
		public int Unknown5 { get; set; }
		public Int64 Unknown6 { get; set; }
		public bool Flag0 { get; set; }

		public HideoutDoodads(BinaryReader inStream)
		{
			Unknown0 = inStream.ReadInt32();
			Unknown1 = inStream.ReadInt32();
			Unknown2 = inStream.ReadInt32();
			AnimationStringPtr = inStream.ReadInt32();
			Unknown4 = inStream.ReadInt32();
			Unknown5 = inStream.ReadInt32();
			Unknown6 = inStream.ReadInt64();
			Flag0 = inStream.ReadBoolean();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Unknown0);
			outStream.Write(Unknown1);
			outStream.Write(Unknown2);
			outStream.Write(AnimationStringPtr);
			outStream.Write(Unknown4);
			outStream.Write(Unknown5);
			outStream.Write(Unknown6);
			outStream.Write(Flag0);
		}

		public override int GetSize()
		{
			return 0x21;
		}
	}
}
