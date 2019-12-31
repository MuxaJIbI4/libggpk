using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using LibDat;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace DatConverter
{
    public partial class MainWindow : INotifyPropertyChanged
    {
        private Dictionary<string, int> _lengths;
        private Dictionary<string, int> _counts;

        private bool _buttonSelectFolderEnnabled;
        public bool ButtonSelectFolderEnnabled
        {
            get { return _buttonSelectFolderEnnabled; }
            set
            {
                _buttonSelectFolderEnnabled = value;
                OnPropertyChanged();
            }
        }

        public bool _buttonSelectFilesEnabled;
        public bool ButtonSelectFilesEnabled
        {
            get { return _buttonSelectFilesEnabled; }
            set
            {
                _buttonSelectFilesEnabled = value;
                OnPropertyChanged();
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            _lengths = new Dictionary<string, int>();
            _counts = new Dictionary<string, int>();

            ButtonSelectFolderEnnabled = true;
            ButtonSelectFilesEnabled = true;

            DataContext = this;
        }

        private void Output(string msg)
        {
            textBoxOutput.Dispatcher.BeginInvoke(new Action(() =>
            {
                textBoxOutput.AppendText(msg);
                textBoxOutput.SelectionStart = textBoxOutput.Text.Length;
                textBoxOutput.ScrollToEnd();
            }), null);
        }

        private void OutputLine(string msg)
        {
            Output(msg + "\r\n");
        }

        private void OnSelectFileClick(object sender, RoutedEventArgs e)
        {
            var files = new List<string>();
            var ofd = new OpenFileDialog
            {
                CheckFileExists = true,
                Filter = "DAT File|*.dat;*.dat64",
                Multiselect = true
            };
            if (ofd.ShowDialog() != true)
                return;

            files.AddRange(ofd.FileNames.Where(File.Exists));
            ConvertFiles(files);
        }


        private void OnSelectFolderClick(object sender, RoutedEventArgs e)
        {
            var ofd = new FolderBrowserDialog();
            if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            var folder = ofd.SelectedPath;
            if (!Directory.Exists(folder))
                return;
            var list = Directory.GetFiles(folder);
            var files = list.Where(file => file.EndsWith(".dat")).Concat(list.Where(file => file.EndsWith(".dat64"))).ToList();
            ConvertFiles(files);
        }

        /// <summary>
        /// Creates for each given .dat files it's .csv analogue
        /// </summary>
        /// <param name="files"></param>
        private void ConvertFiles(List<string> files)
        {
            if (files == null)
                return;
            ButtonSelectFolderEnnabled = false;
            ButtonSelectFilesEnabled = false;
            var taskAction = new Action(() =>
                    {
                        foreach (var file in files)
                        {
                            try
                            {
                                var dat = new DatContainer(file);
                                var csvData = dat.GetCsvFormat();
                                var csvName = file + ".csv";
                                File.WriteAllText(csvName, csvData);
                                OutputLine(String.Format("Success: {0}", file));
                                
                                // stats
                                var fileName = Path.GetFileName(file);
                                if (dat.RecordInfo.Fields.Count > 0)
                                {
                                    _lengths[fileName] = dat.Records.Count;
                                }
                                else
                                {
                                    _counts[fileName] = dat.Count;
                                }

                            }
                            catch (Exception e)
                            {
                                OutputLine(String.Format("Error: {0}\n\t{1}", file, e.Message));
                            }
                        }
                        OutputLine("\nFinished.");
                    });
            var taskFinished = new Action<Task>(t =>
                {
                    ButtonSelectFolderEnnabled = true;
                    ButtonSelectFilesEnabled = true;

                    var tmp100 = new Dictionary<string, string>();
                    var tmp1000 = new Dictionary<string, string>();
                    var tmp10000 = new Dictionary<string, string>();
                    foreach (var kv in _lengths)
                    {
                        if (kv.Value < 100)
                        {
                            tmp100[kv.Key] = String.Format("{0}\t{1,-7}\t{2}", 0, kv.Value, kv.Key);
                        }
                        else if (kv.Value < 1000)
                        {
                            tmp1000[kv.Key] = String.Format("{0}\t{1,-7}\t{2}", 0, kv.Value, kv.Key);
                        }
                        else
                        {
                            tmp10000[kv.Key] = String.Format("{0}\t{1,-7}\t{2}", 0, kv.Value, kv.Key);
                        }
                    }

                    // 1000+ records
                    var sortedFiles = tmp10000.Keys.ToList();
                    sortedFiles.Sort();
                    foreach (var s in sortedFiles)
                    {
                        OutputLine(tmp10000[s]);
                    }
                    OutputLine("");
                    
                    // 100+ records
                    sortedFiles = tmp1000.Keys.ToList();
                    sortedFiles.Sort();
                    foreach (var s in sortedFiles)
                    {
                        OutputLine(tmp1000[s]);
                    }
                    OutputLine("");

                    // <100 records
                    sortedFiles = tmp100.Keys.ToList();
                    sortedFiles.Sort();
                    foreach (var s in sortedFiles)
                    {
                        OutputLine(tmp100[s]);
                    }
                    OutputLine("");

                    // no recors
                    sortedFiles = _counts.Keys.ToList();
                    sortedFiles.Sort();
                    foreach (var s in sortedFiles)
                    {
                        OutputLine(String.Format("{0}\t{1,-7}\t{2}", _counts[s], 0, s));
                    }
                    
                }
            );

            var task = new Task(taskAction);
            task.ContinueWith(taskFinished);
            task.Start();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}