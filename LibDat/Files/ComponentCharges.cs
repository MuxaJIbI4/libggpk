using System.IO;

namespace LibDat.Files
{
	public class ComponentCharges : BaseDat
	{
		[StringIndex]
		public int Index0 { get; set; }
		public int MaxCharges { get; set; }
		public int PerCharge { get; set; }

		public ComponentCharges(BinaryReader inStream)
		{
			Index0 = inStream.ReadInt32();
			MaxCharges = inStream.ReadInt32();
			PerCharge = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Index0);
			outStream.Write(MaxCharges);
			outStream.Write(PerCharge);
		}

		public override int GetSize()
		{
			return 0xC;
		}
	}
}