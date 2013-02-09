using System;
using System.IO;

namespace LibDat.Files
{
	public class MonsterVarieties : BaseDat
	{
		[StringIndex]
		public int MonsterTypeIndex;
		public Int64 Unknown0;
		public int Unknown1;
		public int ObjectSize; // ?
		public int MinimumAttackDistance;
		public int MaximumAttackDistance;
		[StringIndex]
		public int ActorIndex;
		public int AnimatedObjectIndex;
		public int BaseMonsterTypeIndex;
		public int Unknown2;
		[DataIndex]
		public int UnknownDataIndex0;
		public int Unkown3; // rarity_bias ?
		[DataIndex]
		public int UnknownDataIndex1;
		[DataIndex]
		public int UnknownDataIndex2;
		[StringIndex]
		public int SegmentsIndex;
		public int Unknown4; // scale ?
		public int Unknown5;
		public int Unknown6;
		public int Unknown7;
		public int Unknown8;
		[StringIndex]
		public int UnknownStringIndex0;
		public Int64 Unknown9;
		[DataIndex]
		public int UnknownDataIndex3;
		public int Unknown10;
		[DataIndex]
		public int UnknownDataIndex4;
		public int Unknown11;
		[DataIndex]
		public int UnknownDataIndex5;
		public int Unknown12; // scale ?
		public int Unknown13;
		[DataIndex]
		public int UnknownDataIndex6;
		public int Unknown14;
		public int Unknown15;
		public int Unknown16;
		public Int64 Unknown17;
		public int Unknown18;
		[DataIndex]
		public int UnknownDataIndex7;
		[StringIndex]
		public int AisIndex;
		public int Unknown19;
		[DataIndex]
		public int UnknownDataIndex8;
		[DataIndex]
		public int UnknownDataIndex9;
		public Int64 Unknown20;
		[StringIndex]
		public int ActorNameIndex;
		public int Unknown21;
		public int Unknown22;
		public int WeaponSpeed;
		public Int64 Unknown23;
		public Int64 Unknown24;
		public Int64 Unknown25;
		public int Unknown26;
		public int Unknown27;

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
