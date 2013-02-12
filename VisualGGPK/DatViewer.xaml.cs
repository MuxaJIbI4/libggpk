using LibDat;
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

namespace VisualGGPK
{
	/// <summary>
	/// Interaction logic for DatViewer.xaml
	/// </summary>
	public partial class DatViewer : UserControl
	{
		private DatWrapper data;
		public string FileName { get; set; }
		public List<UnicodeString> DataStrings
		{
			get
			{
				return data.Strings;
			}
		}

		public System.Collections.IEnumerable Entries
		{
			get
			{
				return data.Entries;
			}
		}

		public DatViewer(string filename, Stream inStream)
		{
			this.FileName = filename;
			data = new DatWrapper(inStream, filename);
			InitializeComponent();
			DataContext = this;

			buttonSave.Content = Settings.Strings["DatViewer_Button_Save"];
			buttonExportCSV.Content = Settings.Strings["DatViewer_Button_Export"];
		}

		public DatViewer(string filename)
		{
			this.FileName = filename;
			data = new DatWrapper(filename);
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
			data = new DatWrapper(inStream, filename);
			DataContext = null;
			dataGridEntries.ItemsSource = null;
			dataGridEntries.Columns.Clear();

			// 
			if (data.Entries.Count <= 0)
			{
				return;
			}

			BuildGrid(data.Entries[0].GetType());

			DataContext = this;
		}

		private void SaveDat()
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.DefaultExt = ".dat";
			sfd.FileName = Path.GetFileNameWithoutExtension(FileName) + "_NEW.dat";

			if (sfd.ShowDialog() == true)
			{
				data.Save(sfd.FileName);
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
					File.WriteAllText(sfd.FileName, data.Dat.GetCSV());
				}
				catch (Exception ex)
				{
					MessageBox.Show(string.Format(Settings.Strings["DatViewer_ExportCSV_Failed"], ex.Message), Settings.Strings["Error_Caption"], MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				MessageBox.Show(string.Format(Settings.Strings["DatViewer_ExportCSV_Successful"], sfd.FileName), Settings.Strings["DatViewer_ExportCSV_Successful_Caption"], MessageBoxButton.OK, MessageBoxImage.Information);
			}
		}

		private void BuildGrid(Type datType)
		{
			// Add columns
			foreach (var propInfo in datType.GetProperties())
			{
				DataGridTextColumn col = new DataGridTextColumn();
				col.Header = propInfo.Name;
				col.Binding = new Binding(propInfo.Name);
				dataGridEntries.Columns.Add(col);
			}

			dataGridEntries.ItemsSource = Entries;
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
	public class DatWrapper
	{
		private string fileName;
		private string datName;
		private readonly List<UnicodeString> _dataStrings = new List<UnicodeString>();

		public DatContainer Dat { get; protected set; }
		public List<BaseDat> Entries 
		{
			get { return Dat.Entries; }
		}
		public Dictionary<int, BaseData> DataEntries 
		{
			get { return Dat.DataEntries; }
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
			Dat = new DatContainer(inStream, datName);

			try
			{
				var containerData = DataEntries.ToList();

				foreach (var keyValuePair in containerData)
				{
					if (keyValuePair.Value is UnicodeString)
					{
						Strings.Add((UnicodeString)keyValuePair.Value);
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
				Dat.Save(savePath);
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
	}
}
