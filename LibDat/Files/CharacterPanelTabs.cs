using System;
using System.IO;

namespace LibDat.Files
{
	public class CharacterPanelTabs : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
        public int Unknown2 { get; set; }
		[UserStringIndex]
		public int Text { get; set; }

        public CharacterPanelTabs(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
            Unknown2 = inStream.ReadInt32();
			Text = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
            outStream.Write(Unknown2);
			outStream.Write(Text);
		}

		public override int GetSize()
		{
			return 0x0C;
		}
	}
}