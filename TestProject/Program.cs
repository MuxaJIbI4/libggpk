using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibGGPK;
using System.IO;

namespace TestProject
{
	class Program
	{
		static void Output(string msg)
		{
			Console.Write(msg);
		}


		
		static void Main(string[] args)
		{
			// TESTING PURPOSES ONLY

			//HashSet<string> extentions = new HashSet<string>();
			//var fileNames = Directory.GetFiles(@"\PoeExtracted", "*.*", SearchOption.AllDirectories);
			//foreach (var item in fileNames)
			//{
			//	string ext = Path.GetExtension(item).ToLower();
			//	extentions.Add(ext);
			//}
			//
			//var extList = extentions.ToList();
			//extList.Sort();
			//foreach (var item in extList)
			//{
			//	Console.WriteLine("\t{{\"{0}\", DataFormat.Unknown}},", item);
			//}
			GGPK content = new GGPK();
			content.Read(@"O:\Program Files (x86)\Grinding Gear Games\Path of Exile\content.ggpk", Output);

			Console.WriteLine("Records: " + content.RecordOffsets.Count());
		}


	}
}
