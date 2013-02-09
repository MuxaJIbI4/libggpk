using System.IO;

namespace LibDat.Files
{
	class VoteType : BaseDat
	{
		[StringIndex]
		public int Id;
		[StringIndex]
		public int Text;
		[StringIndex]
		public int AcceptText;
		[StringIndex]
		public int RejectText;
		public int Unknown0;

		public VoteType(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Text = inStream.ReadInt32();
			AcceptText = inStream.ReadInt32();
			RejectText = inStream.ReadInt32();
			Unknown0 = inStream.ReadInt32();
		}
		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Text);
			outStream.Write(AcceptText);
			outStream.Write(RejectText);
			outStream.Write(Unknown0);
		}

		public override int GetSize()
		{
			return 0x14;
		}
	}
}
