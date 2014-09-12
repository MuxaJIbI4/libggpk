using LibDat;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections;

namespace VisualGGPK
{
    /// <summary>
    /// Interaction logic for DatViewer.xaml
    /// </summary>
    public partial class DatViewer
    {
        private DatWrapper _wrapper;
        private byte[] _data;
        private bool _showPointerDataValue;

        private string FileName { get; set; }

        public List<DatString> DataStrings
        {
            get { return _wrapper == null ? new List<DatString>() : _wrapper.Strings; }
        }

        private IEnumerable Records
        {
            get { return _wrapper.Records; }
        }

        public DatViewer(string filename, Stream inStream)
        {
            FileName = filename;
            _showPointerDataValue = false;
            _wrapper = new DatWrapper(inStream, filename);
            InitializeComponent();
            DataContext = this;


            buttonSave.Content = Settings.Strings["DatViewer_Button_Save"];
            buttonExportCSV.Content = Settings.Strings["DatViewer_Button_Export"];
        }

        public DatViewer(string filename)
        {
            FileName = filename;
            _showPointerDataValue = false;
            _wrapper = new DatWrapper(filename);
            InitializeComponent();
            DataContext = this;

            buttonSave.Content = Settings.Strings["DatViewer_Button_Save"];
            buttonExportCSV.Content = Settings.Strings["DatViewer_Button_Export"];
        }

        public DatViewer()
        {
            InitializeComponent();
            DataContext = this;
            _showPointerDataValue = false;

            buttonSave.Content = Settings.Strings["DatViewer_Button_Save"];
            buttonExportCSV.Content = Settings.Strings["DatViewer_Button_ExportCSV"];
        }

        public void Reset(string filename, byte[] data)
        {
            FileName = filename;
            _data = data;

            using (var ms = new MemoryStream(data))
            {
                _wrapper = new DatWrapper(ms, filename);
            }

            DataContext = null;
            datInfo.Content =
                filename + " info: " +
                "\n\tRecord length (bytes)      = " + _wrapper.RecordInfo.Length +
                "\n\tNumber of records          = " + _wrapper.Records.Count +
                "\n\tData section offset        = " + _wrapper.DataSectionffset +
                "\n\tLenth of data section data = " + _wrapper.DataSectionDataLength;

            // Records DataGrid
            BuildGrid();
            DataContext = this;
        }

        private void BuildGrid()
        {
            dataGridRecords.ItemsSource = null;
            dataGridRecords.Columns.Clear();
            if (_wrapper.Records.Count <= 0)
                return;

            var recordInfo = _wrapper.RecordInfo;

            // first column: row index
            var colFirst = new DataGridTextColumn { Header = "Row", Binding = new Binding("column_0") };
            dataGridRecords.Columns.Add(colFirst);

            // field columns 
            var i = 1;
            foreach (var fieldInfo in recordInfo.Fields)
            {
                var col = new DataGridTextColumn();
                col.Header = fieldInfo.ToString("\n");
                col.Binding = new Binding("column_" + i++);
                dataGridRecords.Columns.Add(col);
            }
            dataGridRecords.ItemsSource = GenerateData(Records, recordInfo.Fields.Count).ToDataSource();
        }

        private IEnumerable<IDictionary> GenerateData(IEnumerable records, int columnsCount)
        {
            var iter = records.GetEnumerator();
            var rowIndex = 0;
            while (iter.MoveNext())
            {
                var recordData = (RecordData)iter.Current;
                var dict = new Dictionary<string, object>();
                dict["column_0"] = rowIndex;
                for (var i = 0; i < columnsCount; i++)
                {
                    // TODO: add offset
                    var fieldData = recordData.FieldsData[i];
                    string str;
                    if (_showPointerDataValue && fieldData.FieldInfo.IsPointer)
                    {
                        var offset = (fieldData.ValueOffset == 0 ? fieldData.Offset : fieldData.ValueOffset);
                        var prefix = fieldData.GetOffsetPrefix();
                        //  TODO: now in case of PointerData prefix contains Offset instead of PointerOffset
                        var value = _wrapper.DataEntries.ContainsKey(offset)
                            ? _wrapper.DataEntries[offset].GetValueString()
                            : "(Error: Unknown Data)";
                        str = String.Format("{0} = {1}", prefix, value);
                    }
                    else
                    {
                        str = fieldData.FieldInfo.IsPointer
                            ? fieldData.GetOffsetPrefix()
                            : fieldData.Value.ToString();

                    }
                    dict["column_" + (i + 1)] = str;
                }
                rowIndex++;
                yield return dict;
            }
        }

        private void buttonSave_Click_1(object sender, RoutedEventArgs e)
        {
            SaveDat();
        }

        private void buttonExportCSV_Click_1(object sender, RoutedEventArgs e)
        {
            ExportCSV();
        }

        private void reload_XML_Click(object sender, RoutedEventArgs e)
        {
            // update records info
            RecordFactory.UpdateRecordsInfo();
            // reset files
            Reset(FileName, _data);
        }

        private void showPointerData_Checked(object sender, RoutedEventArgs e)
        {
            hadnle_showPointerData(sender as CheckBox);
        }

        private void showPointerData_Unchecked(object sender, RoutedEventArgs e)
        {
            hadnle_showPointerData(sender as CheckBox);
        }

        void hadnle_showPointerData(CheckBox checkBox)
        {
            // Use IsChecked.
            var flag = checkBox.IsChecked.Value;

            if (flag != _showPointerDataValue)
            {
                _showPointerDataValue = flag;
                BuildGrid();
                DataContext = this; // TODO test that it's shown at UI
            }
        }

        private void SaveDat()
        {
            var sfd = new SaveFileDialog();
            sfd.DefaultExt = ".dat";
            sfd.FileName = Path.GetFileNameWithoutExtension(FileName) + "_NEW.dat";

            if (sfd.ShowDialog() == true)
            {
                _wrapper.Save(sfd.FileName);
            }
        }

        private void ExportCSV()
        {
            var sfd = new SaveFileDialog();
            sfd.FileName = Path.GetFileNameWithoutExtension(FileName) + ".csv";

            if (sfd.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllText(sfd.FileName, _wrapper.GetCSV());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format(Settings.Strings["DatViewer_ExportCSV_Failed"], ex.Message), Settings.Strings["Error_Caption"], MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MessageBox.Show(string.Format(Settings.Strings["DatViewer_ExportCSV_Successful"], sfd.FileName), Settings.Strings["DatViewer_ExportCSV_Successful_Caption"], MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
