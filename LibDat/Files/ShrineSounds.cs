using System.IO;

namespace LibDat.Files
{
	public class ShrineSounds : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		[StringIndex]
		public int SoundStereo { get; set; }
		[StringIndex]
		public int SoundMono { get; set; }

		public ShrineSounds()
		{
			
		}
		public ShrineSounds(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			SoundStereo = inStream.ReadInt32();
			SoundMono = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(SoundStereo);
			outStream.Write(SoundMono);
		}

		public override int GetSize()
		{
			return 0x0C;
		}
	}
}