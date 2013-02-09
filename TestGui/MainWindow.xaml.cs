using LibDat;
using LibDat.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

//
// Warning: Reflection horrors ahead!
// Warning: Reflection horrors ahead!
// Warning: Reflection horrors ahead!
//

namespace TestGui
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private DatWrapper data;
		string fileName = @"C:\Program Files (x86)\Grinding Gear Games\Path of Exile\dataExtracted\Data\ActiveSkills.dat";
		
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


		public MainWindow()
		{
			data = new DatWrapper(fileName);
			DataContext = this;

			InitializeComponent();
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			data.Save(fileName + ".new.dat");
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
				using (BinaryReader br = new BinaryReader(ms))
				{
					ParseDatFile(br);
				}
			}
		}

		private void ParseDatFile(BinaryReader br)
		{
			datTypeInUse = Type.GetType(string.Format("LibDat.Files.{0}, LibDat", Path.GetFileNameWithoutExtension(fileName)));
			if (datTypeInUse == null)
			{
				throw new Exception("No handler for file " + fileName);
			}


			datContainerType = typeof(DatContainer<>).MakeGenericType(new Type[] { datTypeInUse });
			datContainer = Activator.CreateInstance(datContainerType, new object[] { fileName });

			Entries = datContainerType.GetField("Entries").GetValue(datContainer) as System.Collections.IEnumerable;

			try
			{
				Dictionary<int, BaseData> dataEntries = (Dictionary<int, BaseData>)datContainerType.GetField("DataEntries").GetValue(datContainer);
				var containerData = dataEntries.ToList();

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
				throw new Exception("Failed to read dat: " + ex.Message);
			}
		}

		public DatWrapper(BinaryReader inStream, string fileName)
		{
			this.fileName = fileName;

			ParseDatFile(inStream);
		}

		public void Save(string savePath)
		{
			var saveContainer = datContainerType.GetMethod("Save", new Type[] { typeof(string) });
			saveContainer.Invoke(datContainer, new object[] { fileName + ".new.dat" });
		}
	}
}
