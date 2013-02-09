using LibDat;
using System;
using System.Collections.Generic;
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

namespace TestGui
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();


			try
			{
				var container = new DatContainer(@"C:\Program Files (x86)\Grinding Gear Games\Path of Exile\dataExtracted\Data\BackendErrors.dat");

				dataGridMain.DataContext = this;
				dataGridMain.ItemsSource = container.Entries;
				//File.WriteAllText(datFiles[i] + ".csv", dump);
				//Console.WriteLine(dump);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Failed to read dat: " + ex.Message);
			}
		}
	}
}
