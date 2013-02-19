using System.IO;

namespace LibDat.Files
{
	public class BloodTypes : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		[StringIndex]
		public int Index1 { get; set; }
		[StringIndex]
		public int Index2 { get; set; }
		[StringIndex]
		public int Index3 { get; set; }
		[StringIndex]
		public int Index4 { get; set; }
		[StringIndex]
		public int Index5 { get; set; }
		[StringIndex]
		public int Index6 { get; set; }
		[StringIndex]
		public int Index7 { get; set; }
		[StringIndex]
		public int Index8 { get; set; }


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
		}

		public override int GetSize()
		{
			return 0x24;
		}
	}
}
