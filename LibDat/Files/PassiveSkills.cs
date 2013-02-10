using System.IO;

namespace LibDat.Files
{
	public class PassiveSkills : BaseDat
	{
		[StringIndex]
		public int Index0 { get; set; }
		[StringIndex]
		public int Index1 { get; set; }
		public int Unknown2 { get; set; }
		public int Unknown3 { get; set; }
		public int Unknown4 { get; set; }
		public int Unknown5 { get; set; }
		public int Unknown6 { get; set; }
		public int Unknown7 { get; set; }
		public int Unknown8 { get; set; }
		[StringIndex]
		public int Index2 { get; set; }
		public int Unknown10 { get; set; }
		public int Unknown11 { get; set; }
		public bool Flag0 { get; set; }
		public bool Flag1 { get; set; }
		public int Unknown12 { get; set; }
		public bool Flag2 { get; set; }

		public PassiveSkills(BinaryReader inStream)
		{
			Index0 = inStream.ReadInt32();
			Index1 = inStream.ReadInt32();
			Unknown2 = inStream.ReadInt32();
			Unknown3 = inStream.ReadInt32();
			Unknown4 = inStream.ReadInt32();
			Unknown5 = inStream.ReadInt32();
			Unknown6 = inStream.ReadInt32();
			Unknown7 = inStream.ReadInt32();
			Unknown8 = inStream.ReadInt32();
			Index2 = inStream.ReadInt32();
			Unknown10 = inStream.ReadInt32();
			Unknown11 = inStream.ReadInt32();
			Flag0 = inStream.ReadBoolean();
			Flag1 = inStream.ReadBoolean();
			Unknown12 = inStream.ReadInt32();
			Flag2 = inStream.ReadBoolean();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Index0);
			outStream.Write(Index1);
			outStream.Write(Unknown2);
			outStream.Write(Unknown3);
			outStream.Write(Unknown4);
			outStream.Write(Unknown5);
			outStream.Write(Unknown6);
			outStream.Write(Unknown7);
			outStream.Write(Unknown8);
			outStream.Write(Index2);
			outStream.Write(Unknown10);
			outStream.Write(Unknown11);
			outStream.Write(Flag0);
			outStream.Write(Flag1);
			outStream.Write(Unknown12);
			outStream.Write(Flag2);
		}

		public override int GetSize()
		{
			return 0x37;
		}
	}
}