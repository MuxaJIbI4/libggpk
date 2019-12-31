using LibDat;
using LibDat.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace VisualGGPK
{
    public class DatString
    {
        private readonly AbstractData _data;
        private readonly bool _isUser;

        public string DataType { get { return _data.GetType().ToString().Replace("LibDat.Data.", ""); } }

        public int OffsetStart { get { return _data.Offset; } }

        public int OffsetEnd { get { return _data.Offset + _data.Length; } }

        public int Length { get { return _data.Length; } }

        public bool IsUser { get { return _isUser; } }

        public string NewValue
        {
            get
            {
                var str = _data as StringData;
                return (str == null ? "" : str.NewValue);
            }
            set
            {
                var str = _data as StringData;
                if (str != null)
                    str.NewValue = value;
            }
        }

        public string Value { get { return _data.GetValueString(); } }

        public DatString(AbstractData data, bool isUser)
        {
            _data = data;
            _isUser = isUser;
        }
    }

    public class DatWrapper
    {
        private readonly string _datName;


        private DatContainer _dat;

        public RecordInfo RecordInfo
        {
            get { return _dat.RecordInfo; }
        }

        public long DataSectionffset
        {
            get { return DatContainer.DataSectionOffset; }
        }

        public long DataSectionDataLength
        {
            get { return _dat.DataSectionDataLength; }

        }
        public List<RecordData> Records
        {
            get { return _dat.Records; }
        }
        public Dictionary<int, AbstractData> DataEntries
        {
            get { return DatContainer.DataEntries; }
        }

        private readonly List<DatString> _dataStrings = new List<DatString>();
        public List<DatString> Strings
        {
            get { return _dataStrings; }
        }

        public DatWrapper(string fileName)
        {
            if (Path.GetExtension(fileName) == ".dat64") _datName = Path.GetFileName(fileName);
            else _datName = Path.GetFileNameWithoutExtension(fileName);
            var fileBytes = File.ReadAllBytes(fileName);

            using (var ms = new MemoryStream(fileBytes))
            {
                ParseDatFile(ms);
            }
        }

        public DatWrapper(Stream inStream, string fileName)
        {
            if (Path.GetExtension(fileName) == ".dat64") _datName = Path.GetFileName(fileName);
            else _datName = Path.GetFileNameWithoutExtension(fileName);
            ParseDatFile(inStream);
        }

        private void ParseDatFile(Stream inStream)
        {
            try
            {
                _dat = new DatContainer(inStream, _datName);

                var containerData = DataEntries.ToList();
                var userStringOffsets = _dat.GetUserStringOffsets();
                foreach (var keyValuePair in containerData)
                {
                    var data = keyValuePair.Value;
                    var isUser = userStringOffsets.Contains(keyValuePair.Key);
                    Strings.Add(new DatString(data, isUser));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(Settings.Strings["DatWrapper_ParseDatFile_Failed"], ex.ToString()), ex);
            }
        }

        public void Save(string savePath)
        {
            try
            {
                _dat.Save(savePath);
            }
            catch (Exception ex)
            {
                var errorString = new StringBuilder();

                var temp = ex;
                while (temp != null)
                {
                    errorString.AppendLine(temp.Message);
                    temp = temp.InnerException;
                }

                MessageBox.Show(string.Format(Settings.Strings["DatWrapper_Save_Failed"], errorString), Settings.Strings["Error_Caption"], MessageBoxButton.OK, MessageBoxImage.Error);
            }

            MessageBox.Show(string.Format(Settings.Strings["DatWrapper_Save_Successful"], savePath), Settings.Strings["DatWrapper_Save_Successful_Caption"], MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public string GetCSV()
        {
            return _dat.GetCsvFormat();
        }
    }
}