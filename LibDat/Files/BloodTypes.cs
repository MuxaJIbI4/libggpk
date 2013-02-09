using System.IO;

namespace LibDat.Files
{
	class BloodTypes : BaseDat
	{
		[StringIndex]
		public int Id;
		[StringIndex]
		public int Index1;
		[StringIndex]
		public int Index2;
		[StringIndex]
		public int Index3;
		[StringIndex]
		public int Index4;
		[StringIndex]
		public int Index5;
		[StringIndex]
		public int Index6;
		[StringIndex]
		public int Index7;
		[StringIndex]
		public int Index8;
		[StringIndex]
		public int Index9;
		[StringIndex]
		public int Index10;


		public BloodTypes(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Index1 = inStream.ReadInt32();
			Index2 = inStream.ReadInt32();
			Index3 = inStream.ReadInt32();
			Index4 = inStream.ReadInt32();
			Index5 = inStream.ReadInt32();
			Index6 = inStream.ReadInt32();
			Index7 = inStream.ReadInt32();
			Index8 = inStream.ReadInt32();
			Index9 = inStream.ReadInt32();
			Index10 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Index1);
			outStream.Write(Index2);
			outStream.Write(Index3);
			outStream.Write(Index4);
			outStream.Write(Index5);
			outStream.Write(Index6);
			outStream.Write(Index7);
			outStream.Write(Index8);
			outStream.Write(Index9);
			outStream.Write(Index10);
		}

		public override int GetSize()
		{
			return 0x2c;
		}
	}
}
