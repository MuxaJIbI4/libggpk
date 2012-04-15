using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LibGGPK
{
	public class GGPKRecord : BaseRecord
	{
		public long[] RecordOffsets;
		public const string Tag = "GGPK";


		public GGPKRecord(uint length, BinaryReader br)
		{
			RecordBegin = br.BaseStream.Position - 8;
			Length = length;
			Read(br);
		}

		public override void Read(BinaryReader br)
		{
			int totalRecordOffsets = br.ReadInt32();
			RecordOffsets = new long[totalRecordOffsets];

			for (int i = 0; i < totalRecordOffsets; i++)
			{
				RecordOffsets[i] = br.ReadInt64();
			}
		}

		public override string ToString()
		{
			return Tag;
		}
	}

}
