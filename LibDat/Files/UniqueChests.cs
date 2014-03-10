using System.IO;

namespace LibDat.Files
{
	public class UniqueChests : BaseDat
	{
		[StringIndex]
		public int Index0 { get; set; }
		public int Unknown0 { get; set; }
		public int Unknown1 { get; set; }
		public int Unknown2 { get; set; }
		public int Unknown3 { get; set; }
		public int Unknown4 { get; set; }
		public int Unknown5 { get; set; }
		public int Unknown6 { get; set; }
		public int Data0Length { get; set; }
		[UInt64Index]
		public int Data0 { get; set; }
		public int Unknown9 { get; set; }
		public int Data1Length { get; set; }
		public int Data1 { get; set; }
		[StringIndex]
		public int Unknown12 { get; set; }

		public UniqueChests(BinaryReader inStream)
		{
			Index0 = inStream.ReadInt32();
			Unknown0 = inStream.ReadInt32();
			Unknown1 = inStream.ReadInt32();
			Unknown2 = inStream.ReadInt32();
			Unknown3 = inStream.ReadInt32();
			Unknown4 = inStream.ReadInt32();
			Unknown5 = inStream.ReadInt32();
			Unknown6 = inStream.ReadInt32();
			Data0Length = inStream.ReadInt32();
			Data0 = inStream.ReadInt32();
			Unknown9 = inStream.ReadInt32();
			Data0Length = inStream.ReadInt32();
			Data1 = inStream.ReadInt32();
			Unknown12 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Index0);
			outStream.Write(Unknown0);
			outStream.Write(Unknown1);
			outStream.Write(Unknown2);
			outStream.Write(Unknown3);
			outStream.Write(Unknown4);
			outStream.Write(Unknown5);
			outStream.Write(Unknown6);
			outStream.Write(Data0Length);
			outStream.Write(Data0);
			outStream.Write(Unknown9);
			outStream.Write(Data1Length);
			outStream.Write(Data1);
			outStream.Write(Unknown12);
		}

		public override int GetSize()
		{
			return 0x38;
		}
	}
}