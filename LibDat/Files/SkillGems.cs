using System;
using System.IO;

namespace LibDat.Files
{
	public class SkillGems : BaseDat
	{
		public Int64 ItemKey { get; set; }
		public Int64 EffectKey { get; set; }
		public int Str { get; set; }
		public int Dex { get; set; }
		public int Int { get; set; }
		public int Data0Length { get; set; }
		[UInt64Index]
		public int Data0 { get; set; }

		public SkillGems(BinaryReader inStream)
		{
			ItemKey = inStream.ReadInt64();
			EffectKey = inStream.ReadInt64();
			Str = inStream.ReadInt32();
			Dex = inStream.ReadInt32();
			Int = inStream.ReadInt32();
			Data0Length = inStream.ReadInt32();
			Data0 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(ItemKey);
			outStream.Write(EffectKey);
			outStream.Write(Str);
			outStream.Write(Dex);
			outStream.Write(Int);
			outStream.Write(Data0Length);
			outStream.Write(Data0);
		}

		public override int GetSize()
		{
			return 0x24;
		}
	}
}