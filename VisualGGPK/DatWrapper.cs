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
        private readonly string _datName;
        private readonly List<UnicodeString> _dataStrings = new List<UnicodeString>();

        private DatContainer _dat;

        public RecordInfo RecordInfo
        {
            get { return _dat.RecordInfo; }
        }

        public long DataSectionffset
        {
            get { return _dat.DataSectionOffset; }
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
            get { return _dat.DataEntries; }
        }

        public List<UnicodeString> Strings
        {
            get { return _dataStrings; }
        }

        public DatWrapper(string fileName)
        {
            _datName = Path.GetFileNameWithoutExtension(fileName);

            var fileBytes = File.ReadAllBytes(fileName);

            using (var ms = new MemoryStream(fileBytes))
            {
                ParseDatFile(ms);
            }
        }

        public DatWrapper(Stream inStream, string fileName)
        {
            _datName = Path.GetFileNameWithoutExtension(fileName);
            ParseDatFile(inStream);
        }


        private void ParseDatFile(Stream inStream)
        {
            _dat = new DatContainer(inStream, _datName);

            try
            {
                var containerData = DataEntries.ToList();

                foreach (var keyValuePair in containerData)
                {
                    var data = keyValuePair.Value;
                    var s = data as UnicodeString;
                    if (s != null)
                    {
                        Strings.Add(s);
                        continue;
                    }

                    var int64 = data as ListUInt64;
                    if (int64 != null)
                    {
                        Strings.Add(
                            new UnicodeString(int64.Offset, int64.DataTableOffset,
                                int64.GetValueString(), int64.ListLength * 8));
                        continue;
                    }

                    var uInt32 = data as ListUInt32;
                    if (uInt32 != null)
                    {
                        Strings.Add(
                            new UnicodeString(uInt32.Offset, uInt32.DataTableOffset,
                                uInt32.GetValueString(), uInt32.ListLength * 4));
                        continue;
                    }

                    var int32 = data as ListInt32;
                    if (int32 != null)
                    {
                        var ul = int32;
                        Strings.Add(
                            new UnicodeString(int32.Offset, int32.DataTableOffset,
                                int32.GetValueString(), int32.ListLength * 4));
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
            return _dat.GetCSV();
        }
    }
}
