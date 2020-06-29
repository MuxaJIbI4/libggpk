using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Ionic.Zip;
using KUtility;
using LibGGPK;
using LibGGPK.Records;
using Microsoft.Win32;
using Image = System.Windows.Controls.Image;

namespace VisualGGPK
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private string _ggpkPath = String.Empty;
        private string _binPath = null;
        private GrindingGearsPackageContainer _content;
        private Thread _workerThread;

        /// <summary>
        /// Dictionary mapping ggpk file paths to FileRecords for easy lookup
        /// EG: "Scripts\foobar.mel" -> FileRecord{Foobar.mel}
        /// </summary>
        private Dictionary<string, FileRecord> _recordsByPath;

        // contains currently selected file/directory from tree of GGPK files
        private TreeViewItem ClickedItem { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OutputLine(string msg)
        {
            Output(msg + Environment.NewLine);
        }

        private void Output(string msg)
        {
            TextBoxOutput.Dispatcher.BeginInvoke(new Action(() =>
            {
                TextBoxOutput.AppendText(msg);
                if (AutoScrollCheckBox.IsChecked.Value)
                {
                    TextBoxOutput.ScrollToEnd();
                }
            }), null);
        }

        private void UpdateTitle(string newTitle)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Title = newTitle;
            }), null);
        }

        /// <summary>
        /// Reloads the entire content.ggpk, rebuilds the tree
        /// </summary>
        private void ReloadGgpkFile()
        {
            TreeView1.Items.Clear();
            ResetViewer();
            TextBoxOutput.Visibility = Visibility.Visible;
            TextBoxOutput.Text = string.Empty;
            _content = null;
            SaveButton.IsEnabled = false;
            SerializeButton.IsEnabled = false;

            _workerThread = new Thread(() =>
            {
                _content = new GrindingGearsPackageContainer();
                try
                {
                    _content.Read(_ggpkPath, Output);
                }
                catch (Exception ex)
                {
                    Output(string.Format(Settings.Strings["ReloadGGPK_Failed"], ex.ToString()));
                    return;
                }

                if (_content.IsReadOnly)
                {
                    Output(Settings.Strings["ReloadGGPK_ReadOnly"] + Environment.NewLine);
                    UpdateTitle(Settings.Strings["MainWindow_Title_Readonly"]);
                }

                OutputLine(Settings.Strings["ReloadGGPK_Traversing_Tree"]);

                // Collect all FileRecordPath -> FileRecord pairs for easier replacing
                _recordsByPath = new Dictionary<string, FileRecord>(_content.RecordOffsets.Count);
                DirectoryTreeNode.TraverseTreePostorder(
                    _content.DirectoryRoot,
                    null,
                    n => _recordsByPath.Add(n.GetDirectoryPath() + n.Name, n));

                TreeView1.Dispatcher.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        var rootItem = CreateLazyTreeViewItem(_content.DirectoryRoot);
                        TreeView1.Items.Add(rootItem);
                        rootItem.IsExpanded = true;
                        rootItem.RaiseEvent(new RoutedEventArgs(TreeViewItem.ExpandedEvent, rootItem));
                    }
                    catch (Exception ex)
                    {
                        Output(string.Format(Settings.Strings["Error_Read_Directory_Tree"], ex.ToString()));
                        Output(ex.StackTrace);
                        return;
                    }

                    
                }), null);

                _workerThread = null;

                OutputLine(Settings.Strings["ReloadGGPK_Successful"]);

                TextBoxOutput.Dispatcher.BeginInvoke(new Action(() =>
                {
                    SaveButton.IsEnabled = true;
                    SerializeButton.IsEnabled = true;
                }));
            });

            _workerThread.Start();
        }

        /// <summary>
        /// Load the Records.bin, rebuilds the tree
        /// </summary>
        private void LoadBinFile()
        {
            TreeView1.Items.Clear();
            ResetViewer();
            TextBoxOutput.Visibility = Visibility.Visible;
            TextBoxOutput.Text = string.Empty;
            _content = null;
            SaveButton.IsEnabled = false;
            SerializeButton.IsEnabled = false;

            _workerThread = new Thread(() =>
            {
                _content = new GrindingGearsPackageContainer();
                try
                {
                    _content.Read(_ggpkPath, _binPath, Output);
                }
                catch (Exception ex)
                {
                    Output(string.Format(Settings.Strings["ReloadGGPK_Failed"], ex.ToString()));
                    return;
                }

                if (_content.IsReadOnly)
                {
                    Output(Settings.Strings["ReloadGGPK_ReadOnly"] + Environment.NewLine);
                    UpdateTitle(Settings.Strings["MainWindow_Title_Readonly"]);
                }

                OutputLine(Settings.Strings["ReloadGGPK_Traversing_Tree"]);

                // Collect all FileRecordPath -> FileRecord pairs for easier replacing
                _recordsByPath = new Dictionary<string, FileRecord>(_content.RecordOffsets.Count);
                DirectoryTreeNode.TraverseTreePostorder(
                    _content.DirectoryRoot,
                    null,
                    n => _recordsByPath.Add(n.GetDirectoryPath() + n.Name, n));

                TreeView1.Dispatcher.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        var rootItem = CreateLazyTreeViewItem(_content.DirectoryRoot);
                        TreeView1.Items.Add(rootItem);
                        rootItem.IsExpanded = true;
                        rootItem.RaiseEvent(new RoutedEventArgs(TreeViewItem.ExpandedEvent, rootItem));
                    }
                    catch (Exception ex)
                    {
                        Output(string.Format(Settings.Strings["Error_Read_Directory_Tree"], ex.ToString()));
                        Output(ex.StackTrace);
                        return;
                    }

                }), null);

                _workerThread = null;

                OutputLine(Settings.Strings["ReloadGGPK_Successful"]);

                SaveButton.Dispatcher.BeginInvoke(new Action(() =>
                {
                    SaveButton.IsEnabled = true;
                    SerializeButton.IsEnabled = true;
                }));
            });

            _workerThread.Start();
        }

        /// <summary>
        /// Resets all of the file viewers
        /// </summary>
        private void ResetViewer()
        {
            TextBoxOutput.Visibility = Visibility.Hidden;
            ImageOutput.Visibility = Visibility.Hidden;
            RichTextOutput.Visibility = Visibility.Hidden;
            DataGridOutput.Visibility = Visibility.Hidden;
            DatViewerOutput.Visibility = Visibility.Hidden;

            TextBoxOutput.Clear();
            ImageOutput.Source = null;
            RichTextOutput.Document.Blocks.Clear();
            DataGridOutput.ItemsSource = null;
            TextBoxOffset.Text = String.Empty;
            TextBoxSize.Text = String.Empty;
            TextBoxNameHash.Text = String.Empty;
            TextBoxHash.Text = String.Empty;
        }

        /// <summary>
        /// Updates the FileViewers to display the currently selected item in the TreeView
        /// </summary>
        private void UpdateDisplayPanel()
        {
            ResetViewer();
            var selectedItem = TreeView1.SelectedItem;
            if (selectedItem == null)
            {
                TextBoxOutput.Visibility = Visibility.Visible;
                return;
            }
            var item = selectedItem as TreeViewItem;
            if (item == null)
            {
                TextBoxOutput.Visibility = Visibility.Visible;
                return;
            }

            if (item.Tag is DirectoryTreeNode)
            {
                TextBoxOutput.Visibility = Visibility.Visible;
                var selectedDirectory = item.Tag as DirectoryTreeNode;
                if (selectedDirectory.Record == null)
                    return;

                TextBoxOffset.Text = selectedDirectory.Record.RecordBegin.ToString("X");
                TextBoxSize.Text = selectedDirectory.Record.Entries.Count.ToString();
                TextBoxNameHash.Text = selectedDirectory.Record.GetNameHash().ToString("X");
                TextBoxHash.Text = BitConverter.ToString(selectedDirectory.Record.Hash);
                return;
            }
            if (!(item.Tag is FileRecord))
                return; // TODO: this is error, need to report it

            var selectedRecord = item.Tag as FileRecord;

            TextBoxOffset.Text = selectedRecord.RecordBegin.ToString("X");
            TextBoxSize.Text = selectedRecord.DataLength.ToString();
            TextBoxNameHash.Text = selectedRecord.GetNameHash().ToString("X");
            TextBoxHash.Text = BitConverter.ToString(selectedRecord.Hash);

            try
            {
                switch (selectedRecord.FileFormat)
                {
                    case FileRecord.DataFormat.Image:
                        DisplayImageFile(selectedRecord);
                        break;
                    case FileRecord.DataFormat.TextureDds:
                        DisplayDdsFile(selectedRecord);
                        break;
                    case FileRecord.DataFormat.Ascii:
                        DisplayTextFileAsAscii(selectedRecord);
                        break;
                    case FileRecord.DataFormat.Unicode:
                        DisplayFileAsUnicode(selectedRecord);
                        break;
                    case FileRecord.DataFormat.RichText:
                        DisplayTextFileAsRichText(selectedRecord);
                        break;
                    case FileRecord.DataFormat.Dat:
                        DisplayDatFile(selectedRecord);
                        break;
                }
            }
            catch (Exception ex)
            {
                ResetViewer();
                TextBoxOffset.Text = selectedRecord.RecordBegin.ToString("X");
                TextBoxSize.Text = selectedRecord.DataLength.ToString();
                TextBoxNameHash.Text = selectedRecord.GetNameHash().ToString("X");
                TextBoxHash.Text = BitConverter.ToString(selectedRecord.Hash);
                TextBoxOutput.Visibility = Visibility.Visible;

                var sb = new StringBuilder();
                while (ex != null)
                {
                    sb.AppendLine(ex.ToString());
                    ex = ex.InnerException;
                }

                TextBoxOutput.Text = string.Format(Settings.Strings["UpdateDisplayPanel_Failed"], sb);
            }

        }

        /// <summary>
        /// Displays the contents of a FileRecord in the DatViewer
        /// </summary>
        /// <param name="selectedRecord">FileRecord to display</param>
        private void DisplayDatFile(FileRecord selectedRecord)
        {
            var data = selectedRecord.ReadFileContent(_ggpkPath);
            DatViewerOutput.Visibility = Visibility.Visible;

            DatViewerOutput.Reset(selectedRecord.Name, data);
        }

        /// <summary>
        /// Displays the contents of a FileRecord in the RichTextBox
        /// </summary>
        /// <param name="selectedRecord">FileRecord to display</param>
        private void DisplayTextFileAsRichText(FileRecord selectedRecord)
        {
            var buffer = selectedRecord.ReadFileContent(_ggpkPath);
            RichTextOutput.Visibility = Visibility.Visible;

            using (var ms = new MemoryStream(buffer))
            {
                RichTextOutput.Selection.Load(ms, DataFormats.Rtf);
            }
        }

        /// <summary>
        /// Displays the contents of a FileRecord in the TextBox as Unicode text
        /// </summary>
        /// <param name="selectedRecord">FileRecord to display</param>
        private void DisplayFileAsUnicode(FileRecord selectedRecord)
        {
            var buffer = selectedRecord.ReadFileContent(_ggpkPath);
            TextBoxOutput.Visibility = Visibility.Visible;

            TextBoxOutput.Text = Encoding.Unicode.GetString(buffer);
        }

        /// <summary>
        /// Displays the contents of a FileRecord in the TextBox as Ascii text
        /// </summary>
        /// <param name="selectedRecord">FileRecord to display</param>
        private void DisplayTextFileAsAscii(FileRecord selectedRecord)
        {
            var buffer = selectedRecord.ReadFileContent(_ggpkPath);
            TextBoxOutput.Visibility = Visibility.Visible;

            TextBoxOutput.Text = Encoding.ASCII.GetString(buffer);
        }

        /// <summary>
        /// Displays the contents of a FileRecord in the ImageBox
        /// </summary>
        /// <param name="selectedRecord">FileRecord to display</param>
        private void DisplayImageFile(FileRecord selectedRecord)
        {
            var buffer = selectedRecord.ReadFileContent(_ggpkPath);
            ImageOutput.Visibility = Visibility.Visible;

            using (var ms = new MemoryStream(buffer))
            {
                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.StreamSource = ms;
                bmp.EndInit();
                ImageOutput.Source = bmp;
            }
        }

        /// <summary>
        /// Displays the contents of a FileRecord in the ImageBox (DDS Texture mode)
        /// </summary>
        /// <param name="selectedRecord">FileRecord to display</param>
        private void DisplayDdsFile(FileRecord selectedRecord)
        {
            var buffer = selectedRecord.ReadFileContent(_ggpkPath);
            ImageOutput.Visibility = Visibility.Visible;

            var dds = new DDSImage(buffer);

            using (var ms = new MemoryStream())
            {
                dds.images[0].Save(ms, ImageFormat.Png);

                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.StreamSource = ms;
                bmp.EndInit();
                ImageOutput.Source = bmp;
            }
        }

        /// <summary>
        /// Exports the specified FileRecord to disk
        /// </summary>
        /// <param name="selectedRecord">FileRecord to export</param>
        private void ExportFileRecord(FileRecord selectedRecord)
        {
            try
            {
                var saveFileDialog = new SaveFileDialog { FileName = selectedRecord.Name };
                if (saveFileDialog.ShowDialog() != true) return;

                selectedRecord.ExtractFile(_ggpkPath, saveFileDialog.FileName);
                MessageBox.Show(
                    string.Format(Settings.Strings["ExportSelectedItem_Successful"], selectedRecord.DataLength),
                    Settings.Strings["ExportAllItemsInDirectory_Successful_Caption"], MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(Settings.Strings["ExportSelectedItem_Failed"], ex.ToString()),
                    Settings.Strings["Error_Caption"], MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Attempts to display the specified record on the gui
        /// </summary>
        /// <param name="selectedRecord">Record to view</param>
        private void OpenFileRecord(FileRecord selectedRecord)
        {
            if (selectedRecord == null)
                return;
            string extractedFileName;
            try
            {
                extractedFileName = selectedRecord.ExtractTempFile(_ggpkPath);
                var extension = Path.GetExtension(selectedRecord.Name);

                // If we're dealing with .dat files then just create a human readable CSV and view that instead
                if (!String.IsNullOrEmpty(extension) && (extension.ToLower().Equals(".dat") || extension.ToLower().Equals(".dat64")))
                {
                    var extractedCsv = Path.GetTempFileName();
                    File.Move(extractedCsv, extractedCsv + ".csv");
                    extractedCsv = extractedCsv + ".csv";

                    using (var inStream = File.Open(extractedFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        var tempWrapper = new DatWrapper(inStream, selectedRecord.Name);
                        File.WriteAllText(extractedCsv, tempWrapper.GetCSV());
                    }

                    File.Delete(extractedFileName);
                    extractedFileName = extractedCsv;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(Settings.Strings["ViewSelectedItem_Failed"], ex.ToString()),
                    Settings.Strings["Error_Caption"],
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var fileViewerProcess = new Process
            {
                StartInfo = new ProcessStartInfo(extractedFileName),
                EnableRaisingEvents = true
            };
            fileViewerProcess.Exited += fileViewerProcess_Exited;
            fileViewerProcess.Start();
        }

        /// <summary>
        /// Exports entire DirectoryTreeNode to disk, preserving directory structure
        /// </summary>
        /// <param name="selectedDirectoryNode">Node to export to disk</param>
        private void ExportAllItemsInDirectory(DirectoryTreeNode selectedDirectoryNode)
        {
            var recordsToExport = new List<FileRecord>();

            var fileAction = new Action<FileRecord>(recordsToExport.Add);

            DirectoryTreeNode.TraverseTreePreorder(selectedDirectoryNode, null, fileAction);

            try
            {
                var saveFileDialog = new SaveFileDialog
                {
                    FileName = Settings.Strings["ExportAllItemsInDirectory_Default_FileName"]
                };
                if (saveFileDialog.ShowDialog() == true)
                {
                    var exportDirectory = Path.GetDirectoryName(saveFileDialog.FileName) + Path.DirectorySeparatorChar;
                    foreach (var item in recordsToExport)
                    {
                        item.ExtractFileWithDirectoryStructure(_ggpkPath, exportDirectory);
                    }
                    MessageBox.Show(string.Format(Settings.Strings["ExportAllItemsInDirectory_Successful"], recordsToExport.Count), Settings.Strings["ExportAllItemsInDirectory_Successful_Caption"], MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(Settings.Strings["ExportAllItemsInDirectory_Failed"], ex.ToString()), Settings.Strings["Error_Caption"], MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Replaces selected file with file user selects via MessageBox
        /// </summary>
        /// <param name="recordToReplace"></param>
        private void ReplaceFileRecord(FileRecord recordToReplace)
        {
            if (_content.IsReadOnly)
            {
                MessageBox.Show(
                    Settings.Strings["ReplaceItem_Readonly"],
                    Settings.Strings["ReplaceItem_ReadonlyCaption"]);
                return;
            }

            try
            {
                var openFileDialog = new OpenFileDialog
                {
                    FileName = "",
                    CheckFileExists = true,
                    CheckPathExists = true
                };

                if (openFileDialog.ShowDialog() != true) return;
                recordToReplace.ReplaceContents(_ggpkPath, File.ReadAllBytes(openFileDialog.FileName), _content);
                MessageBox.Show(
                    String.Format(Settings.Strings["ReplaceItem_Successful"], recordToReplace.Name, recordToReplace.RecordBegin.ToString("X")),
                    Settings.Strings["ReplaceItem_Successful_Caption"],
                    MessageBoxButton.OK, MessageBoxImage.Information);
                UpdateDisplayPanel();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(Settings.Strings["ReplaceItem_Failed"], ex.ToString()),
                    Settings.Strings["Error_Caption"],
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Extracts specified archive and replaces files in GGPK with extracted files. Files in
        /// archive must have same directory structure as in GGPK.
        /// </summary>
        /// <param name="archivePath">Path to archive containing</param>
        private void HandleDropArchive(string archivePath)
        {
            if (_content.IsReadOnly)
            {
                MessageBox.Show(
                    Settings.Strings["ReplaceItem_Readonly"],
                    Settings.Strings["ReplaceItem_ReadonlyCaption"]);
                return;
            }

            OutputLine(string.Format(Settings.Strings["MainWindow_HandleDropArchive_Info"], archivePath));

            using (var zipFile = new ZipFile(archivePath))
            {
                //var fileNames = zipFile.EntryFileNames;

                // Archive Version Check: Read version.txt and check with patch_notes.rtf's Hash
                foreach (var item in zipFile.Entries)
                {
                    if (item.FileName.Equals("version.txt"))
                    {
                        using (var reader = item.OpenReader())
                        {
                            var versionData = new byte[item.UncompressedSize];
                            reader.Read(versionData, 0, versionData.Length);
                            var versionStr = Encoding.UTF8.GetString(versionData, 0, versionData.Length);
                            if (_recordsByPath.ContainsKey("patch_notes.rtf"))
                            {
                                var hash = BitConverter.ToString(_recordsByPath["patch_notes.rtf"].Hash);
                                if (!versionStr.Substring(0, hash.Length).Equals(hash))
                                {
                                    OutputLine(Settings.Strings["MainWindow_VersionCheck_Failed"]);
                                    return;
                                }
                            }
                        }
                        break;
                    }
                }

                foreach (var item in zipFile.Entries)
                {
                    if (item.IsDirectory)
                    {
                        continue;
                    }
                    if (item.FileName.Equals("version.txt"))
                    {
                        continue;
                    }

                    var fixedFileName = "ROOT" + Path.DirectorySeparatorChar + item.FileName;
                    if (Path.DirectorySeparatorChar != '/')
                    {
                        fixedFileName = fixedFileName.Replace('/', Path.DirectorySeparatorChar);
                    }

                    if (!_recordsByPath.ContainsKey(fixedFileName))
                    {
                        OutputLine(string.Format(Settings.Strings["MainWindow_HandleDropDirectory_Failed"], fixedFileName));
                        continue;
                    }

                    using (var reader = item.OpenReader())
                    {
                        var replacementData = new byte[item.UncompressedSize];
                        reader.Read(replacementData, 0, replacementData.Length);

                        _recordsByPath[fixedFileName].ReplaceContents(_ggpkPath, replacementData, _content);
                    }

                    OutputLine(string.Format(Settings.Strings["MainWindow_HandleDropDirectory_Replace"], fixedFileName));
                }
            }
        }

        /// <summary>
        /// Replaces the currently selected TreeViewItem with specified file on disk
        /// </summary>
        /// <param name="fileName">Path of file to replace currently selected item with.</param>
        private void HandleDropFile(string fileName)
        {
            if (_content.IsReadOnly)
            {
                MessageBox.Show(Settings.Strings["ReplaceItem_Readonly"], Settings.Strings["ReplaceItem_ReadonlyCaption"]);
                return;
            }

            var record = TreeView1.SelectedItem as FileRecord;
            if (record == null)
            {
                OutputLine(Settings.Strings["MainWindow_HandleDropFile_Failed"]);
                return;
            }

            record.ReplaceContents(_ggpkPath, File.ReadAllBytes(fileName), _content);

            OutputLine(string.Format(Settings.Strings["MainWindow_HandleDropFile_Replace"], record.GetDirectoryPath(), record.Name));

        }

        /// <summary>
        /// Specified directory was dropped onto interface, attept to replace GGPK files with same directory
        /// structure with files in directory. Directory must have same directory structure as GGPK file.
        /// EG:
        /// dropping 'Art' directory containing '2DArt' directory containing 'BuffIcons' directory containing 'buffbleed.dds' will replace
        /// \Art\2DArt\BuffIcons\buffbleed.dds with buffbleed.dds from dropped directory
        /// </summary>
        /// <param name="baseDirectory">Directory containing files to replace</param>
        private void HandleDropDirectory(string baseDirectory)
        {
            if (_content.IsReadOnly)
            {
                MessageBox.Show(Settings.Strings["ReplaceItem_Readonly"], Settings.Strings["ReplaceItem_ReadonlyCaption"]);
                return;
            }

            var filesToReplace = Directory.GetFiles(baseDirectory, "*.*", SearchOption.AllDirectories);
            var fileName = Path.GetFileName(baseDirectory);
            if (fileName == null) return;

            var baseDirectoryNameLength = fileName.Length;
            OutputLine(string.Format(Settings.Strings["MainWindow_HandleDropDirectory_Count"], filesToReplace.Length));
            foreach (var item in filesToReplace)
            {
                var fixedFileName = item.Remove(0, baseDirectory.Length - baseDirectoryNameLength);
                if (!fixedFileName.StartsWith("ROOT" + Path.DirectorySeparatorChar))
                {
                    fixedFileName = "ROOT" + Path.DirectorySeparatorChar + fixedFileName;
                }
                if (!_recordsByPath.ContainsKey(fixedFileName))
                {
                    OutputLine(string.Format(Settings.Strings["MainWindow_HandleDropDirectory_Failed"], fixedFileName));
                    continue;
                }

                _recordsByPath[fixedFileName].ReplaceContents(_ggpkPath, File.ReadAllBytes(item), _content);

                OutputLine(string.Format(Settings.Strings["MainWindow_HandleDropDirectory_Replace"], fixedFileName));

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                AddExtension = true,
                CheckFileExists = true,
                DefaultExt = "ggpk",
                FileName = "Content.ggpk",
                Filter = Settings.Strings["Load_GGPK_Filter"]
            };

            // Get InstallLocation From RegistryKey
            if (string.IsNullOrEmpty(ofd.InitialDirectory))
            {
                var start = Registry.CurrentUser;
                var programName = start.OpenSubKey(@"Software\GrindingGearGames\Path of Exile");
                if (programName != null)
                {
                    var pathString = (string)programName.GetValue("InstallLocation");
                    if (pathString != string.Empty && File.Exists(pathString + @"\Content.ggpk"))
                    {
                        pathString = pathString.Trim();
                        if (pathString.EndsWith("\\"))
                        {
                            pathString = pathString.Remove(pathString.Length - 1);
                        }
                        ofd.InitialDirectory = pathString;
                    }
                }
            }

            // Get Garena PoE
            if (string.IsNullOrEmpty(ofd.InitialDirectory))
            {
                var start = Registry.LocalMachine;
                var programName = start.OpenSubKey(@"SOFTWARE\Wow6432Node\Garena\PoE");
                if (programName != null)
                {
                    var pathString = (string)programName.GetValue("Path");
                    if (pathString != string.Empty && File.Exists(pathString + @"\Content.ggpk"))
                    {
                        ofd.InitialDirectory = pathString;
                    }
                }
            }
            if (ofd.ShowDialog() == true)
            {
                if (!File.Exists(ofd.FileName))
                {
                    Close();
                    return;
                }
                if (Path.GetExtension(ofd.FileName).ToLower() == ".bin")
                {
                    _binPath = ofd.FileName;
                    ofd.FileName = "Content.ggpk";
                    if (ofd.ShowDialog() == true)
                    {
                        if (!File.Exists(ofd.FileName))
                        {
                            Close();
                            return;
                        }
                        _ggpkPath = ofd.FileName;
                        LoadBinFile();
                    }
                    else
                    {
                        Close();
                        return;
                    }
                } else
                {
                    _ggpkPath = ofd.FileName;
                    ReloadGgpkFile();
                }
            }
            else
            {
                Close();
                return;
            }

            MenuItemExport.Header = Settings.Strings["MainWindow_Menu_Export"];
            MenuItemReplace.Header = Settings.Strings["MainWindow_Menu_Replace"];
            MenuItemOpen.Header = Settings.Strings["MainWindow_Menu_View"];
            LabelFileOffset.Content = Settings.Strings["MainWindow_Label_FileOffset"];
        }

        private void Window_PreviewDrop_1(object sender, DragEventArgs e)
        {
            if (!_content.IsReadOnly)
            {
                e.Effects = DragDropEffects.Link;
            }
        }

        private void Window_Drop_1(object sender, DragEventArgs e)
        {
            if (_content.IsReadOnly)
            {
                MessageBox.Show(Settings.Strings["ReplaceItem_Readonly"], Settings.Strings["ReplaceItem_ReadonlyCaption"]);
                return;
            }

            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return;
            }

            // Bring-to-front hack
            Topmost = true;
            Topmost = false;

            // reset viewer to show output message
            ResetViewer();
            TextBoxOutput.Text = string.Empty;
            TextBoxOutput.Visibility = Visibility.Visible;

            if (MessageBox.Show(
                Settings.Strings["MainWindow_Window_Drop_Confirm"],
                Settings.Strings["MainWindow_Window_Drop_Confirm_Caption"],
                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            _workerThread = new Thread(() =>
            {
                var fileNames = e.Data.GetData(DataFormats.FileDrop) as string[];
                if (fileNames == null || fileNames.Length != 1)
                {
                    OutputLine(Settings.Strings["MainWindow_Drop_Failed"]);
                    return;
                }

                var extension = Path.GetExtension(fileNames[0]);
                if (Directory.Exists(fileNames[0]))
                {
                    HandleDropDirectory(fileNames[0]);
                }
                else if (!String.IsNullOrEmpty(extension) && extension.Equals(".zip")) // Zip file
                {
                    HandleDropArchive(fileNames[0]);
                }
                else
                {
                    HandleDropFile(fileNames[0]);
                }

                OutputLine(Environment.NewLine + "Finished!");

                _workerThread = null;
            });

            _workerThread.Start();
        }

        private void Window_Closing_1(object sender, CancelEventArgs e)
        {
            if (_workerThread != null)
            {
                _workerThread.Abort();
            }
        }

        private void fileViewerProcess_Exited(object sender, EventArgs e)
        {
            var sourceProcess = sender as Process;
            if (sourceProcess == null)
                return;

            try
            {
                File.Delete(sourceProcess.StartInfo.FileName);
            }
            catch (Exception)
            {
                /* Suppress any exceptions, they're just temporary files */
                //                MessageBox.Show(
                //                    String.Format("Failed to delete temporary file '{0}': {1}", 
                //                    sourceProcess.StartInfo.FileName, ex.Message)); 
            }
        }

        private void OnTreeViewItemExpanded(object sender, RoutedEventArgs e)
        {
            var item = e.Source as TreeViewItem;
            if (item == null)
                return;
            if (!(item.Tag is DirectoryTreeNode))
                return;
            if ((item.Items.Count != 1) || (!(item.Items[0] is string)))
                return;
            item.Items.Clear();

            var directoryTreeNode = item.Tag as DirectoryTreeNode;
            directoryTreeNode.Children.Sort();
            directoryTreeNode.Files.Sort();

            foreach (var dir in directoryTreeNode.Children)
            {
                item.Items.Add(CreateLazyTreeViewItem(dir));
            }

            foreach (var file in directoryTreeNode.Files)
            {
                item.Items.Add(CreateLazyTreeViewItem(file));
            }
        }

        private TreeViewItem CreateLazyTreeViewItem(object o)
        {
            var item = new TreeViewItem
            {
                Tag = o
            };

            Icon icon;
            string text;
            if (o is FileRecord)
            {
                icon = IconReader.OfExtension(o.ToString());
                text = o.ToString();
            }
            else if (o is DirectoryTreeNode)
            {
                icon = IconReader.OfFolder(false);
                item.Items.Add("Loading...");
                text = o.ToString();
            }
            else
                throw new Exception("Unknown tree view item type: " + o.GetType());

            // create stack panel
            var stack = new StackPanel { Orientation = Orientation.Horizontal };
            // create Image
            var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
                icon.ToBitmap().GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            var img = new Image
            {
                Source = bitmapSource,
                Width = 10,
                Height = 10
            };
            // Label
            var lbl = new Label
            {
                Content = text
            };

            // Add into stack
            stack.Children.Add(img);
            stack.Children.Add(lbl);

            // assign stack to header
            item.Header = stack;
            return item;
        }

        private void OnTreeViewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var clickedItem = e.Source as UIElement;
            if (clickedItem == null)
                return;
            while ((clickedItem != null) && !(clickedItem is TreeViewItem))
            {
                clickedItem = VisualTreeHelper.GetParent(clickedItem) as UIElement;
            }
            ClickedItem = clickedItem as TreeViewItem;
            if (ClickedItem == null)
                return;

            var header = ClickedItem.Tag;
            MenuItemReplace.IsEnabled = header is FileRecord;
            MenuItemOpen.IsEnabled = header is FileRecord;
            MenuItemExport.IsEnabled = (header is FileRecord || header is DirectoryTreeNode);

            if (e.ChangedButton == MouseButton.Right)                           // right-click
            {
            }
            else if (e.ChangedButton == MouseButton.Left && e.ClickCount == 1)  // single left-click
            {
            }
            else if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)  // double left-click
            {
                if (ClickedItem != null && ClickedItem.Tag is FileRecord)
                    OpenFileRecord(ClickedItem.Tag as FileRecord);
            }
        }

        private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            UpdateDisplayPanel();
        }

        private void OnItemContextMenuExportClicked(object sender, RoutedEventArgs e)
        {
            if (ClickedItem == null)
                return;

            var item = ClickedItem.Tag;
            if (item is DirectoryTreeNode)
            {
                ExportAllItemsInDirectory(item as DirectoryTreeNode);
            }
            else if (item is FileRecord)
            {
                ExportFileRecord(item as FileRecord);
            }
        }

        private void OnItemContextMenuReplaceClicked(object sender, RoutedEventArgs e)
        {
            if (ClickedItem == null)
                return;

            var file = ClickedItem.Tag as FileRecord;
            if (file == null)
                return;

            ReplaceFileRecord(file);
        }

        private void OnItemContextMenuOpenClicked(object sender, RoutedEventArgs e)
        {
            if (ClickedItem == null)
                return;
            var file = ClickedItem.Tag as FileRecord;
            if (file == null)
                return;

            OpenFileRecord(file);
        }

        // TODO: Fix deleting files
        private void OnItemContextMenuDeleteClicked(object sender, RoutedEventArgs e)
        {
            if (ClickedItem == null)
                return;

            var tag = ClickedItem.Tag;
            if (tag is FileRecord)
            {
                _content.DeleteFileRecord(tag as FileRecord);
            }
            else if (tag is DirectoryTreeNode)
            {
                _content.DeleteDirectoryRecord(tag as DirectoryTreeNode);
            }
            else
            {
                return;
            }

            var parent = ClickedItem.Parent;
            if (!(parent is TreeViewItem))
            {
                parent = VisualTreeHelper.GetParent(ClickedItem);
                while (parent != null && !(parent is TreeViewItem))
                {
                    parent = VisualTreeHelper.GetParent(parent);
                }
            }
            if (parent == null)
                return;
            var treeParent = parent as TreeViewItem;
            treeParent.Items.Remove(ClickedItem);
            ClickedItem = null;
        }

        private void OnSaveClicked(object sender, RoutedEventArgs e)
        {
            // save GGPK contents
            var openFileDialog = new OpenFileDialog
            {
                FileName = "",
                CheckFileExists = true,
                CheckPathExists = true
            };

            if (openFileDialog.ShowDialog() != true) return;
            ResetViewer();
            TextBoxOutput.Visibility = Visibility.Visible;
            TextBoxOutput.Text = string.Empty;
            _content.Save(openFileDialog.FileName, OutputLine);
        }

        private void OnSerializeClicked(object sender, RoutedEventArgs e)
        {
            // Serialize GGPK Records
            var saveFileDialog = new SaveFileDialog
            {
                AddExtension = true,
                CheckPathExists = true,
                DefaultExt = "bin",
                FileName = "Records.bin",
                OverwritePrompt = true
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                TextBoxOutput.Visibility = Visibility.Visible;
                _content.SerializeRecords(saveFileDialog.FileName, Output);
            }
        }
    }
}