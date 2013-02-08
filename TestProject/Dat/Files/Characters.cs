using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestProject.Dat.Files
{
	class Characters : BaseDat
	{
		[StringIndex]
		public int Index0;
		[StringIndex]
		public int Index1;
		[StringIndex]
		public int Index2;
		[StringIndex]
		public int Index3;
		public int Unknown0;
		public int Unknown1;
		public int Unknown2;
		public int Unknown3;
		public int Unknown4;
		public int Unknown5;
		[StringIndex]
		public int Index4;
		public int Unknown6;
		public int Unknown7;
		public int Unknown8;
		public int Unknown9;
		public int Unknown10;
		[DataIndex]
		public int Data0;
		[StringIndex]
		public int Index5;
		public int Unknown11;
		public int Unknown12;
		public int Unknown13;
		public int Unknown14;
		public int Unknown15;
		public int Unknown16;
		public int Unknown17;

		public Characters(BinaryReader inStream)
		{
			Index0 = inStream.ReadInt32();
			Index1 = inStream.ReadInt32();
			Index2 = inStream.ReadInt32();
			Index3 = inStream.ReadInt32();
			Unknown0 = inStream.ReadInt32();
			Unknown1 = inStream.ReadInt32();
			Unknown2 = inStream.ReadInt32();
			Unknown3 = inStream.ReadInt32();
			Unknown4 = inStream.ReadInt32();
			Unknown5 = inStream.ReadInt32();
			Index4 = inStream.ReadInt32();
			Unknown6 = inStream.ReadInt32();
			Unknown7 = inStream.ReadInt32();
			Unknown8 = inStream.ReadInt32();
			Unknown9 = inStream.ReadInt32();
			Unknown10 = inStream.ReadInt32();
			Data0 = inStream.ReadInt32();
			Index5 = inStream.ReadInt32();
			Unknown11 = inStream.ReadInt32();
			Unknown12 = inStream.ReadInt32();
			Unknown13 = inStream.ReadInt32();
			Unknown14 = inStream.ReadInt32();
			Unknown15 = inStream.ReadInt32();
			Unknown16 = inStream.ReadInt32();
			Unknown17 = inStream.ReadInt32();
		}

		public override void Save(System.IO.BinaryWriter outStream)
		{
			outStream.Write(Index0);
			outStream.Write(Index1);
			outStream.Write(Index2);
			outStream.Write(Index3);
			outStream.Write(Unknown0);
			outStream.Write(Unknown1);
			outStream.Write(Unknown2);
			outStream.Write(Unknown3);
			outStream.Write(Unknown4);
			outStream.Write(Unknown5);
			outStream.Write(Index4);
			outStream.Write(Unknown6);
			outStream.Write(Unknown7);
			outStream.Write(Unknown8);
			outStream.Write(Unknown9);
			outStream.Write(Unknown10);
			outStream.Write(Data0);
			outStream.Write(Index5);
			outStream.Write(Unknown11);
			outStream.Write(Unknown12);
			outStream.Write(Unknown13);
			outStream.Write(Unknown14);
			outStream.Write(Unknown15);
			outStream.Write(Unknown16);
			outStream.Write(Unknown17);

		}

		public override int GetSize()
		{
			throw new NotImplementedException();
		}
	}
}
