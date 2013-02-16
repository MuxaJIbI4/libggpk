using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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


namespace PoeStrings
{
	/// <summary>
	/// Interaction logic for StringEditor.xaml
	/// </summary>
	public partial class StringEditor : UserControl
	{
		public List<Translation> Translations
		{
			get
			{
				return _translations;
			}
			set
			{
				_translations = value;
				UpdateStringList();
			}
		}
		private List<Translation> _translations;

		public Translation SelectedTranslation
		{
			get
			{
				return _selectedTranslation;
			}
			set
			{
				_selectedTranslation = value;
				UpdateTextBoxes();
			}
		}
		private Translation _selectedTranslation;

		public StringEditor()
		{
			InitializeComponent();

			labelTranslation.Content = Settings.Strings["StringEditor_Label_Original"];
			labelOriginal.Content = Settings.Strings["StringEditor_Label_Translation"];

			Translations = null;

			listBoxStrings.ItemsSource = Translations;
			listBoxStrings.DataContext = this;

			listBoxStrings.Items.SortDescriptions.Clear();
			listBoxStrings.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("OriginalText", System.ComponentModel.ListSortDirection.Ascending));
		}

		private void changed_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ListBox listBox = sender as ListBox;
			if (listBox == null)
			{
				return;
			}

			SelectedTranslation = listBox.SelectedItem as Translation;
		}

		private void UpdateStringList()
		{
			listBoxStrings.ItemsSource = null;
			listBoxStrings.ItemsSource = Translations;
		}

		private void UpdateTextBoxes()
		{
			textBoxOriginal.DataContext = null;
			textBoxOriginal.DataContext = this;
			textBoxReplacement.DataContext = null;
			textBoxReplacement.DataContext = this;
		}

		private void listBoxStrings_KeyDown_1(object sender, KeyEventArgs e)
		{
			ListBox source = sender as ListBox;
			if (source == null)
				return;
			if (e.Key != Key.Delete)
				return;

			Translation sourceTranslation = source.SelectedItem as Translation;
			if (sourceTranslation == null)
				return;

			if (MessageBox.Show(string.Format(Settings.Strings["StringEditor_DeleteTranslation"], sourceTranslation.ShortNameCurrent), Settings.Strings["StringEditor_DeleteTranslation_Caption"], MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
			{
				Translations.Remove(sourceTranslation);
				UpdateStringList();
			}
		}
	}
}
