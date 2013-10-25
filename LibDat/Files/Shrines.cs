using System;
using System.IO;

namespace LibDat.Files
{
	public class Shrines : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		public int Unknown0 { get; set; }
        [UserStringIndex]
        public int Name { get; set; }
		public bool ChargesShared { get; set; } // guess
        public int Unknown2 { get; set; }
        public int Unknown3 { get; set; }
        public int Unknown4 { get; set; }
        public int Unknown5 { get; set; }
        [UserStringIndex]
        public int Description { get; set; }
        public Int64 Unknown7 { get; set; }
        public Int64 Unknown8 { get; set; }
        public Int64 Unknown9 { get; set; }
        public int Unknown10 { get; set; }
        public int Unknown11 { get; set; }
        public int Unknown12 { get; set; }
        public int Unknown13 { get; set; }
        public bool Unknown14 { get; set; }
        public Int64 Unknown15 { get; set; }

		public Shrines(BinaryReader inStream)
		{
            Id = inStream.ReadInt32();
            Unknown0 = inStream.ReadInt32();
            Name = inStream.ReadInt32();
            ChargesShared = inStream.ReadBoolean();
            Unknown2 = inStream.ReadInt32();
            Unknown3 = inStream.ReadInt32();
            Unknown4 = inStream.ReadInt32();
            Unknown5 = inStream.ReadInt32();
            Description = inStream.ReadInt32();
            Unknown7 = inStream.ReadInt64();
            Unknown8 = inStream.ReadInt64();
            Unknown9 = inStream.ReadInt64();
            Unknown10 = inStream.ReadInt32();
            Unknown11 = inStream.ReadInt32();
            Unknown12 = inStream.ReadInt32();
            Unknown13 = inStream.ReadInt32();
            Unknown14 = inStream.ReadBoolean();
            Unknown15 = inStream.ReadInt64();
		}
		public override void Save(BinaryWriter outStream)
		{
            outStream.Write(Id);
            outStream.Write(Unknown0);
            outStream.Write(Name);
            outStream.Write(ChargesShared);
            outStream.Write(Unknown2);
            outStream.Write(Unknown3);
            outStream.Write(Unknown4);
            outStream.Write(Unknown5);
            outStream.Write(Description);
            outStream.Write(Unknown7);
            outStream.Write(Unknown8);
            outStream.Write(Unknown9);
            outStream.Write(Unknown10);
            outStream.Write(Unknown11);
            outStream.Write(Unknown12);
            outStream.Write(Unknown13);
            outStream.Write(Unknown14);
            outStream.Write(Unknown15);
		}

		public override int GetSize()
		{
			return 0x52;
		}
	}
}
