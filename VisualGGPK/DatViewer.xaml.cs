using LibDat;
using LibDat.Data;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using VisualGGPK.Properties;
using System.Windows.Data;
using System.Collections;

namespace VisualGGPK
{
    /// <summary>
    /// Interaction logic for DatViewer.xaml
    /// </summary>
    public partial class DatViewer : UserControl
    {
        private DatWrapper _wrapper;
        public string FileName { get; set; }
        public List<UnicodeString> DataStrings
        {
            get
            {
                if (_wrapper != null)
                    return _wrapper.Strings;
                else
                    return new List<UnicodeString>();
            }
        }

        public System.Collections.IEnumerable Records
        {
            get
            {
                return _wrapper.Records;
            }
        }

        public DatViewer(string filename, Stream inStream)
        {
            this.FileName = filename;
            _wrapper = new DatWrapper(inStream, filename);
            InitializeComponent();
            DataContext = this;

            buttonSave.Content = Settings.Strings["DatViewer_Button_Save"];
            buttonExportCSV.Content = Settings.Strings["DatViewer_Button_Export"];
        }

        public DatViewer(string filename)
        {
            this.FileName = filename;
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

            buttonSave.Content = Settings.Strings["DatViewer_Button_Save"];
            buttonExportCSV.Content = Settings.Strings["DatViewer_Button_ExportCSV"];
        }

        public void Reset(string filename, Stream inStream)
        {
            this.FileName = filename;
            _wrapper = new DatWrapper(inStream, filename);
            DataContext = null;
            dataGridRecords.ItemsSource = null;
            dataGridRecords.Columns.Clear();

            // 
            if (_wrapper.Records.Count <= 0)
            {
                return;
            }

            BuildGrid(_wrapper.RecordInfo);

            DataContext = this;
        }

        private void SaveDat()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = ".dat";
            sfd.FileName = Path.GetFileNameWithoutExtension(FileName) + "_NEW.dat";

            if (sfd.ShowDialog() == true)
            {
                _wrapper.Save(sfd.FileName);
            }
        }

        private void ExportCSV()
        {
            SaveFileDialog sfd = new SaveFileDialog();
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

        private void BuildGrid(DatRecordInfo recordInfo)
        {
            // first column: row index
            DataGridTextColumn col_first = new DataGridTextColumn();
            col_first.Header = "Row";
            col_first.Binding = new Binding("column_0");
            dataGridRecords.Columns.Add(col_first);

            // field columns 
            int i = 1;
            foreach (var field in recordInfo.Fields)
            {
                DataGridTextColumn col = new DataGridTextColumn();
                col.Header = field.ToString("\n");
                col.Binding = new Binding("column_" + i++);
                dataGridRecords.Columns.Add(col);
            }
            dataGridRecords.ItemsSource = GenerateData(Records, recordInfo.Fields.Count).ToDataSource();
        }

        public IEnumerable<IDictionary> GenerateData(IEnumerable records, int columnsCount)
        {
            IEnumerator iter = records.GetEnumerator();
            int row_index = 0;
            while (iter.MoveNext())
            {
                DatRecord record = (DatRecord)iter.Current;
                var dict = new Dictionary<string, object>();
                dict["column_0"] = row_index;
                for (int i = 0; i < columnsCount; i++)
                {
                    DatRecordFieldInfo field = record.RecordInfo.Fields[i];
                    if ( field.HasPointer )
                    {
                        int offset = (int)record.GetFieldValue(i);
                        if (_wrapper.DataEntries.ContainsKey(offset))
                        {
                            dict["column_" + (i+1) ] = "*" + offset + " = " + _wrapper.DataEntries[offset].ToString();
                        }
                        else
                        {
                            dict["column_" + (i + 1)] = record.GetFieldValue(i);
                        }
                    }
                    else
                    {
                        dict["column_" + (i + 1)] = record.GetFieldValue(i);
                    }
                }
                row_index++;
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
    }
}
