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
			////GGPK content = new GGPK();
			////content.Read(@"\Path of Exile\content.ggpk", Output);

			string ggpkPath = @"m:\path of exile\content.ggpk";

			GGPK content = new GGPK();
			content.Read(ggpkPath, Output);
			Output("Traversing tree....\n");

			// TESTING PURPOSES ONLY
			foreach (var item in content.RecordOffsets)
			{
				if (item.Value is FreeRecord)
				{
					FreeRecord freeData = (item.Value as FreeRecord);
					if (freeData.DataLength > 64)
					{
						byte[] tempData = freeData.ReadData(ggpkPath);
						byte[] data = new byte[tempData.Length-36];
						for (int i = 0; i < data.Length; i++)
						{
							data[i] = tempData[i+36];
						}

						MemoryStream ms = new MemoryStream(data);
						BinaryReader br = new BinaryReader(ms);
						string fileName;

						try
						{
							fileName = ReadUnicodeString(br);
							Path.GetFileName(fileName);
						}
						catch (Exception)
						{
							continue;
						}

						int rawFileDataOffset = (fileName.Length + 1) * 2;
						byte[] rawFileData = new byte[data.Length - rawFileDataOffset];
						for (int i = 0; i < rawFileData.Length; i++)
						{
							rawFileData[i] = data[i + rawFileDataOffset];
						}
						File.WriteAllBytes(@"h:\poeTemp\" + fileName, rawFileData);
						
					}
				}
			}
		}

		// TESTING PURPOSES ONLY
		private static string ReadUnicodeString(BinaryReader br)
		{
			StringBuilder sb = new StringBuilder();
			while (true)
			{
				char a = br.ReadChar();
				char b = br.ReadChar();
				if (a == 0 && b == 0)
				{
					break;
				}
				if (a == b || b != 0 || !isValidAsciiChar(a))
				{
					throw new Exception("Derp");
				}

				sb.Append(a);
			}
			if (sb.Length == 0)
				throw new Exception("herpdederp");

			return sb.ToString();
		}

		// TESTING PURPOSES ONLY
		private static bool isValidAsciiChar(char ch)
		{
			return ch >= 32 && ch <= 126;
		}

	}
}
