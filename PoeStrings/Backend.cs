using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using LibDat;
using LibDat.Data;
using LibGGPK;
using PoeStrings.Properties;

namespace PoeStrings
{
	public class Backend
	{
		//private const string settingsPath = ".\\translation.xml";
		private readonly Action<string> outputFunc;
		private GGPK content = new GGPK();
		private string ggpkPath;
		private string settingsPath;


		/// <summary>
		/// Maps file names to FileRecord
		/// </summary>
		private Dictionary<string, FileRecord> fileRecordMap;
		public Dictionary<string, DatTranslation> AllDatTranslations { get; set; }

		public Backend(Action<string> outputFunc, string settingsPath)
		{
			this.settingsPath = settingsPath;
			this.outputFunc = outputFunc;
		}

		public void ReloadAllData(string ggpkPath)
		{
			this.ggpkPath = ggpkPath;
			content = new GGPK();
			content.Read(ggpkPath, outputFunc);

			CollectTranslatableStrings();
			MergeUserTranslations();
		}

		/// <summary>
		/// Merges user translations with master list of transleatable strings and determiens if the user
		/// translation is already applied or is invalid (possibly due to a patch).
		/// </summary>
		private void MergeUserTranslations()
		{
			Dictionary<string, DatTranslation> userDatTranslations;
			try
			{
				userDatTranslations = ReadTranslationData();
			}
			catch (Exception ex)
			{
				OutputLine(string.Format(Settings.Strings["ReloadAllData_Failed"], ex.Message));
				return;
			}

			if (userDatTranslations == null)
			{
				return;
			}

			foreach (var userTranslation in userDatTranslations)
			{
				if (!AllDatTranslations.ContainsKey(userTranslation.Key))
				{
					AllDatTranslations.Add(userTranslation.Key, new DatTranslation());
				}

				DatTranslation currentDatTranslation = AllDatTranslations[userTranslation.Key];

				if (AllDatTranslations[userTranslation.Key].Translations == null)
					continue;

				// Mapping of originalText -> Translation pairs to determine if the user translation is already applied, not yet applied, or no longer valid
				Dictionary<string, Translation> translationsByOriginalHash = AllDatTranslations[userTranslation.Key].Translations.ToDictionary(k => k.OriginalText);

				foreach (var translation in userTranslation.Value.Translations)
				{
					if (translationsByOriginalHash.ContainsKey(translation.OriginalText))
					{
						translation.Status = Translation.TranslationStatus.NeedToApply;

						translationsByOriginalHash[translation.OriginalText].Status = translation.Status;
						translationsByOriginalHash[translation.OriginalText].TranslatedText = translation.TranslatedText;
						translationsByOriginalHash[translation.OriginalText].CurrentText = translation.OriginalText;
					}
					else if (translationsByOriginalHash.ContainsKey(translation.TranslatedText))
					{
						translation.Status = Translation.TranslationStatus.AlreadyApplied;

						translationsByOriginalHash[translation.TranslatedText].Status = translation.Status;
						translationsByOriginalHash[translation.TranslatedText].TranslatedText = translation.TranslatedText;
						translationsByOriginalHash[translation.TranslatedText].CurrentText = translation.TranslatedText;
						translationsByOriginalHash[translation.TranslatedText].OriginalText = translation.OriginalText;
					}
					else
					{
						translation.Status = Translation.TranslationStatus.Invalid;
						currentDatTranslation.Translations.Add(translation);
					}
				}
			}
		}

		/// <summary>
		/// Applies translations to content.ggpk
		/// </summary>
		public void ApplyTranslations()
		{
			StringBuilder outputBuffer = new StringBuilder();

			foreach (var datTranslation in AllDatTranslations)
			{
				// Map of originalText -> Translation containing all translations to apply
				Dictionary<string, Translation> translationsToApply = (from n in datTranslation.Value.Translations
																	   where n.Status == Translation.TranslationStatus.NeedToApply
																	   select n).ToDictionary(k => k.OriginalText);
				if (translationsToApply.Count == 0)
				{
					continue;
				}

				// Record we will be translating with data from translationTable
				FileRecord datRecord = fileRecordMap[datTranslation.Value.DatName];

				// Raw bytes of the .dat file we will be translating
				byte[] datBytes = datRecord.ReadData(ggpkPath);

				// Dat parser for changing the actual strings
				DatContainer dc = new DatContainer(new MemoryStream(datBytes), datTranslation.Value.DatName);

				// Replace the actual strings
				foreach (var item in dc.DataEntries)
				{
					UnicodeString currentDatString = (item.Value as UnicodeString);
					if (currentDatString == null || !currentDatString.IsUserString)
					{
						continue;
					}

					if (!translationsToApply.ContainsKey(currentDatString.Data))
					{
						continue;
					}

					Translation translationBeingApplied = translationsToApply[currentDatString.Data];
					currentDatString.NewData = translationBeingApplied.TranslatedText;

					outputBuffer.AppendLine(string.Format(Settings.Strings["ApplyTranslations_TextReplaced"], translationBeingApplied.ShortNameCurrent, translationBeingApplied.ShortNameTranslated));
					translationBeingApplied.Status = Translation.TranslationStatus.AlreadyApplied;
				}

				// dc.GetBytes() will return the new data for this .dat file after replacing the original strings with whatever's in 'NewData'
				datRecord.ReplaceContents(ggpkPath, dc.GetBytes(), content.FreeRoot);
			}

			if (outputBuffer.Length > 0)
			{
				Output(outputBuffer.ToString());
			}
		}

