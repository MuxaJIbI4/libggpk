using System;
using System.IO;

namespace LibDat.Files
{
	public class InvasionMonstersPerArea : BaseDat
	{
		public Int64 Index0 { get; set; }
		public int Unknown0 { get; set; }
		public int Unknown1 { get; set; }
		public int Data0Length { get; set; }
		[Int32Index]
		public int Data0 { get; set; }
		public int Data1Length { get; set; }
		[UInt64Index]
		public int Data1 { get; set; }
		public int Data2Length { get; set; }
		[UInt64Index]
		public int Data2 { get; set; }
		public int Unknown2 { get; set; }
		public int Unknown3 { get; set; }

		public InvasionMonstersPerArea(BinaryReader inStream)
		{
			Index0 = inStream.ReadInt64();
			Unknown0 = inStream.ReadInt32();
			Unknown1 = inStream.ReadInt32();
			Data0Length = inStream.ReadInt32();
			Data0 = inStream.ReadInt32();
			Data1Length = inStream.ReadInt32();
			Data1 = inStream.ReadInt32();
			Data2Length = inStream.ReadInt32();
			Data2 = inStream.ReadInt32();
			Unknown2 = inStream.ReadInt32();
			Unknown3 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Index0);
			outStream.Write(Unknown0);
			outStream.Write(Unknown1);
			outStream.Write(Data0Length);
			outStream.Write(Data0);
			outStream.Write(Data1Length);
			outStream.Write(Data1);
			outStream.Write(Data2Length);
			outStream.Write(Data2);
			outStream.Write(Unknown2);
			outStream.Write(Unknown3);
		}

		public override int GetSize()
		{
			return 0x30;
		}
	}
}