using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestProject.Dat.Files
{
	class BuffDefinitions : BaseDat
	{
		[StringIndex]
		public int Id;
		[StringIndex]
		public int Description;
		public bool Invisible;
		public bool Removable;
		[StringIndex]
		public int Name;
		[StringIndex]
		public int Icon;
		public int Unknown0;
		[DataIndex]
		public int Data0;
		[StringIndex]
		public int Index4;
		public int Unknown4;
		public bool Flag2;
		public bool Flag3;
		public bool Flag4;
		public Int64 Unkown5;
		public Int64 Unkown6;
		public bool Flag5;
		public Int64 Unknown7;
		[StringIndex]
		public int Index5;
		public bool Flag6;
		public bool Flag7;
		public bool Flag8;
		public bool Flag9;
		public bool Flag10;
		public int Unknown8;

		public BuffDefinitions(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Description = inStream.ReadInt32();
			Invisible = inStream.ReadBoolean();
			Removable = inStream.ReadBoolean();
			Name = inStream.ReadInt32();
			Icon = inStream.ReadInt32();
			Unknown0 = inStream.ReadInt32();
			Data0 = inStream.ReadInt32();
			Index4 = inStream.ReadInt32();
			Unknown4 = inStream.ReadInt32();
			Flag2 = inStream.ReadBoolean();
			Flag3 = inStream.ReadBoolean();
			Flag4 = inStream.ReadBoolean();
			Unkown5 = inStream.ReadInt64();
			Unkown6 = inStream.ReadInt64();
			Flag5 = inStream.ReadBoolean();
			Unknown7 = inStream.ReadInt64();
			Index5 = inStream.ReadInt32();
			Flag6 = inStream.ReadBoolean();
			Flag7 = inStream.ReadBoolean();
			Flag8 = inStream.ReadBoolean();
			Flag9 = inStream.ReadBoolean();
			Flag10 = inStream.ReadBoolean();
			Unknown8 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Description);
			outStream.Write(Invisible);
			outStream.Write(Removable);
			outStream.Write(Name);
			outStream.Write(Icon);
			outStream.Write(Unknown0);
			outStream.Write(Data0);
			outStream.Write(Index4);
			outStream.Write(Unknown4);
			outStream.Write(Flag2);
			outStream.Write(Flag3);
			outStream.Write(Flag4);
			outStream.Write(Unkown5);
			outStream.Write(Unkown6);
			outStream.Write(Flag5);
			outStream.Write(Unknown7);
			outStream.Write(Index5);
			outStream.Write(Flag6);
			outStream.Write(Flag7);
			outStream.Write(Flag8);
			outStream.Write(Flag9);
			outStream.Write(Flag10);
			outStream.Write(Unknown8);
		}

		public override int GetSize()
		{
			return 0x4b;
		}
	}
}