		/// <summary>
		/// Searches all of the /data/*.dat files in content.ggpk for user strings that can be translated. Also fills
		/// out 'fileRecordMap' with valid datName -> FileRecord mappings.
		/// </summary>
		private void CollectTranslatableStrings()
		{
			AllDatTranslations = new Dictionary<string, DatTranslation>();
			fileRecordMap = new Dictionary<string, FileRecord>();

			foreach (var recordOffset in content.RecordOffsets)
			{
				FileRecord record = recordOffset.Value as FileRecord;

				if (record == null || record.ContainingDirectory == null || Path.GetExtension(record.Name) != ".dat")
				{
					continue;
				}

				// Make sure parser for .dat type actually exists
				if (!RecordFactory.HasParser(record.Name))
				{
					continue;
				}

				// We'll need this .dat FileRecord later on so we're storing it in a map of fileName -> FileRecord
				fileRecordMap.Add(record.Name, record);

				List<string> translatableStrings;

				try
				{
					translatableStrings = GetTranslatableStringsFromDatFile(record);
				}
				catch (Exception ex)
				{
					OutputLine(string.Format(Settings.Strings["CollectTranslatableStrings_FailedReading"], record.Name, ex.Message));
					continue;
				}

				DatTranslation newDatTranslation = new DatTranslation();
				newDatTranslation.DatName = record.Name;
				newDatTranslation.Translations = new List<Translation>();

				foreach (string str in translatableStrings)
				{
					newDatTranslation.Translations.Add(new Translation(str));
				}

				if (translatableStrings.Count > 0)
				{
					AllDatTranslations.Add(record.Name, newDatTranslation);
				}
			}
		}

		/// <summary>
		/// Gets a list of all translatable strings in specified record. Record must be a FileRecord of a valid dat file.
		/// </summary>
		/// <param name="record">Dat File Record to extract translatable strings from</param>
		/// <returns>List of translatable strings contained in specified dat file</returns>
		private List<string> GetTranslatableStringsFromDatFile(FileRecord record)
		{
			// Map of all strings that can be safely translated (not used as ID's, paths, etc) stored by their hash
			HashSet<string> currentStrings = new HashSet<string>();

			byte[] datBytes = record.ReadData(ggpkPath);
			using (MemoryStream datStream = new MemoryStream(datBytes))
			{
				DatContainer container = new DatContainer(datStream, record.Name);

				// Any properties with the UserStringIndex attribute are translatable
				foreach (var fieldInfo in container.recordInfo.Fields)
				{
					if (!fieldInfo.IsUserString() )
					{
						continue;
					}

                    // get fielda value for each record
                    int index = fieldInfo.Index;
					foreach (var r in container.Records)
					{

                        int stringIndex = (int)container.GetFieldValue(r, index);
						string stringValue = container.DataEntries[stringIndex].ToString();

						if (string.IsNullOrWhiteSpace(stringValue))
						{
							continue;
						}

						if (!currentStrings.Contains(stringValue))
						{
							currentStrings.Add(stringValue);
						}
					}
				}
			}

			return currentStrings.ToList();
		}

		/// <summary>
		/// Saves all translations to file
		/// </summary>
		public void SaveTranslationData()
		{
			int debugTranslationCount = 0;

			List<DatTranslation> datTranslations = new List<DatTranslation>();
			XmlSerializer serializer = new XmlSerializer(datTranslations.GetType());
			using (FileStream fs = new FileStream(settingsPath, FileMode.Create))
			{
				foreach (var datTranslationTable in AllDatTranslations)
				{
					if (datTranslationTable.Value.Translations == null || datTranslationTable.Value.Translations.Count == 0)
						continue;

					DatTranslation newDatTranslation = new DatTranslation()
					{
						DatName = datTranslationTable.Key,
						Translations = (from n in datTranslationTable.Value.Translations
										where n.Status != Translation.TranslationStatus.Ignore
										select
											new Translation()
											{
												TranslatedText = n.TranslatedText.Replace(Environment.NewLine, "__BREAK__"),
												OriginalText = n.OriginalText.Replace(Environment.NewLine, "__BREAK__"),
											}).ToList()
					};

					if (newDatTranslation.Translations.Count > 0)
					{
						datTranslations.Add(newDatTranslation);
						debugTranslationCount += newDatTranslation.Translations.Count;
					}
				}

				serializer.Serialize(fs, datTranslations);
				OutputLine(String.Format(Settings.Strings["SaveTranslationData_Successful"], debugTranslationCount, settingsPath));
			}
		}

		/// <summary>
		/// Reads user translations from file
		/// </summary>
		/// <returns></returns>
		private Dictionary<string, DatTranslation> ReadTranslationData()
		{
			int debugTranslationCount = 0;

			Dictionary<string, DatTranslation> newUserTranslations = new Dictionary<string, DatTranslation>();

			XmlSerializer serializer = new XmlSerializer(typeof(List<DatTranslation>));
			List<DatTranslation> deserializedTranslations = (List<DatTranslation>)serializer.Deserialize(XmlReader.Create(settingsPath));

			foreach (var datTranslationTable in deserializedTranslations)
			{
				newUserTranslations.Add(datTranslationTable.DatName, datTranslationTable);

				foreach (var translation in datTranslationTable.Translations)
				{
					++debugTranslationCount;
					translation.TranslatedText = translation.TranslatedText.Replace("__BREAK__", Environment.NewLine);
					translation.OriginalText = translation.OriginalText.Replace("__BREAK__", Environment.NewLine);
					translation.Status = Translation.TranslationStatus.Invalid;
				}
			}

			OutputLine(string.Format(Settings.Strings["ReadTranslationData_Successful"], debugTranslationCount));
			return newUserTranslations;
		}

		private void Output(string text)
		{
			if (outputFunc != null)
				outputFunc(text);
		}

		private void OutputLine(string text)
		{
			Output(text + Environment.NewLine);
		}
	}
}
