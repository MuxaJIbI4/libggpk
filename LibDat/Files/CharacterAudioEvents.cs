using System.IO;

namespace LibDat.Files
{
	public class CharacterAudioEvents : BaseDat
	{
		// It appears that the sound path strings are now prefixed with one or more 4-byte integers
		//   if the corresponding 'PrefixCount' is not 0. This file doesn't contain any translatable
		//   strings so it doesn't really matter.

		[StringIndex]
		public int Id { get; set; }
		public int SoundMaruaderPrefixCount { get; set; }
		public int SoundMaruader { get; set; }
		public int SoundRangerPrefixCount { get; set; }
		public int SoundRanger { get; set; }
		public int SoundWitchPrefixCount { get; set; }
		public int SoundWitch { get; set; }
		public int SoundDualistPrefixCount { get; set; }
		public int SoundDualist { get; set; }
		public int SoundShadowPrefixCount { get; set; }
		public int SoundShadow { get; set; }
		public int SoundTemplarPrefixCount { get; set; }
		public int SoundTemplar { get; set; }
		public int Index7PrefixCount { get; set; }
		public int Index7 { get; set; } // Sound file for the next class?
		public int Unknown0 { get; set; }
		public int Unknown1 { get; set; }
		public int Unknown2 { get; set; }
		[StringIndex]
		public int Index8 { get; set; } // Sound for all classes?


		public CharacterAudioEvents(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			SoundMaruaderPrefixCount = inStream.ReadInt32();
			SoundMaruader = inStream.ReadInt32();
			SoundRangerPrefixCount = inStream.ReadInt32();
			SoundRanger = inStream.ReadInt32();
			SoundWitchPrefixCount = inStream.ReadInt32();
			SoundWitch = inStream.ReadInt32();
			SoundDualistPrefixCount = inStream.ReadInt32();
			SoundDualist = inStream.ReadInt32();
			SoundShadowPrefixCount = inStream.ReadInt32();
			SoundShadow = inStream.ReadInt32();
			SoundTemplarPrefixCount = inStream.ReadInt32();
			SoundTemplar = inStream.ReadInt32();
			Index7PrefixCount = inStream.ReadInt32();
			Index7 = inStream.ReadInt32();
			Unknown0 = inStream.ReadInt32();
			Unknown1 = inStream.ReadInt32();
			Unknown2 = inStream.ReadInt32();
			Index8 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(SoundMaruaderPrefixCount);
			outStream.Write(SoundMaruader);
			outStream.Write(SoundRangerPrefixCount);
			outStream.Write(SoundRanger);
			outStream.Write(SoundWitchPrefixCount);
			outStream.Write(SoundWitch);
			outStream.Write(SoundDualistPrefixCount);
			outStream.Write(SoundDualist);
			outStream.Write(SoundShadowPrefixCount);
			outStream.Write(SoundShadow);
			outStream.Write(SoundTemplarPrefixCount);
			outStream.Write(SoundTemplar);
			outStream.Write(Index7PrefixCount);
			outStream.Write(Index7);
			outStream.Write(Unknown0);
			outStream.Write(Unknown1);
			outStream.Write(Unknown2);
			outStream.Write(Index8);
		}

		public override int GetSize()
		{
			return 0x4c;
		}
	}
}
