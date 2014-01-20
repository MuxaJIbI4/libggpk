using System;
using System.IO;

namespace LibDat.Files
{
	public class Topologies : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		[StringIndex]
		public int Graph { get; set; }
		public int Unknown2 { get; set; }
		public int Unknown3 { get; set; }
		public int Unknown4 { get; set; }
		public  Int64 EnvironmentKey { get; set; }

		public Topologies(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Graph = inStream.ReadInt32();
			Unknown2 = inStream.ReadInt32();
			Unknown3 = inStream.ReadInt32();
			Unknown4 = inStream.ReadInt32();
			EnvironmentKey = inStream.ReadInt64();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Graph);
			outStream.Write(Unknown2);
			outStream.Write(Unknown3);
			outStream.Write(Unknown4);
			outStream.Write(EnvironmentKey);
		}

		public override int GetSize()
		{
			return 0x1C;
		}
	}
}