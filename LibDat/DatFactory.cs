using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LibDat.Files;
using System.Reflection;

namespace LibDat
{
	public static class DatFactory
	{
		public static Type GetTypeName(string fileName)
		{
			return Type.GetType(string.Format("LibDat.Files.{0}, LibDat", fileName));
			
			/*
			switch (fileName)
			{
				case "AchievementItems":
					return typeof(AchievementItems);
				case "Achievements":
					return typeof(Achievements);
				case "AchievementSets":
					return typeof(AchievementSets);
				case "ActiveSkills":
					return typeof(ActiveSkills);
				case "ActiveSkillTargetTypes":
					return typeof(ActiveSkillTargetTypes);
				case "ActiveSkillType":
					return typeof(ActiveSkillType);
				case "ArmourClasses":
					return typeof(ArmourClasses);
				case "ArmourSurfaceTypes":
					return typeof(ArmourSurfaceTypes);
				case "ArmourTypes":
					return typeof(ArmourTypes);
				case "Attributes":
					return typeof(Attributes);
				case "BackendErrors":
					return typeof(BackendErrors);
				case "BaseItemTypes":
					return typeof(BaseItemTypes);
				case "BloodTypes":
					return typeof(BloodTypes);
				case "BuffCategories":
					return typeof(BuffCategories);
				case "BuffDefinitions":
					return typeof(BuffDefinitions);
				case "BuffGroups":
					return typeof(BuffGroups);
				case "BuffMergeModes":
					return typeof(BuffMergeModes);
				case "BuffVisuals":
					return typeof(BuffVisuals);
				case "CharacterAudioEvents":
					return typeof(CharacterAudioEvents);
				case "Characters":
					return typeof(Characters);
				case "ChestClusters":
					return typeof(ChestClusters);
				case "Chests":
					return typeof(Chests);
				case "ClientStrings":
					return typeof(ClientStrings);
				case "Commands":
					return typeof(Commands);
				case "ComponentArmour":
					return typeof(ComponentArmour);
				case "ComponentAttributeRequirements":
					return typeof(ComponentAttributeRequirements);
				case "ComponentCharges":
					return typeof(ComponentCharges);
				case "CooldownBypassTypes":
					return typeof(CooldownBypassTypes);
				case "CraftingBenchOptions":
					return typeof(CraftingBenchOptions);
				case "CurrencyItems":
					return typeof(CurrencyItems);
				case "CurrencyUseTypes":
					return typeof(CurrencyUseTypes);
				case "Dances":
					return typeof(Dances);
				case "Default":
					return typeof(Default);
				case "DefaultMonsterStats":
					return typeof(DefaultMonsterStats);
				case "DescentExiles":
					return typeof(DescentExiles);
				case "DescentRewardChests":
					return typeof(DescentRewardChests);
				case "DescentStarterChest":
					return typeof(DescentStarterChest);
				case "Difficulties":
					return typeof(Difficulties);
				case "DropPool":
					return typeof(DropPool);
				case "Effectiveness":
					return typeof(Effectiveness);
				case "EffectivenessCostConstants":
					return typeof(EffectivenessCostConstants);
				case "EndlessLedgeChests":
					return typeof(EndlessLedgeChests);
				case "Environments":
					return typeof(Environments);
				case "EventSeason":
					return typeof(EventSeason);
				case "EventSeasonRewards":
					return typeof(EventSeasonRewards);
				case "ExperienceLevels":
					return typeof(ExperienceLevels);
				case "Flasks":
					return typeof(Flasks);
				case "FlaskType":
					return typeof(FlaskType);
				case "FlavourText":
					return typeof(FlavourText);
				case "GameConstants":
					return typeof(GameConstants);
				case "GemTags":
					return typeof(GemTags);
				case "GemTypes":
					return typeof(GemTypes);
				case "GlobalAudioConfig":
					return typeof(GlobalAudioConfig);
				case "GrantedEffects":
					return typeof(GrantedEffects);
				case "GrantedEffectsPerLevel":
					return typeof(GrantedEffectsPerLevel);
				case "HideoutDoodads":
					return typeof(HideoutDoodads);
				case "ImpactSoundData":
					return typeof(ImpactSoundData);
				case "InvasionMonsterGroups":
					return typeof(InvasionMonsterGroups);
				case "InvasionMonsterRestrictions":
					return typeof(InvasionMonsterRestrictions);
				case "InvasionMonsterRoles":
					return typeof(InvasionMonsterRoles);
				case "InvasionMonstersPerArea":
					return typeof(InvasionMonstersPerArea);
				case "ItemClasses":
					return typeof(ItemClasses);
				case "ItemExperiencePerLevel":
					return typeof(ItemExperiencePerLevel);
				case "ItemisedVisualEffect":
					return typeof(ItemisedVisualEffect);
				case "ItemVisualEffect":
					return typeof(ItemVisualEffect);
				case "ItemVisualIdentity":
					return typeof(ItemVisualIdentity);
				case "LeagueCategory":
					return typeof(LeagueCategory);
				case "LeagueFlag":
					return typeof(LeagueFlag);
				case "MapConnections":
					return typeof(MapConnections);
				case "MapPins":
					return typeof(MapPins);
				case "Maps":
					return typeof(Maps);
				case "MicrotransactionPortalVariations":
					return typeof(MicrotransactionPortalVariations);
				case "MicrotransactionSocialFrameVariations":
					return typeof(MicrotransactionSocialFrameVariations);
				case "MiscAnimated":
					return typeof(MiscAnimated);
				case "MiscObjects":
					return typeof(MiscObjects);
				case "ModAuraFlags":
					return typeof(ModAuraFlags);
				case "ModDomains":
					return typeof(ModDomains);
				case "ModFamily":
					return typeof(ModFamily);
				case "ModGenerationType":
					return typeof(ModGenerationType);
				case "Mods":
					return typeof(Mods);
				case "ModSellPrices":
					return typeof(ModSellPrices);
				case "ModType":
					return typeof(ModType);
				case "MonsterBehavior":
					return typeof(MonsterBehavior);
				case "MonsterFleeConditions":
					return typeof(MonsterFleeConditions);
				case "MonsterPackEntries":
					return typeof(MonsterPackEntries);
				case "MonsterPacks":
					return typeof(MonsterPacks);
				case "MonsterScalingByLevel":
					return typeof(MonsterScalingByLevel);
				case "MonsterSize":
					return typeof(MonsterSize);
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
				case "NPCTalkCategory":
					return typeof(NPCTalkCategory);
				case "NPCTextAudio":
					return typeof(NPCTextAudio);
				case "PassiveSkills":
					return typeof(PassiveSkills);
				case "PerLevelValues":
					return typeof(PerLevelValues);
				case "Pet":
					return typeof(Pet);
				case "Projectiles":
					return typeof(Projectiles);
				case "PVPTypes":
					return typeof(PVPTypes);
				case "Quest":
					return typeof(Quest);
				case "QuestFlags":
					return typeof(QuestFlags);
				case "QuestRewards":
					return typeof(QuestRewards);
				case "QuestStates":
					return typeof(QuestStates);
				case "QuestStaticRewards":
					return typeof(QuestStaticRewards);
				case "Rarity":
					return typeof(Rarity);
				case "Realms":
					return typeof(Realms);
				case "RelativeImportanceConstants":
					return typeof(RelativeImportanceConstants);
				case "ShieldTypes":
					return typeof(ShieldTypes);
				case "ShopCategory":
					return typeof(ShopCategory);
				case "ShopItem":
					return typeof(ShopItem);
				case "ShopPaymentPackage":
					return typeof(ShopPaymentPackage);
				case "ShrineBuffs":
					return typeof(ShrineBuffs);
				case "Shrines":
					return typeof(Shrines);
				case "ShrineSounds":
					return typeof(ShrineSounds);
				case "SkillGems":
					return typeof(SkillGems);
				case "SoundEffects":
					return typeof(SoundEffects);
				case "Stats":
					return typeof(Stats);
				case "StatSemantics":
					return typeof(StatSemantics);
				case "StrongBoxEffects":
					return typeof(StrongBoxEffects);
				case "Tags":
					return typeof(Tags);
				case "Topologies":
					return typeof(Topologies);
				case "UniqueChests":
					return typeof(UniqueChests);
				case "UniqueSetNames":
					return typeof(UniqueSetNames);
				case "UniqueSets":
					return typeof(UniqueSets);
				case "VoteState":
					return typeof(VoteState);
				case "VoteType":
					return typeof(VoteType);
				case "WeaponArmourCommon":
					return typeof(WeaponArmourCommon);
				case "WeaponClasses":
					return typeof(WeaponClasses);
				case "WeaponDamageScaling":
					return typeof(WeaponDamageScaling);
				case "WeaponImpactSoundData":
					return typeof(WeaponImpactSoundData);
				case "WeaponSoundTypes":
					return typeof(WeaponSoundTypes);
				case "WeaponTypes":
					return typeof(WeaponTypes);
				case "Wordlists":
					return typeof(Wordlists);
				case "Words":
					return typeof(Words);
				case "WorldAreas":
					return typeof(WorldAreas);
			}
			*/
			return null;
		}

