using System;
using System.IO;

namespace LibDat.Files
{
	class NPCTextAudio : BaseDat
	{
		[StringIndex]
		public int Id;
		public Int64 Unknown0;
		[StringIndex]
		public int Text;
		[StringIndex]
		public int AudioPath;

		public NPCTextAudio(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Unknown0 = inStream.ReadInt64();
			Text = inStream.ReadInt32();
			AudioPath = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Unknown0);
			outStream.Write(Text);
			outStream.Write(AudioPath);
		}

		public override int GetSize()
		{
			return 0x12;
		}
	}
}
