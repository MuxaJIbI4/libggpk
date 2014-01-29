using System;
using System.IO;

namespace LibDat.Files
{
	public class MicrotransactionPortalVariations : BaseDat
	{
		public Int64 ItemKey { get; set; }
		[StringIndex]
		public int AnimatedObject { get; set; }

		public MicrotransactionPortalVariations(BinaryReader inStream)
		{
			ItemKey = inStream.ReadInt64();
			AnimatedObject = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(ItemKey);
			outStream.Write(AnimatedObject);
		}

		public override int GetSize()
		{
			return 0x0C;
		}
	}
}