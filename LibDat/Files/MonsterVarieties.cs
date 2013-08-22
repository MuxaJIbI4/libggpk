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
		public int Unknown10 { get; set; }
		[DataIndex]
		public int Unknown11 { get; set; }
		public int Unknown12 { get; set; } // rarity_bias ?
		[DataIndex]
		public int Unknown13 { get; set; }
		[DataIndex]
		public int Unknown14 { get; set; }
		[StringIndex]
		public int SegmentsIndex { get; set; }
		public int Unknown16 { get; set; }
		public int Unknown17 { get; set; }
		public int Unknown18 { get; set; }
		public int Unknown19 { get; set; }
		public int Unknown20 { get; set; }
		[StringIndex]
		public int Unknown21 { get; set; }
		public int Unknown22 { get; set; }
		public int Unknown23 { get; set; }
		public int Unknown24 { get; set; }
		public int Unknown25 { get; set; }
		public int Unknown26 { get; set; }
		public int Unknown27 { get; set; }
		public int Unknown28 { get; set; }
		public int Unknown29 { get; set; } // scale ?
		public int Unknown30 { get; set; }
		public int Unknown31 { get; set; }
		public int Unknown32 { get; set; }
		public int Unknown33 { get; set; }
		public int Unknown34 { get; set; }
		[StringIndex]
		public int Unknown35 { get; set; }
		public int Unknown36 { get; set; }
		public int Unknown37 { get; set; }
		public int Unknown38 { get; set; }
		public int Unknown39 { get; set; }
		public int Unknown40 { get; set; }
		[UserStringIndex]
		public int Unknown41 { get; set; }
		public int Unknown42 { get; set; }
		public int Unknown43 { get; set; }
		public int Unknown44 { get; set; }
		public int Unknown45 { get; set; }
		public int Unknown46 { get; set; }
		public int Unknown47 { get; set; }
		public int Unknown48 { get; set; }
		public Int64 Unknown49 { get; set; }
		public int Unknown51 { get; set; }
		public int Unknown52 { get; set; }
		public Int64 Unknown53 { get; set; }
		public Int64 Unknown55 { get; set; }
		public int Unknown57 { get; set; }
		public Int64 Unknown58 { get; set; }
		public int Unknown60 { get; set; }
		public int Unknown61 { get; set; }

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
			Unknown11 = inStream.ReadInt32();
			Unknown12 = inStream.ReadInt32();
			Unknown13 = inStream.ReadInt32();
			Unknown14 = inStream.ReadInt32();
			SegmentsIndex = inStream.ReadInt32();
			Unknown16 = inStream.ReadInt32();
			Unknown17 = inStream.ReadInt32();
			Unknown18 = inStream.ReadInt32();
			Unknown19 = inStream.ReadInt32();
			Unknown20 = inStream.ReadInt32();
			Unknown21 = inStream.ReadInt32();
			Unknown22 = inStream.ReadInt32();
			Unknown23 = inStream.ReadInt32();
			Unknown24 = inStream.ReadInt32();
			Unknown25 = inStream.ReadInt32();
			Unknown26 = inStream.ReadInt32();
			Unknown27 = inStream.ReadInt32();
			Unknown28 = inStream.ReadInt32();
			Unknown29 = inStream.ReadInt32();
			Unknown30 = inStream.ReadInt32();
			Unknown31 = inStream.ReadInt32();
			Unknown32 = inStream.ReadInt32();
			Unknown33 = inStream.ReadInt32();
			Unknown34 = inStream.ReadInt32();
			Unknown35 = inStream.ReadInt32();
			Unknown36 = inStream.ReadInt32();
			Unknown37 = inStream.ReadInt32();
			Unknown38 = inStream.ReadInt32();
			Unknown39 = inStream.ReadInt32();
			Unknown40 = inStream.ReadInt32();
			Unknown41 = inStream.ReadInt32();
			Unknown42 = inStream.ReadInt32();
			Unknown43 = inStream.ReadInt32();
			Unknown44 = inStream.ReadInt32();
			Unknown45 = inStream.ReadInt32();
			Unknown46 = inStream.ReadInt32();
			Unknown47 = inStream.ReadInt32();
			Unknown48 = inStream.ReadInt32();
			Unknown49 = inStream.ReadInt64();
			Unknown51 = inStream.ReadInt32();
			Unknown52 = inStream.ReadInt32();
			Unknown53 = inStream.ReadInt64();
			Unknown55 = inStream.ReadInt64();
			Unknown57 = inStream.ReadInt32();
			Unknown58 = inStream.ReadInt64();
			Unknown60 = inStream.ReadInt32();
			Unknown61 = inStream.ReadInt32();
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
			outStream.Write(Unknown11);
			outStream.Write(Unknown12);
			outStream.Write(Unknown13);
			outStream.Write(Unknown14);
			outStream.Write(SegmentsIndex);
			outStream.Write(Unknown16);
			outStream.Write(Unknown17);
			outStream.Write(Unknown18);
			outStream.Write(Unknown19);
			outStream.Write(Unknown20);
			outStream.Write(Unknown21);
			outStream.Write(Unknown22);
			outStream.Write(Unknown24);
			outStream.Write(Unknown25);
			outStream.Write(Unknown26);
			outStream.Write(Unknown27);
			outStream.Write(Unknown28);
			outStream.Write(Unknown29);
			outStream.Write(Unknown30);
			outStream.Write(Unknown31);
			outStream.Write(Unknown32);
			outStream.Write(Unknown33);
			outStream.Write(Unknown34);
			outStream.Write(Unknown35);
			outStream.Write(Unknown36);
			outStream.Write(Unknown37);
			outStream.Write(Unknown38);
			outStream.Write(Unknown39);
			outStream.Write(Unknown40);
			outStream.Write(Unknown41);
			outStream.Write(Unknown42);
			outStream.Write(Unknown43);
			outStream.Write(Unknown45);
			outStream.Write(Unknown46);
			outStream.Write(Unknown47);
			outStream.Write(Unknown48);
			outStream.Write(Unknown49);
			outStream.Write(Unknown51);
			outStream.Write(Unknown52);
			outStream.Write(Unknown53);
			outStream.Write(Unknown55);
			outStream.Write(Unknown57);
			outStream.Write(Unknown58);
			outStream.Write(Unknown60);
			outStream.Write(Unknown61);
		}

		public override int GetSize()
		{
			return 0xf8;
		}
	}
}
