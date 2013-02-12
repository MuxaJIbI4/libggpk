using System;
using System.IO;

namespace LibDat.Files
{
	public class NPCTextAudio : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		public Int64 Unknown0 { get; set; }
		[UserStringIndex]
		public int Text { get; set; }
		[StringIndex]
		public int AudioPath { get; set; }

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
			return 0x14;
		}
	}
}
