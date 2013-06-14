using System.IO;

namespace LibDat.Files
{
	public class ItemVisualEffect : BaseDat
	{
		[StringIndex]
		public int Index0 { get; set; }
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
		public int Unknown0 { get; set; }
		[StringIndex]
		public int Index9 { get; set; }
		[StringIndex]
		public int Index10 { get; set; }
		[StringIndex]
		public int Index11 { get; set; }
		[StringIndex]
		public int Index12 { get; set; }
		[StringIndex]
		public int Index13 { get; set; }
		public bool Flag1 { get; set; }

		public ItemVisualEffect(BinaryReader inStream)
		{
			Index0 = inStream.ReadInt32();
			Index1 = inStream.ReadInt32();
			Index2 = inStream.ReadInt32();
			Index3 = inStream.ReadInt32();
			Index4 = inStream.ReadInt32();
			Index5 = inStream.ReadInt32();
			Index6 = inStream.ReadInt32();
			Index7 = inStream.ReadInt32();
			Unknown0 = inStream.ReadInt32();
			Index9 = inStream.ReadInt32();
			Index10 = inStream.ReadInt32();
			Index11 = inStream.ReadInt32();
			Index12 = inStream.ReadInt32();
			Index13 = inStream.ReadInt32();
			Flag1 = inStream.ReadBoolean();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Index0);
			outStream.Write(Index1);
			outStream.Write(Index2);
			outStream.Write(Index3);
			outStream.Write(Index4);
			outStream.Write(Index5);
			outStream.Write(Index6);
			outStream.Write(Index7);
			outStream.Write(Unknown0);
			outStream.Write(Index9);
			outStream.Write(Index10);
			outStream.Write(Index11);
			outStream.Write(Index12);
			outStream.Write(Index13);
			outStream.Write(Flag1);
		}

		public override int GetSize()
		{
			return 0x39;
		}
	}
}