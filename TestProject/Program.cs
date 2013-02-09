using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibGGPK;
using System.IO;
using System.Linq.Expressions;
using LibDat;

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
				@"ActiveSkills.dat", // done (Needs translation)
				@"ArmourTypes.dat", // done
				@"BackendErrors.dat", // done (Needs translation)
				@"BaseItemTypes.dat", // done
				@"BloodTypes.dat", // done
				@"BuffDefinitions.dat", // done (Needs translation)
				@"CharacterAudioEvents.dat", // done
				@"Characters.dat", // done (can skip translation)
				@"ChestClusters.dat", // done
				@"Chests.dat",
				@"ComponentArmour.dat",
				@"ComponentAttributeRequirements.dat",
				@"ComponentCharges.dat",
				@"CurrencyItems.dat", // done (Needs translation?)
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
				@"Mods.dat", // done
				@"ModSellPrices.dat",
				@"MonsterPackEntries.dat",
				@"MonsterPacks.dat",
				@"MonsterTypes.dat",
				@"MonsterVarieties.dat", // done
				@"Music.dat",
				@"NPCs.dat",
				@"NPCTalk.dat",
				@"NPCTextAudio.dat", // done (Need translation)
				@"PassiveSkills.dat",
				@"Pet.dat",
				@"Projectiles.dat",
				@"Quest.dat",
				@"QuestRewards.dat",
				@"QuestStates.dat", // done (Needs translation)
				@"QuestStaticRewards.dat",
				@"Realms.dat",
				@"ShieldTypes.dat",
				@"ShopItem.dat",
				@"ShopPaymentPackage.dat",
				@"Shrines.dat", // done
				@"SkillGems.dat",
				@"SoundEffects.dat",
				@"Stats.dat",
				@"Tags.dat",
				@"Topologies.dat",
				@"VoteState.dat", // done (Need translation)
				@"VoteType.dat", // done (Need translation)
				@"WeaponTypes.dat",
				@"Words.dat",
				@"WorldAreas.dat",
			};


			for (int i = 0; i < datFiles.Length; i++)
			{
				try
				{
					var container = new DatContainer(datFiles[i]);
					string dump = DumpContainer(container, '\t');
					//File.WriteAllText(datFiles[i] + ".csv", dump);
					//Console.WriteLine(dump);
				}
				catch (Exception ex)
				{
					Console.WriteLine("Failed: {0}", ex.Message);
				}
			}
		}


		private static string DumpContainer(DatContainer container, char seperator)
		{
			StringBuilder sb = new StringBuilder();

			bool displayedHeader = false;
			foreach (var item in container.Entries)
			{
				var fields = item.GetType().GetFields();

				if (!displayedHeader)
				{
					foreach (var fieldInfo in fields)
					{
						sb.AppendFormat("{0}{1}", fieldInfo.Name, seperator);
					}
					sb.Remove(sb.Length - 1, 1);
					sb.AppendLine();
					displayedHeader = true;
				}
				{
					foreach (var fieldInfo in fields)
					{
						object fieldValue = fieldInfo.GetValue(item);

						if (fieldInfo.GetCustomAttributes(false).Length > 0)
							sb.AppendFormat("{0}{1}", container.DataEntries[(int)fieldValue].ToString().Replace("\r\n", " ").Replace('\t', ' '), seperator);
						else
							sb.AppendFormat("{0}{1}", fieldValue, seperator);
					}
					sb.Remove(sb.Length - 1, 1);
					sb.AppendLine();
				}
			}

			return sb.ToString();
		}
	}
}