using System;
using System.IO;

namespace LibDat.Files
{
	public class MonsterVarieties : BaseDat
	{
		[StringIndex]
		public int MonsterTypeIndex { get; set; }
		public Int64 Unknown0 { get; set; }
		public int Unknown1 { get; set; }
		public int ObjectSize { get; set; } // ?
		public int MinimumAttackDistance { get; set; }
		public int MaximumAttackDistance { get; set; }
		[StringIndex]
		public int ActorIndex { get; set; }
		public int AnimatedObjectIndex { get; set; }
		public int BaseMonsterTypeIndex { get; set; }
		public int Unknown2 { get; set; }
		[DataIndex]
		public int UnknownDataIndex0 { get; set; }
		public int Unkown3 { get; set; } // rarity_bias ?
		[DataIndex]
		public int UnknownDataIndex1 { get; set; }
		[DataIndex]
		public int UnknownDataIndex2 { get; set; }
		[StringIndex]
		public int SegmentsIndex { get; set; }
		public int Unknown4 { get; set; } // scale ?
		public int Unknown5 { get; set; }
		public int Unknown6 { get; set; }
		public int Unknown7 { get; set; }
		public int Unknown8 { get; set; }
		[StringIndex]
		public int UnknownStringIndex0 { get; set; }
		public Int64 Unknown9 { get; set; }
		[DataIndex]
		public int UnknownDataIndex3 { get; set; }
		public int Unknown10 { get; set; }
		[DataIndex]
		public int UnknownDataIndex4 { get; set; }
		public int Unknown11 { get; set; }
		[DataIndex]
		public int UnknownDataIndex5 { get; set; }
		public int Unknown12 { get; set; } // scale ?
		public int Unknown13 { get; set; }
		[DataIndex]
		public int UnknownDataIndex6 { get; set; }
		public int Unknown14 { get; set; }
		public int Unknown15 { get; set; }
		public int Unknown16 { get; set; }
		public Int64 Unknown17 { get; set; }
		public int Unknown18 { get; set; }
		[DataIndex]
		public int UnknownDataIndex7 { get; set; }
		[StringIndex]
		public int AisIndex { get; set; }
		public int Unknown19 { get; set; }
		[DataIndex]
		public int UnknownDataIndex8 { get; set; }
		[DataIndex]
		public int UnknownDataIndex9 { get; set; }
		public Int64 Unknown20 { get; set; }
		[UserStringIndex]
		public int ActorNameIndex { get; set; }
		public int Unknown21 { get; set; }
		public int Unknown22 { get; set; }
		public int WeaponSpeed { get; set; }
		public Int64 Unknown23 { get; set; }
		public Int64 Unknown24 { get; set; }
		public Int64 Unknown25 { get; set; }
		public int Unknown26 { get; set; }
		public int Unknown27 { get; set; }

