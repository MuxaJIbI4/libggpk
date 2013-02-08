using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestProject.Dat.Files 
{
	class BaseItemTypes : BaseDat
	{
		[StringIndex]
		public int Index0;
		public int Unknown0;
		public int Unknown1;
		public int Unknown2;
		[StringIndex]
		public int Index1;
		[StringIndex]
		public int Index2;
		public int Unknown3;
		public Int64 Unknown4;
		public int Unknown5;
		[DataIndex]
		public int Data0;
		public int Unknown6;
		public int Unknown7;
		public int Unknown8;
		public Int64 Unknown9;
		public Int64 Unknown10;
		public int Unknown11;
		[DataIndex]
		public int Data1;
		public int Unknown12;
		[DataIndex]
		public int Data2;
		public int Unknown13;
		[DataIndex]
		public int Data3;
		public int Unknown14;
		[DataIndex]
		public int Data4;
		public int Unknown15;
		[DataIndex]
		public int Data5;
		public int Unknown16;
		public Int64 Unknown17;
		public int Unknown18;


		public BaseItemTypes(BinaryReader inStream)
		{
			Index0 = inStream.ReadInt32();
			Unknown0 = inStream.ReadInt32();
			Unknown1 = inStream.ReadInt32();
			Unknown2 = inStream.ReadInt32();
			Index1 = inStream.ReadInt32();
			Index2 = inStream.ReadInt32();
			Unknown3 = inStream.ReadInt32();
			Unknown4 = inStream.ReadInt64();
			Unknown5 = inStream.ReadInt32();
			Data0 = inStream.ReadInt32();
			Unknown6 = inStream.ReadInt32();
			Unknown7 = inStream.ReadInt32();
			Unknown8 = inStream.ReadInt32();
			Unknown9 = inStream.ReadInt64();
			Unknown10 = inStream.ReadInt64();
			Unknown11 = inStream.ReadInt32();
			Data1 = inStream.ReadInt32();
			Unknown12 = inStream.ReadInt32();
			Data2 = inStream.ReadInt32();
			Unknown13 = inStream.ReadInt32();
			Data3 = inStream.ReadInt32();
			Unknown14 = inStream.ReadInt32();
			Data4 = inStream.ReadInt32();
			Unknown15 = inStream.ReadInt32();
			Data5 = inStream.ReadInt32();
			Unknown16 = inStream.ReadInt32();
			Unknown17 = inStream.ReadInt64();
			Unknown18 = inStream.ReadInt32();

		}
		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Index0);
			outStream.Write(Unknown0);
			outStream.Write(Unknown1);
			outStream.Write(Unknown2);
			outStream.Write(Index1);
			outStream.Write(Index2);
			outStream.Write(Unknown3);
			outStream.Write(Unknown4);
			outStream.Write(Unknown5);
			outStream.Write(Data0);
			outStream.Write(Unknown6);
			outStream.Write(Unknown7);
			outStream.Write(Unknown8);
			outStream.Write(Unknown9);
			outStream.Write(Unknown10);
			outStream.Write(Unknown11);
			outStream.Write(Data1);
			outStream.Write(Unknown12);
			outStream.Write(Data2);
			outStream.Write(Unknown13);
			outStream.Write(Data3);
			outStream.Write(Unknown14);
			outStream.Write(Data4);
			outStream.Write(Unknown15);
			outStream.Write(Data5);
			outStream.Write(Unknown16);
			outStream.Write(Unknown17);
			outStream.Write(Unknown18);

		}

		public override int GetSize()
		{
			return 0x80;
		}
	}
}
