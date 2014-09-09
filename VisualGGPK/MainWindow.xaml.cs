using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Ionic.Zip;
using KUtility;
using LibGGPK;
using Microsoft.Win32;

namespace VisualGGPK
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private string _ggpkPath = String.Empty;
        private GGPK _content;
        private Thread _workerThread;

        /// <summary>
        /// Dictionary mapping ggpk file paths to FileRecords for easy lookup
        /// EG: "Scripts\foobar.mel" -> FileRecord{Foobar.mel}
        /// </summary>
        private Dictionary<string, FileRecord> _recordsByPath;

        // contains currently selected file/directory from tree of GGPK files
        private TreeViewItem TreeRightClickSelectedFile { get; set; }

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
            textBoxOutput.Dispatcher.BeginInvoke(new Action(() =>
            {
                textBoxOutput.Text += msg;
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
            treeView1.Items.Clear();
            ResetViewer();
            textBoxOutput.Visibility = Visibility.Visible;
            textBoxOutput.Text = string.Empty;
            _content = null;

            _workerThread = new Thread(() =>
            {
                _content = new GGPK();
                try
                {
                    _content.Read(_ggpkPath, Output);
                }
                catch (Exception ex)
                {
                    Output(string.Format(Settings.Strings["ReloadGGPK_Failed"], ex.Message));
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

                treeView1.Dispatcher.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        var rootItem = CreateLazyTreeViewItem(_content.DirectoryRoot);
                        treeView1.Items.Add(rootItem);
                        rootItem.IsExpanded = true;
                        rootItem.RaiseEvent(new RoutedEventArgs(TreeViewItem.ExpandedEvent, rootItem));
                    }
                    catch (Exception ex)
                    {
                        Output(string.Format(Settings.Strings["Error_Read_Directory_Tree"], ex.Message));
                        return;
                    }

                    _workerThread = null;
                }), null);


                OutputLine(Settings.Strings["ReloadGGPK_Successful"]);
            });

            _workerThread.Start();
        }

        /// <summary>
        /// Resets all of the file viewers
        /// </summary>
        private void ResetViewer()
        {
            textBoxOutput.Visibility = Visibility.Hidden;
            imageOutput.Visibility = Visibility.Hidden;
            richTextOutput.Visibility = Visibility.Hidden;
            dataGridOutput.Visibility = Visibility.Hidden;
            datViewerOutput.Visibility = Visibility.Hidden;

            textBoxOutput.Clear();
            imageOutput.Source = null;
            richTextOutput.Document.Blocks.Clear();
            dataGridOutput.ItemsSource = null;
            textBoxOffset.Text = String.Empty;
            textBoxSize.Text = String.Empty;
            textBoxNameHash.Text = String.Empty;
            textBoxHash.Text = String.Empty;
        }

        /// <summary>
        /// Updates the FileViewers to display the currently selected item in the TreeView
        /// </summary>
        private void UpdateDisplayPanel()
        {
            ResetViewer();

            if (treeView1.SelectedItem == null)
            {
                return;
            }

            if (treeView1.SelectedItem is TreeViewItem && (treeView1.SelectedItem as TreeViewItem).Header is DirectoryTreeNode)
            {
                var selectedDirectory = (treeView1.SelectedItem as TreeViewItem).Header as DirectoryTreeNode;
                if (selectedDirectory.Record == null)
                    return;

                textBoxOffset.Text = selectedDirectory.Record.RecordBegin.ToString("X");
                textBoxSize.Text = selectedDirectory.Record.Entries.Length.ToString();
                textBoxNameHash.Text = selectedDirectory.Record.GetNameHash().ToString("X");
                textBoxHash.Text = BitConverter.ToString(selectedDirectory.Record.Hash);
                return;
            }

            var selectedRecord = treeView1.SelectedItem as FileRecord;
            if (selectedRecord == null)
                return;

            textBoxOffset.Text = selectedRecord.RecordBegin.ToString("X");
            textBoxSize.Text = selectedRecord.DataLength.ToString();
            textBoxNameHash.Text = selectedRecord.GetNameHash().ToString("X");
            textBoxHash.Text = BitConverter.ToString(selectedRecord.Hash);

            try
            {
                switch (selectedRecord.FileFormat)
                {
                    case FileRecord.DataFormat.Image:
                        DisplayImageFile(selectedRecord);
                        break;
                    case FileRecord.DataFormat.TextureDDS:
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
                textBoxOffset.Text = selectedRecord.RecordBegin.ToString("X");
                textBoxSize.Text = selectedRecord.DataLength.ToString();
                textBoxNameHash.Text = selectedRecord.GetNameHash().ToString("X");
                textBoxHash.Text = BitConverter.ToString(selectedRecord.Hash);
                textBoxOutput.Visibility = Visibility.Visible;

                var sb = new StringBuilder();
                while (ex != null)
                {
                    sb.AppendLine(ex.Message);
                    ex = ex.InnerException;
                }

                textBoxOutput.Text = string.Format(Settings.Strings["UpdateDisplayPanel_Failed"], sb);
            }

        }

        /// <summary>
        /// Displays the contents of a FileRecord in the DatViewer
        /// </summary>
        /// <param name="selectedRecord">FileRecord to display</param>
        private void DisplayDatFile(FileRecord selectedRecord)
        {
            var data = selectedRecord.ReadData(_ggpkPath);
            datViewerOutput.Visibility = Visibility.Visible;

            datViewerOutput.Reset(selectedRecord.Name, data);
        }

        /// <summary>
        /// Displays the contents of a FileRecord in the RichTextBox
        /// </summary>
        /// <param name="selectedRecord">FileRecord to display</param>
        private void DisplayTextFileAsRichText(FileRecord selectedRecord)
        {
            var buffer = selectedRecord.ReadData(_ggpkPath);
            richTextOutput.Visibility = Visibility.Visible;

            using (var ms = new MemoryStream(buffer))
            {
                richTextOutput.Selection.Load(ms, DataFormats.Rtf);
            }
        }

        /// <summary>
        /// Displays the contents of a FileRecord in the TextBox as Unicode text
        /// </summary>
        /// <param name="selectedRecord">FileRecord to display</param>
        private void DisplayFileAsUnicode(FileRecord selectedRecord)
        {
            var buffer = selectedRecord.ReadData(_ggpkPath);
            textBoxOutput.Visibility = Visibility.Visible;

            textBoxOutput.Text = Encoding.Unicode.GetString(buffer);
        }

        /// <summary>
        /// Displays the contents of a FileRecord in the TextBox as Ascii text
        /// </summary>
        /// <param name="selectedRecord">FileRecord to display</param>
        private void DisplayTextFileAsAscii(FileRecord selectedRecord)
        {
            var buffer = selectedRecord.ReadData(_ggpkPath);
            textBoxOutput.Visibility = Visibility.Visible;

            textBoxOutput.Text = Encoding.ASCII.GetString(buffer);
        }

        /// <summary>
        /// Displays the contents of a FileRecord in the ImageBox
        /// </summary>
        /// <param name="selectedRecord">FileRecord to display</param>
        private void DisplayImageFile(FileRecord selectedRecord)
        {
            var buffer = selectedRecord.ReadData(_ggpkPath);
            imageOutput.Visibility = Visibility.Visible;

            using (var ms = new MemoryStream(buffer))
            {
                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.StreamSource = ms;
                bmp.EndInit();
                imageOutput.Source = bmp;
            }
        }

        /// <summary>
        /// Displays the contents of a FileRecord in the ImageBox (DDS Texture mode)
        /// </summary>
        /// <param name="selectedRecord">FileRecord to display</param>
        private void DisplayDdsFile(FileRecord selectedRecord)
        {
            var buffer = selectedRecord.ReadData(_ggpkPath);
            imageOutput.Visibility = Visibility.Visible;

            var dds = new DDSImage(buffer);

            using (var ms = new MemoryStream())
            {
                dds.images[0].Save(ms, ImageFormat.Png);

                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.StreamSource = ms;
                bmp.EndInit();
                imageOutput.Source = bmp;
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
                    string.Format(Settings.Strings["ExportSelectedItem_Failed"], ex.Message),
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
                if (!String.IsNullOrEmpty(extension) && extension.ToLower().Equals(".dat"))
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
                    string.Format(Settings.Strings["ViewSelectedItem_Failed"], ex.Message),
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
                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = Settings.Strings["ExportAllItemsInDirectory_Default_FileName"];
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
                MessageBox.Show(string.Format(Settings.Strings["ExportAllItemsInDirectory_Failed"], ex.Message), Settings.Strings["Error_Caption"], MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Replaces selected file with file user selects via MessageBox
        /// </summary>
        /// <param name="recordToReplace"></param>
        private void ReplaceItem(FileRecord recordToReplace)
        {
            if (_content.IsReadOnly)
            {
                MessageBox.Show(Settings.Strings["ReplaceItem_Readonly"], Settings.Strings["ReplaceItem_ReadonlyCaption"]);
                return;
            }

            try
            {
                var openFileDialog = new OpenFileDialog();
                openFileDialog.FileName = "";
                openFileDialog.CheckFileExists = true;
                openFileDialog.CheckPathExists = true;

                if (openFileDialog.ShowDialog() == true)
                {
                    var previousOffset = recordToReplace.RecordBegin;

                    recordToReplace.ReplaceContents(_ggpkPath, openFileDialog.FileName, _content.FreeRoot);
                    MessageBox.Show(String.Format(Settings.Strings["ReplaceItem_Successful"], recordToReplace.Name, recordToReplace.RecordBegin.ToString("X")), Settings.Strings["ReplaceItem_Successful_Caption"], MessageBoxButton.OK, MessageBoxImage.Information);

                    UpdateDisplayPanel();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(Settings.Strings["ReplaceItem_Failed"], ex.Message), Settings.Strings["Error_Caption"], MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show(Settings.Strings["ReplaceItem_Readonly"], Settings.Strings["ReplaceItem_ReadonlyCaption"]);
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
                                var Hash = BitConverter.ToString(_recordsByPath["patch_notes.rtf"].Hash);
                                if (!versionStr.Substring(0, Hash.Length).Equals(Hash))
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

                    var fixedFileName = item.FileName;
                    if (Path.DirectorySeparatorChar != '/')
                    {
                        fixedFileName = fixedFileName.Replace('/', Path.DirectorySeparatorChar);
                    }

                    if (!_recordsByPath.ContainsKey(fixedFileName))
                    {
                        OutputLine(string.Format(Settings.Strings["MainWindow_HandleDropDirectory_Failed"], fixedFileName));
                        continue;
                    }
                    OutputLine(string.Format(Settings.Strings["MainWindow_HandleDropDirectory_Replace"], fixedFileName));

                    using (var reader = item.OpenReader())
                    {
                        var replacementData = new byte[item.UncompressedSize];
                        reader.Read(replacementData, 0, replacementData.Length);

                        _recordsByPath[fixedFileName].ReplaceContents(_ggpkPath, replacementData, _content.FreeRoot);
                    }
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

            var record = treeView1.SelectedItem as FileRecord;
            if (record == null)
            {
                OutputLine(Settings.Strings["MainWindow_HandleDropFile_Failed"]);
                return;
            }

            OutputLine(string.Format(Settings.Strings["MainWindow_HandleDropFile_Replace"], record.GetDirectoryPath(), record.Name));

            record.ReplaceContents(_ggpkPath, fileName, _content.FreeRoot);
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
            var baseDirectoryNameLength = Path.GetFileName(baseDirectory).Length;

            OutputLine(string.Format(Settings.Strings["MainWindow_HandleDropDirectory_Count"], filesToReplace.Length));
            foreach (var item in filesToReplace)
            {
                var fixedFileName = item.Remove(0, baseDirectory.Length - baseDirectoryNameLength);
                if (!_recordsByPath.ContainsKey(fixedFileName))
                {
                    OutputLine(string.Format(Settings.Strings["MainWindow_HandleDropDirectory_Failed"], fixedFileName));
                    continue;
                }
                OutputLine(string.Format(Settings.Strings["MainWindow_HandleDropDirectory_Replace"], fixedFileName));

                _recordsByPath[fixedFileName].ReplaceContents(_ggpkPath, item, _content.FreeRoot);
            }
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.Filter = Settings.Strings["Load_GGPK_Filter"];
            // Get InstallLocation From RegistryKey
            if ((ofd.InitialDirectory == null) || (ofd.InitialDirectory == string.Empty))
            {
                var start = Registry.CurrentUser;
                var programName = start.OpenSubKey(@"Software\GrindingGearGames\Path of Exile");
                if (programName != null)
                {
                    var pathString = (string)programName.GetValue("InstallLocation");
                    if (pathString != string.Empty && File.Exists(pathString + @"\Content.ggpk"))
                    {
                        ofd.InitialDirectory = pathString;
                    }
                }
            }
            // Get Garena PoE
            if ((ofd.InitialDirectory == null) || (ofd.InitialDirectory == string.Empty))
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
                else
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

            menuItemExport.Header = Settings.Strings["MainWindow_Menu_Export"];
            menuItemReplace.Header = Settings.Strings["MainWindow_Menu_Replace"];
            menuItemView.Header = Settings.Strings["MainWindow_Menu_View"];
            labelFileOffset.Content = Settings.Strings["MainWindow_Label_FileOffset"];
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
            textBoxOutput.Text = string.Empty;
            textBoxOutput.Visibility = Visibility.Visible;

            if (MessageBox.Show(Settings.Strings["MainWindow_Window_Drop_Confirm"], Settings.Strings["MainWindow_Window_Drop_Confirm_Caption"], MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }

            var fileNames = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (fileNames == null || fileNames.Length != 1)
            {
                OutputLine(Settings.Strings["MainWindow_Drop_Failed"]);
                return;
            }

            if (Directory.Exists(fileNames[0]))
            {
                HandleDropDirectory(fileNames[0]);
            }
            else if (string.Compare(Path.GetExtension(fileNames[0]), ".zip", true) == 0)
            {
                // Zip file
                HandleDropArchive(fileNames[0]);
            }
            else
            {
                HandleDropFile(fileNames[0]);
            }
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
            {
                return;
            }

            try
            {
                File.Delete(sourceProcess.StartInfo.FileName);
            }
            catch (Exception /*ex*/)
            {
                /* Suppress any exceptions, they're just temporary files */
                //MessageBox.Show(String.Format("Failed to delete temporary file '{0}': {1}", sourceProcess.StartInfo.FileName, ex.Message)); 
            }
        }

        private void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
        {
            var item = e.Source as TreeViewItem;
            if (item == null)
                return;
            if (!(item.Header is DirectoryTreeNode))
                return;
            if ((item.Items.Count != 1) || (!(item.Items[0] is string)))
                return;
            item.Items.Clear();

            var directoryTreeNode = item.Header as DirectoryTreeNode;
            directoryTreeNode.Children.Sort();
            directoryTreeNode.Files.Sort();

            foreach (var dir in directoryTreeNode.Children)
            {
                item.Items.Add(CreateLazyTreeViewItem(dir));
            }

            foreach (var file in directoryTreeNode.Files)
            {
                item.Items.Add(new TreeViewItem { Header = file });
            }
        }

        private TreeViewItem CreateLazyTreeViewItem(object o)
        {
            var item = new TreeViewItem
            {
                Header = o,
                Tag = o
            };
            item.Items.Add("Loading...");
            return item;
        }


        private void treeView1_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            UpdateDisplayPanel();

            menuItemReplace.IsEnabled = treeView1.SelectedItem is FileRecord;
            menuItemView.IsEnabled = treeView1.SelectedItem is FileRecord;

            if (treeView1.SelectedItem is FileRecord)
            {
                // Exporting file
                menuItemExport.IsEnabled = true;
            }
            else if (treeView1.SelectedItem is TreeViewItem && (treeView1.SelectedItem as TreeViewItem).Header is DirectoryTreeNode)
            {
                // Exporting entire directory
                menuItemExport.IsEnabled = true;
            }
            else
            {
                menuItemExport.IsEnabled = false;
            }
        }

        private void treeView1_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            var source = sender as TreeView;

            var hitPoint = e.GetPosition(source);
            var hitElement = (DependencyObject)source.InputHitTest(hitPoint);
            while (hitElement != null && !(hitElement is TreeViewItem))
            {
                hitElement = VisualTreeHelper.GetParent((DependencyObject)hitElement);
            }

            if (hitElement != null)
            {
                var fileToView = (hitElement as TreeViewItem).DataContext as FileRecord;
                if (fileToView == null)
                    return;

                OpenFileRecord(fileToView);
            }
        }

        private void treeView1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                var clickedItem = e.OriginalSource as UIElement;

                while ((clickedItem != null) && !(clickedItem is TreeViewItem))
                {
                    clickedItem = VisualTreeHelper.GetParent(clickedItem) as UIElement;
                }
                TreeRightClickSelectedFile = clickedItem as TreeViewItem;
            }
            e.Handled = false;
        }

        private void menuItemExport_Click(object sender, RoutedEventArgs e)
        {
            if (treeView1.SelectedItem is TreeViewItem)
            {
                var selectedTreeViewItem = treeView1.SelectedItem as TreeViewItem;
                var selectedDirectoryNode = selectedTreeViewItem.Header as DirectoryTreeNode;
                if (selectedDirectoryNode != null)
                {
                    ExportAllItemsInDirectory(selectedDirectoryNode);
                }
            }
            else if (treeView1.SelectedItem is FileRecord)
            {
                ExportFileRecord(treeView1.SelectedItem as FileRecord);
            }
        }

        private void menuItemReplace_Click(object sender, RoutedEventArgs e)
        {
            var recordToReplace = treeView1.SelectedItem as FileRecord;
            if (recordToReplace == null)
                return;

            ReplaceItem(recordToReplace);
        }

        private void menuItemView_Click(object sender, RoutedEventArgs e)
        {
            var viewItem = treeView1.SelectedItem as TreeViewItem;
            if (viewItem == null)
                return;
            var fileToView = viewItem.DataContext as FileRecord;
            if (fileToView == null)
                return;

            OpenFileRecord(fileToView);
        }

        private void menuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            if (TreeRightClickSelectedFile == null)
                return;

            if (TreeRightClickSelectedFile.Header is BaseRecord)
            {
                // TODO actually delete GGPK() records
                // var file1 = TreeRightClickSelectedFile.Header as BaseRecord;
                // content.DeleteRecord(file);
            }
            var parent = TreeRightClickSelectedFile.Parent;
            if (parent == null)
            {
                parent = VisualTreeHelper.GetParent(TreeRightClickSelectedFile);
                while (parent != null && !(parent is TreeViewItem))
                {
                    parent = VisualTreeHelper.GetParent(parent);
                }
            }
            if (!(parent is TreeViewItem))
                return;
            var treeParent = parent as TreeViewItem;
            treeParent.Items.Remove(TreeRightClickSelectedFile);
            TreeRightClickSelectedFile = null;
        }
    }
}
