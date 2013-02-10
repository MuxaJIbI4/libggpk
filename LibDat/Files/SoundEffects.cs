using System.IO;

namespace LibDat.Files
{
	public class SoundEffects : BaseDat
	{
		[StringIndex]
		public int Index0 { get; set; }
		[StringIndex]
		public int Index1 { get; set; }

		public SoundEffects(BinaryReader inStream)
		{
			Index0 = inStream.ReadInt32();
			Index1 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Index0);
			outStream.Write(Index1);
		}

		public override int GetSize()
		{
			return 0x8;
		}
	}
}