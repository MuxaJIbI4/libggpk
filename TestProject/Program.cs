using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using LibGGPK;
using System.IO;
using System.Linq.Expressions;
using LibDat;
using LibDat.Files;

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

			//Environment.CurrentDirectory = @"O:\Program Files (x86)\Grinding Gear Games\Path of Exile\dat\Data";

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
				@"Chests.dat", // done
				@"ComponentArmour.dat", // done
				@"ComponentAttributeRequirements.dat", // done
				@"ComponentCharges.dat", // done
				@"CurrencyItems.dat", // done (Needs translation?)
				@"Dances.dat", // done
				@"DefaultMonsterStats.dat", // done
				@"Difficulties.dat", // done
				@"DropPool.dat", // done
				@"Environments.dat", // done
				@"ExperienceLevels.dat", // done
				@"Flasks.dat", // done
				@"FlavourText.dat", // done
				@"GameConstants.dat", // done
				@"GemTags.dat", // done
				@"GrantedEffects.dat", // done
				@"GrantedEffectsPerLevel.dat", // done
				@"ItemExperiencePerLevel.dat", // done
				@"ItemisedVisualEffect.dat", // done
				@"ItemVisualEffect.dat", // done
				@"ItemVisualIdentity.dat", // done
				@"MapConnections.dat", // done
				@"MapPins.dat", // done
				@"Maps.dat", // done
				@"MiscAnimated.dat", // done
				@"MiscObjects.dat", // done
				@"Mods.dat", // done
				@"ModSellPrices.dat", // done
				@"MonsterPackEntries.dat", // done
				@"MonsterPacks.dat", // done
				@"MonsterTypes.dat", // done
				@"MonsterVarieties.dat", // done
				@"Music.dat", // done
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


			//ReadAndDumpStruct();
			//return;

			//	var container = new DatContainer(datFiles[0]);
			//	Console.WriteLine(datFiles[i] + ":");

			string ggpkPath = @"C:\Program Files (x86)\Grinding Gear Games\Path of Exile\content.ggpk";
			GGPK content = new GGPK();
			content.Read(ggpkPath, Output);

			foreach (var pair in content.RecordOffsets)
			{
				if (!(pair.Value is FileRecord))
					continue;

				FileRecord record = pair.Value as FileRecord;
				if (Path.GetExtension(record.Name) != ".dat")
					continue;

				if (record.Name != "NPCTextAudio.dat")
					continue;
				Console.WriteLine(record.Name);

				byte[] datBytes = record.ReadData(ggpkPath);
				using (MemoryStream datStream = new MemoryStream(datBytes))
				{
					DatContainer container;
					//try
					//{
					container = new DatContainer(datStream, record.Name);
					//}
					//catch (Exception ex)
					//{
					//	continue;
					//}


					foreach (var propInfo in container.datType.GetProperties())
					{
						if (propInfo.GetCustomAttributes(false).Any(n => n is UserStringIndex))
						{
							foreach (var entry in container.Entries)
							{
								int stringIndex = (int)propInfo.GetValue(entry, null);
								string stringValue = container.DataEntries[stringIndex].ToString();

								Console.WriteLine("  " + stringValue);
								//userStrings.Add(stringValue);
							}
						}
					}
				}
			}
			//Dictionary<string, HashSet<string>> userStringsPerFile = new Dictionary<string, HashSet<string>>();

			//HashSet<string> userStrings = new HashSet<string>();
			//for (int i = 0; i < datFiles.Length; i++)
			//{

			//	var container = new DatContainer(datFiles[i]);

			//	foreach (var propInfo in container.datType.GetProperties())
			//	{
			//		if (propInfo.GetCustomAttributes(false).Any(n => n is UserStringIndex))
			//		{
			//			foreach (var entry in container.Entries)
			//			{
			//				int stringIndex = (int)propInfo.GetValue(entry, null);
			//				string stringValue = container.DataEntries[stringIndex].ToString();

			//				userStrings.Add(stringValue);
			//			}
			//		}
			//	}
			//}
			//userStringsPerFile.Add(datFiles[0], userStrings);


			//int itemCount = 0;
			//foreach (var set in userStringsPerFile)
			//{
			//	//Console.WriteLine(set.Key + ":");
			//	foreach (var item in set.Value)
			//	{
			//		++itemCount;
			//		//Console.WriteLine("  " + item);
			//	}
			//}
			//Console.WriteLine("Total strings: " + itemCount);
		}






























		private static void DumpDat(string filePath)
		{
			byte[] fileBytes = File.ReadAllBytes(filePath);
			StringBuilder sb = new StringBuilder();
			using (MemoryStream ms = new MemoryStream(fileBytes))
			{
				using (BinaryReader br = new BinaryReader(ms, System.Text.Encoding.Unicode))
				{
					int entryCount = br.ReadInt32();
					int dataTableStart = -1;

					while (true)
					{
						if (br.ReadByte() == 0xbb &&
							br.ReadByte() == 0xbb &&
							br.ReadByte() == 0xbb &&
							br.ReadByte() == 0xbb &&
							br.ReadByte() == 0xbb &&
							br.ReadByte() == 0xbb &&
							br.ReadByte() == 0xbb &&
							br.ReadByte() == 0xbb)
						{
							dataTableStart = (int)(br.BaseStream.Position - 8);
							break;
						}
					}
					br.BaseStream.Seek(4, SeekOrigin.Begin);

					int entrySize = dataTableStart / entryCount;


					sb.AppendLine(Path.GetFileNameWithoutExtension(filePath));
					for (int i = 0; i < entryCount; i++)
					{
						byte[] data = br.ReadBytes(entrySize);
						for (int j = 0; j < data.Length; j++)
						{
							sb.AppendFormat("{0:X2} ", data[j]);
						}
						sb.AppendLine();
					}

					br.BaseStream.Seek(dataTableStart, SeekOrigin.Begin);
					File.WriteAllBytes(filePath + "_data.bin", br.ReadBytes((int)(br.BaseStream.Length - br.BaseStream.Position)));
				}
			}

			File.WriteAllText(filePath + "_bytes.txt", sb.ToString());
		}

		private static void DumpProps(string[] vars)
		{
			foreach (var @var in vars)
			{
				string lowerVar = var.ToLower();

				if (lowerVar.Contains("flag"))
					Console.WriteLine("		public bool {0} {{ get; set; }}", var);
				else if (lowerVar.Contains("64"))
					Console.WriteLine("		public Int64 {0} {{ get; set; }}", var);
				else
				{
					if(lowerVar.Contains("index"))
						Console.WriteLine("		[StringIndex]");
					else if(lowerVar.Contains("data"))
						Console.WriteLine("		[DataIndex]");

					Console.WriteLine("		public int {0} {{ get; set; }}", var);
				}
			}
		}

		private static void DumpRead(string[] vars, string className)
		{
			Console.WriteLine();
			Console.WriteLine("		public {0}(BinaryReader inStream)", className);
			Console.WriteLine("		{");
			foreach (var @var in vars)
			{
				string lowerVar = var.ToLower();

				if (lowerVar.Contains("flag"))
					Console.WriteLine("			{0} = inStream.ReadBoolean();", var);
				else if (lowerVar.Contains("64"))
					Console.WriteLine("			{0} = inStream.ReadInt64();", var);
				else
					Console.WriteLine("			{0} = inStream.ReadInt32();", var);
			}
			Console.WriteLine("		}");
		}

		private static void DumpWrite(string[] vars)
		{
			Console.WriteLine();
			Console.WriteLine("		public override void Save(BinaryWriter outStream)");
			Console.WriteLine("		{");
			foreach (var @var in vars)
			{
				string lowerVar = var.ToLower();

				Console.WriteLine("			outStream.Write({0});", var);
			}
			Console.WriteLine("		}");
		}

		private static void DumpSize(string[] vars)
		{
			int size = 0;

			foreach (var @var in vars)
			{
				string lowerVar = var.ToLower();

				if (lowerVar.Contains("flag"))
					size += 1;
				else if (lowerVar.Contains("64"))
					size += 8;
				else
					size += 4;
			}

			Console.WriteLine();
			Console.WriteLine("		public override int GetSize()");
			Console.WriteLine("		{");
			Console.WriteLine("			return 0x{0:X};", size);
			Console.WriteLine("		}");
		}

		private static void ReadAndDumpStruct()
		{
			string className = Console.ReadLine().Trim();
			string input = Console.ReadLine().Trim();
			string previousInput = input;
			Console.Clear();
			do
			{
				previousInput = input;
				input = input.Replace("\t\t", "\t");
			} while (previousInput != input);

			string[] varNames = input.Split(new char[] { '\t' });

			Console.WriteLine("using System.IO;");
			Console.WriteLine();
			Console.WriteLine("namespace LibDat.Files");
			Console.WriteLine("{");
			Console.WriteLine("	public class {0} : BaseDat", className);
			Console.WriteLine("	{");
			DumpProps(varNames);
			DumpRead(varNames, className);
			DumpWrite(varNames);
			DumpSize(varNames);
			Console.WriteLine("	}");
			Console.WriteLine("}");

			Console.ReadKey();
		}
	}
}