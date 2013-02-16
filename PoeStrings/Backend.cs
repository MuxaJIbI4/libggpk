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
using LibGGPK;
using PoeStrings.Properties;

namespace PoeStrings
{
	public class Backend
	{
		private const string SettingsPath = ".\\translation.xml";
		private readonly Action<string> outputFunc;


		private GGPK content = new GGPK();
		private string ggpkPath;
		private Dictionary<string, DatTranslation> userTranslations;
		private Dictionary<string, Dictionary<uint, string>> allStrings;
		private Dictionary<string, FileRecord> fileRecordMap;


		public Dictionary<string, DatTranslation> UserTranslations
		{
			get
			{
				return userTranslations;
			}
		}

		public Backend(Action<string> outputFunc)
		{
			this.outputFunc = outputFunc;
		}


		public void ReloadAllData(string ggpkPath)
		{
			this.ggpkPath = ggpkPath;
			content = new GGPK();
			content.Read(ggpkPath, outputFunc);

			try
			{
				userTranslations = ReadTranslationData();
			}
			catch (Exception ex)
			{
				OutputLine(string.Format(Settings.Strings["ReloadAllData_Failed"], ex.Message));
			}

			allStrings = GetTranslatableDatStrings(content);

			UpdateUserTranslations();
		}

		private void UpdateUserTranslations()
		{
			if (userTranslations == null)
			{
				userTranslations = new Dictionary<string, DatTranslation>();
			}

			// Invalidate all user translations. After we go through all of the current .dat strings any translations
			//   with an 'Invalid' status are no longer present possibly due to a patch changing the original string.
			foreach (var userTranslation in userTranslations)
			{
				foreach (var translation in userTranslation.Value.Translations)
				{
					translation.CurrentText = translation.OriginalText;
					translation.Status = Translation.TranslationStatus.Invalid;
				}
			}

			//foreach (var datTranslation in userTranslations)
			foreach (var allStringKvp in allStrings)
			{
				if (allStringKvp.Value.Count == 0)
				{
					continue;
				}

				if (!userTranslations.ContainsKey(allStringKvp.Key))
				{
					userTranslations.Add(allStringKvp.Key, new DatTranslation(allStringKvp.Key));
				}

				DatTranslation translationTable = userTranslations[allStringKvp.Key];
				Dictionary<uint, string> allStringsForThisDat = allStrings[allStringKvp.Key];

				Dictionary<uint, Translation> userTranslationsByOriginalHash = translationTable.Translations.ToDictionary(n => n.OriginalHash);
				Dictionary<uint, Translation> userTranslationsByTranslationHash = translationTable.Translations.ToDictionary(n => n.TranslationHash);

				// Map of hash->string pairs of strings we will need to replace in the target dat file

				// Lots of text can be generated ahead, just write to buffer then dump the buffer to the output function
				StringBuilder outputTextBuffer = new StringBuilder();
				foreach (var datStringKvp in allStringsForThisDat)
				{
					uint datStringHash = datStringKvp.Key;

					if (userTranslationsByOriginalHash.ContainsKey(datStringHash))
					{
						// Original string still exists in dat file, we need to replace it with our translation text
						userTranslationsByOriginalHash[datStringHash].CurrentText = datStringKvp.Value;
						userTranslationsByOriginalHash[datStringHash].Status = Translation.TranslationStatus.NeedToApply;
					}
					else if (userTranslationsByTranslationHash.ContainsKey(datStringHash))
					{
						// Original string has already been replaced by our translation text.
						userTranslationsByTranslationHash[datStringHash].CurrentText = datStringKvp.Value;
						userTranslationsByTranslationHash[datStringHash].Status = Translation.TranslationStatus.AlreadyApplied;
					}
					else
					{
						// New string found, either a previously ignored string or a string modified by a recent patch
						Translation newTranslation = new Translation()
						{
							CurrentText = datStringKvp.Value,
							OriginalText = datStringKvp.Value,
							TranslatedText = string.Empty,
							Status = Translation.TranslationStatus.Ignore,
						};

						translationTable.Translations.Add(newTranslation);
						userTranslationsByOriginalHash.Add(datStringHash, newTranslation);
						userTranslationsByTranslationHash.Add(datStringHash, newTranslation);
					}
				}
			}
		}

		public void ApplyAllTranslations()
		{
			ApplyTranslations(false);
		}

		public void RevertAllTranslations()
		{
			ApplyTranslations(true);
		}

