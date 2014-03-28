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
				textBoxContentGGPK.Text = ggpkPath;
				OutputLine(ggpkPath);
			}
			ApplyLabelColor();
		}

		private void Output(string msg)
		{
			textBoxOutput.AppendText(msg);
			textBoxOutput.SelectionStart = textBoxOutput.Text.Length;
			textBoxOutput.ScrollToCaret();
			textBoxOutput.Refresh();
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

			string ggpkPath = textBoxContentGGPK.Text;
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

			textBoxContentGGPK.Enabled = false;
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

						RecordsByPath[fixedFileName].ReplaceContents(textBoxContentGGPK.Text, replacementData, content.FreeRoot);
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
			ofd.InitialDirectory = Path.GetDirectoryName(textBoxContentGGPK.Text);
			if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				if (!File.Exists(ofd.FileName))
				{
					this.Close();
					return;
				}
				else
				{
					textBoxContentGGPK.Text = ofd.FileName;
					OutputLine(textBoxContentGGPK.Text);
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

		public static string Utf8ToUtf16(string utf8String)
		{
			// Get UTF8 bytes by reading each byte with ANSI encoding
			byte[] utf8Bytes = Encoding.Default.GetBytes(utf8String);

			// Convert UTF8 bytes to UTF16 bytes
			byte[] utf16Bytes = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, utf8Bytes);

			// Return UTF16 bytes as UTF16 string
			return Encoding.Unicode.GetString(utf16Bytes);
		}

		public string UTF8ToUnicode(string utf8String)
		{
			// Get UTF8 bytes by reading each byte with ANSI encoding
			byte[] utf8Bytes = Encoding.UTF8.GetBytes(utf8String);

			// Convert UTF8 bytes to UTF16 bytes
			byte[] utf16Bytes = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, utf8Bytes);

			// Return UTF16 bytes as UTF16 string
			return Encoding.Unicode.GetString(utf16Bytes);
		}

		private void buttonApplyFont_Click(object sender, EventArgs e)
		{
			InitGGPK();

			if (content == null)
				return;

			foreach (var recordOffset in content.RecordOffsets)
			{
				FileRecord record = recordOffset.Value as FileRecord;

				if (record == null || record.ContainingDirectory == null || record.Name != "Common.ui")
				{
					continue;
				}

				byte[] datBytes = record.ReadData(textBoxContentGGPK.Text);
				char c = '\ufeff';
				string lines = c.ToString();
				using (MemoryStream datStream = new MemoryStream(datBytes))
				{
					using (var reader = new StreamReader(datStream, Encoding.Unicode))
					{
						string line;
						
						while ((line = reader.ReadLine()) != null)
						{
							if (line.Contains("const $globalFontSizeSmall  = "))
							{
								int Small = Convert.ToInt32(textBoxSmallFont.Text);
								if (Small > 10 && Small < 100)
								{
									OutputLine("Small:" + line.Substring(30, 2) + " to " + Small);
									line = "const $globalFontSizeSmall  = " + Small + ";";
								}
							}
							else if (line.Contains("const $globalFontSizeNormal = "))
							{
								int Normal = Convert.ToInt32(textBoxNormalFont.Text);
								if (Normal > 10 && Normal < 100)
								{
									OutputLine("Normal:" + line.Substring(30, 2) + " to " + Normal);
									line = "const $globalFontSizeNormal = " + Normal + ";";
								}
							}
							else if (line.Contains("const $globalFontSizeLarge  = "))
							{
								int Large = Convert.ToInt32(textBoxLargeFont.Text);
								if (Large > 10 && Large < 100)
								{
									OutputLine("Large:" + line.Substring(30, 2) + " to " + Large);
									line = "const $globalFontSizeLarge  = " + Large + ";";
								}
							}
							lines += line + "\r\n";
						}
						
					}
				}
				//System.IO.File.WriteAllText("WriteLines.txt", lines, Encoding.Unicode);
				string common_ui = "Metadata\\UI\\Common.ui";
				if (lines.Length > 2000 && RecordsByPath.ContainsKey(common_ui))
				{
					RecordsByPath[common_ui].ReplaceContents(textBoxContentGGPK.Text, Encoding.Unicode.GetBytes(lines), content.FreeRoot);
					OutputLine("Font Size Changed.");
				}

				break;
			}
		}

		private void ApplyLabelColor()
		{
			labelUnique.BackColor = Color.FromName("black");
			labelUnique.ForeColor = Color.FromArgb(
				Convert.ToInt32(textBoxUniqueR.Text), 
				Convert.ToInt32(textBoxUniqueG.Text),
				Convert.ToInt32(textBoxUniqueB.Text));
			labelRare.BackColor = Color.FromName("black");
			labelRare.ForeColor = Color.FromArgb(
				Convert.ToInt32(textBoxRareR.Text),
				Convert.ToInt32(textBoxRareG.Text),
				Convert.ToInt32(textBoxRareB.Text));
			labelMagic.BackColor = Color.FromName("black");
			labelMagic.ForeColor = Color.FromArgb(
				Convert.ToInt32(textBoxMagicR.Text),
				Convert.ToInt32(textBoxMagicG.Text),
				Convert.ToInt32(textBoxMagicB.Text));
			labelGem.BackColor = Color.FromName("black");
			labelGem.ForeColor = Color.FromArgb(
				Convert.ToInt32(textBoxGemR.Text),
				Convert.ToInt32(textBoxGemG.Text),
				Convert.ToInt32(textBoxGemB.Text));
			labelCurrency.BackColor = Color.FromName("black");
			labelCurrency.ForeColor = Color.FromArgb(
				Convert.ToInt32(textBoxCurrencyR.Text),
				Convert.ToInt32(textBoxCurrencyG.Text),
				Convert.ToInt32(textBoxCurrencyB.Text));
		}

		private void buttonApplyColor_Click(object sender, EventArgs e)
		{
			ApplyLabelColor();

			InitGGPK();

			if (content == null)
				return;

			foreach (var recordOffset in content.RecordOffsets)
			{
				FileRecord record = recordOffset.Value as FileRecord;

				if (record == null || record.ContainingDirectory == null || record.Name != "named_colours.txt")
				{
					continue;
				}

				byte[] datBytes = record.ReadData(textBoxContentGGPK.Text);
				char c = '\ufeff';
				string lines = c.ToString();
				using (MemoryStream datStream = new MemoryStream(datBytes))
				{
					using (var reader = new StreamReader(datStream, Encoding.Unicode))
					{
						string line;

						while ((line = reader.ReadLine()) != null)
						{
							if (line.Contains("uniqueitem rgb"))
							{
								line = string.Format("uniqueitem rgb({0},{1},{2})", textBoxUniqueR.Text,
									textBoxUniqueG.Text, textBoxUniqueB.Text);
							}
							else if (line.Contains("rareitem rgb"))
							{
								line = string.Format("rareitem rgb({0},{1},{2})", textBoxRareR.Text,
									textBoxRareG.Text, textBoxRareB.Text);
							}
							else if (line.Contains("magicitem rgb"))
							{
								line = string.Format("magicitem rgb({0},{1},{2})", textBoxMagicR.Text,
									textBoxMagicG.Text, textBoxMagicB.Text);
							}
							else if (line.Contains("gemitem rgb"))
							{
								line = string.Format("gemitem rgb({0},{1},{2})", textBoxGemR.Text,
									textBoxGemG.Text, textBoxGemB.Text);
							}
							else if (line.Contains("currencyitem rgb"))
							{
								line = string.Format("currencyitem rgb({0},{1},{2})", textBoxCurrencyR.Text,
									textBoxCurrencyG.Text, textBoxCurrencyB.Text);
							}
							lines += line + "\r\n";
						}

					}
				}
				//System.IO.File.WriteAllText("WriteLines.txt", lines, Encoding.Unicode);
				string common_ui = "Metadata\\UI\\named_colours.txt";
				if (RecordsByPath.ContainsKey(common_ui))
				{
					RecordsByPath[common_ui].ReplaceContents(textBoxContentGGPK.Text, Encoding.Unicode.GetBytes(lines), content.FreeRoot);
					OutputLine("Color Changed.");
				}

				break;
			}
		}

		private void button1_Click_1(object sender, EventArgs e)
		{
			ApplyLabelColor();
		}
	}
}
