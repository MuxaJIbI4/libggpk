using System;
using System.IO;

namespace LibDat.Files
{
	public class ModSellPrices : BaseDat
	{
		public Int64 Unknown0 { get; set; }
		public int Data0Length { get; set; }
		[UInt64Index]
		public int Data0 { get; set; }
		public int Data1Length { get; set; }
		[UInt64Index]
		public int Data1 { get; set; }

		public ModSellPrices(BinaryReader inStream)
		{
			Unknown0 = inStream.ReadInt64();
			Data0Length = inStream.ReadInt32();
			Data0 = inStream.ReadInt32();
			Data1Length = inStream.ReadInt32();
			Data1 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Unknown0);
			outStream.Write(Data0Length);
			outStream.Write(Data0);
			outStream.Write(Data1Length);
			outStream.Write(Data1);
		}

		public override int GetSize()
		{
			return 0x18;
		}
	}
}