using System.IO;

namespace LibDat.Files
{
	public class VoteType : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		[StringIndex]
		public int Text { get; set; }
		[StringIndex]
		public int AcceptText { get; set; }
		[StringIndex]
		public int RejectText { get; set; }
		public int Unknown0 { get; set; }

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
