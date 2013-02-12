using System.IO;

namespace LibDat.Files
{
	public class GemTags : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		[UserStringIndex]
		public int Tag { get; set; }

		public GemTags(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Tag = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Tag);
		}

		public override int GetSize()
		{
			return 0x8;
		}
	}
}