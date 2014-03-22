using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using LibGGPK;
using System.IO;
using System.Linq.Expressions;
using LibDat;
using LibDat.Files;
using Ionic.Zip;

namespace VPatchGGPK
{
	public partial class Form1 : Form
	{
		private static GGPK content = null;
		private static Dictionary<string, FileRecord> RecordsByPath;

		public Form1()
		{
			InitializeComponent();

			string ggpkPath = searchContentGGPK();
			if (File.Exists(ggpkPath))
			{
				textBox1.Text = ggpkPath;
				OutputLine(ggpkPath);
			}
		}

		private void Output(string msg)
		{
			textBox3.AppendText(msg);
			textBox3.SelectionStart = textBox3.Text.Length;
			textBox3.ScrollToCaret();
			textBox3.Refresh();
		}

		private void OutputLine(string msg)
		{
			Output(msg + "\r\n");
		}

		private static string searchContentGGPK()
		{
			string contentGGPK = @"\Content.ggpk";
			string ggpkPath = Directory.GetCurrentDirectory() + contentGGPK;
			// Search GGG ggpk
			if (!File.Exists(ggpkPath))
			{
				Microsoft.Win32.RegistryKey start = Microsoft.Win32.Registry.CurrentUser;
				Microsoft.Win32.RegistryKey programName = start.OpenSubKey(@"Software\GrindingGearGames\Path of Exile");
				if (programName != null)
				{
					string pathString = (string)programName.GetValue("InstallLocation");
					if (pathString != string.Empty && File.Exists(pathString + contentGGPK))
					{
						ggpkPath = pathString + contentGGPK;
					}
				}
			}
			if (!File.Exists(ggpkPath))
			{
				if (File.Exists(@"C:\Program Files (x86)\Grinding Gear Games\Path of Exile" + contentGGPK))
				{
					ggpkPath = @"C:\Program Files (x86)\Grinding Gear Games\Path of Exile" + contentGGPK;
				}
			}
			if (!File.Exists(ggpkPath))
			{
				if (File.Exists(@"C:\Program Files\Grinding Gear Games\Path of Exile" + contentGGPK))
				{
					ggpkPath = @"C:\Program Files\Grinding Gear Games\Path of Exile" + contentGGPK;
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
					if (pathString != string.Empty && File.Exists(pathString + contentGGPK))
					{
						ggpkPath = pathString + contentGGPK;
					}
				}
			}
			if (!File.Exists(ggpkPath))
			{
				if (File.Exists(@"C:\Program Files (x86)\GarenaPoE\GameData\Apps\PoE" + contentGGPK))
				{
					ggpkPath = @"C:\Program Files (x86)\GarenaPoE\GameData\Apps\PoE" + contentGGPK;
				}
			}
			if (!File.Exists(ggpkPath))
			{
				if (File.Exists(@"C:\Program Files\GarenaPoE\GameData\Apps\PoE" + contentGGPK))
				{
					ggpkPath = @"C:\Program Files\GarenaPoE\GameData\Apps\PoE" + contentGGPK;
				}
			}

			return ggpkPath;
		}

		private void InitGGPK()
		{
			if (content != null)
				return;

			string ggpkPath = textBox1.Text;
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

			textBox1.Enabled = false;
			buttonSelectPOE.Enabled = false;
		}

		private void HandlePatchArchive(string archivePath)
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

						RecordsByPath[fixedFileName].ReplaceContents(textBox1.Text, replacementData, content.FreeRoot);
					}
				}
				OutputLine("Content.ggpk is Fine.");
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.CheckFileExists = true;
			ofd.Filter = "GGPK Pack File|*.ggpk";
			ofd.InitialDirectory = Path.GetDirectoryName(textBox1.Text);
			if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				if (!File.Exists(ofd.FileName))
				{
					this.Close();
					return;
				}
				else
				{
					textBox1.Text = ofd.FileName;
					OutputLine(textBox1.Text);
					InitGGPK();
				}
			}
			else
			{
				return;
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.CheckFileExists = true;
			ofd.Filter = "ZIP File|*.zip";
			if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				if (!File.Exists(ofd.FileName))
				{
					this.Close();
					return;
				}
				else
				{
					OutputLine(ofd.FileName);
					InitGGPK();
					string archivePath = ofd.FileName;
					HandlePatchArchive(archivePath);
				}
			}
			else
			{
				return;
			}
		}

		private void buttonExit_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
