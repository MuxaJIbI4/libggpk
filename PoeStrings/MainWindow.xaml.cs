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
		private readonly string ggpkPath;
		private Backend backend;

		public Dictionary<string, DatTranslation> UserTranslations
		{
			get
			{
				return backend.UserTranslations;
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
			InitializeComponent();

			buttonApplyAll.Content = Settings.Strings["MainWindow_Button_ApplyAll"];
			buttonRevertAll.Content = Settings.Strings["MainWindow_Button_RevertAll"];
			buttonSaveConfig.Content = Settings.Strings["MainWindow_Button_SaveConfig"];

			OpenFileDialog ofd = new OpenFileDialog();
			ofd.FileName = "Content.ggpk";
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
			ofd.CheckFileExists = true;

			if (ofd.ShowDialog() == true)
			{
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
				backend = new Backend(Output);
				backend.ReloadAllData(ggpkPath);
				OnBackendLoaded();
			}));

			driverThread.Start();
		}

		private void OnBackendLoaded()
		{
			listBoxFiles.Dispatcher.BeginInvoke(new Action(UpdateBindings), null);
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
			listBoxFiles.ItemsSource = UserTranslations;
			stringEditorMain.Translations = null;
			stringEditorMain.DataContext = null;
			stringEditorMain.DataContext = this;

			if (previouslySelectedFileName != null && UserTranslations.ContainsKey(previouslySelectedFileName))
			{
				listBoxFiles.SelectedItem = new KeyValuePair<string, DatTranslation>(previouslySelectedFileName, UserTranslations[previouslySelectedFileName]);
			}
		}

		private void buttonSaveConfig_Click_1(object sender, RoutedEventArgs e)
		{
			backend.SaveTranslationData();
		}

		private void buttonApplyAll_Click_1(object sender, RoutedEventArgs e)
		{
			backend.ApplyAllTranslations();
			backend.ReloadAllData(ggpkPath);
			UpdateBindings();
		}

		private void buttonRevertAll_Click_1(object sender, RoutedEventArgs e)
		{
			backend.RevertAllTranslations();
			backend.ReloadAllData(ggpkPath);
			UpdateBindings();
		}
	}
}
