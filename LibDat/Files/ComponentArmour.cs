using System.IO;

namespace LibDat.Files
{
	public class ComponentArmour : BaseDat
	{
		[StringIndex]
		public int Index0 { get; set; }
		public int Unknown0 { get; set; }
		public int Unkown2 { get; set; }
		public int Unknown3 { get; set; }
		public int Unknown4 { get; set; }

		public ComponentArmour(BinaryReader inStream)
		{
			Index0 = inStream.ReadInt32();
			Unknown0 = inStream.ReadInt32();
			Unkown2 = inStream.ReadInt32();
			Unknown3 = inStream.ReadInt32();
			Unknown4 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Index0);
			outStream.Write(Unknown0);
			outStream.Write(Unkown2);
			outStream.Write(Unknown3);
			outStream.Write(Unknown4);
		}

		public override int GetSize()
		{
			return 0x14;
		}
	}
}