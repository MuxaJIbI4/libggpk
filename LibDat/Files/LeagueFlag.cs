using System.IO;

namespace LibDat.Files
{
	public class LeagueFlag : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		[StringIndex]
		public int Image { get; set; }

		public LeagueFlag()
		{
			
		}
		public LeagueFlag(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Image = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Image);
		}

		public override int GetSize()
		{
			return 0x08;
		}
	}
}