		/// <summary>
		/// Applies translations to content.ggpk
		/// </summary>
		private void ApplyTranslations(bool isRevertingTranslation)
		{
			SaveTranslationData();

			StringBuilder outputBuffer = new StringBuilder();

			foreach (var userTranslation in userTranslations)
			{
				// Record we will be translating with data from translationTable
				FileRecord datRecord = fileRecordMap[userTranslation.Value.DatName];

				// Raw bytes of the .dat file we will be translating
				byte[] datBytes = datRecord.ReadData(ggpkPath);

				// Dat parser for changing the actual strings
				DatContainer dc = new DatContainer(new MemoryStream(datBytes), userTranslation.Value.DatName);

				foreach (var translation in userTranslation.Value.Translations)
				{
					if (isRevertingTranslation)
					{
						if (translation.Status != Translation.TranslationStatus.AlreadyApplied)
							continue;
					}
					else
					{
						if (translation.Status != Translation.TranslationStatus.NeedToApply)
							continue;
					}

					bool wasTranslationApplied = false;

					// Replace the actual strings
					foreach (var item in dc.DataEntries)
					{
						if (!(item.Value is UnicodeString))
							continue;

						UnicodeString currentDatString = (item.Value as UnicodeString);
						if (!currentDatString.IsUserString)
							continue;

						uint currentDatStringHash = Hash.MurmurHash2(currentDatString.Data);

						if (isRevertingTranslation)
						{
							if (translation.TranslationHash != currentDatStringHash)
								continue;

							outputBuffer.AppendLine(string.Format(Settings.Strings["ApplyTranslations_TextReplaced"], translation.ShortNameTranslated, translation.ShortNameOriginal));
							currentDatString.NewData = translation.OriginalText;
							translation.Status = Translation.TranslationStatus.NeedToApply;
							wasTranslationApplied = true;
						}
						else
						{
							if (translation.CurrentHash != currentDatStringHash)
								continue;

							outputBuffer.AppendLine(string.Format(Settings.Strings["ApplyTranslations_TextReplaced"], translation.ShortNameCurrent, translation.ShortNameTranslated));
							currentDatString.NewData = translation.TranslatedText;
							translation.Status = Translation.TranslationStatus.AlreadyApplied;
							wasTranslationApplied = true;
						}
					}

					if (wasTranslationApplied)
					{
						translation.CurrentText = translation.OriginalText;
					}
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
			int debugItemsRead = 0;

			var translatableDatStrings = new Dictionary<string, Dictionary<uint, string>>();
			fileRecordMap = new Dictionary<string, FileRecord>();

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
					catch (Exception)
					{
						continue;
					}

					// Any properties with the UserStringIndex attribute are translatable
					foreach (var propInfo in container.DatType.GetProperties())
					{
						if (!propInfo.GetCustomAttributes(false).Any(n => n is UserStringIndex)) 
							continue;

						foreach (var entry in container.Entries)
						{
							int stringIndex = (int)propInfo.GetValue(entry, null);
							string stringValue = container.DataEntries[stringIndex].ToString();

							if (string.IsNullOrWhiteSpace(stringValue))
								continue;

							uint hash = Hash.MurmurHash2(stringValue);
							if (translatableStringMap.ContainsKey(hash))
								continue;

							++debugItemsRead;
							translatableStringMap.Add(hash, stringValue);
						}
					}
				}

				translatableDatStrings.Add(record.Name, translatableStringMap);
			}

			OutputLine(string.Format(Settings.Strings["GetTranslatableDatStrings_TotalStrings"], debugItemsRead));
			return translatableDatStrings;
		}

		public void SaveTranslationData()
		{
			int debugTranslationCount = 0;

			List<DatTranslation> datTranslations = new List<DatTranslation>();
			XmlSerializer serializer = new XmlSerializer(datTranslations.GetType());
			using (FileStream fs = new FileStream(SettingsPath, FileMode.Create))
			{
				foreach (var datTranslationTable in userTranslations)
				{
					if (datTranslationTable.Value.Translations.Count == 0)
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
				OutputLine(String.Format(Settings.Strings["SaveTranslationData_Successful"], debugTranslationCount, SettingsPath));
			}
		}

		private Dictionary<string, DatTranslation> ReadTranslationData()
		{
			int debugTranslationCount = 0;

			Dictionary<string, DatTranslation> newUserTranslations = new Dictionary<string, DatTranslation>();

			XmlSerializer serializer = new XmlSerializer(typeof(List<DatTranslation>));
			List<DatTranslation> deserializedTranslations = (List<DatTranslation>)serializer.Deserialize(XmlReader.Create(SettingsPath));

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
