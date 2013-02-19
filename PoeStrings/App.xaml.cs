using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PoeStrings
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			if (e.Args.Length > 0)
			{
				Properties["FileToLoad"] = e.Args[0];
				MessageBox.Show("Test");
				Shutdown(0);
				return;
			}

			base.OnStartup(e);
		}
	}

}
