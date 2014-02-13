using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibGGPK;
using System.IO;
using System.Linq.Expressions;
using LibDat;
using LibDat.Files;
using Ionic.Zip;

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

		private static string ggpkPath = @"o:\Program Files (x86)\Grinding Gear Games\Path of Exile\content.ggpk";
		private static Dictionary<string, FileRecord> RecordsByPath;
		private static GGPK content;

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
			OutputLine("Press any key to continue...");
			Console.ReadLine();
		}

		private static void InitGGPK()
		{
			// Search GGG ggpk
			if (!File.Exists(ggpkPath))
			{
				Microsoft.Win32.RegistryKey start = Microsoft.Win32.Registry.CurrentUser;
				Microsoft.Win32.RegistryKey programName = start.OpenSubKey(@"Software\GrindingGearGames\Path of Exile");
				if (programName != null)
				{
					string pathString = (string)programName.GetValue("InstallLocation");
					if (pathString != string.Empty && File.Exists(pathString + @"\Content.ggpk"))
					{
						ggpkPath = pathString + @"\Content.ggpk";
					}
				}
			}
			if (!File.Exists(ggpkPath))
			{
				if (File.Exists(@"C:\Program Files (x86)\Grinding Gear Games\Path of Exile\content.ggpk"))
				{
					ggpkPath = @"C:\Program Files (x86)\Grinding Gear Games\Path of Exile\content.ggpk";
				}
			}
			if (!File.Exists(ggpkPath))
			{
				if (File.Exists(@"C:\Program Files\Grinding Gear Games\Path of Exile\content.ggpk"))
				{
					ggpkPath = @"C:\Program Files\Grinding Gear Games\Path of Exile\content.ggpk";
				}
			}
			// Search GGC ggpk
			if (!File.Exists(ggpkPath))
			{
				Microsoft.Win32.RegistryKey start = Microsoft.Win32.Registry.LocalMachine;
				Microsoft.Win32.RegistryKey programName = start.OpenSubKey(@"SOFTWARE\Wow6432Node\Garena\PoE");
				if (programName != null)
				{
					string pathString = (string)programName.GetValue("Path");
					if (pathString != string.Empty && File.Exists(pathString + @"\Content.ggpk"))
					{
						ggpkPath = pathString + @"\Content.ggpk";
					}
				}
			}
			if (!File.Exists(ggpkPath))
			{
				if (File.Exists(@"C:\Program Files (x86)\GarenaPoE\GameData\Apps\PoE\Content.ggpk"))
				{
					ggpkPath = @"C:\Program Files (x86)\GarenaPoE\GameData\Apps\PoE\Content.ggpk";
				}
			}
			if (!File.Exists(ggpkPath))
			{
				if (File.Exists(@"C:\Program Files\GarenaPoE\GameData\Apps\PoE\Content.ggpk"))
				{
					ggpkPath = @"C:\Program Files\GarenaPoE\GameData\Apps\PoE\Content.ggpk";
				}
			}
			if (!File.Exists(ggpkPath))
			{
				OutputLine(string.Format("GGPK {0} not exists.", ggpkPath));
				return;
			}
			OutputLine(string.Format("Parsing {0}", ggpkPath));

			content = new GGPK();
			content.Read(ggpkPath, Output);

			RecordsByPath = new Dictionary<string, FileRecord>(content.RecordOffsets.Count);
			DirectoryTreeNode.TraverseTreePostorder(content.DirectoryRoot, null, n => RecordsByPath.Add(n.GetDirectoryPath() + n.Name, n as FileRecord));
		}

		private static void InitPatchArchive(string archivePath)
		{
			if (File.Exists(ggpkPath)) 
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
				bool VersionCheck = false;
				bool NeedVersionCheck = false;
				OutputLine(string.Format("Archive {0}", archivePath));
				foreach (var item in zipFile.Entries)
				{
					if (item.FileName.Equals("version.txt"))
					{
						using (var reader = item.OpenReader())
						{
							byte[] versionData = new byte[item.UncompressedSize];
							reader.Read(versionData, 0, versionData.Length);
							string versionStr = Encoding.UTF8.GetString(versionData, 0, versionData.Length);
							foreach (var recordOffset in content.RecordOffsets)
							{
								FileRecord record = recordOffset.Value as FileRecord;
								if (record == null || record.ContainingDirectory == null)
								{
									continue;
								}
								if (record.Name.Equals("patch_notes.rtf"))
								{
									string Hash = BitConverter.ToString(record.Hash);
									if (versionStr.Substring(0, Hash.Length).Equals(Hash))
									{
										VersionCheck = true;
									}
									break;
								}
							}
						}
						break;
					}
					else if (Path.GetExtension(item.FileName).ToLower() == ".dat" || Path.GetExtension(item.FileName).ToLower() == ".txt")
					{
						NeedVersionCheck = true;
					}
				}
				if (NeedVersionCheck && !VersionCheck)
				{
					OutputLine("Version Check Failed");
					return;
				}

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

					string fixedFileName = item.FileName;
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
