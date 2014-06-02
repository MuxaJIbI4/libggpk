using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace PoeStrings
{
	[Serializable]
	public class Translation : INotifyPropertyChanged
	{
		private const int TruncationLength = 25;

		public enum TranslationStatus
		{
			Invalid,
			Ignore,
			NeedToApply,
			AlreadyApplied,
		}

		private static string TruncateString(string source, int truncationLength)
		{
			if (source == null)
				return string.Empty;
			return (source.Length < truncationLength ? source : source.Substring(0, truncationLength - 3) + "...").Replace(Environment.NewLine, "");
		}


		[XmlIgnore]
		public string ShortNameOriginal
		{
			get
			{
				if (_shortNameOriginal == null)
					_shortNameOriginal = TruncateString(OriginalText, TruncationLength);

				return _shortNameOriginal;
			}
		}
		private string _shortNameOriginal;

		[XmlIgnore]
		public string ShortNameCurrent
		{
			get
			{
				if (_shortNameCurrent == null)
					_shortNameCurrent = TruncateString(CurrentText, TruncationLength);

				return _shortNameCurrent;
			}
		}
		private string _shortNameCurrent;

		[XmlIgnore]
		public string ShortNameTranslated
		{
			get
			{
				if (_shortNameTranslated == null)
					_shortNameTranslated = TruncateString(TranslatedText, TruncationLength);

				return _shortNameTranslated;
			}
		}
		private string _shortNameTranslated;

		[XmlIgnore]
		public string Colour
		{
			get
			{
				switch (Status)
				{
					case TranslationStatus.Invalid:
						return "Red";
					case TranslationStatus.NeedToApply:
						return "LimeGreen";
					case TranslationStatus.AlreadyApplied:
						return "Goldenrod";
					case TranslationStatus.Ignore:
						return "Gray";
				}
				throw new ArgumentOutOfRangeException("Colour.get: Invalid Status of translation");
			}
		}

		[XmlIgnore]
		public string FontStyle
		{
			get
			{
				if (Status == TranslationStatus.Ignore)
					return "Normal";
				return "Bold";
			}
		}


		[XmlIgnore]
		public string CurrentText
		{
			get
			{
				return _currentText;
			}
			set
			{
				_currentText = value;
			}
		}
		private string _currentText;

		[XmlIgnore]
		public TranslationStatus Status
		{
			get
			{
				return _status;
			}
			set
			{
				_status = value;
				OnPropertyChange("Colour");
				OnPropertyChange("Status");
			}
		}
		private TranslationStatus _status;


		public string OriginalText
		{
			get
			{
				return _originalText;
			}
			set
			{
				_originalText = value;
			}
		}
		private string _originalText;

		public string TranslatedText
		{
			get
			{
				return _translatedText;
			}
			set
			{
				_translatedText = value;
				if (string.IsNullOrEmpty(_translatedText))
				{
					Status = TranslationStatus.Ignore;
				}
				else if (_translatedText != string.Empty && CurrentText != _translatedText)
				{
					Status = TranslationStatus.NeedToApply;
				}
			}
		}
		private string _translatedText;


		public Translation()
		{
			
		}
		public Translation(string originalString)
		{
			this.OriginalText = originalString;
			this.CurrentText = originalString;
			this.TranslatedText = null;
			this.Status = TranslationStatus.Ignore;
		}

		public override string ToString()
		{
			return CurrentText;
		}

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion
	}
}