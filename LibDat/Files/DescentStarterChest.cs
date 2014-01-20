using System;
using System.IO;

namespace LibDat.Files
{
	public class DescentStarterChest : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		public Int64 Unknown1 { get; set; }
		public Int64 Unknown2 { get; set; }
		[StringIndex]
		public int Unknown3 { get; set; }

		public Int64 Unknown4 { get; set; }


		public DescentStarterChest()
		{
			
		}
		public DescentStarterChest(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Unknown1 = inStream.ReadInt64();
			Unknown2 = inStream.ReadInt64();
			Unknown3 = inStream.ReadInt32();
			Unknown4 = inStream.ReadInt64();

		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Unknown1);
			outStream.Write(Unknown2);
			outStream.Write(Unknown3);
			outStream.Write(Unknown4);
		}

		public override int GetSize()
		{
			return 0x20;
		}
	}
}