using System;
using System.IO;

namespace LibDat.Files
{
	public class BuffVisuals : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		[StringIndex]
		public int BuffIcon { get; set; }
		[StringIndex]
		public int EffectA { get; set; }
		[StringIndex]
		public int EffectB { get; set; }
		public Int64 Unknown4 { get; set; }
		public Int64 Unknown5 { get; set; }

		public BuffVisuals()
		{
			
		}

		public BuffVisuals(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			BuffIcon = inStream.ReadInt32();
			EffectA = inStream.ReadInt32();
			EffectB = inStream.ReadInt32();
			Unknown4 = inStream.ReadInt64();
			Unknown5 = inStream.ReadInt64();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(BuffIcon);
			outStream.Write(EffectA);
			outStream.Write(EffectB);
			outStream.Write(Unknown4);
			outStream.Write(Unknown5);
		}

		public override int GetSize()
		{
			return 0x20;
		}
	}
}