using System;
using System.IO;

namespace LibDat.Files
{
	public class MonsterVarieties : BaseDat
	{
		[StringIndex]
		public int MonsterTypeIndex { get; set; }
		public Int64 Unknown1 { get; set; }
		public int Unknown3 { get; set; }
		public int ObjectSize { get; set; } // ?
		public int MinimumAttackDistance { get; set; }
		public int MaximumAttackDistance { get; set; }
		[StringIndex]
		public int ActorIndex { get; set; }
		[StringIndex]
		public int AnimatedObjectIndex { get; set; }
		[StringIndex]
		public int BaseMonsterTypeIndex { get; set; }
		public int Data0Length { get; set; }
		[UInt64Index]
		public int Data0 { get; set; }
		public int Unknown12 { get; set; } // rarity_bias ?
		[StringIndex]
		public int Unknown13 { get; set; }
		[StringIndex]
		public int Unknown14 { get; set; }
		[StringIndex]
		public int SegmentsIndex { get; set; }
		public int Unknown16 { get; set; }
		public int Unknown17 { get; set; }
		public int Unknown18 { get; set; }
		public int Unknown19 { get; set; }
		public int Unknown20 { get; set; }
		[StringIndex]
		public int ArtModel { get; set; }
		public int Unknown22 { get; set; }
		public int Data1Length { get; set; }
		[UInt64Index]
		public int Data1 { get; set; }
		public int Unknown25 { get; set; }
		public int Data2Length { get; set; }
		[UInt32Index]
		public int Data2 { get; set; }
		public int Unknown28 { get; set; }
		public int Unknown29 { get; set; } // scale ?
		public int Unknown30 { get; set; }
		public int Unknown31 { get; set; }
		public int Unknown32 { get; set; }
		public int Data3Length { get; set; }
		[UInt64Index]
		public int Data3 { get; set; }
		[StringIndex]
		public int Unknown35 { get; set; }
		public int Data4Length { get; set; }
		[UInt64Index]
		public int Data4 { get; set; }
		[StringIndex]
		public int Unknown38 { get; set; }
		public int Unknown39 { get; set; }
		public int Unknown40 { get; set; }
		[UserStringIndex]
		public int Unknown41 { get; set; }
		public int Unknown42 { get; set; }
		public int Unknown43 { get; set; }
		public int Unknown44 { get; set; }
		public int Data5Length { get; set; }
		[UInt64Index]
		public int Data5 { get; set; }
		public int Data6Length { get; set; }
		[UInt64Index]
		public int Data6 { get; set; }
		public Int64 Unknown49 { get; set; }
		public int Unknown51 { get; set; }
		public int Unknown52 { get; set; }
		public Int64 Unknown53 { get; set; }
		public Int64 Unknown55 { get; set; }
		public int Unknown57 { get; set; }
		public Int64 Unknown58 { get; set; }
		public int Data7Length { get; set; }
		[UInt64Index]
		public int Data7 { get; set; }
		public Int64 Unknown62 { get; set; }
		public bool Unknown63 { get; set; }

