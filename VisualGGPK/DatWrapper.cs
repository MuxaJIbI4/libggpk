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
    public class DatWrapper
    {
        private string fileName;
        private string datName;
        private readonly List<UnicodeString> _dataStrings = new List<UnicodeString>();

        private DatContainer _dat;

        public DatRecordInfo RecordInfo
        {
            get { return _dat.RecordInfo; }
        }

        public long DataSectionffset
        {
            get { return _dat.DataTableBegin; }
        }

        public List<DatRecord> Records
        {
            get { return _dat.Records; }
        }
        public Dictionary<int, AbstractData> DataEntries
        {
            get { return _dat.DataEntries; }
        }

        public List<UnicodeString> Strings
        {
            get
            {
                return _dataStrings;
            }
        }

        public DatWrapper(string fileName)
        {
            this.fileName = fileName;
            this.datName = Path.GetFileNameWithoutExtension(fileName);

            byte[] fileBytes = File.ReadAllBytes(fileName);

            using (MemoryStream ms = new MemoryStream(fileBytes))
            {
                ParseDatFile(ms);
            }
        }

        public DatWrapper(Stream inStream, string fileName)
        {
            this.fileName = fileName;
            this.datName = Path.GetFileNameWithoutExtension(fileName);
            ParseDatFile(inStream);
        }


        private void ParseDatFile(Stream inStream)
        {
            _dat = new DatContainer(inStream, datName);

            try
            {
                var containerData = DataEntries.ToList();

                foreach (var keyValuePair in containerData)
                {
                    if (keyValuePair.Value is UnicodeString)
                    {
                        Strings.Add((UnicodeString)keyValuePair.Value);
                    }
                    else if (keyValuePair.Value is UInt64List)
                    {
                        UInt64List ul = (UInt64List)keyValuePair.Value;
                        Strings.Add((UnicodeString)new UnicodeString(ul.Offset, ul.dataTableOffset, ul.ToString()));
                    }
                    else if (keyValuePair.Value is UInt32List)
                    {
                        UInt32List ul = (UInt32List)keyValuePair.Value;
                        Strings.Add((UnicodeString)new UnicodeString(ul.Offset, ul.dataTableOffset, ul.ToString()));
                    }
                    else if (keyValuePair.Value is Int32List)
                    {
                        Int32List ul = (Int32List)keyValuePair.Value;
                        Strings.Add((UnicodeString)new UnicodeString(ul.Offset, ul.dataTableOffset, ul.ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(Settings.Strings["DatWrapper_ParseDatFile_Failed"], ex.Message), ex);
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
                StringBuilder errorString = new StringBuilder();

                Exception temp = ex;
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
            return _dat.GetCSV();
        }
    }
}
