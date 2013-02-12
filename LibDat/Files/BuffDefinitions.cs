using System;
using System.IO;

namespace LibDat.Files
{
	public class BuffDefinitions : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		[UserStringIndex]
		public int Description { get; set; }
		public bool Invisible { get; set; }
		public bool Removable { get; set; }
		[StringIndex]
		public int Name { get; set; }
		[StringIndex]
		public int Icon { get; set; }
		public int Unknown0 { get; set; }
		[DataIndex]
		public int Data0 { get; set; }
		[StringIndex]
		public int Index4 { get; set; }
		public int Unknown4 { get; set; }
		public bool Flag2 { get; set; }
		public bool Flag3 { get; set; }
		public bool Flag4 { get; set; }
		public Int64 Unkown5 { get; set; }
		public Int64 Unkown6 { get; set; }
		public bool Flag5 { get; set; }
		public Int64 Unknown7 { get; set; }
		[StringIndex]
		public int Index5 { get; set; }
		public bool Flag6 { get; set; }
		public bool Flag7 { get; set; }
		public bool Flag8 { get; set; }
		public bool Flag9 { get; set; }
		public bool Flag10 { get; set; }
		public int Unknown8 { get; set; }

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
