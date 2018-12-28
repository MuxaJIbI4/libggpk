using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibGGPK;
using System.IO;
using System.Linq.Expressions;
using LibDat;
using Ionic.Zip;
using LibGGPK.Records;

namespace PatchGGPK
{
	class Program
	{
		static void Output(string msg)
		{
			Console.Write(msg);
		}

		private static void OutputLine(string msg)
		{
			Output(msg + Environment.NewLine);
		}

		private static string contentGGPK = @"\Content.ggpk";
		private static string ggpkPath = Directory.GetCurrentDirectory() + contentGGPK;
		private static Dictionary<string, FileRecord> RecordsByPath;
		private static GrindingGearsPackageContainer content = null;
        private static List<string> ggpkPaths = new List<string>();

		public static void Main(string[] args)
		{
			string[] archiveFiles;
			if (args.Length > 0)
			{
				archiveFiles = args;
			}
			else
			{
				archiveFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.zip");
			}
			if (archiveFiles.Length > 0)
			{
				InitGGPK();
				foreach (string archivePath in archiveFiles)
				{
					InitPatchArchive(archivePath);
				}
			}
			// Addons
			if (Directory.Exists(Directory.GetCurrentDirectory() + @"\Addons\")) 
			{ 
				archiveFiles = Directory.GetFiles(Directory.GetCurrentDirectory()+@"\Addons\", "*.zip");
				if (archiveFiles.Length > 0)
				{
					InitGGPK();
					foreach (string archivePath in archiveFiles)
					{
						string msg = string.Format("Apply {0} [y/N]? ", Path.GetFileName(archivePath));
						Output(msg);
						ConsoleKeyInfo cki = Console.ReadKey();
						OutputLine("");
						if (cki.Key == ConsoleKey.Y) 
						{
							InitPatchArchive(archivePath);
						}
					}
				}
			}
			OutputLine("Press any key to continue...");
			Console.ReadKey();
		}

		private static void searchContentGGPK()
		{
			string contentGGPK = @"\Content.ggpk";
			string ggpkPath = Directory.GetCurrentDirectory() + contentGGPK;
            Microsoft.Win32.RegistryKey start = Microsoft.Win32.Registry.LocalMachine;
			Microsoft.Win32.RegistryKey programName = start.OpenSubKey(@"SOFTWARE\Garena\POETW");

            if (File.Exists(ggpkPath))
            {
                if (!ggpkPaths.Exists(e => e.Equals(ggpkPath)))
                    ggpkPaths.Add(ggpkPath);
            }

			// GarenaTW
			start = Microsoft.Win32.Registry.LocalMachine;
			programName = start.OpenSubKey(@"SOFTWARE\Wow6432Node\Garena\POETW");
			if (programName != null)
			{
				string pathString = (string)programName.GetValue("Path");
				if (pathString != string.Empty && File.Exists(pathString + contentGGPK))
				{
					ggpkPath = pathString + contentGGPK;
                    if (!ggpkPaths.Exists(e => e.Equals(ggpkPath)))
                        ggpkPaths.Add(ggpkPath);
				}
			}
            // GarenaTW reg
			start = Microsoft.Win32.Registry.LocalMachine;
			programName = start.OpenSubKey(@"SOFTWARE\Garena\POETW");
			if (programName != null)
			{
				string pathString = (string)programName.GetValue("Path");
				if (pathString != string.Empty && File.Exists(pathString + contentGGPK))
				{
					ggpkPath = pathString + contentGGPK;
                    if (!ggpkPaths.Exists(e => e.Equals(ggpkPath)))
                        ggpkPaths.Add(ggpkPath);
				}
			}
            // GarenaTW x86 path
			if (File.Exists(@"C:\Program Files (x86)\GarenaPoETW\GameData\Apps\POETW" + contentGGPK))
			{
				ggpkPath = @"C:\Program Files (x86)\GarenaPoETW\GameData\Apps\POETW" + contentGGPK;
                if (!ggpkPaths.Exists(e => e.Equals(ggpkPath)))
                    ggpkPaths.Add(ggpkPath);
			}
			// GarenaTW 32b path
			if (File.Exists(@"C:\Program Files\GarenaPoETW\GameData\Apps\POETW" + contentGGPK))
			{
				ggpkPath = @"C:\Program Files\GarenaPoETW\GameData\Apps\POETW" + contentGGPK;
                if (!ggpkPaths.Exists(e => e.Equals(ggpkPath)))
                    ggpkPaths.Add(ggpkPath);
			}
			// GGG path
			start = Microsoft.Win32.Registry.CurrentUser;
			programName = start.OpenSubKey(@"Software\GrindingGearGames\Path of Exile");
			if (programName != null)
			{
				string pathString = (string)programName.GetValue("InstallLocation");
				if (pathString != string.Empty && File.Exists(pathString + contentGGPK))
				{
					ggpkPath = pathString + contentGGPK;
                    if (!ggpkPaths.Exists(e => e.Equals(ggpkPath)))
                        ggpkPaths.Add(ggpkPath);
				}
			}
			// GGG x86
			if (File.Exists(@"C:\Program Files (x86)\Grinding Gear Games\Path of Exile" + contentGGPK))
			{
				ggpkPath = @"C:\Program Files (x86)\Grinding Gear Games\Path of Exile" + contentGGPK;
                if (!ggpkPaths.Exists(e => e.Equals(ggpkPath)))
                    ggpkPaths.Add(ggpkPath);
			}
			// GGG 32b
			if (File.Exists(@"C:\Program Files\Grinding Gear Games\Path of Exile" + contentGGPK))
			{
				ggpkPath = @"C:\Program Files\Grinding Gear Games\Path of Exile" + contentGGPK;
                if (!ggpkPaths.Exists(e => e.Equals(ggpkPath)))
                    ggpkPaths.Add(ggpkPath);
			}
			// GGG reg
			start = Microsoft.Win32.Registry.LocalMachine;
			programName = start.OpenSubKey(@"SOFTWARE\Wow6432Node\Garena\PoE");
			if (programName != null)
			{
				string pathString = (string)programName.GetValue("Path");
				if (pathString != string.Empty && File.Exists(pathString + contentGGPK))
				{
					ggpkPath = pathString + contentGGPK;
                    if (!ggpkPaths.Exists(e => e.Equals(ggpkPath)))
                        ggpkPaths.Add(ggpkPath);
				}
			}
			// GareneHK reg
			start = Microsoft.Win32.Registry.LocalMachine;
			programName = start.OpenSubKey(@"SOFTWARE\Garena\PoE");
			if (programName != null)
			{
				string pathString = (string)programName.GetValue("Path");
				if (pathString != string.Empty && File.Exists(pathString + contentGGPK))
				{
					ggpkPath = pathString + contentGGPK;
                    if (!ggpkPaths.Exists(e => e.Equals(ggpkPath)))
                        ggpkPaths.Add(ggpkPath);
				}
			}
			// GarenaHK x64
			if (File.Exists(@"C:\Program Files (x86)\GarenaPoE\GameData\Apps\PoE" + contentGGPK))
			{
				ggpkPath = @"C:\Program Files (x86)\GarenaPoE\GameData\Apps\PoE" + contentGGPK;
                if (!ggpkPaths.Exists(e => e.Equals(ggpkPath)))
                    ggpkPaths.Add(ggpkPath);
			}
			// GarenaHK 32b
			if (File.Exists(@"C:\Program Files\GarenaPoE\GameData\Apps\PoE" + contentGGPK))
			{
				ggpkPath = @"C:\Program Files\GarenaPoE\GameData\Apps\PoE" + contentGGPK;
                if (!ggpkPaths.Exists(e => e.Equals(ggpkPath)))
                    ggpkPaths.Add(ggpkPath);
			}
		}

		private static void InitGGPK()
		{
			if (content != null)
				return;

			searchContentGGPK();

            if (ggpkPaths.Count == 1)
            {
                ggpkPath = ggpkPaths[0];
            }
            else if (ggpkPaths.Count > 1)
            {
                int pos = 0;
                foreach (string ggpkPath2 in ggpkPaths)
                {
                    OutputLine(string.Format("[{0}] {1}", pos++, ggpkPath2));
                }
                OutputLine(string.Format("Choose [0-{0}]: ", pos-1));
                ConsoleKeyInfo cki = Console.ReadKey();
                OutputLine("");
                Int32 number;
                if (Int32.TryParse(cki.KeyChar.ToString(), out number))
                {
                    if (number >= 0 && number <= pos-1)
                        ggpkPath = ggpkPaths[number];
                }
            }
            if (!File.Exists(ggpkPath))
            {
                OutputLine(string.Format("GGPK {0} not exists.", ggpkPath));
                return;
            }

			OutputLine(string.Format("Parsing {0}", ggpkPath));

			content = new GrindingGearsPackageContainer();
			content.Read(ggpkPath, Output);

			RecordsByPath = new Dictionary<string, FileRecord>(content.RecordOffsets.Count);
			DirectoryTreeNode.TraverseTreePostorder(content.DirectoryRoot, null, n => RecordsByPath.Add(n.GetDirectoryPath() + n.Name, n as FileRecord));
		}

		private static void InitPatchArchive(string archivePath)
		{
			if (content != null)
			{ 
				if (content.IsReadOnly)
				{
					OutputLine("Content.ggpk is Read Only.");
				}
				else if (File.Exists(archivePath) && Path.GetExtension(archivePath).ToLower() == ".zip")
				{
					HandlePatchArchive(archivePath);
				}
			}
		}

		private static void HandlePatchArchive(string archivePath)
		{
			using (ZipFile zipFile = new ZipFile(archivePath))
			{
				OutputLine(string.Format("Archive {0}", archivePath));
                /*
				foreach (var item in zipFile.Entries)
				{
					if (item.FileName.Equals("version.txt"))
					{
						using (var reader = item.OpenReader())
						{
							byte[] versionData = new byte[item.UncompressedSize];
							reader.Read(versionData, 0, versionData.Length);
							string versionStr = Encoding.UTF8.GetString(versionData, 0, versionData.Length);
							if (RecordsByPath.ContainsKey("patch_notes.rtf"))
							{
								string Hash = BitConverter.ToString(RecordsByPath["patch_notes.rtf"].Hash);
								if (versionStr.Substring(0, Hash.Length).Equals(Hash))
								{
									VersionCheck = true;
								}
							}
						}
						break;
					}
					else if (Path.GetDirectoryName(item.FileName) == "Data" && Path.GetExtension(item.FileName).ToLower() == ".dat")
					{
						NeedVersionCheck = true;
					}
					else if (Path.GetDirectoryName(item.FileName) == "Metadata" && Path.GetExtension(item.FileName).ToLower() == ".txt")
					{
						NeedVersionCheck = true;
					}
				}
				if (NeedVersionCheck && !VersionCheck)
				{
					OutputLine("Version Check Failed");
					return;
				}
                */

				foreach (var item in zipFile.Entries)
				{
					if (item.IsDirectory)
					{
						continue;
					}
					if (item.FileName.Equals("version.txt"))
					{
						continue;
					}

                    string fixedFileName = "ROOT" + Path.DirectorySeparatorChar + item.FileName;
					if (Path.DirectorySeparatorChar != '/')
					{
						fixedFileName = fixedFileName.Replace('/', Path.DirectorySeparatorChar);
					}

					if (!RecordsByPath.ContainsKey(fixedFileName))
					{
						OutputLine(string.Format("Failed {0}", fixedFileName));
						continue;
					}
					OutputLine(string.Format("Replace {0}", fixedFileName));

					using (var reader = item.OpenReader())
					{
						byte[] replacementData = new byte[item.UncompressedSize];
						reader.Read(replacementData, 0, replacementData.Length);

						RecordsByPath[fixedFileName].ReplaceContents(ggpkPath, replacementData, content.FreeRoot);
					}
				}
				OutputLine("Content.ggpk is Fine.");
			}
		}
	}
}
