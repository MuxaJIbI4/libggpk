using System.IO;

namespace LibDat.Files
{
	public class DropPool : BaseDat
	{
		[StringIndex]
		public int Index0 { get; set; }
		public int Unknown0 { get; set; }

		public DropPool(BinaryReader inStream)
		{
			Index0 = inStream.ReadInt32();
			Unknown0 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Index0);
			outStream.Write(Unknown0);
		}

		public override int GetSize()
		{
			return 0x8;
		}
	}
}