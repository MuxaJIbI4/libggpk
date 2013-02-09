using System.IO;

namespace LibDat.Files
{
	class CharacterAudioEvents : BaseDat
	{
		[StringIndex]
		public int Id;
		[StringIndex]
		public int SoundMaruader;
		[StringIndex]
		public int SoundRanger;
		[StringIndex]
		public int SoundWitch;
		[StringIndex]
		public int SoundDualist;
		[StringIndex]
		public int SoundShadow;
		[StringIndex]
		public int SoundTemplar;
		[StringIndex]
		public int Index7; // Sound file for the next class?
		public int Unknown0;
		public int Unknown1;
		public int Unknown2;
		[StringIndex]
		public int Index8; // Sound for all classes?


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
