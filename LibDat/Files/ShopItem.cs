using System.IO;

namespace LibDat.Files
{
	public class ShopItem : BaseDat
	{
		[StringIndex]
		public int Index0 { get; set; }
		[StringIndex]
		public int Index1 { get; set; }
		[StringIndex]
		public int Index2 { get; set; }
		public int Unknown3 { get; set; }
		[StringIndex]
		public int Index3 { get; set; }
		public bool Flag0 { get; set; }
		public int Unknown5 { get; set; }
		[StringIndex]
		public int Index4 { get; set; }
		public int Unknown7 { get; set; }

		public ShopItem(BinaryReader inStream)
		{
			Index0 = inStream.ReadInt32();
			Index1 = inStream.ReadInt32();
			Index2 = inStream.ReadInt32();
			Unknown3 = inStream.ReadInt32();
			Index3 = inStream.ReadInt32();
			Flag0 = inStream.ReadBoolean();
			Unknown5 = inStream.ReadInt32();
			Index4 = inStream.ReadInt32();
			Unknown7 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Index0);
			outStream.Write(Index1);
			outStream.Write(Index2);
			outStream.Write(Unknown3);
			outStream.Write(Index3);
			outStream.Write(Flag0);
			outStream.Write(Unknown5);
			outStream.Write(Index4);
			outStream.Write(Unknown7);
		}

		public override int GetSize()
		{
			return 0x21;
		}
	}
}