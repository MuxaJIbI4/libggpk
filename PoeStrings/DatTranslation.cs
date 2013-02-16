using System;
using System.Collections.Generic;

namespace PoeStrings
{
	[Serializable]
	public class DatTranslation
	{
		public DatTranslation()
		{

		}
		public DatTranslation(string datName)
		{
			this.DatName = datName;
			Translations = new List<Translation>();
		}

		public string DatName { get; set; }
		public List<Translation> Translations { get; set; }
	}
}