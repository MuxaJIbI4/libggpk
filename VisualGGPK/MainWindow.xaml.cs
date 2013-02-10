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
using LibGGPK;
using System.IO;
using System.Data;
using Microsoft.Win32;
using System.Threading;
using System.Diagnostics;

namespace VisualGGPK
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private string ggpkPath = String.Empty;
		private GGPK content = null;

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

		private void ReloadGGPK()
		{
			treeView1.Items.Clear();
			ResetViewer();
			textBoxOutput.Visibility = System.Windows.Visibility.Visible;
			textBoxOutput.Text = string.Empty;
			content = null;

			Thread worker = new Thread(new ThreadStart(() =>
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

				OutputLine(Settings.Strings["ReloadGGPK_Traversing_Tree"]);

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
				}), null);

				OutputLine(Settings.Strings["ReloadGGPK_Successful"]);
			}));

			worker.Start();
		}

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
		}

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
				return;
			}

			FileRecord selectedRecord = treeView1.SelectedItem as FileRecord;
			if (selectedRecord == null)
				return;

			textBoxOffset.Text = selectedRecord.RecordBegin.ToString("X");

			try
			{
				switch (selectedRecord.FileFormat)
				{
					case FileRecord.DataFormat.Image:
						DisplayImage(selectedRecord);
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

		private void DisplayDat(FileRecord selectedRecord)
		{
			byte[] data = selectedRecord.ReadData(ggpkPath);
			datViewerOutput.Visibility = System.Windows.Visibility.Visible;

			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms, System.Text.Encoding.Unicode))
				{
					datViewerOutput.Reset(selectedRecord.Name, br);
				}
			}
		}

		private void DisplayRichText(FileRecord selectedRecord)
		{
			byte[] buffer = selectedRecord.ReadData(ggpkPath);
			richTextOutput.Visibility = System.Windows.Visibility.Visible;

			using (MemoryStream ms = new MemoryStream(buffer))
			{
				richTextOutput.Selection.Load(ms, DataFormats.Rtf);
			}
		}

		private void DisplayUnicode(FileRecord selectedRecord)
		{
			byte[] buffer = selectedRecord.ReadData(ggpkPath);
			textBoxOutput.Visibility = System.Windows.Visibility.Visible;

			textBoxOutput.Text = Encoding.Unicode.GetString(buffer);
		}

		private void DisplayAscii(FileRecord selectedRecord)
		{
			byte[] buffer = selectedRecord.ReadData(ggpkPath);
			textBoxOutput.Visibility = System.Windows.Visibility.Visible;

			textBoxOutput.Text = Encoding.ASCII.GetString(buffer);
		}

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

		private void ExportSelectedItem(object selectedItem)
		{
			if (selectedItem == null)
			{
				return;
			}


			FileRecord selectedRecord = selectedItem as FileRecord;
			if (selectedRecord == null)
				return;

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

		private void ViewSelectedItem(object selectedItem)
		{
			if (selectedItem == null)
				return;

			FileRecord selectedRecord = selectedItem as FileRecord;
			if (selectedRecord == null)
				return;


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

					using (BinaryReader br = new BinaryReader(File.OpenRead(extractedFileName), System.Text.Encoding.Unicode))
					{
						DatWrapper tempWrapper = new DatWrapper(br, selectedRecord.Name);
						File.WriteAllText(extractedCSV, tempWrapper.GetCSV());
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
					string exportDirectory = Path.GetDirectoryName(saveFileDialog.FileName) + "/";
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

		private void ReplaceItem(FileRecord recordToReplace)
		{
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

					// this is actually needed to avoid writing to old records that have already been replaced. Replacing a record
					//   will relocate it and we'll need to refresh the whole tree to avoid any possible errors.
					ReloadGGPK();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(string.Format(Settings.Strings["ReplaceItem_Failed"], ex.Message), Settings.Strings["Error_Caption"], MessageBoxButton.OK, MessageBoxImage.Error);
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
				ExportSelectedItem(treeView1.SelectedItem);
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
			ViewSelectedItem(treeView1.SelectedItem);
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
				ViewSelectedItem((hitElement as TreeViewItem).DataContext);
			}
		}

		private void menuItemExit_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}
