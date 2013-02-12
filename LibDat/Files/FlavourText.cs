using System.IO;

namespace LibDat.Files
{
	public class FlavourText : BaseDat
	{
		[StringIndex]
		public int Index0 { get; set; }
		public int Unknown0 { get; set; }
		[UserStringIndex]
		public int Index1 { get; set; }

		public FlavourText(BinaryReader inStream)
		{
			Index0 = inStream.ReadInt32();
			Unknown0 = inStream.ReadInt32();
			Index1 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Index0);
			outStream.Write(Unknown0);
			outStream.Write(Index1);
		}

		public override int GetSize()
		{
			return 0xC;
		}
	}
}