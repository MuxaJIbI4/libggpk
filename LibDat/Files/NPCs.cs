using System.IO;

namespace LibDat.Files
{
	public class NPCs : BaseDat
	{
		[StringIndex]
		public int Index0 { get; set; }
		[UserStringIndex]
		public int Index1 { get; set; }
		[StringIndex]
		public int Index2 { get; set; }
		[StringIndex]
		public int Index3 { get; set; }
		public int Unknown0 { get; set; }
		public int Unknown1 { get; set; }
		[StringIndex]
		public int Index4 { get; set; }
		public int Unknown2 { get; set; }
		public int Index5 { get; set; }
		public int Unknown3 { get; set; }
		public int Unknown4 { get; set; }
		[StringIndex]
		public int Index6 { get; set; }
		public int Unknown5 { get; set; }
		public int Unknown6 { get; set; }
		public int Unknown7 { get; set; }
		public int Index7 { get; set; }
		public int Unknown8 { get; set; }
		public int Unknown9 { get; set; }

		public NPCs(BinaryReader inStream)
		{
			Index0 = inStream.ReadInt32();
			Index1 = inStream.ReadInt32();
			Index2 = inStream.ReadInt32();
			Index3 = inStream.ReadInt32();
			Unknown0 = inStream.ReadInt32();
			Unknown1 = inStream.ReadInt32();
			Index4 = inStream.ReadInt32();
			Unknown2 = inStream.ReadInt32();
			Index5 = inStream.ReadInt32();
			Unknown3 = inStream.ReadInt32();
			Unknown4 = inStream.ReadInt32();
			Index6 = inStream.ReadInt32();
			Unknown5 = inStream.ReadInt32();
			Unknown6 = inStream.ReadInt32();
			Unknown7 = inStream.ReadInt32();
			Index7 = inStream.ReadInt32();
			Unknown8 = inStream.ReadInt32();
			Unknown9 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Index0);
			outStream.Write(Index1);
			outStream.Write(Index2);
			outStream.Write(Index3);
			outStream.Write(Unknown0);
			outStream.Write(Unknown1);
			outStream.Write(Index4);
			outStream.Write(Unknown2);
			outStream.Write(Index5);
			outStream.Write(Unknown3);
			outStream.Write(Unknown4);
			outStream.Write(Index6);
			outStream.Write(Unknown5);
			outStream.Write(Unknown6);
			outStream.Write(Unknown7);
			outStream.Write(Index7);
			outStream.Write(Unknown8);
			outStream.Write(Unknown9);
		}

		public override int GetSize()
		{
			return 0x48;
		}
	}
}