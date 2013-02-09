using System;
using System.IO;

namespace LibDat.Files
{
		public class MapPins : BaseDat
		{
				[StringIndex]
				public int Index0 { get; set; }
				public int Unknown0 { get; set; }
				public int Unknown1 { get; set; }
				public Int64 Unknown2 { get; set; }
				public int Unknown3 { get; set; }
				public int Unknown4 { get; set; }
				[StringIndex]
				public int Index1 { get; set; }
				[StringIndex]
				public int Index2 { get; set; }
				public int Unknown5 { get; set; }
				public int Unknown6 { get; set; }
				public int Unknown7 { get; set; }
				public int Unknown8 { get; set; }
				public int Unknown9 { get; set; }
				public int Unknown10 { get; set; }
				public int Unknown11 { get; set; }
				public int Unknown12 { get; set; }
				public Int64 Unknown13 { get; set; }
				public Int64 Unknown14 { get; set; }
				[StringIndex]
				public int Index3 { get; set; }

				public MapPins(BinaryReader inStream)
				{
						Index0 = inStream.ReadInt32();
						Unknown0 = inStream.ReadInt32();
						Unknown1 = inStream.ReadInt32();
						Unknown2 = inStream.ReadInt64();
						Unknown3 = inStream.ReadInt32();
						Unknown4 = inStream.ReadInt32();
						Index1 = inStream.ReadInt32();
						Index2 = inStream.ReadInt32();
						Unknown5 = inStream.ReadInt32();
						Unknown6 = inStream.ReadInt32();
						Unknown7 = inStream.ReadInt32();
						Unknown8 = inStream.ReadInt32();
						Unknown9 = inStream.ReadInt32();
						Unknown10 = inStream.ReadInt32();
						Unknown11 = inStream.ReadInt32();
						Unknown12 = inStream.ReadInt32();
						Unknown13 = inStream.ReadInt64();
						Unknown14 = inStream.ReadInt64();
						Index3 = inStream.ReadInt32();
				}

				public override void Save(BinaryWriter outStream)
				{
						outStream.Write(Index0);
						outStream.Write(Unknown0);
						outStream.Write(Unknown1);
						outStream.Write(Unknown2);
						outStream.Write(Unknown3);
						outStream.Write(Unknown4);
						outStream.Write(Index1);
						outStream.Write(Index2);
						outStream.Write(Unknown5);
						outStream.Write(Unknown6);
						outStream.Write(Unknown7);
						outStream.Write(Unknown8);
						outStream.Write(Unknown9);
						outStream.Write(Unknown10);
						outStream.Write(Unknown11);
						outStream.Write(Unknown12);
						outStream.Write(Unknown13);
						outStream.Write(Unknown14);
						outStream.Write(Index3);
				}

				public override int GetSize()
				{
						return 0x5C;
				}
		}
}