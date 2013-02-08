using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibGGPK;
using System.IO;
using System.Linq.Expressions;
using TestProject.Dat;
using TestProject.Dat.Files;
//4 bytes - Number of entries
//Entry 1
//Entry 2
//...
//Entry N
//8 bytes - BB BB BB BB BB BB BB BB
//String0 String1 String2 ...

namespace TestProject
{
	class Program
	{
		static void Output(string msg)
		{
			Console.Write(msg);
		}

		static void Main(string[] args)
		{
			// TESTING PURPOSES ONLY

			Environment.CurrentDirectory = @"C:\Program Files (x86)\Grinding Gear Games\Path of Exile\dataExtracted\Data";

			string[] datFiles = 
			{
				@"ActiveSkills.dat", // done
				@"ArmourTypes.dat", // done
				@"BackendErrors.dat", // done
				@"BaseItemTypes.dat", // done
				@"BloodTypes.dat", // done
				@"BuffDefinitions.dat",
				@"CharacterAudioEvents.dat",
				@"Characters.dat",
				@"ChestClusters.dat",
				@"Chests.dat",
				@"ComponentArmour.dat",
				@"ComponentAttributeRequirements.dat",
				@"ComponentCharges.dat",
				@"CurrencyItems.dat",
				@"Dances.dat",
				@"DefaultMonsterStats.dat",
				@"Difficulties.dat",
				@"DropPool.dat",
				@"Environments.dat",
				@"ExperienceLevels.dat",
				@"Flasks.dat",
				@"FlavourText.dat",
				@"GameConstants.dat",
				@"GemTags.dat",
				@"GrantedEffects.dat",
				@"GrantedEffectsPerLevel.dat",
				@"ItemExperiencePerLevel.dat",
				@"ItemisedVisualEffect.dat",
				@"ItemVisualEffect.dat",
				@"ItemVisualIdentity.dat",
				@"MapConnections.dat",
				@"MapPins.dat",
				@"Maps.dat",
				@"MiscAnimated.dat",
				@"MiscObjects.dat",
				@"Mods.dat",
				@"ModSellPrices.dat",
				@"MonsterPackEntries.dat",
				@"MonsterPacks.dat",
				@"MonsterTypes.dat",
				@"MonsterVarieties.dat", // done
				@"Music.dat",
				@"NPCs.dat",
				@"NPCTalk.dat",
				@"NPCTextAudio.dat",
				@"PassiveSkills.dat",
				@"Pet.dat",
				@"Projectiles.dat",
				@"Quest.dat",
				@"QuestRewards.dat",
				@"QuestStates.dat",
				@"QuestStaticRewards.dat",
				@"Realms.dat",
				@"ShieldTypes.dat",
				@"ShopItem.dat",
				@"ShopPaymentPackage.dat",
				@"Shrines.dat",
				@"SkillGems.dat",
				@"SoundEffects.dat",
				@"Stats.dat",
				@"Tags.dat",
				@"Topologies.dat",
				@"VoteState.dat",
				@"VoteType.dat",
				@"WeaponTypes.dat",
				@"Words.dat",
				@"WorldAreas.dat",
			};

			var container = new DatContainer<BloodTypes>(datFiles[4]);

			//foreach (var item in container.Entries)
			//{
			//	Console.WriteLine(item);
			//}
		}


	}
}