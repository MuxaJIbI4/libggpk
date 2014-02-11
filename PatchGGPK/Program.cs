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



		private static string ggpkPath = @"o:\Program Files (x86)\Grinding Gear Games\Path of Exile\content.ggpk";
		private static Dictionary<string, FileRecord> RecordsByPath;
		private static GGPK content;

		public static void Main(string[] args)
		{
			string archivePath = string.Empty;
			if (args.Length != 1)
			{
				string[] SubFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.zip");
				if (SubFiles.Length > 0)
				{
					InitGGPK();
					for (int i = 0; i < SubFiles.Length; i++)
					{
						PatchGGPK(SubFiles[0]);
					}
				}
				Console.WriteLine("Press any key to continue...");
				Console.ReadLine();
				return;
			}
			else
			{
				archivePath = args[0];
			}
			InitGGPK();
			PatchGGPK(archivePath);
			Console.WriteLine("Press any key to continue...");
			Console.ReadLine();
		}

		private static void InitGGPK()
		{
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
				Console.WriteLine("GGPK {0} not exists.", ggpkPath);
				return;
			}
			Console.WriteLine("Parsing {0}", ggpkPath);

			content = new GGPK();
			content.Read(ggpkPath, Output);

			RecordsByPath = new Dictionary<string, FileRecord>(content.RecordOffsets.Count);
			DirectoryTreeNode.TraverseTreePostorder(content.DirectoryRoot, null, n => RecordsByPath.Add(n.GetDirectoryPath() + n.Name, n as FileRecord));
		}

		private static void PatchGGPK(string archivePath)
		{
			if (File.Exists(ggpkPath)) 
			{ 
				if (content.IsReadOnly)
				{
					Console.WriteLine("Content.ggpk is Read Only.");
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
				Console.WriteLine("Archive {0}", archivePath);
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
					Console.WriteLine("Version Check Failed");
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
						Console.WriteLine("Failed {0}", fixedFileName);
						continue;
					}
					Console.WriteLine("Replace {0}", fixedFileName);

					using (var reader = item.OpenReader())
					{
						byte[] replacementData = new byte[item.UncompressedSize];
						reader.Read(replacementData, 0, replacementData.Length);

						RecordsByPath[fixedFileName].ReplaceContents(ggpkPath, replacementData, content.FreeRoot);
					}
				}
				Console.WriteLine("Content.ggpk is Fine.");
			}
		}
	}
}
