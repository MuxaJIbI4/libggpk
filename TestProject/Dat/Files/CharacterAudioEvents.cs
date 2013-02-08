using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestProject.Dat.Files
{
	class CharacterAudioEvents : BaseDat
	{
		[StringIndex]
		public int Index0;
		[StringIndex]
		public int Index1;
		[StringIndex]
		public int Index2;
		[StringIndex]
		public int Index3;
		[StringIndex]
		public int Index4;
		[StringIndex]
		public int Index5;
		[StringIndex]
		public int Index6;
		[StringIndex]
		public int Index7;
		public int Unknown0;
		public int Unknown1;
		public int Unknown2;
		[StringIndex]
		public int Index8;


		public CharacterAudioEvents(BinaryReader inStream)
		{
			Index0 = inStream.ReadInt32();
			Index1 = inStream.ReadInt32();
			Index2 = inStream.ReadInt32();
			Index3 = inStream.ReadInt32();
			Index4 = inStream.ReadInt32();
			Index5 = inStream.ReadInt32();
			Index6 = inStream.ReadInt32();
			Index7 = inStream.ReadInt32();
			Unknown0 = inStream.ReadInt32();
			Unknown1 = inStream.ReadInt32();
			Unknown2 = inStream.ReadInt32();
			Index8 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Index0);
			outStream.Write(Index1);
			outStream.Write(Index2);
			outStream.Write(Index3);
			outStream.Write(Index4);
			outStream.Write(Index5);
			outStream.Write(Index6);
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
