using System;
using System.IO;

namespace LibDat.Files
{
	public class BloodTypes : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		[StringIndex]
		public int Index1 { get; set; }
		[StringIndex]
		public int Index2 { get; set; }
		[StringIndex]
		public int Index3 { get; set; }
		public Int64 Unknown4 { get; set; }
		[StringIndex]
		public int Index4 { get; set; }
		[StringIndex]
		public int Index5 { get; set; }
		[StringIndex]
		public int Index6 { get; set; }
		public Int64 Unknown9 { get; set; }
		public int Data0Length { get; set; }
		[UInt64Index]
		public int Data0 { get; set; }
		public Int64 Unknown13 { get; set; }

		public BloodTypes(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Index1 = inStream.ReadInt32();
			Index2 = inStream.ReadInt32();
			Index3 = inStream.ReadInt32();
			Unknown4 = inStream.ReadInt64();
			Index4 = inStream.ReadInt32();
			Index5 = inStream.ReadInt32();
			Index6 = inStream.ReadInt32();
			Unknown9 = inStream.ReadInt64();
			Data0Length = inStream.ReadInt32();
			Data0 = inStream.ReadInt32();
			Unknown13 = inStream.ReadInt64();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Index1);
			outStream.Write(Index2);
			outStream.Write(Index3);
			outStream.Write(Unknown4);
			outStream.Write(Index4);
			outStream.Write(Index5);
			outStream.Write(Index6);
			outStream.Write(Unknown9);
			outStream.Write(Data0Length);
			outStream.Write(Data0);
			outStream.Write(Unknown13);
		}

		public override int GetSize()
		{
			return 0x3C;
		}
	}
}
