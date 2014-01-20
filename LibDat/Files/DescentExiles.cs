using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibDat.Files
{
	public class DescentExiles : BaseDat
	{
		public int Id { get; set; }
		public Int64 AreaKey { get; set; }
		public Int64 Unknown3 { get; set; }
		public Int64 VarietyKey { get; set; }
		public int Unknown7 { get; set; }

		public DescentExiles(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			AreaKey = inStream.ReadInt64();
			Unknown3 = inStream.ReadInt64();
			VarietyKey = inStream.ReadInt64();
			Unknown7 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(AreaKey);
			outStream.Write(Unknown3);
			outStream.Write(VarietyKey);
			outStream.Write(Unknown7);
		}

		public override int GetSize()
		{
			return 0x20;
		}
	}
}
