using LibDat;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace VisualGGPK
{
	/// <summary>
	/// Interaction logic for DatViewer.xaml
	/// </summary>
	public partial class DatViewer : UserControl
	{
		private DatWrapper data;
		public string FileName { get; set; }

		public DatViewer(string filename, BinaryReader inStream)
		{
			this.FileName = filename;
			data = new DatWrapper(inStream, filename);
			InitializeComponent();
			DataContext = this;
		}

		public DatViewer(string filename)
		{
			this.FileName = filename;
			data = new DatWrapper(filename);
			InitializeComponent();
			DataContext = this;
		}

		public DatViewer()
		{
			InitializeComponent();
			DataContext = this;
		}

		public void Reset(string filename, BinaryReader inStream)
		{
			this.FileName = filename;
			data = new DatWrapper(inStream, filename);
			DataContext = null;
			DataContext = this;
		}

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

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.DefaultExt = ".dat";
			sfd.FileName = Path.GetFileNameWithoutExtension(FileName) + "_NEW.dat";

			if (sfd.ShowDialog() == true)
			{
				data.Save(sfd.FileName);
			}
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.FileName = Path.GetFileNameWithoutExtension(FileName) + ".csv";

			if (sfd.ShowDialog() == true)
			{
				try
				{
					File.WriteAllText(sfd.FileName, data.GetCSV());
				}
				catch (Exception ex)
				{
					MessageBox.Show("Failed to save CSV file: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				MessageBox.Show("Saved CSV file to " + sfd.FileName, "Successfully saved CSV", MessageBoxButton.OK, MessageBoxImage.Information);
			}
		}
	}
	public class DatWrapper
	{
		private string fileName;
		private Type datTypeInUse;
		private object datContainer;
		private Type datContainerType;
		private readonly List<UnicodeString> _dataStrings = new List<UnicodeString>();

		public System.Collections.IEnumerable Entries { get; private set; }
		public Dictionary<int, BaseData> DataEntries { get; private set; }

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

			byte[] fileBytes = File.ReadAllBytes(fileName);

			using (MemoryStream ms = new MemoryStream(fileBytes))
			{
				using (BinaryReader br = new BinaryReader(ms, System.Text.Encoding.Unicode))
				{
					ParseDatFile(br);
				}
			}
		}

		public DatWrapper(BinaryReader inStream, string fileName)
		{
			this.fileName = fileName;
			ParseDatFile(inStream);
		}

		private void ParseDatFile(BinaryReader br)
		{
			datTypeInUse = Type.GetType(string.Format("LibDat.Files.{0}, LibDat", System.IO.Path.GetFileNameWithoutExtension(fileName)));
			if (datTypeInUse == null)
			{
				throw new Exception("No handler for file " + fileName);
			}

			datContainerType = typeof(DatContainer<>).MakeGenericType(new Type[] { datTypeInUse });
			datContainer = Activator.CreateInstance(datContainerType, new object[] { br });

			Entries = datContainerType.GetField("Entries").GetValue(datContainer) as System.Collections.IEnumerable;

			try
			{
				DataEntries = (Dictionary<int, BaseData>)datContainerType.GetField("DataEntries").GetValue(datContainer);
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
				throw new Exception("Failed to read dat: " + ex.Message, ex);
			}
		}



		public void Save(string savePath)
		{
			try
			{
				var saveContainer = datContainerType.GetMethod("Save", new Type[] { typeof(string) });
				saveContainer.Invoke(datContainer, new object[] { savePath });
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

				MessageBox.Show("Failed to save: " + errorString, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}

			MessageBox.Show("Saved '" + savePath + "'", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		/// <summary>
		/// Returns a CSV table with the contents of this dat container.
		/// </summary>
		/// <returns></returns>
		public string GetCSV()
		{
			char seperator = ',';
			StringBuilder sb = new StringBuilder();

			bool displayedHeader = false;
			foreach (var item in Entries)
			{
				var properties = item.GetType().GetProperties();

				if (!displayedHeader)
				{
					foreach (var propertyInfo in properties)
					{
						sb.AppendFormat("{0}{1}", propertyInfo.Name, seperator);
					}
					sb.Remove(sb.Length - 1, 1);
					sb.AppendLine();
					displayedHeader = true;
				}

				foreach (var propertyInfo in properties)
				{
					object fieldValue = propertyInfo.GetValue(item, null);
					object[] customAttributes = propertyInfo.GetCustomAttributes(false);

					if (customAttributes.Length > 0)
					{
						if (customAttributes.Any(n => n is StringIndex))
						{
							sb.AppendFormat("\"{0}\"{1}",  DataEntries[(int)fieldValue].ToString().Replace("\"", "\"\""), seperator);
						}
						else
						{
							sb.AppendFormat("{0}{1}", DataEntries[(int)fieldValue].ToString().Replace("\"", "\"\""), seperator);
						}
					}
					else
					{
						sb.AppendFormat("{0}{1}", fieldValue, seperator);
					}
				}
				sb.Remove(sb.Length - 1, 1);
				sb.AppendLine();
			}

			return sb.ToString();
		}
	}
}