		public MonsterVarieties(BinaryReader inStream)
		{
			MonsterTypeIndex = inStream.ReadInt32();
			Unknown1 = inStream.ReadInt64();
			Unknown3 = inStream.ReadInt32();
			ObjectSize = inStream.ReadInt32();
			MinimumAttackDistance = inStream.ReadInt32();
			MaximumAttackDistance = inStream.ReadInt32();
			ActorIndex = inStream.ReadInt32();
			AnimatedObjectIndex = inStream.ReadInt32();
			BaseMonsterTypeIndex = inStream.ReadInt32();
			Data0Length = inStream.ReadInt32();
			Data0 = inStream.ReadInt32();
			Unknown12 = inStream.ReadInt32();
			Unknown13 = inStream.ReadInt32();
			Unknown14 = inStream.ReadInt32();
			SegmentsIndex = inStream.ReadInt32();
			Unknown16 = inStream.ReadInt32();
			Unknown17 = inStream.ReadInt32();
			Unknown18 = inStream.ReadInt32();
			Unknown19 = inStream.ReadInt32();
			Unknown20 = inStream.ReadInt32();
			ArtModel = inStream.ReadInt32();
			Unknown22 = inStream.ReadInt32();
			Data1Length = inStream.ReadInt32();
			Data1 = inStream.ReadInt32();
			Unknown25 = inStream.ReadInt32();
			Data2Length = inStream.ReadInt32();
			Data2 = inStream.ReadInt32();
			Unknown28 = inStream.ReadInt32();
			Unknown29 = inStream.ReadInt32();
			Unknown30 = inStream.ReadInt32();
			Unknown31 = inStream.ReadInt32();
			Unknown32 = inStream.ReadInt32();
			Data3Length = inStream.ReadInt32();
			Data3 = inStream.ReadInt32();
			Unknown35 = inStream.ReadInt32();
			Data4Length = inStream.ReadInt32();
			Data4 = inStream.ReadInt32();
			Unknown38 = inStream.ReadInt32();
			Unknown39 = inStream.ReadInt32();
			Unknown40 = inStream.ReadInt32();
			Unknown41 = inStream.ReadInt32();
			Unknown42 = inStream.ReadInt32();
			Unknown43 = inStream.ReadInt32();
			Unknown44 = inStream.ReadInt32();
			Data5Length = inStream.ReadInt32();
			Data5 = inStream.ReadInt32();
			Data6Length = inStream.ReadInt32();
			Data6 = inStream.ReadInt32();
			Unknown49 = inStream.ReadInt64();
			Unknown51 = inStream.ReadInt32();
			Unknown52 = inStream.ReadInt32();
			Unknown53 = inStream.ReadInt64();
			Unknown55 = inStream.ReadInt64();
			Unknown57 = inStream.ReadInt32();
			Unknown58 = inStream.ReadInt64();
			Data7Length = inStream.ReadInt32();
			Data7 = inStream.ReadInt32();
			Unknown62 = inStream.ReadInt64();
			Unknown63 = inStream.ReadBoolean();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(MonsterTypeIndex);
			outStream.Write(Unknown1);
			outStream.Write(Unknown3);
			outStream.Write(ObjectSize);
			outStream.Write(MinimumAttackDistance);
			outStream.Write(MaximumAttackDistance);
			outStream.Write(ActorIndex);
			outStream.Write(AnimatedObjectIndex);
			outStream.Write(BaseMonsterTypeIndex);
			outStream.Write(Data0Length);
			outStream.Write(Data0);
			outStream.Write(Unknown12);
			outStream.Write(Unknown13);
			outStream.Write(Unknown14);
			outStream.Write(SegmentsIndex);
			outStream.Write(Unknown16);
			outStream.Write(Unknown17);
			outStream.Write(Unknown18);
			outStream.Write(Unknown19);
			outStream.Write(Unknown20);
			outStream.Write(ArtModel);
			outStream.Write(Unknown22);
			outStream.Write(Data1Length);
			outStream.Write(Data1);
			outStream.Write(Unknown25);
			outStream.Write(Data2Length);
			outStream.Write(Data2);
			outStream.Write(Unknown28);
			outStream.Write(Unknown29);
			outStream.Write(Unknown30);
			outStream.Write(Unknown31);
			outStream.Write(Unknown32);
			outStream.Write(Data3Length);
			outStream.Write(Data3);
			outStream.Write(Unknown35);
			outStream.Write(Data4Length);
			outStream.Write(Data4);
			outStream.Write(Unknown38);
			outStream.Write(Unknown39);
			outStream.Write(Unknown40);
			outStream.Write(Unknown41);
			outStream.Write(Unknown42);
			outStream.Write(Unknown43);
			outStream.Write(Unknown44);
			outStream.Write(Data5Length);
			outStream.Write(Data5);
			outStream.Write(Data6Length);
			outStream.Write(Data6);
			outStream.Write(Unknown49);
			outStream.Write(Unknown51);
			outStream.Write(Unknown52);
			outStream.Write(Unknown53);
			outStream.Write(Unknown55);
			outStream.Write(Unknown57);
			outStream.Write(Unknown58);
			outStream.Write(Data7Length);
			outStream.Write(Data7);
			outStream.Write(Unknown62);
			outStream.Write(Unknown63);
		}

		public override int GetSize()
		{
			return 0x101;
		}
	}
}
