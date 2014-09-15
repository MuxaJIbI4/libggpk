using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace PoEGGPKPatcher
{
    public class MainViewModel : IDataErrorInfo, INotifyPropertyChanged
    {
        private int _largeFontSize;
        private int _normalFontSize;
        private int _quality;
        private int _smallFontSize;

        public MainViewModel()
        {
            Quality = 0;
            ButtonApplyQualityIsEnabled = true;
            SmallFontSize = 26;
            NormalFontSize = 33;
            LargeFontSize = 45;

            UniqueColor = Color.FromRgb(175, 96, 37);
            RareColor = Color.FromRgb(255, 255, 119);
            MagicColor = Color.FromRgb(136, 136, 255);
            GemColor = Color.FromRgb(27, 162, 155);
            CurrencyColor = Color.FromRgb(170, 158, 130);
        }

        public bool ButtonApplyQualityIsEnabled { get; set; }

        public bool ButtonApplyFontIsEnabled { get; set; }

        public Color UniqueColor { get; set; }

        public Color RareColor { get; set; }

        public Color MagicColor { get; set; }

        public Color GemColor { get; set; }

        public Color CurrencyColor { get; set; }

        public int Quality
        {
            get { return _quality; }
            set
            {
                _quality = value;
                ButtonApplyQualityIsEnabled = (value >= 0 && value <= 10);
                NotifyPropertyChanged("buttonApplyQualityIsEnabled");
            }
        }

        public int SmallFontSize
        {
            get { return _smallFontSize; }
            set
            {
                _smallFontSize = value;
                CheckApplyFontButton();
            }
        }

        public int NormalFontSize
        {
            get { return _normalFontSize; }
            set
            {
                _normalFontSize = value;
                CheckApplyFontButton();
            }
        }

        public int LargeFontSize
        {
            get { return _largeFontSize; }
            set
            {
                _largeFontSize = value;
                CheckApplyFontButton();
            }
        }

        public string this[string name]
        {
            get
            {
                string result = null;

                if (name == "Quality")
                {
                    if (_quality < 0 || _quality > 10)
                    {
                        result = "Quality must not be less than 0 or greater than 10.";
                    }
                }
                if (name == "SmallFontSize")
                {
                    if (_smallFontSize < 10 || _smallFontSize > 100)
                    {
                        result = "SmallFontSize = 10-100";
                    }
                }
                if (name == "NormalFontSize")
                {
                    if (_normalFontSize < 10 || _normalFontSize > 100)
                    {
                        result = "NormalFontSize = 10-100";
                    }
                }
                if (name == "LargeFontSize")
                {
                    if (_largeFontSize < 10 || _largeFontSize > 100)
                    {
                        result = "LargeFontSize = 10-100";
                    }
                }

                return result;
            }
        }

        public string Error
        {
            get { return null; }
        }

        private void CheckApplyFontButton()
        {
            ButtonApplyFontIsEnabled =
                _smallFontSize >= 10 && _smallFontSize <= 100 &&
                _normalFontSize >= 10 && _normalFontSize <= 100 &&
                _largeFontSize >= 10 && _largeFontSize <= 100;
            NotifyPropertyChanged("ButtonApplyFontIsEnabled");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property. 
        // The CallerMemberName attribute that is applied to the optional propertyName 
        // parameter causes the property name of the caller to be substituted as an argument. 
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}