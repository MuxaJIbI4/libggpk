using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
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
	public class Program
	{
		static void Output(string msg)
		{
			Console.Write(msg);
		}

		public class DatFile
		{
			public string Name { get; set; }

		}
		[Serializable]
		public class DatTranslation
		{
			public string DatName { get; set; }
			public List<Translation> Translations { get; set; }
		}

		[Serializable]
		public class Translation
		{
			public enum TranslationStatus
			{
				Invalid,
				Missing,
				Outdated,
				Active,
				NeedToApply,
			}

			[XmlIgnore]
			public uint OriginalHash { get; set; }
			[XmlIgnore]
			public uint TranslationHash { get; set; }
			[XmlIgnore]
			public TranslationStatus Status { get; set; }
			public string OriginalText { get; set; }
			public string TranslatedText { get; set; }
		}

		public static void Main(string[] args)
		{
			new Program();
		}

		private GGPK content = new GGPK();
		private const string ggpkPath = @"c:\Program Files (x86)\Grinding Gear Games\Path of Exile\content.ggpk";
		private Dictionary<string, FileRecord> fileRecordMap = new Dictionary<string, FileRecord>();




		private List<DatTranslation> userTranslations;
		private const string SettingsPath = ".\\translation.xml";



		public void DumpStrings(GGPK content)
		{
			var strings = GetAllStrings(content);

			foreach (var item in strings)
			{
				if (item.Value.Count > 0)
				{
					Console.WriteLine();
					Console.WriteLine("--------------------------------------");
					Console.WriteLine("        " + item.Key);
					Console.WriteLine("--------------------------------------");
					foreach (var str in item.Value)
					{
						Console.WriteLine(str.Replace(Environment.NewLine, "__BREAK__"));
					}
				}
			}
		}

		public Program()
		{
			//CreateExampleUserTranslations();
			//SaveTranslationData();
			//ReadTranslationData();
			//
			//// TESTING PURPOSES ONLY
			content.Read(ggpkPath, Output);


			//CreateExampleUserTranslations();
			//return;
			userTranslations = ReadTranslationData();
			//SaveTranslationData(userTranslations);
			
			//Console.WriteLine("Reading and hashing...");
			var translatableStringsTable = GetTranslatableDatStrings(content);
			
			foreach (var translationTable in userTranslations)
			{
				if (!translatableStringsTable.ContainsKey(translationTable.DatName))
					continue;
			
				ProcessTranslation(translationTable, translatableStringsTable);
			}
		}

		/// <summary>
		/// Applies translations to content.ggpk
		/// </summary>
		/// <param name="translationTable">Table of translations to apply</param>
		/// <param name="translatableStringsTable">Map of all strings that can be translated</param>
		private void ProcessTranslation(DatTranslation translationTable, Dictionary<string, Dictionary<uint, string>> translatableStringsTable)
		{
			Dictionary<uint, string> translatableStrings = translatableStringsTable[translationTable.DatName];

			// Record we will be translating with data from translationTable
			FileRecord datRecord = fileRecordMap[translationTable.DatName];

			// Raw bytes of the .dat file we will be translating
			byte[] datBytes = datRecord.ReadData(ggpkPath);

			// Dat parser for changing the actual strings
			DatContainer dc = new DatContainer(new MemoryStream(datBytes), translationTable.DatName);

			// Map of hash->string pairs of strings we will need to replace in the target dat file
			Dictionary<uint, string> stringsToReplace = new Dictionary<uint, string>();

			// Go through all of the user .dat translations and build a table of strings that need to be replaced.
			foreach (var translation in translationTable.Translations)
			{
				if (translatableStrings.ContainsKey(translation.OriginalHash))
				{
					//Console.WriteLine("Need to apply" + Environment.NewLine + translation.OriginalText + Environment.NewLine);
					translation.Status = Translation.TranslationStatus.NeedToApply;
					stringsToReplace.Add(translation.OriginalHash, translation.TranslatedText);
				}
				else if (translatableStrings.ContainsKey(translation.TranslationHash))
				{
					//Console.WriteLine("Already applied " + Environment.NewLine + translation.OriginalText + Environment.NewLine);
					translation.Status = Translation.TranslationStatus.Active;
				}
				else
				{
					//Console.WriteLine("Missing original text: " + Environment.NewLine + translation.OriginalText + Environment.NewLine);
					translation.Status = Translation.TranslationStatus.Missing;
				}
			}

			// Replace the actual strings
			foreach (var item in dc.DataEntries)
			{
				if (!(item.Value is UnicodeString))
					continue;

				UnicodeString currentDatString = (item.Value as UnicodeString);
				if (!currentDatString.IsUserString)
					continue;

				uint currentDatStringHash = Hash.MurmurHash2(currentDatString.Data);
				if (!stringsToReplace.ContainsKey(currentDatStringHash))
					continue;

				currentDatString.NewData = stringsToReplace[currentDatStringHash];

				//Console.WriteLine("Replacing " + currentDatString.Data + " with " + stringsToReplace[currentDatStringHash]);
			}

			// dc.GetBytes() will return the new data for this .dat file after replacing the original strings with whatever's in 'NewData'
			datRecord.ReplaceContents(ggpkPath, dc.GetBytes(), content.FreeRoot);
		}

		private Dictionary<string, HashSet<string>> GetAllStrings(GGPK content)
		{
			var allStrings = new Dictionary<string, HashSet<string>>();

			foreach (var recordOffset in content.RecordOffsets)
			{
				if (!(recordOffset.Value is FileRecord))
					continue;

				FileRecord record = recordOffset.Value as FileRecord;
				if (record.ContainingDirectory == null || Path.GetExtension(record.Name) != ".dat")
					continue;

				byte[] datBytes = record.ReadData(ggpkPath);
				using (MemoryStream datStream = new MemoryStream(datBytes))
				{
					HashSet<string> stringsForThisDatFile = new HashSet<string>();

					DatContainer container;
					try
					{
						container = new DatContainer(datStream, record.Name);
					}
					catch (Exception ex)
					{
						continue;
					}

					// Any properties with the UserStringIndex attribute are translatable
					foreach (var propInfo in container.DatType.GetProperties())
					{
						if (propInfo.GetCustomAttributes(false).Any(n => n is UserStringIndex))
						{
							foreach (var entry in container.Entries)
							{
								int stringIndex = (int)propInfo.GetValue(entry, null);
								string stringValue = container.DataEntries[stringIndex].ToString();

								stringsForThisDatFile.Add(stringValue);
							}
						}
					}

					allStrings.Add(record.Name, stringsForThisDatFile);
				}
			}

			return allStrings;
		}

		/// <summary>
		/// Searches all of the /data/*.dat files in content.ggpk for user strings that can be translated.
		/// </summary>
		/// <returns>
		/// Map of hash|string maps for each file
		/// eg:
		/// result = map  
		/// {
		///		Key = "VoteState.dat", 
		///		Value = Map {
		///			{Key = Hash("Help the bandit"),  Value = "Help the bandit"},
		///			{Key = Hash("Kill the bandit"),  Value = "Kill the bandit"}
		///		}
		/// }
		/// </returns>
		private Dictionary<string, Dictionary<uint, string>> GetTranslatableDatStrings(GGPK content)
		{
			var translatableDatStrings = new Dictionary<string, Dictionary<uint, string>>();

			foreach (var recordOffset in content.RecordOffsets)
			{
				if (!(recordOffset.Value is FileRecord))
					continue;

				FileRecord record = recordOffset.Value as FileRecord;
				if (record.ContainingDirectory == null || Path.GetExtension(record.Name) != ".dat")
					continue;

				// We'll need this .dat FileRecord later on so we're storing it in a map of fileName -> FileRecord
				fileRecordMap.Add(record.Name, record);
				// Map of all strings that can be safely translated (not used as ID's, paths, etc) stored by their hash
				Dictionary<uint, string> translatableStringMap = new Dictionary<uint, string>();

				byte[] datBytes = record.ReadData(ggpkPath);
				using (MemoryStream datStream = new MemoryStream(datBytes))
				{
					DatContainer container;
					try
					{
						container = new DatContainer(datStream, record.Name);
					}
					catch (Exception ex)
					{
						continue;
					}

					// Any properties with the UserStringIndex attribute are translatable
					foreach (var propInfo in container.DatType.GetProperties())
					{
						if (propInfo.GetCustomAttributes(false).Any(n => n is UserStringIndex))
						{
							foreach (var entry in container.Entries)
							{
								int stringIndex = (int)propInfo.GetValue(entry, null);
								string stringValue = container.DataEntries[stringIndex].ToString();

								if (string.IsNullOrWhiteSpace(stringValue))
									continue;

								uint hash = Hash.MurmurHash2(stringValue);
								if (translatableStringMap.ContainsKey(hash))
									continue;

								translatableStringMap.Add(hash, stringValue);
							}
						}
					}
				}

				translatableDatStrings.Add(record.Name, translatableStringMap);
			}

			return translatableDatStrings;
		}

		private void CreateExampleUserTranslations()
		{
			var translations = new List<DatTranslation>()
			{
				new DatTranslation()
				{
					DatName = "BackendErrors.dat",
					Translations = new List<Translation>()
					{
						new Translation()
						{
							OriginalText = "The specified account does not exist.",
							TranslatedText = "No such account :("
						},
					}
				},
				new DatTranslation()
				{
					DatName = "Characters.dat",
					Translations = new List<Translation>()
					{
						new Translation()
						{
							OriginalText = "I am a warrior. I was born to fight, raised to defend my tribe. Now, I have been taken from my people, chained and exiled by cowards who fear Death.__BREAK____BREAK__Death is my brother. I do not fear him. I see him in the eyes of the men and beasts that I kill. I feel him travelling beside me to this new land, this Wraeclast. He will take me to meet the ancestors when I am ready, and I am not ready.",
							TranslatedText = "我是一个战士。我出生在打，提出了捍卫我的部落。现在，我已经从我的人，用铁链锁住了并且被放逐害怕死亡的人的懦夫。__BREAK____BREAK__死亡是我的兄弟。我不担心他。我看到他的眼睛，我杀了人兽。我觉得他在我的身边，这个新的土地，这Wraeclast。他会带我的祖先当我准备好了，和我没有准备好。"
						},
						new Translation()
						{
							OriginalText = "They were right to fear me. If only they had listened to their cowardice. Had they not taken my home with fire, I would not have taken their children.__BREAK____BREAK__Now I am banished to Wraeclast. I've heard the dark rumours... seen the terror in other exiles' eyes. But to me one land is like any other. The elements are my allies. The dead are my servants. And fear will be my closest friend. ",
							TranslatedText = "Tenían razón para temerme. Si tan sólo hubiera escuchado a su cobardía. Si no hubieran llevado a mi casa con el fuego, no habría llevado a sus hijos.__BREAK____BREAK__Ahora estoy desterrado a Wraeclast. He oído los rumores oscuros ... visto el terror en los ojos de los exiliados de otros. Pero para mí una tierra es como cualquier otro. Los elementos son mis aliados. Los muertos son mis siervos. Y el miedo va a ser mi mejor amigo."
						},
						new Translation()
						{
							OriginalText = "You can't own a life. Not a deer's, not a rabbit's, and not mine. Every soul has a birth-given right to live however they will... however they can. I take from the wilderness and one day it'll take me in return.__BREAK____BREAK__Not that the fat lords of Oriath believe in such things. They call me <italic>{poacher} and <italic>{thief}. Clap me in irons and haul me like cargo into exile. No matter. I'll make the forests and mountains of Wraeclast my new home. My freedom and my bow: that's all I need.",
							TranslatedText = "Вы не можете владеть жизни. Не олень, а не кролик, а не моя. Каждая душа имеет рождения дано право на жизнь, однако, они будут ... Однако они могут. Я беру от пустыни и в один прекрасный день она будет принимать меня в ответ.__BREAK____BREAK__Не то, чтобы жир лордов Oriath верю в такие вещи. Они называют меня <italic>{браконьера} и <italic>{вора}. Хлоп меня в кандалы и тащить меня, как груз в изгнании. Неважно. Я сделаю лесах и горах Wraeclast мой новый дом. Моя свобода и мой лук: это все, что мне нужно."
						},
					},
				},
			};

			SaveTranslationData(translations);
		}

		/// <summary>
		/// WARNING: 'translations' is modified when saving. Newline in strings will be replaced with '__BREAK__'.
		/// </summary>
		private void SaveTranslationData(IEnumerable<DatTranslation> translations)
		{
			XmlSerializer serializer = new XmlSerializer(translations.GetType());
			using (FileStream fs = new FileStream(".\\translation.xml" + ".Example", FileMode.Create))
			{
				foreach (var datTranslationTable in translations)
				{
					foreach (var translation in datTranslationTable.Translations)
					{
						translation.TranslatedText = translation.TranslatedText.Replace(Environment.NewLine, "__BREAK__");
						translation.OriginalText = translation.OriginalText.Replace(Environment.NewLine, "__BREAK__");
					}
				}

				serializer.Serialize(fs, translations);
			}
		}

		private static List<DatTranslation> ReadTranslationData()
		{
			List<DatTranslation> translations = new List<DatTranslation>();

			XmlSerializer serializer = new XmlSerializer(translations.GetType());
			translations = (List<DatTranslation>)serializer.Deserialize(XmlReader.Create(SettingsPath));
			foreach (var datTranslationTable in translations)
			{
				foreach (var translation in datTranslationTable.Translations)
				{
					translation.TranslatedText = translation.TranslatedText.Replace("__BREAK__", Environment.NewLine);
					translation.OriginalText = translation.OriginalText.Replace("__BREAK__", Environment.NewLine);
					translation.OriginalHash = Hash.MurmurHash2(translation.OriginalText);
					translation.TranslationHash = Hash.MurmurHash2(translation.TranslatedText);
					translation.Status = Translation.TranslationStatus.Invalid;
				}
			}

			return translations;
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
					if (lowerVar.Contains("index"))
						Console.WriteLine("		[StringIndex]");
					else if (lowerVar.Contains("data"))
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