using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestProject.Dat.Files
{
	class ActiveSkills : BaseDat
	{
		[StringIndex]
		public int Index0;
		[StringIndex]
		public int Index1;
		[StringIndex]
		public int Index2;
		[StringIndex]
		public int Index3;
		[StringIndex]
		public int Index4;
		public int Unkown0;
		[DataIndex]
		public int Data0;
		public int Unknown1;
		public int Unknown2;
		[DataIndex]
		public int Data1;
		public int Unknown3;
		[DataIndex]
		public int Data2;
		[StringIndex]
		public int Index5;
		[StringIndex]
		public int Index6;
		public bool Flag0;
		public int Unknown4;
		public bool Flag1;
		public int Unknown5;
		[DataIndex] 
		public int Data3;

		public ActiveSkills(BinaryReader inStream)
		{
			Index0 = inStream.ReadInt32();
			Index1 = inStream.ReadInt32();
			Index2 = inStream.ReadInt32();
			Index3 = inStream.ReadInt32();
			Index4 = inStream.ReadInt32();
			Unkown0 = inStream.ReadInt32();
			Data0 = inStream.ReadInt32();
			Unknown1 = inStream.ReadInt32();
			Unknown2 = inStream.ReadInt32();
			Data1 = inStream.ReadInt32();
			Unknown3 = inStream.ReadInt32();
			Data2 = inStream.ReadInt32();
			Index5 = inStream.ReadInt32();
			Index6 = inStream.ReadInt32();
			Flag0 = inStream.ReadBoolean();
			Unknown4 = inStream.ReadInt32();
			Flag1 = inStream.ReadBoolean();
			Unknown5 = inStream.ReadInt32();
			Data3 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Index0);
			outStream.Write(Index1);
			outStream.Write(Index2);
			outStream.Write(Index3);
			outStream.Write(Index4);
			outStream.Write(Unkown0);
			outStream.Write(Data0);
			outStream.Write(Unknown1);
			outStream.Write(Unknown2);
			outStream.Write(Data1);
			outStream.Write(Unknown3);
			outStream.Write(Data2);
			outStream.Write(Index5);
			outStream.Write(Index6);
			outStream.Write(Flag0);
			outStream.Write(Unknown4);
			outStream.Write(Flag1);
			outStream.Write(Unknown5);
			outStream.Write(Data3);
		}

		public override int GetSize()
		{
			return 0x46;
		}
	}
}
