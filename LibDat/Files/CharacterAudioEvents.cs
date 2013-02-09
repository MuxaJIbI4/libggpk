using System.IO;

namespace LibDat.Files
{
	public class CharacterAudioEvents : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		[StringIndex]
		public int SoundMaruader { get; set; }
		[StringIndex]
		public int SoundRanger { get; set; }
		[StringIndex]
		public int SoundWitch { get; set; }
		[StringIndex]
		public int SoundDualist { get; set; }
		[StringIndex]
		public int SoundShadow { get; set; }
		[StringIndex]
		public int SoundTemplar { get; set; }
		[StringIndex]
		public int Index7 { get; set; } // Sound file for the next class?
		public int Unknown0 { get; set; }
		public int Unknown1 { get; set; }
		public int Unknown2 { get; set; }
		[StringIndex]
		public int Index8 { get; set; } // Sound for all classes?


		public CharacterAudioEvents(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			SoundMaruader = inStream.ReadInt32();
			SoundRanger = inStream.ReadInt32();
			SoundWitch = inStream.ReadInt32();
			SoundDualist = inStream.ReadInt32();
			SoundShadow = inStream.ReadInt32();
			SoundTemplar = inStream.ReadInt32();
			Index7 = inStream.ReadInt32();
			Unknown0 = inStream.ReadInt32();
			Unknown1 = inStream.ReadInt32();
			Unknown2 = inStream.ReadInt32();
			Index8 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(SoundMaruader);
			outStream.Write(SoundRanger);
			outStream.Write(SoundWitch);
			outStream.Write(SoundDualist);
			outStream.Write(SoundShadow);
			outStream.Write(SoundTemplar);
			outStream.Write(Index7);
			outStream.Write(Unknown0);
			outStream.Write(Unknown1);
			outStream.Write(Unknown2);
			outStream.Write(Index8);
		}

		public override int GetSize()
		{
			return 0x30;
		}
	}
}
