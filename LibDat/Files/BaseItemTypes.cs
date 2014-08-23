using System;
using System.IO;

namespace LibDat.Files 
{
	public class BaseItemTypes : BaseDat
	{
		[StringIndex]
		public int Index0 { get; set; }
		public int ItemClass { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		[UserStringIndex]
		public int Name { get; set; }
		[StringIndex]
		public int InheritsFrom { get; set; }
		public int Unknown3 { get; set; }
		public Int64 Unknown4 { get; set; }
		public int Data0Length { get; set; }
		[UInt64Index]
		public int Data0 { get; set; }
		public int Unknown6 { get; set; }
		public int Unknown7 { get; set; }
		public int Unknown8 { get; set; }
		public Int64 Unknown9 { get; set; }
		public Int64 Unknown10 { get; set; }
		public int Data1Length { get; set; }
		[UInt32Index]
		public int Data1 { get; set; }
		public int Data2Length { get; set; }
		[UInt32Index]
		public int Data2 { get; set; }
		public int Data3Length { get; set; }
		[UInt32Index]
		public int Data3 { get; set; }
		public int Data4Length { get; set; }
		[UInt32Index]
		public int Data4 { get; set; }
		public int Data5Length { get; set; }
		[UInt64Index]
		public int Data5 { get; set; }
		public int Unknown16 { get; set; }
		public Int64 Unknown17 { get; set; }
		public int Unknown18 { get; set; }

		public int Data6Length { get; set; }
		[UInt32Index]
		public int Data6 { get; set; }
		public Int64 Unknown34 { get; set; } // unknown 34
		public int Unknown36 { get; set; }
		public int Unknown37 { get; set; }
		public int Unknown38 { get; set; }
		public int Unknown39 { get; set; }
		public int Unknown40 { get; set; }
		public int Unknown41 { get; set; }
		public int Unknown42 { get; set; }
		public int Unknown43 { get; set; }	

		public BaseItemTypes(BinaryReader inStream)
		{
			Index0 = inStream.ReadInt32();
			ItemClass = inStream.ReadInt32();
			Width = inStream.ReadInt32();
			Height = inStream.ReadInt32();
			Name = inStream.ReadInt32();
			InheritsFrom = inStream.ReadInt32();
			Unknown3 = inStream.ReadInt32();
			Unknown4 = inStream.ReadInt64();
			Data0Length = inStream.ReadInt32();
			Data0 = inStream.ReadInt32();
			Unknown6 = inStream.ReadInt32();
			Unknown7 = inStream.ReadInt32();
			Unknown8 = inStream.ReadInt32();
			Unknown9 = inStream.ReadInt64();
			Unknown10 = inStream.ReadInt64();
			Data1Length = inStream.ReadInt32();
			Data1 = inStream.ReadInt32();
			Data2Length = inStream.ReadInt32();
			Data2 = inStream.ReadInt32();
			Data3Length = inStream.ReadInt32();
			Data3 = inStream.ReadInt32();
			Data4Length = inStream.ReadInt32();
			Data4 = inStream.ReadInt32();
			Data5Length = inStream.ReadInt32();
			Data5 = inStream.ReadInt32();
			Unknown16 = inStream.ReadInt32();
			Unknown17 = inStream.ReadInt64();
			Unknown18 = inStream.ReadInt32();
			Data6Length = inStream.ReadInt32();
			Data6 = inStream.ReadInt32();
			Unknown34 = inStream.ReadInt64();
			Unknown36 = inStream.ReadInt32();
			Unknown37 = inStream.ReadInt32();
			Unknown38 = inStream.ReadInt32();
			Unknown39 = inStream.ReadInt32();
			Unknown40 = inStream.ReadInt32();
			Unknown41 = inStream.ReadInt32();
			Unknown42 = inStream.ReadInt32();
			Unknown43 = inStream.ReadInt32();
		}
		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Index0);
			outStream.Write(ItemClass);
			outStream.Write(Width);
			outStream.Write(Height);
			outStream.Write(Name);
			outStream.Write(InheritsFrom);
			outStream.Write(Unknown3);
			outStream.Write(Unknown4);
			outStream.Write(Data0Length);
			outStream.Write(Data0);
			outStream.Write(Unknown6);
			outStream.Write(Unknown7);
			outStream.Write(Unknown8);
			outStream.Write(Unknown9);
			outStream.Write(Unknown10);
			outStream.Write(Data1Length);
			outStream.Write(Data1);
			outStream.Write(Data2Length);
			outStream.Write(Data2);
			outStream.Write(Data3Length);
			outStream.Write(Data3);
			outStream.Write(Data4Length);
			outStream.Write(Data4);
			outStream.Write(Data5Length);
			outStream.Write(Data5);
			outStream.Write(Unknown16);
			outStream.Write(Unknown17);
			outStream.Write(Unknown18);
			outStream.Write(Data6Length);
			outStream.Write(Data6);
			outStream.Write(Unknown34);
			outStream.Write(Unknown36);
			outStream.Write(Unknown37);
			outStream.Write(Unknown38);
			outStream.Write(Unknown39);
			outStream.Write(Unknown40);
			outStream.Write(Unknown41);
			outStream.Write(Unknown42);
			outStream.Write(Unknown43);

		}

		public override int GetSize()
		{
			return 0xB0;
		}
	}
}
