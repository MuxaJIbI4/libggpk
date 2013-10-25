using System.IO;

namespace LibDat.Files
{
	public class NPCs : BaseDat
	{
		[StringIndex]
		public int Index0 { get; set; }
		[UserStringIndex]
		public int Index1 { get; set; }
		public int Unkonwn0 { get; set; }
		public int Unknown1 { get; set; }
		public int Unknown2 { get; set; }
		public int Unknown3 { get; set; }
		public int Unknown4 { get; set; }
		public int Unknown5 { get; set; }
		public int Unknown6 { get; set; }
		public int Unknown7 { get; set; }
		public int Unknown8 { get; set; }
		public int Unknown9 { get; set; }
		public int Unknown10 { get; set; }
		public int Unknown11 { get; set; }
		public int Unknown12 { get; set; }
		public int Unknown13 { get; set; }
        public int Unknown14 { get; set; }
        public int Unknown15 { get; set; }
        public int Unknown16 { get; set; }
        public int Unknown17 { get; set; }

		public NPCs(BinaryReader inStream)
		{
			Index0 = inStream.ReadInt32();
			Index1 = inStream.ReadInt32();
			Unkonwn0 = inStream.ReadInt32();
			Unknown1 = inStream.ReadInt32();
			Unknown2 = inStream.ReadInt32();
			Unknown3 = inStream.ReadInt32();
			Unknown4 = inStream.ReadInt32();
			Unknown5 = inStream.ReadInt32();
			Unknown6 = inStream.ReadInt32();
			Unknown7 = inStream.ReadInt32();
			Unknown8 = inStream.ReadInt32();
			Unknown9 = inStream.ReadInt32();
			Unknown10 = inStream.ReadInt32();
			Unknown11 = inStream.ReadInt32();
			Unknown12 = inStream.ReadInt32();
			Unknown13 = inStream.ReadInt32();
            Unknown14 = inStream.ReadInt32();
            Unknown15 = inStream.ReadInt32();
            Unknown16 = inStream.ReadInt32();
            Unknown17 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Index0);
			outStream.Write(Index1);
			outStream.Write(Unkonwn0);
			outStream.Write(Unknown1);
			outStream.Write(Unknown2);
			outStream.Write(Unknown3);
			outStream.Write(Unknown4);
			outStream.Write(Unknown5);
			outStream.Write(Unknown6);
			outStream.Write(Unknown7);
			outStream.Write(Unknown8);
			outStream.Write(Unknown9);
			outStream.Write(Unknown10);
			outStream.Write(Unknown11);
			outStream.Write(Unknown12);
			outStream.Write(Unknown13);
            outStream.Write(Unknown14);
            outStream.Write(Unknown15);
            outStream.Write(Unknown16);
            outStream.Write(Unknown17);
		}

		public override int GetSize()
		{
			return 0x50;
		}
	}
}