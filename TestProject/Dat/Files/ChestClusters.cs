using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestProject.Dat.Files
{
	class ChestClusters : BaseDat
	{
		[StringIndex]
		public int Index0;
		public int Unknown0;
		[DataIndex]
		public int Data0;
		public int Unknown1;
		public int Unknown2;
		public int Unknown3;
		public int Unknown4;
		public int Unknown5;

		public ChestClusters(BinaryReader inStream)
		{
			Index0 = inStream.ReadInt32();
			Unknown0 = inStream.ReadInt32();
			Data0 = inStream.ReadInt32();
			Unknown1 = inStream.ReadInt32();
			Unknown2 = inStream.ReadInt32();
			Unknown3 = inStream.ReadInt32();
			Unknown4 = inStream.ReadInt32();
			Unknown5 = inStream.ReadInt32();
		}
		public override void Save(System.IO.BinaryWriter outStream)
		{
			outStream.Write(Index0);
			outStream.Write(Unknown0);
			outStream.Write(Data0);
			outStream.Write(Unknown1);
			outStream.Write(Unknown2);
			outStream.Write(Unknown3);
			outStream.Write(Unknown4);
			outStream.Write(Unknown5);
		}

		public override int GetSize()
		{
			return 0x20;
		}
	}
}
