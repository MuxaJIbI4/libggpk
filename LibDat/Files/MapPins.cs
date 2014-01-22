using System;
using System.IO;

namespace LibDat.Files
{
		public class MapPins : BaseDat
		{
				[StringIndex]
				public int Id { get; set; }
				public int Unknown0 { get; set; }
				public int Unknown1 { get; set; }
				public Int64 Unknown2 { get; set; }
				public int Data0Length { get; set; }
				[UInt64Index]
				public int Data0 { get; set; }
				[UserStringIndex]
				public int Name { get; set; }
				[UserStringIndex]
				public int Notes { get; set; }
				public int Data1Length { get; set; }
				[UInt32Index]
				public int Data1 { get; set; }
				public int Unknown7 { get; set; }
				public int Act { get; set; }
				public int Data2Length { get; set; }
				[UInt64Index]
				public int Data2 { get; set; }
				public int Data3Length { get; set; }
				[UInt64Index]
				public int Data3 { get; set; }
				public Int64 Unknown13 { get; set; }
				public Int64 Unknown14 { get; set; }
				[StringIndex]
				public int Index3 { get; set; }

				public MapPins(BinaryReader inStream)
				{
						Id = inStream.ReadInt32();
						Unknown0 = inStream.ReadInt32();
						Unknown1 = inStream.ReadInt32();
						Unknown2 = inStream.ReadInt64();
						Data0Length = inStream.ReadInt32();
						Data0 = inStream.ReadInt32();
						Name = inStream.ReadInt32();
						Notes = inStream.ReadInt32();
						Data1Length = inStream.ReadInt32();
						Data1 = inStream.ReadInt32();
						Unknown7 = inStream.ReadInt32();
						Act = inStream.ReadInt32();
						Data2Length = inStream.ReadInt32();
						Data2 = inStream.ReadInt32();
						Data3Length = inStream.ReadInt32();
						Data3 = inStream.ReadInt32();
						Unknown13 = inStream.ReadInt64();
						Unknown14 = inStream.ReadInt64();
						Index3 = inStream.ReadInt32();
				}

				public override void Save(BinaryWriter outStream)
				{
						outStream.Write(Id);
						outStream.Write(Unknown0);
						outStream.Write(Unknown1);
						outStream.Write(Unknown2);
						outStream.Write(Data0Length);
						outStream.Write(Data0);
						outStream.Write(Name);
						outStream.Write(Notes);
						outStream.Write(Data1Length);
						outStream.Write(Data1);
						outStream.Write(Unknown7);
						outStream.Write(Act);
						outStream.Write(Data2Length);
						outStream.Write(Data2);
						outStream.Write(Data3Length);
						outStream.Write(Data3);
						outStream.Write(Unknown13);
						outStream.Write(Unknown14);
						outStream.Write(Index3);
				}

				public override int GetSize()
				{
						return 0x58;
				}
		}
}