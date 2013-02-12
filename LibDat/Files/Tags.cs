using System.IO;

namespace LibDat.Files
{
	public class Tags : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		public int Unknown1 { get; set; }

		public Tags(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Unknown1 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Unknown1);
		}

		public override int GetSize()
		{
			return 0x8;
		}
	}
}