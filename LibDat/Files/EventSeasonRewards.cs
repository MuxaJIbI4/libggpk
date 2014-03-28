using System;
using System.IO;

namespace LibDat.Files
{
	public class EventSeasonRewards : BaseDat
	{
		public Int64 EventSeasonKey { get; set; }
		public int Point { get; set; }
		[StringIndex]
		public int Unknown3 { get; set; }

		public EventSeasonRewards(BinaryReader inStream)
		{
			EventSeasonKey = inStream.ReadInt64();
			Point = inStream.ReadInt32();
			Unknown3 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(EventSeasonKey);
			outStream.Write(Point);
			outStream.Write(Unknown3);
		}

		public override int GetSize()
		{
			return 0x10;
		}
	}
}