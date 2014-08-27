using System;
using System.IO;

namespace LibDat.Files
{
	public class WorldAreas : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		[UserStringIndex]
		public int Name { get; set; }
		public int Act { get; set; }
		public bool IsTown { get; set; }
		public bool HasWaypoint { get; set; }
		public int ConnectionsLength { get; set; } // Number of area connections. (Only to "transition" objects, not other waypoints. Eg; Lioneye's Watch has 2)
		[UInt32Index]
		public int Connections { get; set; } // int* -> array of indexes in the WorldArea.dat rows for the connecting areas
		public Int64 MonsterLevel { get; set; }
		public bool Flag2 { get; set; }
		public int WorldAreaId { get; set; } // Used for waypoints and other UI-related things.
		public int Unknown7 { get; set; }
		public int Unknown8 { get; set; }
		[StringIndex]
		public int LoadingScreen { get; set; }
		public int Unknown10 { get; set; }
		public int Data1Length { get; set; }
		[UInt32Index]
		public int Data1 { get; set; }
		public int Unknown13 { get; set; }
		public int TopologiesLength { get; set; } // How many different types of tile sets there are for the area.
		[UInt64Index]
		public int Topologies { get; set; } // Points to an array of Topologies rows
		public int GoverningTown { get; set; } // Index to the "governing" town for this area. (Eg; Terraces' governing town is Lioneye's Watch)
		public int Difficulty { get; set; }
		public int Unknown18 { get; set; }
		public int Unknown19 { get; set; }
		public int Unknown20 { get; set; }
		public int Unknown21 { get; set; }
		public int Unknown22 { get; set; }
		public int BossVarietiesLength { get; set; } // // This, and the below, are just defining which types of monsters can spawn in an area.
		[UInt64Index]
		public int BossVarieties { get; set; }
		public int MonsterVarietiesLength { get; set; } // // I believe this is for rares only? Not confirmed
		[UInt64Index]
		public int MonsterVarieties { get; set; }
		public int Unknown27 { get; set; }
		public int DropTagsLength { get; set; }
		[UInt64Index]
		public int DropTags { get; set; }
		public int Data6Length { get; set; } // DropTagWeight?
		[UInt32Index]
		public int Data6 { get; set; }
		public bool IsMap { get; set; }
		public int Data7Length { get; set; }
		[UInt64Index]
		public int Data7 { get; set; }
		public int Unknown33 { get; set; }
		public int Unknown34 { get; set; }
		public Int64 AchievementItemsKey { get; set; } // Not a clue what this is for. It's only referenced by the AchievementItem manager. It's an array in the data section
		public int Data8Length { get; set; }
		[UInt64Index]
		public int Data8 { get; set; }
		[StringIndex]
		public int MapSound { get; set; }
		public int Unknown39 { get; set; }
		// TODO: Verify the following
		public int Unknown40 { get; set; }
		public int Unknown41 { get; set; }
		public int Unknown42 { get; set; }
		public int Unknown43 { get; set; }
		public int Unknown44 { get; set; }
		public int Unknown45 { get; set; }
		public int Unknown46 { get; set; }
		public int Unknown47 { get; set; }
		public bool Flag3 { get; set; }
		public int Unknown48 { get; set; }
		public int Unknown49 { get; set; }
		public int Unknown50 { get; set; }
		public int Unknown51 { get; set; }
		public int Unknown52 { get; set; }
		public byte Flag4 { get; set; }
		public int Unknown53 { get; set; }
		public byte Flag5 { get; set; }


