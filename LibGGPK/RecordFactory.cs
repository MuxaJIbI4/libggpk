using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LibGGPK
{
	internal static class RecordFactory
	{
		public static BaseRecord ReadRecord(BinaryReader br)
		{
			uint Length = br.ReadUInt32();
			string Tag = ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));

			switch (Tag)
			{
				case FileRecord.Tag:
					return new FileRecord(Length, br);
				case GGPKRecord.Tag:
					return new GGPKRecord(Length, br);
				case FreeRecord.Tag:
					return new FreeRecord(Length, br);
				case DirectoryRecord.Tag:
					return new DirectoryRecord(Length, br);
			}

			throw new Exception("Invalid tag");
		}
	}
}
