using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LibDat.Files;

namespace LibDat
{
	static class DatFactory
	{
		public static Type GetType(string fileName)
		{
			switch (fileName)
			{
				case "ActiveSkills":
					return typeof(ActiveSkills);
				case "ArmourTypes":
					return typeof(ArmourTypes);
				case "BackendErrors":
					return typeof(BackendErrors);
				case "BaseItemTypes":
					return typeof(BaseItemTypes);
				case "BloodTypes":
					return typeof(BloodTypes);
				case "BuffDefinitions":
					return typeof(BuffDefinitions);
				case "CharacterAudioEvents":
					return typeof(CharacterAudioEvents);
				case "Characters":
					return typeof(Characters);
				case "ChestClusters":
					return typeof(ChestClusters);
				case "Chests":
					return typeof(Chests);
				case "ComponentArmour":
					return typeof(ComponentArmour);
				case "ComponentAttributeRequirements":
					return typeof(ComponentAttributeRequirements);
				case "ComponentCharges":
					return typeof(ComponentCharges);
				case "CurrencyItems":
					return typeof(CurrencyItems);
				case "Dances":
					return typeof(Dances);
				case "DefaultMonsterStats":
					return typeof(DefaultMonsterStats);
				case "Difficulties":
					return typeof(Difficulties);
				case "DropPool":
					return typeof(DropPool);
				case "Environments":
					return typeof(Environments);
				case "ExperienceLevels":
					return typeof(ExperienceLevels);
				case "Flasks":
					return typeof(Flasks);
				case "FlavourText":
					return typeof(FlavourText);
				case "GameConstants":
					return typeof(GameConstants);
				case "GemTags":
					return typeof(GemTags);
				case "GrantedEffects":
					return typeof(GrantedEffects);
				case "GrantedEffectsPerLevel":
					return typeof(GrantedEffectsPerLevel);
				case "ItemExperiencePerLevel":
					return typeof(ItemExperiencePerLevel);
				case "ItemisedVisualEffect":
					return typeof(ItemisedVisualEffect);
				case "ItemVisualEffect":
					return typeof(ItemVisualEffect);
				case "ItemVisualIdentity":
					return typeof(ItemVisualIdentity);
				case "MapConnections":
					return typeof(MapConnections);
				case "MapPins":
					return typeof(MapPins);
				case "Maps":
					return typeof(Maps);
				case "MiscAnimated":
					return typeof(MiscAnimated);
				case "MiscObjects":
					return typeof(MiscObjects);
				case "Mods":
					return typeof(Mods);
				case "ModSellPrices":
					return typeof(ModSellPrices);
				case "MonsterPackEntries":
					return typeof(MonsterPackEntries);
				case "MonsterPacks":
					return typeof(MonsterPacks);
				case "MonsterTypes":
					return typeof(MonsterTypes);
				case "MonsterVarieties":
					return typeof(MonsterVarieties);
				case "Music":
					return typeof(Music);
				case "NPCs":
					return typeof(NPCs);
				case "NPCTalk":
					return typeof(NPCTalk);
				case "NPCTextAudio":
					return typeof(NPCTextAudio);
				case "PassiveSkills":
					return typeof(PassiveSkills);
				case "Pet":
					return typeof(Pet);
				case "Projectiles":
					return typeof(Projectiles);
				case "Quest":
					return typeof(Quest);
				case "QuestRewards":
					return typeof(QuestRewards);
				case "QuestStates":
					return typeof(QuestStates);
				case "QuestStaticRewards":
					return typeof(QuestStaticRewards);
				case "Realms":
					return typeof(Realms);
				case "ShieldTypes":
					return typeof(ShieldTypes);
				case "ShopItem":
					return typeof(ShopItem);
				case "ShopPaymentPackage":
					return typeof(ShopPaymentPackage);
				case "Shrines":
					return typeof(Shrines);
				case "SkillGems":
					return typeof(SkillGems);
				case "SoundEffects":
					return typeof(SoundEffects);
				case "Stats":
					return typeof(Stats);
				case "Tags":
					return typeof(Tags);
				case "Topologies":
					return typeof(Topologies);
				case "VoteState":
					return typeof(VoteState);
				case "VoteType":
					return typeof(VoteType);
				case "WeaponTypes":
					return typeof(WeaponTypes);
				case "Words":
					return typeof(Words);
				case "WorldAreas":
					return typeof(WorldAreas);
			}

			return null;
		}

