using System;
using System.IO;

namespace LibDat.Files
{
	public class WeaponTypes : BaseDat
	{
		public Int64 ItemKey { get; set; }
		public int Critical { get; set; }
		public int Speed { get; set; }
		public int DamageMin { get; set; }
		public int DamageMax { get; set; }
		public int RangeMax { get; set; }
		public int Unknown6 { get; set; }

		public WeaponTypes(BinaryReader inStream)
		{
			ItemKey = inStream.ReadInt64();
			Critical = inStream.ReadInt32();
			Speed = inStream.ReadInt32();
			DamageMin = inStream.ReadInt32();
			DamageMax = inStream.ReadInt32();
			RangeMax = inStream.ReadInt32();
			Unknown6 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(ItemKey);
			outStream.Write(Critical);
			outStream.Write(Speed);
			outStream.Write(DamageMin);
			outStream.Write(DamageMax);
			outStream.Write(RangeMax);
			outStream.Write(Unknown6);
		}

		public override int GetSize()
		{
			return 0x20;
		}
	}
}