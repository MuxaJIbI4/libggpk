using System.IO;

namespace LibDat.Files
{
	public class Projectiles : BaseDat
	{
		[StringIndex]
		public int Index0 { get; set; }
		[StringIndex]
		public int Index1 { get; set; }
		[StringIndex]
		public int Index2 { get; set; }
		[StringIndex]
		public int Index3 { get; set; }
		public int Unknown4 { get; set; }
		[StringIndex]
		public int Index4 { get; set; }
		public bool Flag0 { get; set; }
		public int Unknown6 { get; set; }
		public bool Flag1 { get; set; }

		public Projectiles(BinaryReader inStream)
		{
			Index0 = inStream.ReadInt32();
			Index1 = inStream.ReadInt32();
			Index2 = inStream.ReadInt32();
			Index3 = inStream.ReadInt32();
			Unknown4 = inStream.ReadInt32();
			Index4 = inStream.ReadInt32();
			Flag0 = inStream.ReadBoolean();
			Unknown6 = inStream.ReadInt32();
			Flag1 = inStream.ReadBoolean();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Index0);
			outStream.Write(Index1);
			outStream.Write(Index2);
			outStream.Write(Index3);
			outStream.Write(Unknown4);
			outStream.Write(Index4);
			outStream.Write(Flag0);
			outStream.Write(Unknown6);
			outStream.Write(Flag1);
		}

		public override int GetSize()
		{
			return 0x1E;
		}
	}
}