using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Ionic.Zip;
using LibGGPK;
using System.IO;
using System.Data;
using Microsoft.Win32;
using System.Threading;
using System.Diagnostics;
using KUtility;
using System.Drawing.Imaging;

namespace VisualGGPK
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private string ggpkPath = String.Empty;
		private GGPK content = null;
		private Thread workerThread = null;
		/// <summary>
		/// Dictionary mapping ggpk file paths to FileRecords for easy lookup
		/// EG: "Scripts\foobar.mel" -> FileRecord{Foobar.mel}
		/// </summary>
		Dictionary<string, FileRecord> RecordsByPath;

		public MainWindow()
		{
			InitializeComponent();
		}

		private void OutputLine(string msg)
		{
			Output(msg + Environment.NewLine);
		}

		void Output(string msg)
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
		private void ReloadGGPK()
		{
			treeView1.Items.Clear();
			ResetViewer();
			textBoxOutput.Visibility = System.Windows.Visibility.Visible;
			textBoxOutput.Text = string.Empty;
			content = null;

			workerThread = new Thread(new ThreadStart(() =>
			{
				content = new GGPK();
				try
				{
					content.Read(ggpkPath, Output);
				}
				catch (Exception ex)
				{
					Output(string.Format(Settings.Strings["ReloadGGPK_Failed"], ex.Message));
					return;
				}

                if (content.IsReadOnly)
                {
                    Output(Settings.Strings["ReloadGGPK_ReadOnly"] + Environment.NewLine);
                    UpdateTitle(Settings.Strings["MainWindow_Title_Readonly"]);
                }

				OutputLine(Settings.Strings["ReloadGGPK_Traversing_Tree"]);

				// Collect all FileRecordPath -> FileRecord pairs for easier replacing
				RecordsByPath = new Dictionary<string, FileRecord>(content.RecordOffsets.Count);
				DirectoryTreeNode.TraverseTreePostorder(content.DirectoryRoot, null, n => RecordsByPath.Add(n.GetDirectoryPath() + n.Name, n as FileRecord));

				treeView1.Dispatcher.BeginInvoke(new Action(() =>
				{
					try
					{
						AddDirectoryTreeToControl(content.DirectoryRoot, null);
					}
					catch (Exception ex)
					{
						Output(string.Format(Settings.Strings["Error_Read_Directory_Tree"], ex.Message));
						return;
					}

					workerThread = null;
				}), null);


				OutputLine(Settings.Strings["ReloadGGPK_Successful"]);
			}));

			workerThread.Start();
		}

		/// <summary>
		/// Recursivly adds the specified GGPK DirectoryTree to the TreeListView
		/// </summary>
		/// <param name="directoryTreeNode">Node to add to tree</param>
		/// <param name="parentControl">TreeViewItem to add children to</param>
		private void AddDirectoryTreeToControl(DirectoryTreeNode directoryTreeNode, TreeViewItem parentControl)
		{
			TreeViewItem rootItem = new TreeViewItem();
			rootItem.Header = directoryTreeNode;

			if (parentControl == null)
			{
				treeView1.Items.Add(rootItem);
			}
			else
			{
				parentControl.Items.Add(rootItem);
			}

			directoryTreeNode.Children.Sort();
			foreach (var item in directoryTreeNode.Children)
			{
				AddDirectoryTreeToControl(item, rootItem);
			}

			directoryTreeNode.Files.Sort();
			foreach (var item in directoryTreeNode.Files)
			{
				rootItem.Items.Add(item);
			}
		}

		/// <summary>
		/// Resets all of the file viewers
		/// </summary>
		private void ResetViewer()
		{
			textBoxOutput.Visibility = System.Windows.Visibility.Hidden;
			imageOutput.Visibility = System.Windows.Visibility.Hidden;
			richTextOutput.Visibility = System.Windows.Visibility.Hidden;
			dataGridOutput.Visibility = System.Windows.Visibility.Hidden;
			datViewerOutput.Visibility = System.Windows.Visibility.Hidden;

			textBoxOutput.Clear();
			imageOutput.Source = null;
			richTextOutput.Document.Blocks.Clear();
			dataGridOutput.ItemsSource = null;
			textBoxOffset.Text = String.Empty;
			textBoxSize.Text = String.Empty;
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
				DirectoryTreeNode selectedDirectory = (treeView1.SelectedItem as TreeViewItem).Header as DirectoryTreeNode;
				if (selectedDirectory.Record == null)
					return;

				textBoxOffset.Text = selectedDirectory.Record.RecordBegin.ToString("X");
				textBoxSize.Text = String.Empty;
				return;
			}

			FileRecord selectedRecord = treeView1.SelectedItem as FileRecord;
			if (selectedRecord == null)
				return;

			textBoxOffset.Text = selectedRecord.RecordBegin.ToString("X");
			textBoxSize.Text = selectedRecord.DataLength.ToString();

			try
			{
				switch (selectedRecord.FileFormat)
				{
					case FileRecord.DataFormat.Image:
						DisplayImage(selectedRecord);
                        break;
                    case FileRecord.DataFormat.TextureDDS:
                        DisplayDDS(selectedRecord);
                        break;
					case FileRecord.DataFormat.Ascii:
						DisplayAscii(selectedRecord);
						break;
					case FileRecord.DataFormat.Unicode:
						DisplayUnicode(selectedRecord);
						break;
					case FileRecord.DataFormat.RichText:
						DisplayRichText(selectedRecord);
						break;
					case FileRecord.DataFormat.Dat:
						DisplayDat(selectedRecord);
						break;
					default:
						break;
				}
			}
			catch (Exception ex)
			{
				ResetViewer();
				textBoxOffset.Text = selectedRecord.RecordBegin.ToString("X");
				textBoxSize.Text = selectedRecord.DataLength.ToString();
				textBoxOutput.Visibility = System.Windows.Visibility.Visible;

				StringBuilder sb = new StringBuilder();
				while (ex != null)
				{
					sb.AppendLine(ex.Message);
					ex = ex.InnerException;
				} 

				textBoxOutput.Text = string.Format(Settings.Strings["UpdateDisplayPanel_Failed"], sb.ToString());
			}

		}

		/// <summary>
		/// Displays the contents of a FileRecord in the DatViewer
		/// </summary>
		/// <param name="selectedRecord">FileRecord to display</param>
		private void DisplayDat(FileRecord selectedRecord)
		{
			byte[] data = selectedRecord.ReadData(ggpkPath);
			datViewerOutput.Visibility = System.Windows.Visibility.Visible;

			using (MemoryStream ms = new MemoryStream(data))
			{
				datViewerOutput.Reset(selectedRecord.Name, ms);
			}
		}

		/// <summary>
		/// Displays the contents of a FileRecord in the RichTextBox
		/// </summary>
		/// <param name="selectedRecord">FileRecord to display</param>
		private void DisplayRichText(FileRecord selectedRecord)
		{
			byte[] buffer = selectedRecord.ReadData(ggpkPath);
			richTextOutput.Visibility = System.Windows.Visibility.Visible;

			using (MemoryStream ms = new MemoryStream(buffer))
			{
				richTextOutput.Selection.Load(ms, DataFormats.Rtf);
			}
		}

		/// <summary>
		/// Displays the contents of a FileRecord in the TextBox as Unicode text
		/// </summary>
		/// <param name="selectedRecord">FileRecord to display</param>
		private void DisplayUnicode(FileRecord selectedRecord)
		{
			byte[] buffer = selectedRecord.ReadData(ggpkPath);
			textBoxOutput.Visibility = System.Windows.Visibility.Visible;

			textBoxOutput.Text = Encoding.Unicode.GetString(buffer);
		}

		/// <summary>
		/// Displays the contents of a FileRecord in the TextBox as Ascii text
		/// </summary>
		/// <param name="selectedRecord">FileRecord to display</param>
		private void DisplayAscii(FileRecord selectedRecord)
		{
			byte[] buffer = selectedRecord.ReadData(ggpkPath);
			textBoxOutput.Visibility = System.Windows.Visibility.Visible;

			textBoxOutput.Text = Encoding.ASCII.GetString(buffer);
		}

		/// <summary>
		/// Displays the contents of a FileRecord in the ImageBox
		/// </summary>
		/// <param name="selectedRecord">FileRecord to display</param>
		private void DisplayImage(FileRecord selectedRecord)
		{
			byte[] buffer = selectedRecord.ReadData(ggpkPath);
			imageOutput.Visibility = System.Windows.Visibility.Visible;

			using (MemoryStream ms = new MemoryStream(buffer))
			{
				BitmapImage bmp = new BitmapImage();
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
        private void DisplayDDS(FileRecord selectedRecord)
        {
            byte[] buffer = selectedRecord.ReadData(ggpkPath);
            imageOutput.Visibility = System.Windows.Visibility.Visible;

            DDSImage dds = new DDSImage(buffer);
                
			using (MemoryStream ms = new MemoryStream())
			{
                dds.images[0].Save(ms, ImageFormat.Png);

				BitmapImage bmp = new BitmapImage();
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
				SaveFileDialog saveFileDialog = new SaveFileDialog();
				saveFileDialog.FileName = selectedRecord.Name;
				if (saveFileDialog.ShowDialog() == true)
				{
					selectedRecord.ExtractFile(ggpkPath, saveFileDialog.FileName);
					MessageBox.Show(string.Format(Settings.Strings["ExportSelectedItem_Successful"], selectedRecord.DataLength), Settings.Strings["ExportAllItemsInDirectory_Successful_Caption"], MessageBoxButton.OK, MessageBoxImage.Information);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(string.Format(Settings.Strings["ExportSelectedItem_Failed"], ex.Message), Settings.Strings["Error_Caption"], MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
		}

		/// <summary>
		/// Attempts to display the specified record on the gui
		/// </summary>
		/// <param name="selectedRecord">Record to view</param>
		private void ViewFileRecord(FileRecord selectedRecord)
		{
			string extractedFileName;
			try
			{
				extractedFileName = selectedRecord.ExtractTempFile(ggpkPath);

				// If we're dealing with .dat files then just create a human readable CSV and view that instead
				if (Path.GetExtension(selectedRecord.Name).ToLower() == ".dat")
				{
					string extractedCSV = Path.GetTempFileName();
					File.Move(extractedCSV, extractedCSV + ".csv");
					extractedCSV = extractedCSV + ".csv";

					using (FileStream inStream = File.Open(extractedFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
					{
						DatWrapper tempWrapper = new DatWrapper(inStream, selectedRecord.Name);
						File.WriteAllText(extractedCSV, tempWrapper.Dat.GetCSV());
					}

					File.Delete(extractedFileName);
					extractedFileName = extractedCSV;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(string.Format(Settings.Strings["ViewSelectedItem_Failed"], ex.Message), Settings.Strings["Error_Caption"], MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			Process fileViewerProcess = new Process();
			fileViewerProcess.StartInfo = new ProcessStartInfo(extractedFileName);
			fileViewerProcess.EnableRaisingEvents = true;
			fileViewerProcess.Exited += fileViewerProcess_Exited;
			fileViewerProcess.Start();
		}

		/// <summary>
		/// Exports entire DirectoryTreeNode to disk, preserving directory structure
		/// </summary>
		/// <param name="selectedDirectoryNode">Node to export to disk</param>
		private void ExportAllItemsInDirectory(DirectoryTreeNode selectedDirectoryNode)
		{
			List<FileRecord> recordsToExport = new List<FileRecord>();

			Action<FileRecord> fileAction = new Action<FileRecord>(recordsToExport.Add);

			DirectoryTreeNode.TraverseTreePreorder(selectedDirectoryNode, null, fileAction);

			try
			{
				SaveFileDialog saveFileDialog = new SaveFileDialog();
				saveFileDialog.FileName = Settings.Strings["ExportAllItemsInDirectory_Default_FileName"];
				if (saveFileDialog.ShowDialog() == true)
				{
					string exportDirectory = Path.GetDirectoryName(saveFileDialog.FileName) + Path.DirectorySeparatorChar;
					foreach (var item in recordsToExport)
					{
						item.ExtractFileWithDirectoryStructure(ggpkPath, exportDirectory);
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
            if (content.IsReadOnly)
            {
                MessageBox.Show(Settings.Strings["ReplaceItem_Readonly"], Settings.Strings["ReplaceItem_ReadonlyCaption"]);
                return;
            }

			try
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.FileName = "";
				openFileDialog.CheckFileExists = true;
				openFileDialog.CheckPathExists = true;

				if (openFileDialog.ShowDialog() == true)
				{
					long previousOffset = recordToReplace.RecordBegin;

					recordToReplace.ReplaceContents(ggpkPath, openFileDialog.FileName, content.FreeRoot);
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
		/// Constructs the full GGPK path of the specified DirectoryTreeNode.
		/// EG:
		/// Directory 'BuffIcons' has parent '2DArt', which has parent 'Art', so the DirectoryPath
		/// would result int Art\2DArt\BuffIcons\
		/// </summary>
		/// <param name="containingDirectory">Directory to get full path from</param>
		/// <returns>Full path (in GGPK structure) of specified directory</returns>
		public string GetDirectoryPath(DirectoryTreeNode containingDirectory)
		{
			Stack<string> pathQueue = new Stack<string>();
			StringBuilder sb = new StringBuilder();

			// Traverse the directory tree until we hit the root node, pushing all
			//  encountered directory names onto the stack
			DirectoryTreeNode iter = containingDirectory;
			while (iter != null && iter.Name.Length > 0)
			{
				pathQueue.Push(iter.Name);
				iter = iter.Parent;
			}

			foreach (var item in pathQueue)
			{
				sb.Append(item + Path.DirectorySeparatorChar);
			}

			return sb.ToString();
		}

		/// <summary>
		/// Extracts specified archive and replaces files in GGPK with extracted files. Files in
		/// archive must have same directory structure as in GGPK.
		/// </summary>
		/// <param name="archivePath">Path to archive containing</param>
		private void HandleDropArchive(string archivePath)
		{
            if (content.IsReadOnly)
            {
                MessageBox.Show(Settings.Strings["ReplaceItem_Readonly"], Settings.Strings["ReplaceItem_ReadonlyCaption"]);
                return;
            }

			OutputLine(string.Format(Settings.Strings["MainWindow_HandleDropArchive_Info"], archivePath));

			using (ZipFile zipFile = new ZipFile(archivePath))
			{
				var fileNames = zipFile.EntryFileNames;
				foreach (var item in zipFile.Entries)
				{
					if (item.IsDirectory)
					{
						continue;
					}

					string fixedFileName = item.FileName;
					if (Path.DirectorySeparatorChar != '/')
					{
						fixedFileName = fixedFileName.Replace('/', Path.DirectorySeparatorChar);
					}

					if (!RecordsByPath.ContainsKey(fixedFileName))
					{
						OutputLine(string.Format(Settings.Strings["MainWindow_HandleDropDirectory_Failed"], fixedFileName));
						continue;
					}
					OutputLine(string.Format(Settings.Strings["MainWindow_HandleDropDirectory_Replace"], fixedFileName));

					using (var reader = item.OpenReader())
					{
						byte[] replacementData = new byte[item.UncompressedSize];
						reader.Read(replacementData, 0, replacementData.Length);

						RecordsByPath[fixedFileName].ReplaceContents(ggpkPath, replacementData, content.FreeRoot);
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
            if (content.IsReadOnly)
            {
                MessageBox.Show(Settings.Strings["ReplaceItem_Readonly"], Settings.Strings["ReplaceItem_ReadonlyCaption"]);
                return;
            }

			FileRecord record = treeView1.SelectedItem as FileRecord;
			if (record == null)
			{
				OutputLine(Settings.Strings["MainWindow_HandleDropFile_Failed"]);
				return;
			}

			OutputLine(string.Format(Settings.Strings["MainWindow_HandleDropFile_Replace"], record.GetDirectoryPath(), record.Name));

			record.ReplaceContents(ggpkPath, fileName, content.FreeRoot);
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
            if (content.IsReadOnly)
            {
                MessageBox.Show(Settings.Strings["ReplaceItem_Readonly"], Settings.Strings["ReplaceItem_ReadonlyCaption"]);
                return;
            }

			string[] filesToReplace = Directory.GetFiles(baseDirectory, "*.*", SearchOption.AllDirectories);
			int baseDirectoryNameLength = Path.GetFileName(baseDirectory).Length;

			OutputLine(string.Format(Settings.Strings["MainWindow_HandleDropDirectory_Count"], filesToReplace.Length));
			foreach (var item in filesToReplace)
			{
				string fixedFileName = item.Remove(0, baseDirectory.Length - baseDirectoryNameLength);
				if (!RecordsByPath.ContainsKey(fixedFileName))
				{
					OutputLine(string.Format(Settings.Strings["MainWindow_HandleDropDirectory_Failed"], fixedFileName));
					continue;
				}
				OutputLine(string.Format(Settings.Strings["MainWindow_HandleDropDirectory_Replace"], fixedFileName));

				RecordsByPath[fixedFileName].ReplaceContents(ggpkPath, item, content.FreeRoot);
			}
		}



		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.CheckFileExists = true;
			ofd.Filter = Settings.Strings["Load_GGPK_Filter"];
			if (ofd.ShowDialog() == true)
			{
				if (!File.Exists(ofd.FileName))
				{
					this.Close();
					return;
				}
				else
				{
					ggpkPath = ofd.FileName;
					ReloadGGPK();
				}
			}
			else
			{
				this.Close();
				return;
			}

			menuItemExport.Header = Settings.Strings["MainWindow_Menu_Export"];
			menuItemReplace.Header = Settings.Strings["MainWindow_Menu_Replace"];
			menuItemView.Header = Settings.Strings["MainWindow_Menu_View"];
			labelFileOffset.Content = Settings.Strings["MainWindow_Label_FileOffset"];
		}

		private void fileViewerProcess_Exited(object sender, EventArgs e)
		{
			Process sourceProcess = sender as Process;
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

		private void menuItemExport_Click(object sender, RoutedEventArgs e)
		{
			if (treeView1.SelectedItem is TreeViewItem)
			{
				TreeViewItem selectedTreeViewItem = treeView1.SelectedItem as TreeViewItem;
				DirectoryTreeNode selectedDirectoryNode = selectedTreeViewItem.Header as DirectoryTreeNode;
				if (selectedDirectoryNode != null)
				{
					ExportAllItemsInDirectory(selectedDirectoryNode);
				}

				return;
			}
			else if (treeView1.SelectedItem is FileRecord)
			{
				ExportFileRecord(treeView1.SelectedItem as FileRecord);
			}
		}

		private void menuItemReplace_Click(object sender, RoutedEventArgs e)
		{
			FileRecord recordToReplace = treeView1.SelectedItem as FileRecord;
			if (recordToReplace == null)
				return;

			ReplaceItem(recordToReplace);
		}

		private void menuItemView_Click(object sender, RoutedEventArgs e)
		{
			FileRecord fileToView = (treeView1.SelectedItem as TreeViewItem).DataContext as FileRecord;
			if (fileToView == null)
				return;

			ViewFileRecord(fileToView);
		}

		private void treeView1_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
		{
			TreeView source = sender as TreeView;

			Point hitPoint = e.GetPosition(source);
			DependencyObject hitElement = (DependencyObject)source.InputHitTest(hitPoint);
			while (hitElement != null && !(hitElement is TreeViewItem))
			{
				hitElement = VisualTreeHelper.GetParent((DependencyObject)hitElement);
			}

			if (hitElement != null)
			{
				FileRecord fileToView = (hitElement as TreeViewItem).DataContext as FileRecord;
				if (fileToView == null)
					return;

				ViewFileRecord(fileToView);
			}
		}

		private void Window_PreviewDrop_1(object sender, DragEventArgs e)
		{
            if (!content.IsReadOnly)
            {
                e.Effects = DragDropEffects.Link;
            }
		}

		private void Window_Drop_1(object sender, DragEventArgs e)
		{
            if (content.IsReadOnly)
            {
                MessageBox.Show(Settings.Strings["ReplaceItem_Readonly"], Settings.Strings["ReplaceItem_ReadonlyCaption"]);
                return;
            }

			if (!e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
			{
				return;
			}

			// Brint-to-front hack
			this.Topmost = true;
			this.Topmost = false;

			textBoxOutput.Text = string.Empty;
			textBoxOutput.Visibility = System.Windows.Visibility.Visible;

			if (MessageBox.Show(Settings.Strings["MainWindow_Window_Drop_Confirm"], Settings.Strings["MainWindow_Window_Drop_Confirm_Caption"], MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
			{
				return;
			}

			string[] fileNames = e.Data.GetData(System.Windows.DataFormats.FileDrop) as string[];
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

		private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (workerThread != null)
			{
				workerThread.Abort();
			}
		}
	}
}
