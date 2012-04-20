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

namespace VisualGGPK
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private string ggpkPath = String.Empty;
		GGPK content = null;

		public MainWindow()
		{
			InitializeComponent();
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
			ofd.Filter = "GGPK Pack File|*.ggpk";
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
				content.Read(ggpkPath, Output);

				Output("Traversing tree....\n");
				OnReadComplete();
			}));

			worker.Start();
		}

		private void OnReadComplete()
		{
			treeView1.Dispatcher.BeginInvoke(new Action(() =>
			{
				AddDirectoryTreeToControl(content.DirectoryRoot, null);
			}), null);
			Output("All done!\n");
		}

		private void AddDirectoryTreeToControl(DirectoryTreeNode directoryTreeNode, TreeViewItem parentControl)
		{
			TreeViewItem rootItem = new TreeViewItem(); ;
			rootItem.Header = directoryTreeNode;

			if (parentControl == null)
			{
				treeView1.Items.Add(rootItem);
			}
			else
			{
				parentControl.Items.Add(rootItem);
			}

			foreach (var item in directoryTreeNode.Children)
			{
				AddDirectoryTreeToControl(item, rootItem);
			}

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
					case FileRecord.DataFormat.CommaSeperatedValue:
						DisplayCSV(selectedRecord);
						break;
					case FileRecord.DataFormat.RichText:
						DisplayRichText(selectedRecord);
						break;
					default:
						break;
				}
			}
			catch (Exception)
			{
				ResetViewer();
				textBoxOutput.Visibility = System.Windows.Visibility.Visible;
				textBoxOutput.Text = " * Unable to view item, export it if you want to view it *";
			}

		}

		private void DisplayCSV(FileRecord selectedRecord)
		{
			string tempFile = selectedRecord.ExtractTempFile(ggpkPath);

			dataGridOutput.Visibility = System.Windows.Visibility.Visible;
			DataTable csvData = FileHelpers.CsvEngine.CsvToDataTable(tempFile, ',');

			dataGridOutput.AutoGenerateColumns = true;
			dataGridOutput.ItemsSource = csvData.DefaultView;
			File.Delete(tempFile);
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

			textBoxOutput.Text = ASCIIEncoding.Unicode.GetString(buffer);
		}

		private void DisplayAscii(FileRecord selectedRecord)
		{
			byte[] buffer = selectedRecord.ReadData(ggpkPath);
			textBoxOutput.Visibility = System.Windows.Visibility.Visible;

			textBoxOutput.Text = ASCIIEncoding.ASCII.GetString(buffer);
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
					MessageBox.Show(string.Format("Exported {0} bytes", selectedRecord.DataLength));
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Failed to export item: " + ex.Message);
				return;
			}
		}

		private void ExportAllItemsInDirectory(DirectoryTreeNode selectedDirectoryNode)
		{
			List<FileRecord> recordsToExport = new List<FileRecord>();

			Action<FileRecord> fileAction = new Action<FileRecord>(record =>
			{
				recordsToExport.Add(record);
			});

			DirectoryTreeNode.TraverseTreePreorder(selectedDirectoryNode, null, fileAction);

			try
			{
				SaveFileDialog saveFileDialog = new SaveFileDialog();
				saveFileDialog.FileName = "selectedRecord.Name";
				if (saveFileDialog.ShowDialog() == true)
				{
					string exportDirectory = Path.GetDirectoryName(saveFileDialog.FileName) + "/";
					foreach (var item in recordsToExport)
					{
						item.ExtractFileWithDirectoryStructure(ggpkPath, exportDirectory);
					}
					MessageBox.Show(string.Format("Exported {0} files", recordsToExport.Count));
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Failed to export item: " + ex.Message);
			}
		}

		private void treeView1_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			UpdateDisplayPanel();

			menuItemReplace.IsEnabled = treeView1.SelectedItem is FileRecord;

			if (treeView1.SelectedItem is FileRecord)
			{
				menuItemExport.IsEnabled = true;
			}
			else if (treeView1.SelectedItem is TreeViewItem && (treeView1.SelectedItem as TreeViewItem).Header is DirectoryTreeNode)
			{
				menuItemExport.IsEnabled = true;
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
					MessageBox.Show(String.Format("Record {0} updated and relocated to offset {1}", recordToReplace.Name, recordToReplace.RecordBegin.ToString("X")));
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Failed to export item: " + ex.Message);
			}

			ReloadGGPK();
		}




	}
}
