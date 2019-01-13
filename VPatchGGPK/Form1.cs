using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using LibGGPK;
using System.IO;
using Ionic.Zip;
using System.Globalization;
using LibGGPK.Records;
using Microsoft.Win32;

namespace VPatchGGPK
{
    public partial class Form1 : Form
    {
        private static GrindingGearsPackageContainer content;
        private static Dictionary<string, FileRecord> _recordsByPath;

        public Form1()
        {
            InitializeComponent();

            var ggpkPath = SearchContentGgpk();
            if (File.Exists(ggpkPath))
            {
                textBoxContentGGPK.Text = ggpkPath;
                OutputLine(ggpkPath);
            }

            if (CultureInfo.CurrentCulture.Name.Equals("zh-TW"))
            {
                labelContentGGPKPath.Text = "Content.ggpk 路徑";
                buttonSelectPOE.Text = "選擇 POE";
                buttonApplyZIP.Text = "套用 ZIP";
                buttonClose.Text = "關閉";
            }
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

        private static string SearchContentGgpk()
        {
            const string contentGgpk = @"\Content.ggpk";
            var ggpkPath = Directory.GetCurrentDirectory() + contentGgpk;
            // GarenaTW
            if (!File.Exists(ggpkPath))
            {
                var start = Registry.LocalMachine;
                var programName = start.OpenSubKey(@"SOFTWARE\Wow6432Node\Garena\POETW");
                if (programName != null)
                {
                    var pathString = (string)programName.GetValue("Path");
                    if (pathString != string.Empty && File.Exists(pathString + contentGgpk))
                    {
                        ggpkPath = pathString + contentGgpk;
                    }
                }
            }
            if (!File.Exists(ggpkPath))
            {
                var start = Registry.LocalMachine;
                var programName = start.OpenSubKey(@"SOFTWARE\Garena\POETW");
                if (programName != null)
                {
                    var pathString = (string)programName.GetValue("Path");
                    if (pathString != string.Empty && File.Exists(pathString + contentGgpk))
                    {
                        ggpkPath = pathString + contentGgpk;
                    }
                }
            }
            if (!File.Exists(ggpkPath))
            {
                if (File.Exists(@"C:\Program Files (x86)\GarenaPoETW\GameData\Apps\POETW" + contentGgpk))
                {
                    ggpkPath = @"C:\Program Files (x86)\GarenaPoETW\GameData\Apps\POETW" + contentGgpk;
                }
            }
            if (!File.Exists(ggpkPath))
            {
                if (File.Exists(@"C:\Program Files\GarenaPoETW\GameData\Apps\POETW" + contentGgpk))
                {
                    ggpkPath = @"C:\Program Files\GarenaPoETW\GameData\Apps\POETW" + contentGgpk;
                }
            }
            // Search GGG ggpk
            if (!File.Exists(ggpkPath))
            {
                var start = Registry.CurrentUser;
                var programName = start.OpenSubKey(@"Software\GrindingGearGames\Path of Exile");
                if (programName != null)
                {
                    var pathString = (string)programName.GetValue("InstallLocation");
                    if (pathString != string.Empty && File.Exists(pathString + contentGgpk))
                    {
                        ggpkPath = pathString + contentGgpk;
                    }
                }
            }
            if (!File.Exists(ggpkPath))
            {
                if (File.Exists(@"C:\Program Files (x86)\Grinding Gear Games\Path of Exile" + contentGgpk))
                {
                    ggpkPath = @"C:\Program Files (x86)\Grinding Gear Games\Path of Exile" + contentGgpk;
                }
            }
            if (!File.Exists(ggpkPath))
            {
                if (File.Exists(@"C:\Program Files\Grinding Gear Games\Path of Exile" + contentGgpk))
                {
                    ggpkPath = @"C:\Program Files\Grinding Gear Games\Path of Exile" + contentGgpk;
                }
            }
            // Search GGC ggpk
            if (!File.Exists(ggpkPath))
            {
                var start = Registry.LocalMachine;
                var programName = start.OpenSubKey(@"SOFTWARE\Wow6432Node\Garena\PoE");
                if (programName != null)
                {
                    var pathString = (string)programName.GetValue("Path");
                    if (pathString != string.Empty && File.Exists(pathString + contentGgpk))
                    {
                        ggpkPath = pathString + contentGgpk;
                    }
                }
            }
            if (!File.Exists(ggpkPath))
            {
                var start = Registry.LocalMachine;
                var programName = start.OpenSubKey(@"SOFTWARE\Garena\PoE");
                if (programName != null)
                {
                    var pathString = (string)programName.GetValue("Path");
                    if (pathString != string.Empty && File.Exists(pathString + contentGgpk))
                    {
                        ggpkPath = pathString + contentGgpk;
                    }
                }
            }
            if (!File.Exists(ggpkPath))
            {
                if (File.Exists(@"C:\Program Files (x86)\GarenaPoE\GameData\Apps\PoE" + contentGgpk))
                {
                    ggpkPath = @"C:\Program Files (x86)\GarenaPoE\GameData\Apps\PoE" + contentGgpk;
                }
            }
            if (!File.Exists(ggpkPath))
            {
                if (File.Exists(@"C:\Program Files\GarenaPoE\GameData\Apps\PoE" + contentGgpk))
                {
                    ggpkPath = @"C:\Program Files\GarenaPoE\GameData\Apps\PoE" + contentGgpk;
                }
            }

            return ggpkPath;
        }

        private void CreateExampleRegistryFile(string ggpkPath)
        {
            var reg = "Windows Registry Editor Version 5.00" + Environment.NewLine;
            reg += Environment.NewLine;
            reg += "[HKEY_CURRENT_USER\\Software\\GrindingGearGames\\Path of Exile]" + Environment.NewLine;
            reg += "\"InstallLocation\"=\"" + Path.GetDirectoryName(ggpkPath).Replace("\\", "\\\\") + "\\\\\"" + Environment.NewLine;
            File.WriteAllText("GGG.reg", reg, Encoding.Unicode);
        }

        private void InitGgpk()
        {
            if (content != null)
                return;

            var ggpkPath = textBoxContentGGPK.Text;
            if (!File.Exists(ggpkPath))
            {
                OutputLine(string.Format("GGPK {0} not exists.", ggpkPath));
                return;
            }
            OutputLine(string.Format("Parsing {0}", ggpkPath));

            content = new GrindingGearsPackageContainer();
            content.Read(ggpkPath, Output);

            _recordsByPath = new Dictionary<string, FileRecord>(content.RecordOffsets.Count);
            DirectoryTreeNode.TraverseTreePostorder(
                content.DirectoryRoot,
                null,
                n => _recordsByPath.Add(n.GetDirectoryPath() + n.Name, n));

            textBoxContentGGPK.Enabled = false;
            buttonSelectPOE.Enabled = false;

            CreateExampleRegistryFile(ggpkPath);
        }

        private void HandlePatchArchive(string archivePath)
        {
            using (var zipFile = new ZipFile(archivePath))
            {
                OutputLine(string.Format("Archive {0}", archivePath));
                /*
                foreach (var item in zipFile.Entries)
                {
                    if (item.FileName.Equals("version.txt"))
                    {
                        using (var reader = item.OpenReader())
                        {
                            var versionData = new byte[item.UncompressedSize];
                            reader.Read(versionData, 0, versionData.Length);
                            var versionStr = Encoding.UTF8.GetString(versionData, 0, versionData.Length);
                            if (_recordsByPath.ContainsKey("patch_notes.rtf"))
                            {
                                var Hash = BitConverter.ToString(_recordsByPath["patch_notes.rtf"].Hash);
                                if (versionStr.Substring(0, Hash.Length).Equals(Hash))
                                {
                                    versionCheck = true;
                                }
                            }
                        }
                        break;
                    }
                    else if (Path.GetExtension(item.FileName).ToLower() == ".dat" || Path.GetExtension(item.FileName).ToLower() == ".txt")
                    {
                        needVersionCheck = true;
                    }
                }
                if (needVersionCheck && !versionCheck)
                {
                    OutputLine("Version Check Failed");
                    return;
                }*/

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

                    var fixedFileName = "ROOT" + Path.DirectorySeparatorChar + item.FileName;
                    if (Path.DirectorySeparatorChar != '/')
                    {
                        fixedFileName = fixedFileName.Replace('/', Path.DirectorySeparatorChar);
                    }

                    if (!_recordsByPath.ContainsKey(fixedFileName))
                    {
                        OutputLine(string.Format("Failed {0}", fixedFileName));
                        continue;
                    }
                    OutputLine(string.Format("Replace {0}", fixedFileName));

                    using (var reader = item.OpenReader())
                    {
                        var replacementData = new byte[item.UncompressedSize];
                        reader.Read(replacementData, 0, replacementData.Length);

                        _recordsByPath[fixedFileName].ReplaceContents(textBoxContentGGPK.Text, replacementData, content.FreeRoot);
                    }
                }
                OutputLine("Content.ggpk is Fine.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                CheckFileExists = true,
                Filter = "GGPK Pack File|*.ggpk"
            };
            if (textBoxContentGGPK.Text != string.Empty)
                ofd.InitialDirectory = Path.GetDirectoryName(textBoxContentGGPK.Text);
            if (Directory.Exists(Properties.Settings.Default.ContentGGPK))
            {
                ofd.InitialDirectory = Properties.Settings.Default.ContentGGPK;
            }
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (!File.Exists(ofd.FileName))
                {
                    Close();
                }
                else
                {
                    textBoxContentGGPK.Text = ofd.FileName;
                    OutputLine(textBoxContentGGPK.Text);
                    Properties.Settings.Default.ContentGGPK = Path.GetDirectoryName(textBoxContentGGPK.Text);
                    Properties.Settings.Default.Save();
                    InitGgpk();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.Filter = "ZIP File|*.zip";
            if (Directory.Exists(Properties.Settings.Default.ZipDirectory))
            {
                ofd.InitialDirectory = Properties.Settings.Default.ZipDirectory;
            }
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (!File.Exists(ofd.FileName))
                {
                    Close();
                }
                else
                {
                    OutputLine(ofd.FileName);
                    InitGgpk();
                    Properties.Settings.Default.ZipDirectory = Path.GetDirectoryName(ofd.FileName);
                    Properties.Settings.Default.Save();
                    HandlePatchArchive(ofd.FileName);
                }
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        public static string Utf8ToUtf16(string utf8String)
        {
            // Get UTF8 bytes by reading each byte with ANSI encoding
            var utf8Bytes = Encoding.Default.GetBytes(utf8String);

            // Convert UTF8 bytes to UTF16 bytes
            var utf16Bytes = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, utf8Bytes);

            // Return UTF16 bytes as UTF16 string
            return Encoding.Unicode.GetString(utf16Bytes);
        }

        public string UTF8ToUnicode(string utf8String)
        {
            // Get UTF8 bytes by reading each byte with ANSI encoding
            var utf8Bytes = Encoding.UTF8.GetBytes(utf8String);

            // Convert UTF8 bytes to UTF16 bytes
            var utf16Bytes = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, utf8Bytes);

            // Return UTF16 bytes as UTF16 string
            return Encoding.Unicode.GetString(utf16Bytes);
        }
    }
}
