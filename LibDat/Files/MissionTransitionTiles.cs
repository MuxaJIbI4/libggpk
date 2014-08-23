using System.IO;

namespace LibDat.Files
{
	public class MissionTransitionTiles : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		[StringIndex]
		public int Metadata { get; set; }
		public int Unknown2 { get; set; }
		public int Unknown3 { get; set; }
		public int Unknown4 { get; set; }

		public MissionTransitionTiles(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Metadata = inStream.ReadInt32();
			Unknown2 = inStream.ReadInt32();
			Unknown3 = inStream.ReadInt32();
			Unknown4 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Metadata);
			outStream.Write(Unknown2);
			outStream.Write(Unknown3);
			outStream.Write(Unknown4);
		}

		public override int GetSize()
		{
			return 0x14;
		}
	}
}