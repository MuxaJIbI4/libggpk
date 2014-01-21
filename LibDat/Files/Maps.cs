using System;
using System.IO;

namespace LibDat.Files
{
	public class Maps : BaseDat
	{
		public Int64 ItemKey { get; set; }
		public Int64 Unknown1 { get; set; }
		public Int64 Unknown2 { get; set; }
		public Int64 Unknown3 { get; set; }
		public int Data0Length { get; set; }
		public int Data0 { get; set; }
		public Int64 Unknown6 { get; set; }
		public int Data1Length { get; set; }
		public int Data1 { get; set; }
		public int Data2Length { get; set; }
		public int Data2 { get; set; }

		public Maps(BinaryReader inStream)
		{
			ItemKey = inStream.ReadInt64();
			Unknown1 = inStream.ReadInt64();
			Unknown2 = inStream.ReadInt64();
			Unknown3 = inStream.ReadInt64();
			Data0Length = inStream.ReadInt32();
			Data0 = inStream.ReadInt32();
			Unknown6 = inStream.ReadInt64();
			Data1Length = inStream.ReadInt32();
			Data1 = inStream.ReadInt32();
			Data2Length = inStream.ReadInt32();
			Data2 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(ItemKey);
			outStream.Write(Unknown1);
			outStream.Write(Unknown2);
			outStream.Write(Unknown3);
			outStream.Write(Data0Length);
			outStream.Write(Data0);
			outStream.Write(Unknown6);
			outStream.Write(Data1Length);
			outStream.Write(Data1);
			outStream.Write(Data2Length);
			outStream.Write(Data2);
		}

		public override int GetSize()
		{
			return 0x40;
		}
	}
}