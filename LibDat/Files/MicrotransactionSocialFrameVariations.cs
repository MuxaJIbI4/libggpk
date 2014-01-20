using System;
using System.IO;

namespace LibDat.Files
{
	public class MicrotransactionSocialFrameVariations : BaseDat
	{
		public Int64 ItemKey { get; set; }

		public MicrotransactionSocialFrameVariations(BinaryReader inStream)
		{
			ItemKey = inStream.ReadInt64();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(ItemKey);
		}

		public override int GetSize()
		{
			return 0x08;
		}
	}
}