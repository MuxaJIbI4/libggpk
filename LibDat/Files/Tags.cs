using System.IO;

namespace LibDat.Files
{
	public class Tags : BaseDat
	{
		[StringIndex]
		public int Index0 { get; set; }
		public int Unknown1 { get; set; }

		public Tags(BinaryReader inStream)
		{
			Index0 = inStream.ReadInt32();
			Unknown1 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Index0);
			outStream.Write(Unknown1);
		}

		public override int GetSize()
		{
			return 0x8;
		}
	}
}