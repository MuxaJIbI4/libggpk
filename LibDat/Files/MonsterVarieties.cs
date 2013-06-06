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
		public int AnimatedObjectIndex { get; set; }
		public int BaseMonsterTypeIndex { get; set; }
		public int Unknown10 { get; set; }
		[DataIndex]
		public int UnknownDataIndex0 { get; set; }
		public int Unkown12 { get; set; } // rarity_bias ?
		[DataIndex]
		public int UnknownDataIndex1 { get; set; }
		[DataIndex]
		public int UnknownDataIndex2 { get; set; }
		[StringIndex]
		public int SegmentsIndex { get; set; }
		public int Unknown16 { get; set; }
		public int Unknown17 { get; set; }
		public int Unknown18 { get; set; }
		public int Unknown19 { get; set; }
		public int Unknown20 { get; set; }
		[StringIndex]
		public int UnknownStringIndex0 { get; set; }
		public Int64 Unknown22 { get; set; }
		[DataIndex]
		public int UnknownDataIndex3 { get; set; }
		public int Unknown25 { get; set; }
		[DataIndex]
		public int UnknownDataIndex4 { get; set; }
		public int Unknown26 { get; set; }
		[DataIndex]
		public int UnknownDataIndex5 { get; set; }
		public int Unknown29 { get; set; } // scale ?
		public int Unknown30 { get; set; }
		[DataIndex]
		public int UnknownDataIndex6 { get; set; }
		public int Unknown32 { get; set; }
		public int Unknown33 { get; set; }
		public int Unknown34 { get; set; }
		public Int64 Unknown35 { get; set; }
		public int Unknown37 { get; set; }
		[DataIndex]
		public int UnknownDataIndex7 { get; set; }
		[StringIndex]
		public int AisIndex { get; set; }
		public int Unknown40 { get; set; }
		[DataIndex]
		public int UnknownDataIndex8 { get; set; }
		[DataIndex]
		public int UnknownDataIndex9 { get; set; }
		public Int64 Unknown43 { get; set; }
		[UserStringIndex]
		public int ActorNameIndex { get; set; }
		public int Unknown46 { get; set; }
		public int Unknown47 { get; set; }
		public int WeaponSpeed { get; set; }
		public Int64 Unknown49 { get; set; }
		public Int64 Unknown51 { get; set; }
		public Int64 Unknown53 { get; set; }
		public int Unknown55 { get; set; }
		public int Unknown56 { get; set; }
		public Int64 Unknown57 { get; set; }
		public Int64 Unknown59 { get; set; }
		public int Unknown61 { get; set; }
		public Int64 Unknown62 { get; set; }
		public int Unknown64 { get; set; }
		public int Unknown65 { get; set; }

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
			Unknown10 = inStream.ReadInt32();
			UnknownDataIndex0 = inStream.ReadInt32();
			Unkown12 = inStream.ReadInt32();
			UnknownDataIndex1 = inStream.ReadInt32();
			UnknownDataIndex2 = inStream.ReadInt32();
			SegmentsIndex = inStream.ReadInt32();
			Unknown16 = inStream.ReadInt32();
			Unknown17 = inStream.ReadInt32();
			Unknown18 = inStream.ReadInt32();
			Unknown19 = inStream.ReadInt32();
			Unknown20 = inStream.ReadInt32();
			UnknownStringIndex0 = inStream.ReadInt32();
			Unknown22 = inStream.ReadInt64();
			UnknownDataIndex3 = inStream.ReadInt32();
			Unknown25 = inStream.ReadInt32();
			UnknownDataIndex4 = inStream.ReadInt32();
			Unknown26 = inStream.ReadInt32();
			UnknownDataIndex5 = inStream.ReadInt32();
			Unknown29 = inStream.ReadInt32();
			Unknown30 = inStream.ReadInt32();
			UnknownDataIndex6 = inStream.ReadInt32();
			Unknown32 = inStream.ReadInt32();
			Unknown33 = inStream.ReadInt32();
			Unknown34 = inStream.ReadInt32();
			Unknown35 = inStream.ReadInt64();
			Unknown37 = inStream.ReadInt32();
			UnknownDataIndex7 = inStream.ReadInt32();
			AisIndex = inStream.ReadInt32();
			Unknown40 = inStream.ReadInt32();
			UnknownDataIndex8 = inStream.ReadInt32();
			UnknownDataIndex9 = inStream.ReadInt32();
			Unknown43 = inStream.ReadInt64();
			ActorNameIndex = inStream.ReadInt32();
			Unknown46 = inStream.ReadInt32();
			Unknown47 = inStream.ReadInt32();
			WeaponSpeed = inStream.ReadInt32();
			Unknown49 = inStream.ReadInt64();
			Unknown51 = inStream.ReadInt64();
			Unknown53 = inStream.ReadInt64();
			Unknown55 = inStream.ReadInt32();
			Unknown56 = inStream.ReadInt32();
			Unknown57 = inStream.ReadInt64();
			Unknown59 = inStream.ReadInt64();
			Unknown61 = inStream.ReadInt32();
			Unknown62 = inStream.ReadInt64();
			Unknown64 = inStream.ReadInt32();
			Unknown65 = inStream.ReadInt32();
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
			outStream.Write(Unknown10);
			outStream.Write(UnknownDataIndex0);
			outStream.Write(Unkown12);
			outStream.Write(UnknownDataIndex1);
			outStream.Write(UnknownDataIndex2);
			outStream.Write(SegmentsIndex);
			outStream.Write(Unknown16);
			outStream.Write(Unknown17);
			outStream.Write(Unknown18);
			outStream.Write(Unknown19);
			outStream.Write(Unknown20);
			outStream.Write(UnknownStringIndex0);
			outStream.Write(Unknown22);
			outStream.Write(UnknownDataIndex3);
			outStream.Write(Unknown25);
			outStream.Write(UnknownDataIndex4);
			outStream.Write(Unknown26);
			outStream.Write(UnknownDataIndex5);
			outStream.Write(Unknown29);
			outStream.Write(Unknown30);
			outStream.Write(UnknownDataIndex6);
			outStream.Write(Unknown32);
			outStream.Write(Unknown33);
			outStream.Write(Unknown34);
			outStream.Write(Unknown35);
			outStream.Write(Unknown37);
			outStream.Write(UnknownDataIndex7);
			outStream.Write(AisIndex);
			outStream.Write(Unknown40);
			outStream.Write(UnknownDataIndex8);
			outStream.Write(UnknownDataIndex9);
			outStream.Write(Unknown43);
			outStream.Write(ActorNameIndex);
			outStream.Write(Unknown46);
			outStream.Write(Unknown47);
			outStream.Write(WeaponSpeed);
			outStream.Write(Unknown49);
			outStream.Write(Unknown51);
			outStream.Write(Unknown53);
			outStream.Write(Unknown55);
			outStream.Write(Unknown56);
			outStream.Write(Unknown57);
			outStream.Write(Unknown59);
			outStream.Write(Unknown61);
			outStream.Write(Unknown62);
			outStream.Write(Unknown64);
			outStream.Write(Unknown65);
		}

		public override int GetSize()
		{
			return 0x108;
		}
	}
}
