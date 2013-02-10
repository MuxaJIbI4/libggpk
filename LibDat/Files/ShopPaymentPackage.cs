using System.IO;

namespace LibDat.Files
{
	public class ShopPaymentPackage : BaseDat
	{
		[StringIndex]
		public int Index0 { get; set; }
		[StringIndex]
		public int Index1 { get; set; }
		public int Unknown2 { get; set; }
		public int Unknown3 { get; set; }

		public ShopPaymentPackage(BinaryReader inStream)
		{
			Index0 = inStream.ReadInt32();
			Index1 = inStream.ReadInt32();
			Unknown2 = inStream.ReadInt32();
			Unknown3 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Index0);
			outStream.Write(Index1);
			outStream.Write(Unknown2);
			outStream.Write(Unknown3);
		}

		public override int GetSize()
		{
			return 0x10;
		}
	}
}