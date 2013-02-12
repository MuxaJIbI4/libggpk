using System.IO;

namespace LibDat.Files
{
	public class VoteState : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		[UserStringIndex]
		public int Text { get; set; }

		public VoteState(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Text = inStream.ReadInt32();
		}
		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Text);
		}

		public override int GetSize()
		{
			return 0x8;
		}
	}
}
