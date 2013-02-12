using System.IO;

namespace LibDat.Files
{
	public class Words : BaseDat
	{
		public int Unknown0 { get; set; }
		[UserStringIndex]
		public int Text { get; set; }
		public int Unknown2 { get; set; }
		public int Unknown3 { get; set; }
		public int Unknown4 { get; set; }
		public int Unknown5 { get; set; }
		public int Unknown6 { get; set; }

		public Words(BinaryReader inStream)
		{
			Unknown0 = inStream.ReadInt32();
			Text = inStream.ReadInt32();
			Unknown2 = inStream.ReadInt32();
			Unknown3 = inStream.ReadInt32();
			Unknown4 = inStream.ReadInt32();
			Unknown5 = inStream.ReadInt32();
			Unknown6 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Unknown0);
			outStream.Write(Text);
			outStream.Write(Unknown2);
			outStream.Write(Unknown3);
			outStream.Write(Unknown4);
			outStream.Write(Unknown5);
			outStream.Write(Unknown6);
		}

		public override int GetSize()
		{
			return 0x1C;
		}
	}
}