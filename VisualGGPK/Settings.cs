using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using VisualGGPK.Properties;

namespace VisualGGPK
{
	public class Settings
	{
		public static readonly Settings Strings = new Settings();

		private static readonly string SettingsPath = typeof(Settings).Namespace + "." + "i18n.xml";
		private static readonly Dictionary<string, string> UserStrings = new Dictionary<string, string>();

		[Serializable]
		[XmlType("Resource")]
		public class UserData
		{
			[XmlAttribute]
			public string Tag { get; set; }
			[XmlElement]
			public string Text { get; set; }
		}

		public string this[string tag]
		{
			get
			{
				if (UserStrings.ContainsKey(tag))
				{
					return UserStrings[tag];
				}

				return Resources.ResourceManager.GetString(tag);
			}
		}

		static Settings()
		{
			if (File.Exists(SettingsPath))
			{
				UserStrings = ReadTranslationData();
			}

			CreateExampleTranlsationFile();
		}

		private static Dictionary<string, string> ReadTranslationData()
		{
			List<UserData> userStringList = new List<UserData>();

			XmlSerializer serializer = new XmlSerializer(userStringList.GetType());
			userStringList = (List<UserData>)serializer.Deserialize(XmlReader.Create(SettingsPath));
			return userStringList.ToDictionary(n => n.Tag, n => n.Text.Replace("__BREAK__", Environment.NewLine));
		}

		private static void CreateExampleTranlsationFile()
		{
			Dictionary<string, string> derp = new Dictionary<string, string>();

			foreach (var propertyInfo in typeof(Resources).GetProperties())
			{
				object propValue = propertyInfo.GetValue(null, null);

				if (propValue is string)
				{
					derp.Add(propertyInfo.Name, (string)propValue);
				}
			}

			List<UserData> userStringList = (from n in derp select new UserData() { Tag = n.Key, Text = n.Value.Replace(Environment.NewLine, "__BREAK__") }).ToList();
			XmlSerializer serializer = new XmlSerializer(userStringList.GetType());

			using (FileStream fs = new FileStream(SettingsPath + ".Example", FileMode.Create))
			{
				serializer.Serialize(fs, userStringList);
			}
		}
	}
}
