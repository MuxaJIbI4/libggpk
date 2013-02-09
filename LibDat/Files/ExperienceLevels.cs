using System.IO;

namespace LibDat.Files
{
	public class ExperienceLevels : BaseDat
	{
		[StringIndex]
		public int Index0 { get; set; }
		public int Level { get; set; }
		public int Experience { get; set; }

		public ExperienceLevels(BinaryReader inStream)
		{
			Index0 = inStream.ReadInt32();
			Level = inStream.ReadInt32();
			Experience = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Index0);
			outStream.Write(Level);
			outStream.Write(Experience);
		}

		public override int GetSize()
		{
			return 0xC;
		}
	}
}