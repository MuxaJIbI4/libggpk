using System.IO;

namespace LibDat.Files
{
	public class ComponentArmour : BaseDat
	{
		[StringIndex]
		public int Index0 { get; set; }
		public int Armour { get; set; }
		public int Evasion { get; set; }
		public int EnergyShield { get; set; }

		public ComponentArmour(BinaryReader inStream)
		{
			Index0 = inStream.ReadInt32();
			Armour = inStream.ReadInt32();
			Evasion = inStream.ReadInt32();
			EnergyShield = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Index0);
			outStream.Write(Armour);
			outStream.Write(Evasion);
			outStream.Write(EnergyShield);
		}

		public override int GetSize()
		{
			return 0x10;
		}
	}
}