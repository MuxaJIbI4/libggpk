using System.IO;

namespace LibDat.Files
{
	public class EventSeason : BaseDat
	{
		[StringIndex]
		public int Index0 { get; set; }
		public int Unknown1 { get; set; }
		[StringIndex]
		public int Unknown2 { get; set; }

		public EventSeason(BinaryReader inStream)
		{
			Index0 = inStream.ReadInt32();
			Unknown1 = inStream.ReadInt32();
			Unknown2 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Index0);
			outStream.Write(Unknown1);
			outStream.Write(Unknown2);
		}

		public override int GetSize()
		{
			return 0xC;
		}
	}
}