		public MonsterVarieties(BinaryReader inStream)
		{
			MonsterTypeIndex = inStream.ReadInt32();
			Unknown0 = inStream.ReadInt64();
			Unknown1 = inStream.ReadInt32();
			ObjectSize = inStream.ReadInt32();
			MinimumAttackDistance = inStream.ReadInt32();
			MaximumAttackDistance = inStream.ReadInt32();
			ActorIndex = inStream.ReadInt32();
			AnimatedObjectIndex = inStream.ReadInt32();
			BaseMonsterTypeIndex = inStream.ReadInt32();
			Unknown2 = inStream.ReadInt32();
			UnknownDataIndex0 = inStream.ReadInt32();
			Unkown3 = inStream.ReadInt32();
			UnknownDataIndex1 = inStream.ReadInt32();
			UnknownDataIndex2 = inStream.ReadInt32();
			SegmentsIndex = inStream.ReadInt32();
			Unknown4 = inStream.ReadInt32();
			Unknown5 = inStream.ReadInt32();
			Unknown6 = inStream.ReadInt32();
			Unknown7 = inStream.ReadInt32();
			Unknown8 = inStream.ReadInt32();
			UnknownStringIndex0 = inStream.ReadInt32();
			Unknown9 = inStream.ReadInt64();
			UnknownDataIndex3 = inStream.ReadInt32();
			Unknown10 = inStream.ReadInt32();
			UnknownDataIndex4 = inStream.ReadInt32();
			Unknown11 = inStream.ReadInt32();
			UnknownDataIndex5 = inStream.ReadInt32();
			Unknown12 = inStream.ReadInt32();
			Unknown13 = inStream.ReadInt32();
			UnknownDataIndex6 = inStream.ReadInt32();
			Unknown14 = inStream.ReadInt32();
			Unknown15 = inStream.ReadInt32();
			Unknown16 = inStream.ReadInt32();
			Unknown17 = inStream.ReadInt64();
			Unknown18 = inStream.ReadInt32();
			UnknownDataIndex7 = inStream.ReadInt32();
			AisIndex = inStream.ReadInt32();
			Unknown19 = inStream.ReadInt32();
			UnknownDataIndex8 = inStream.ReadInt32();
			UnknownDataIndex9 = inStream.ReadInt32();
			Unknown20 = inStream.ReadInt64();
			ActorNameIndex = inStream.ReadInt32();
			Unknown21 = inStream.ReadInt32();
			Unknown22 = inStream.ReadInt32();
			WeaponSpeed = inStream.ReadInt32();
			Unknown23 = inStream.ReadInt64();
			Unknown24 = inStream.ReadInt64();
			Unknown25 = inStream.ReadInt64();
			Unknown26 = inStream.ReadInt32();
			Unknown27 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(MonsterTypeIndex);
			outStream.Write(Unknown0);
			outStream.Write(Unknown1);
			outStream.Write(ObjectSize);
			outStream.Write(MinimumAttackDistance);
			outStream.Write(MaximumAttackDistance);
			outStream.Write(ActorIndex);
			outStream.Write(AnimatedObjectIndex);
			outStream.Write(BaseMonsterTypeIndex);
			outStream.Write(Unknown2);
			outStream.Write(UnknownDataIndex0);
			outStream.Write(Unkown3);
			outStream.Write(UnknownDataIndex1);
			outStream.Write(UnknownDataIndex2);
			outStream.Write(SegmentsIndex);
			outStream.Write(Unknown4);
			outStream.Write(Unknown5);
			outStream.Write(Unknown6);
			outStream.Write(Unknown7);
			outStream.Write(Unknown8);
			outStream.Write(UnknownStringIndex0);
			outStream.Write(Unknown9);
			outStream.Write(UnknownDataIndex3);
			outStream.Write(Unknown10);
			outStream.Write(UnknownDataIndex4);
			outStream.Write(Unknown11);
			outStream.Write(UnknownDataIndex5);
			outStream.Write(Unknown12);
			outStream.Write(Unknown13);
			outStream.Write(UnknownDataIndex6);
			outStream.Write(Unknown14);
			outStream.Write(Unknown15);
			outStream.Write(Unknown16);
			outStream.Write(Unknown17);
			outStream.Write(Unknown18);
			outStream.Write(UnknownDataIndex7);
			outStream.Write(AisIndex);
			outStream.Write(Unknown19);
			outStream.Write(UnknownDataIndex8);
			outStream.Write(UnknownDataIndex9);
			outStream.Write(Unknown20);
			outStream.Write(ActorNameIndex);
			outStream.Write(Unknown21);
			outStream.Write(Unknown22);
			outStream.Write(WeaponSpeed);
			outStream.Write(Unknown23);
			outStream.Write(Unknown24);
			outStream.Write(Unknown25);
			outStream.Write(Unknown26);
			outStream.Write(Unknown27);
		}

		public override int GetSize()
		{
			return 0xE4;
		}
	}
}