		public WorldAreas(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Name = inStream.ReadInt32();
			Act = inStream.ReadInt32();
			IsTown = inStream.ReadBoolean();
			HasWaypoint = inStream.ReadBoolean();
			ConnectionsLength = inStream.ReadInt32();
			Connections = inStream.ReadInt32();
			MonsterLevel = inStream.ReadInt64();
			Flag2 = inStream.ReadBoolean();
			WorldAreaId = inStream.ReadInt32();
			Unknown7 = inStream.ReadInt32();
			Unknown8 = inStream.ReadInt32();
			LoadingScreen = inStream.ReadInt32();
			Unknown10 = inStream.ReadInt32();
			Data1Length = inStream.ReadInt32();
			Data1 = inStream.ReadInt32();
			Unknown13 = inStream.ReadInt32();
			TopologiesLength = inStream.ReadInt32();
			Topologies = inStream.ReadInt32();
			GoverningTown = inStream.ReadInt32();
			Difficulty = inStream.ReadInt32();
			Unknown18 = inStream.ReadInt32();
			Unknown19 = inStream.ReadInt32();
			Unknown20 = inStream.ReadInt32();
			Unknown21 = inStream.ReadInt32();
			Unknown22 = inStream.ReadInt32();
			BossVarietiesLength = inStream.ReadInt32();
			BossVarieties = inStream.ReadInt32();
			MonsterVarietiesLength = inStream.ReadInt32();
			MonsterVarieties = inStream.ReadInt32();
			Unknown27 = inStream.ReadInt32();
			DropTagsLength = inStream.ReadInt32();
			DropTags = inStream.ReadInt32();
			Data6Length = inStream.ReadInt32();
			Data6 = inStream.ReadInt32();
			IsMap = inStream.ReadBoolean();
			Data7Length = inStream.ReadInt32();
			Data7 = inStream.ReadInt32();
			Unknown33 = inStream.ReadInt32();
			Unknown34 = inStream.ReadInt32();
			AchievementItemsKey = inStream.ReadInt64();
			Data8Length = inStream.ReadInt32();
			Data8 = inStream.ReadInt32();
			MapSound = inStream.ReadInt32();
			Unknown39 = inStream.ReadInt32();
			Unknown40 = inStream.ReadInt32();
			Unknown41 = inStream.ReadInt32();
			Unknown42 = inStream.ReadInt32();
			Unknown43 = inStream.ReadInt32();
			Unknown44 = inStream.ReadInt32();
			Unknown45 = inStream.ReadInt32();
			Unknown46 = inStream.ReadInt32();
			Unknown47 = inStream.ReadInt32();
			Flag3 = inStream.ReadBoolean();
			Unknown48 = inStream.ReadInt32();
			Unknown49 = inStream.ReadInt32();
			Unknown50 = inStream.ReadInt32();
			Unknown51 = inStream.ReadInt32();
			Unknown52 = inStream.ReadInt32();
			Flag4 = inStream.ReadByte();
			Unknown53 = inStream.ReadInt32();
			Flag5 = inStream.ReadByte();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Name);
			outStream.Write(Act);
			outStream.Write(IsTown);
			outStream.Write(HasWaypoint);
			outStream.Write(ConnectionsLength);
			outStream.Write(Connections);
			outStream.Write(MonsterLevel);
			outStream.Write(Flag2);
			outStream.Write(WorldAreaId);
			outStream.Write(Unknown7);
			outStream.Write(Unknown8);
			outStream.Write(LoadingScreen);
			outStream.Write(Unknown10);
			outStream.Write(Data1Length);
			outStream.Write(Data1);
			outStream.Write(Unknown13);
			outStream.Write(TopologiesLength);
			outStream.Write(Topologies);
			outStream.Write(GoverningTown);
			outStream.Write(Difficulty);
			outStream.Write(Unknown18);
			outStream.Write(Unknown19);
			outStream.Write(Unknown20);
			outStream.Write(Unknown21);
			outStream.Write(Unknown22);
			outStream.Write(BossVarietiesLength);
			outStream.Write(BossVarieties);
			outStream.Write(MonsterVarietiesLength);
			outStream.Write(MonsterVarieties);
			outStream.Write(Unknown27);
			outStream.Write(DropTagsLength);
			outStream.Write(DropTags);
			outStream.Write(Data6Length);
			outStream.Write(Data6);
			outStream.Write(IsMap);
			outStream.Write(Data7Length);
			outStream.Write(Data7);
			outStream.Write(Unknown33);
			outStream.Write(Unknown34);
			outStream.Write(AchievementItemsKey);
			outStream.Write(Data8Length);
			outStream.Write(Data8);
			outStream.Write(MapSound);
			outStream.Write(Unknown39);
			outStream.Write(Unknown40); 
			outStream.Write(Unknown41); 
			outStream.Write(Unknown42); 
			outStream.Write(Unknown43); 
			outStream.Write(Unknown44); 
			outStream.Write(Unknown45); 
			outStream.Write(Unknown46); 
			outStream.Write(Unknown47); 
			outStream.Write(Flag3);	
			outStream.Write(Unknown48); 
			outStream.Write(Unknown49); 
			outStream.Write(Unknown50); 
			outStream.Write(Unknown51); 
			outStream.Write(Unknown52); 
			outStream.Write(Flag4);	
			outStream.Write(Unknown53); 
			outStream.Write(Flag5);	
		}

		public override int GetSize()
		{
			return 0xEB;
		}
	}
}