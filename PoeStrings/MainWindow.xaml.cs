using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Win32;

namespace PoeStrings
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private bool isApplyingTranslationOnStartup = false;
		private bool hasModifiedData = false;
		private readonly string ggpkPath;
		private readonly string binPath;
		private Backend backend;
		private string settingsPath = @".\translation.xml";

		public Dictionary<string, DatTranslation> AllDatTranslations
		{
			get
			{
				return backend.AllDatTranslations;
			}
		}

		private void Output(string text)
		{
			textBoxOutput.Dispatcher.BeginInvoke(new Action(() =>
			{
				this.textBoxOutput.AppendText(text);
				this.textBoxOutput.CaretIndex = this.textBoxOutput.Text.Length;
				this.textBoxOutput.ScrollToEnd();
			}), null);
		}

		public MainWindow()
		{
			if (Application.Current.Properties.Contains("FileToLoad"))
			{
				isApplyingTranslationOnStartup = true;
				settingsPath = (string)Application.Current.Properties["FileToLoad"];
			}

			InitializeComponent();

			buttonApplyAll.Content = Settings.Strings["MainWindow_Button_ApplyAll"];
			buttonSaveConfig.Content = Settings.Strings["MainWindow_Button_SaveConfig"];
			buttonApplyAllToFile.Content = Settings.Strings["MainWindow_Button_ApplyAllToFile"];
			buttonSerialize.Content = Settings.Strings["MainWindow_Button_Serialize"];

			OpenFileDialog ofd = new OpenFileDialog
			{
				AddExtension = true,
				CheckFileExists = true,
				DefaultExt = "ggpk",
				FileName = "Content.ggpk",
			};
			try
			{
				ofd.Filter = Settings.Strings["Load_GGPK_Filter"];

			}
			catch (Exception ex)
			{
				MessageBox.Show(string.Format(Settings.Strings["MainWindow_InvalidFileFilter"], ex.Message), Settings.Strings["Error"], MessageBoxButton.OK, MessageBoxImage.Error);
				this.Close();
				return;
			}

			if (ofd.ShowDialog() == true)
			{
				if (System.IO.Path.GetExtension(ofd.FileName).ToLower() == ".bin")
				{
					binPath = ofd.FileName;
					ofd.FileName = "Content.ggpk";
					if (ofd.ShowDialog() == true)
						ggpkPath = ofd.FileName;
					else
						return;
				} else
					ggpkPath = ofd.FileName;
			}
			else
			{
				return;
			}

			listBoxFiles.Items.SortDescriptions.Clear();
			listBoxFiles.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Key", System.ComponentModel.ListSortDirection.Ascending));

			Thread driverThread = new Thread(new ThreadStart(() =>
			{
				backend = new Backend(Output, settingsPath);
				if (binPath != null)
					backend.ReloadAllData(ggpkPath, binPath);
				else
					backend.ReloadAllData(ggpkPath);
				OnBackendLoaded();
			}));

			driverThread.Start();
		}

		private void OnBackendLoaded()
		{
			listBoxFiles.Dispatcher.BeginInvoke(new Action(UpdateBindings), null);

			if (isApplyingTranslationOnStartup)
			{
				this.Dispatcher.BeginInvoke(new Action(backend.ApplyTranslations), null);
			}
		}

		private void listBoxFiles_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
		{
			ListBox box = sender as ListBox;
			if (box == null || !(box.SelectedItem is KeyValuePair<string, DatTranslation>))
			{
				return;
			}

			KeyValuePair<string, DatTranslation> selected = (KeyValuePair<string, DatTranslation>)(box.SelectedItem);

			stringEditorMain.Translations = selected.Value.Translations;
		}

		public void UpdateBindings()
		{

			string previouslySelectedFileName = null;

			if ((listBoxFiles.SelectedItem != null) && (listBoxFiles.SelectedItem is KeyValuePair<string, DatTranslation>))
				previouslySelectedFileName = ((KeyValuePair<string, DatTranslation>)listBoxFiles.SelectedItem).Key;

			listBoxFiles.ItemsSource = null;
			listBoxFiles.ItemsSource = AllDatTranslations;
			stringEditorMain.Translations = null;
			stringEditorMain.DataContext = null;
			stringEditorMain.DataContext = this;

			if (previouslySelectedFileName != null && AllDatTranslations.ContainsKey(previouslySelectedFileName))
			{
				listBoxFiles.SelectedItem = new KeyValuePair<string, DatTranslation>(previouslySelectedFileName, AllDatTranslations[previouslySelectedFileName]);
			}
		}

		private void buttonSaveConfig_Click_1(object sender, RoutedEventArgs e)
		{
			hasModifiedData = false;
			stringEditorMain.HasModfiedData = false;
			backend.SaveTranslationData();
		}

		private void buttonApplyAll_Click_1(object sender, RoutedEventArgs e)
		{
			if (hasModifiedData || stringEditorMain.HasModfiedData)
			{
				PromptToSave();
			}
			hasModifiedData = false;
			stringEditorMain.HasModfiedData = false;

			backend.ApplyTranslations();
			//backend.ReloadAllData(ggpkPath);
			UpdateBindings();
		}

		private void buttonApplyAllToFile_Click_1(object sender, RoutedEventArgs e)
		{
			if (hasModifiedData || stringEditorMain.HasModfiedData)
			{
				PromptToSave();
			}
			hasModifiedData = false;
			stringEditorMain.HasModfiedData = false;

			backend.ApplyTranslationsToFile();
			//backend.ReloadAllData(ggpkPath);
			UpdateBindings();
		}

		private void buttonSerialize_Click_1(object sender, RoutedEventArgs e)
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
				backend.content.SerializeRecords(saveFileDialog.FileName, Output);
		}

		private void PromptToSave()
		{
			if (MessageBox.Show(Settings.Strings["WindowClosing_Save"], Settings.Strings["WindowClosing_Save_Title"], MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
			{
				backend.SaveTranslationData();
			}
		}

		private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (hasModifiedData || stringEditorMain.HasModfiedData)
			{
				PromptToSave();
			}
			hasModifiedData = false;
			stringEditorMain.HasModfiedData = false;
		}
    }
}
