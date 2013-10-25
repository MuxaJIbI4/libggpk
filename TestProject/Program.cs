using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using LibGGPK;
using System.IO;
using System.Linq.Expressions;
using LibDat;
using LibDat.Files;

//4 bytes - Number of entries
//Entry 1
//Entry 2
//...
//Entry N
//8 bytes - BB BB BB BB BB BB BB BB
//String0 String1 String2 ...

namespace TestProject
{
	public class Program
	{
		static void Output(string msg)
		{
			Console.Write(msg);
		}

		public class DatFile
		{
			public string Name { get; set; }
		}

		public static void Main(string[] args)
		{
            if (args.Length != 1)
            {
                Console.WriteLine("Derp");
                return;
            }
            DumpDat(args[0]);
		    Console.WriteLine(  "Press any key to continue...");
		    Console.ReadLine(); //new Program();
		}

		private const string ggpkPath = @"o:\Program Files (x86)\Grinding Gear Games\Path of Exile\content.ggpk";
		Dictionary<string, FileRecord> RecordsByPath;

		public Program()
		{
			//GGPK content = new GGPK();
			//content.Read(ggpkPath, Output);
            //
			//RecordsByPath = new Dictionary<string, FileRecord>(content.RecordOffsets.Count);
			//DirectoryTreeNode.TraverseTreePostorder(content.DirectoryRoot, null, n => RecordsByPath.Add(n.GetDirectoryPath() + n.Name, n as FileRecord));
            //
			//foreach (var item in RecordsByPath)
			//{
			//	Console.WriteLine(item.Key + " -> " + item.Value.Name);
			//}
		}



























		private static void DumpDat(string filePath)
		{
			byte[] fileBytes = File.ReadAllBytes(filePath);
			StringBuilder sb = new StringBuilder();
			using (MemoryStream ms = new MemoryStream(fileBytes))
			{
				using (BinaryReader br = new BinaryReader(ms, System.Text.Encoding.Unicode))
				{
					int entryCount = br.ReadInt32();
					int dataTableStart = -1;

					while (true)
					{
						if (br.ReadByte() == 0xbb &&
							br.ReadByte() == 0xbb &&
							br.ReadByte() == 0xbb &&
							br.ReadByte() == 0xbb &&
							br.ReadByte() == 0xbb &&
							br.ReadByte() == 0xbb &&
							br.ReadByte() == 0xbb &&
							br.ReadByte() == 0xbb)
						{
							dataTableStart = (int)(br.BaseStream.Position - 8);
							break;
						}
					}


					br.BaseStream.Seek(4, SeekOrigin.Begin);

					int entrySize = dataTableStart / entryCount;
				    Console.WriteLine("0x{0:X2}", entrySize);
                    for (int i = 0; i < entrySize/4; i++)
				    {
				        sb.Append("Unknown" + i + "\t\t");
				    }
                    sb.AppendLine();

				    //sb.AppendLine(Path.GetFileNameWithoutExtension(filePath));
					for (int i = 0; i < entryCount; i++)
					{
						byte[] data = br.ReadBytes(entrySize);
						for (int j = 0; j < data.Length; j++)
						{
						    if (j != 0 && j%4 == 0)
						        sb.Append("\t");

							sb.AppendFormat("{0:X2} ", data[j]);
						}
						sb.AppendLine();
					}

					br.BaseStream.Seek(dataTableStart, SeekOrigin.Begin);
					File.WriteAllBytes(filePath + "_data.bin", br.ReadBytes((int)(br.BaseStream.Length - br.BaseStream.Position)));
				}
			}

			File.WriteAllText(filePath + "_bytes.txt", sb.ToString());
		}

		private static void DumpProps(string[] vars)
		{
			foreach (var @var in vars)
			{
				string lowerVar = var.ToLower();

				if (lowerVar.Contains("flag"))
					Console.WriteLine("		public bool {0} {{ get; set; }}", var);
				else if (lowerVar.Contains("64"))
					Console.WriteLine("		public Int64 {0} {{ get; set; }}", var);
				else
				{
					if (lowerVar.Contains("index"))
						Console.WriteLine("		[StringIndex]");
					else if (lowerVar.Contains("data"))
						Console.WriteLine("		[DataIndex]");

					Console.WriteLine("		public int {0} {{ get; set; }}", var);
				}
			}
		}

		private static void DumpRead(string[] vars, string className)
		{
			Console.WriteLine();
			Console.WriteLine("		public {0}(BinaryReader inStream)", className);
			Console.WriteLine("		{");
			foreach (var @var in vars)
			{
				string lowerVar = var.ToLower();

				if (lowerVar.Contains("flag"))
					Console.WriteLine("			{0} = inStream.ReadBoolean();", var);
				else if (lowerVar.Contains("64"))
					Console.WriteLine("			{0} = inStream.ReadInt64();", var);
				else
					Console.WriteLine("			{0} = inStream.ReadInt32();", var);
			}
			Console.WriteLine("		}");
		}

		private static void DumpWrite(string[] vars)
		{
			Console.WriteLine();
			Console.WriteLine("		public override void Save(BinaryWriter outStream)");
			Console.WriteLine("		{");
			foreach (var @var in vars)
			{
				string lowerVar = var.ToLower();

				Console.WriteLine("			outStream.Write({0});", var);
			}
			Console.WriteLine("		}");
		}

		private static void DumpSize(string[] vars)
		{
			int size = 0;

			foreach (var @var in vars)
			{
				string lowerVar = var.ToLower();

				if (lowerVar.Contains("flag"))
					size += 1;
				else if (lowerVar.Contains("64"))
					size += 8;
				else
					size += 4;
			}

			Console.WriteLine();
			Console.WriteLine("		public override int GetSize()");
			Console.WriteLine("		{");
			Console.WriteLine("			return 0x{0:X};", size);
			Console.WriteLine("		}");
		}

		private static void ReadAndDumpStruct()
		{
			string className = Console.ReadLine().Trim();
			string input = Console.ReadLine().Trim();
			string previousInput = input;
			Console.Clear();
			do
			{
				previousInput = input;
				input = input.Replace("\t\t", "\t");
			} while (previousInput != input);

			string[] varNames = input.Split(new char[] { '\t' });

			Console.WriteLine("using System.IO;");
			Console.WriteLine();
			Console.WriteLine("namespace LibDat.Files");
			Console.WriteLine("{");
			Console.WriteLine("	public class {0} : BaseDat", className);
			Console.WriteLine("	{");
			DumpProps(varNames);
			DumpRead(varNames, className);
			DumpWrite(varNames);
			DumpSize(varNames);
			Console.WriteLine("	}");
			Console.WriteLine("}");

			Console.ReadKey();
		}
	}
}