		public static BaseDat Create(string fileName, BinaryReader inStream)
		{
			return (BaseDat) Activator.CreateInstance(GetTypeName(fileName), new object[] {inStream});

			switch (fileName)
			{
				case "AchievementItems":
					return new AchievementItems(inStream);
				case "Achievements":
					return new Achievements(inStream);
				case "AchievementSets":
					return new AchievementSets(inStream);
				case "ActiveSkills":
					return new ActiveSkills(inStream);
				case "ActiveSkillTargetTypes":
					return new ActiveSkillTargetTypes(inStream);
				case "ActiveSkillType":
					return new ActiveSkillType(inStream);
				case "ArmourClasses":
					return new ArmourClasses(inStream);
				case "ArmourSurfaceTypes":
					return new ArmourSurfaceTypes(inStream);
				case "ArmourTypes":
					return new ArmourTypes(inStream);
				case "Attributes":
					return new Attributes(inStream);
				case "BackendErrors":
					return new BackendErrors(inStream);
				case "BaseItemTypes":
					return new BaseItemTypes(inStream);
				case "BloodTypes":
					return new BloodTypes(inStream);
				case "BuffCategories":
					return new BuffCategories(inStream);
				case "BuffDefinitions":
					return new BuffDefinitions(inStream);
				case "BuffGroups":
					return new BuffGroups(inStream);
				case "BuffMergeModes":
					return new BuffMergeModes(inStream);
				case "BuffVisuals":
					return new BuffVisuals(inStream);
				case "CharacterAudioEvents":
					return new CharacterAudioEvents(inStream);
				case "Characters":
					return new Characters(inStream);
				case "ChestClusters":
					return new ChestClusters(inStream);
				case "Chests":
					return new Chests(inStream);
				case "ClientStrings":
					return new ClientStrings(inStream);
				case "Commands":
					return new Commands(inStream);
				case "ComponentArmour":
					return new ComponentArmour(inStream);
				case "ComponentAttributeRequirements":
					return new ComponentAttributeRequirements(inStream);
				case "ComponentCharges":
					return new ComponentCharges(inStream);
				case "CooldownBypassTypes":
					return new CooldownBypassTypes(inStream);
				case "CurrencyItems":
					return new CurrencyItems(inStream);
				case "CurrencyUseTypes":
					return new CurrencyUseTypes(inStream);
				case "Dances":
					return new Dances(inStream);
				case "Default":
					return new Default(inStream);
				case "DefaultMonsterStats":
					return new DefaultMonsterStats(inStream);
				case "DescentExiles":
					return new DescentExiles(inStream);
				case "DescentRewardChests":
					return new DescentRewardChests(inStream);
				case "DescentStarterChest":
					return new DescentStarterChest(inStream);
				case "Difficulties":
					return new Difficulties(inStream);
				case "DropPool":
					return new DropPool(inStream);
				case "Effectiveness":
					return new Effectiveness(inStream);
				case "EffectivenessCostConstants":
					return new EffectivenessCostConstants(inStream);
				case "EndlessLedgeChests":
					return new EndlessLedgeChests(inStream);
				case "Environments":
					return new Environments(inStream);
				case "EventSeason":
					return new EventSeason(inStream);
				case "EventSeasonRewards":
					return new EventSeasonRewards(inStream);
				case "ExperienceLevels":
					return new ExperienceLevels(inStream);
				case "Flasks":
					return new Flasks(inStream);
				case "FlaskType":
					return new FlaskType(inStream);
				case "FlavourText":
					return new FlavourText(inStream);
				case "GameConstants":
					return new GameConstants(inStream);
				case "GemTags":
					return new GemTags(inStream);
				case "GemTypes":
					return new GemTypes(inStream);
				case "GlobalAudioConfig":
					return new GlobalAudioConfig(inStream);
				case "GrantedEffects":
					return new GrantedEffects(inStream);
				case "GrantedEffectsPerLevel":
					return new GrantedEffectsPerLevel(inStream);
				case "ImpactSoundData":
					return new ImpactSoundData(inStream);
				case "InvasionMonsterGroups":
					return new InvasionMonsterGroups(inStream);
				case "InvasionMonsterRestrictions":
					return new InvasionMonsterRestrictions(inStream);
				case "InvasionMonsterRoles":
					return new InvasionMonsterRoles(inStream);
				case "InvasionMonstersPerArea":
					return new InvasionMonstersPerArea(inStream);
				case "ItemClasses":
					return new ItemClasses(inStream);
				case "ItemExperiencePerLevel":
					return new ItemExperiencePerLevel(inStream);
				case "ItemisedVisualEffect":
					return new ItemisedVisualEffect(inStream);
				case "ItemVisualEffect":
					return new ItemVisualEffect(inStream);
				case "ItemVisualIdentity":
					return new ItemVisualIdentity(inStream);
				case "LeagueCategory":
					return new LeagueCategory(inStream);
				case "LeagueFlag":
					return new LeagueFlag(inStream);
				case "MapConnections":
					return new MapConnections(inStream);
				case "MapPins":
					return new MapPins(inStream);
				case "Maps":
					return new Maps(inStream);
				case "MicrotransactionPortalVariations":
					return new MicrotransactionPortalVariations(inStream);
				case "MicrotransactionSocialFrameVariations":
					return new MicrotransactionSocialFrameVariations(inStream);
				case "MiscAnimated":
					return new MiscAnimated(inStream);
				case "MiscObjects":
					return new MiscObjects(inStream);
				case "ModAuraFlags":
					return new ModAuraFlags(inStream);
				case "ModDomains":
					return new ModDomains(inStream);
				case "ModFamily":
					return new ModFamily(inStream);
				case "ModGenerationType":
					return new ModGenerationType(inStream);
				case "Mods":
					return new Mods(inStream);
				case "ModSellPrices":
					return new ModSellPrices(inStream);
				case "ModType":
					return new ModType(inStream);
				case "MonsterBehavior":
					return new MonsterBehavior(inStream);
				case "MonsterFleeConditions":
					return new MonsterFleeConditions(inStream);
				case "MonsterPackEntries":
					return new MonsterPackEntries(inStream);
				case "MonsterPacks":
					return new MonsterPacks(inStream);
				case "MonsterScalingByLevel":
					return new MonsterScalingByLevel(inStream);
				case "MonsterSize":
					return new MonsterSize(inStream);
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
				case "NPCTalkCategory":
					return new NPCTalkCategory(inStream);
				case "NPCTextAudio":
					return new NPCTextAudio(inStream);
				case "PassiveSkills":
					return new PassiveSkills(inStream);
				case "PerLevelValues":
					return new PerLevelValues(inStream);
				case "Pet":
					return new Pet(inStream);
				case "Projectiles":
					return new Projectiles(inStream);
				case "PVPTypes":
					return new PVPTypes(inStream);
				case "Quest":
					return new Quest(inStream);
				case "QuestFlags":
					return new QuestFlags(inStream);
				case "QuestRewards":
					return new QuestRewards(inStream);
				case "QuestStates":
					return new QuestStates(inStream);
				case "QuestStaticRewards":
					return new QuestStaticRewards(inStream);
				case "Rarity":
					return new Rarity(inStream);
				case "Realms":
					return new Realms(inStream);
				case "RelativeImportanceConstants":
					return new RelativeImportanceConstants(inStream);
				case "ShieldTypes":
					return new ShieldTypes(inStream);
				case "ShopCategory":
					return new ShopCategory(inStream);
				case "ShopItem":
					return new ShopItem(inStream);
				case "ShopPaymentPackage":
					return new ShopPaymentPackage(inStream);
				case "ShrineBuffs":
					return new ShrineBuffs(inStream);
				case "Shrines":
					return new Shrines(inStream);
				case "ShrineSounds":
					return new ShrineSounds(inStream);
				case "SkillGems":
					return new SkillGems(inStream);
				case "SoundEffects":
					return new SoundEffects(inStream);
				case "Stats":
					return new Stats(inStream);
				case "StatSemantics":
					return new StatSemantics(inStream);
				case "Tags":
					return new Tags(inStream);
				case "StrongBoxEffects":
					return new StrongBoxEffects(inStream);
				case "Topologies":
					return new Topologies(inStream);
				case "UniqueChests":
					return new UniqueChests(inStream);
				case "VoteState":
					return new VoteState(inStream);
				case "UniqueSetNames":
					return new UniqueSetNames(inStream);
				case "UniqueSets":
					return new UniqueSets(inStream);
				case "VoteType":
					return new VoteType(inStream);
				case "WeaponArmourCommon":
					return new WeaponArmourCommon(inStream);
				case "WeaponClasses":
					return new WeaponClasses(inStream);
				case "WeaponDamageScaling":
					return new WeaponDamageScaling(inStream);
				case "WeaponImpactSoundData":
					return new WeaponImpactSoundData(inStream);
				case "WeaponSoundTypes":
					return new WeaponSoundTypes(inStream);
				case "WeaponTypes":
					return new WeaponTypes(inStream);
				case "Wordlists":
					return new Wordlists(inStream);
				case "Words":
					return new Words(inStream);
				case "WorldAreas":
					return new WorldAreas(inStream);
			}
			
			throw new Exception("Missing dat parser for type " + fileName);
		}
	}
}