		public static BaseDat Create(string fileName, BinaryReader inStream)
		{
			switch (fileName)
			{
				case "ActiveSkills":
					return new ActiveSkills(inStream);
				case "ArmourTypes":
					return new ArmourTypes(inStream);
				case "BackendErrors":
					return new BackendErrors(inStream);
				case "BaseItemTypes":
					return new BaseItemTypes(inStream);
				case "BloodTypes":
					return new BloodTypes(inStream);
				case "BuffDefinitions":
					return new BuffDefinitions(inStream);
				case "CharacterAudioEvents":
					return new CharacterAudioEvents(inStream);
				case "Characters":
					return new Characters(inStream);
				case "ChestClusters":
					return new ChestClusters(inStream);
				case "Chests":
					return new Chests(inStream);
				case "ComponentArmour":
					return new ComponentArmour(inStream);
				case "ComponentAttributeRequirements":
					return new ComponentAttributeRequirements(inStream);
				case "ComponentCharges":
					return new ComponentCharges(inStream);
				case "CurrencyItems":
					return new CurrencyItems(inStream);
				case "Dances":
					return new Dances(inStream);
				case "DefaultMonsterStats":
					return new DefaultMonsterStats(inStream);
				case "Difficulties":
					return new Difficulties(inStream);
				case "DropPool":
					return new DropPool(inStream);
				case "Environments":
					return new Environments(inStream);
				case "ExperienceLevels":
					return new ExperienceLevels(inStream);
				case "Flasks":
					return new Flasks(inStream);
				case "FlavourText":
					return new FlavourText(inStream);
				case "GameConstants":
					return new GameConstants(inStream);
				case "GemTags":
					return new GemTags(inStream);
				case "GrantedEffects":
					return new GrantedEffects(inStream);
				case "GrantedEffectsPerLevel":
					return new GrantedEffectsPerLevel(inStream);
				case "ItemExperiencePerLevel":
					return new ItemExperiencePerLevel(inStream);
				case "ItemisedVisualEffect":
					return new ItemisedVisualEffect(inStream);
				case "ItemVisualEffect":
					return new ItemVisualEffect(inStream);
				case "ItemVisualIdentity":
					return new ItemVisualIdentity(inStream);
				case "MapConnections":
					return new MapConnections(inStream);
				case "MapPins":
					return new MapPins(inStream);
				case "Maps":
					return new Maps(inStream);
				case "MiscAnimated":
					return new MiscAnimated(inStream);
				case "MiscObjects":
					return new MiscObjects(inStream);
				case "Mods":
					return new Mods(inStream);
				case "ModSellPrices":
					return new ModSellPrices(inStream);
				case "MonsterPackEntries":
					return new MonsterPackEntries(inStream);
				case "MonsterPacks":
					return new MonsterPacks(inStream);
				case "MonsterTypes":
					return new MonsterTypes(inStream);
				case "MonsterVarieties":
					return new MonsterVarieties(inStream);
				case "Music":
					return new Music(inStream);
				case "NPCs":
					return new NPCs(inStream);
				case "NPCTalk":
					return new NPCTalk(inStream);
				case "NPCTextAudio":
					return new NPCTextAudio(inStream);
				case "PassiveSkills":
					return new PassiveSkills(inStream);
				case "Pet":
					return new Pet(inStream);
				case "Projectiles":
					return new Projectiles(inStream);
				case "Quest":
					return new Quest(inStream);
				case "QuestRewards":
					return new QuestRewards(inStream);
				case "QuestStates":
					return new QuestStates(inStream);
				case "QuestStaticRewards":
					return new QuestStaticRewards(inStream);
				case "Realms":
					return new Realms(inStream);
				case "ShieldTypes":
					return new ShieldTypes(inStream);
				case "ShopItem":
					return new ShopItem(inStream);
				case "ShopPaymentPackage":
					return new ShopPaymentPackage(inStream);
				case "Shrines":
					return new Shrines(inStream);
				case "SkillGems":
					return new SkillGems(inStream);
				case "SoundEffects":
					return new SoundEffects(inStream);
				case "Stats":
					return new Stats(inStream);
				case "Tags":
					return new Tags(inStream);
				case "Topologies":
					return new Topologies(inStream);
				case "VoteState":
					return new VoteState(inStream);
				case "VoteType":
					return new VoteType(inStream);
				case "WeaponTypes":
					return new WeaponTypes(inStream);
				case "Words":
					return new Words(inStream);
				case "WorldAreas":
					return new WorldAreas(inStream);	
			}

			throw new Exception("Missing dat parser for type " + fileName);
		}
	}
}
