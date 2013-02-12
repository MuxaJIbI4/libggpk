using System.IO;

namespace LibDat.Files
{
	public class SoundEffects : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		[StringIndex]
		public int Sound { get; set; }

		public SoundEffects(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Sound = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Sound);
		}

		public override int GetSize()
		{
			return 0x8;
		}
	}
}