using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;

using LibGGPK;
using System.IO;
using Ionic.Zip;
using System.Globalization;
using LibGGPK.Records;
using Microsoft.Win32;

using Newtonsoft.Json.Linq;

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
                buttonApplyChinese.Text = "套用中文化";
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
            if (File.Exists(Properties.Settings.Default.ContentGGPK))
            {
                return Properties.Settings.Default.ContentGGPK;
            }
            const string contentGgpk = @"\Content.ggpk";
            var ggpkPath = Directory.GetCurrentDirectory() + contentGgpk;
            
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
            if (ggpkPath == String.Empty || !File.Exists(ggpkPath))
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

            //CreateExampleRegistryFile(ggpkPath);
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
                        OutputLine(string.Format("Failed {0}", item.FileName));
                        continue;
                    }
                    OutputLine(string.Format("Replace {0}", item.FileName));

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

        private void chooseGgpk()
        {
            var ofd = new OpenFileDialog
            {
                CheckFileExists = true,
                Filter = "GGPK Pack File|*.ggpk"
            };
            if (textBoxContentGGPK.Text != string.Empty)
            {
                ofd.InitialDirectory = Path.GetDirectoryName(textBoxContentGGPK.Text);
            }
            if (File.Exists(Properties.Settings.Default.ContentGGPK))
            {
                ofd.InitialDirectory = Path.GetDirectoryName(Properties.Settings.Default.ContentGGPK);
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
                    Properties.Settings.Default.ContentGGPK = textBoxContentGGPK.Text;
                    Properties.Settings.Default.Save();
                    InitGgpk();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            chooseGgpk();
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

        string getServerVersion()
        {
            string server_version = null;

            try
            {
                byte[] bytes = new byte[1024];
                const string strHostName = "us.login.pathofexile.com";
                const int port = 12995;
                IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                const int patching_protocol_version_opcode = 1;
                const int patching_protocol_version_number = 4;
                byte[] msg = new byte[] { patching_protocol_version_opcode, patching_protocol_version_number };
                const int patching_url_offset = 0x22;

                try
                {
                    sender.Connect(remoteEP);
                    int bytesSent = sender.Send(msg);
                    int bytesRec = sender.Receive(bytes);
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                    int size = (int)bytes[patching_url_offset];
                    String text = Encoding.Unicode.GetString(bytes, patching_url_offset + 1, size * 2);
                    string[] ary = text.Split('/');
                    server_version = ary[ary.Length - 2];
                    OutputLine("Server Version: " + server_version);
                    return server_version;
                }
                catch (ArgumentNullException ane)
                {
                    OutputLine(ane.ToString());
                }
                catch (SocketException se)
                {
                    OutputLine(se.ToString());
                }
                catch (Exception ee)
                {
                    OutputLine(ee.ToString());
                }
            }
            catch (Exception ee)
            {
                OutputLine(ee.ToString());
            }
            return server_version;
        }

        void ignoreCertificateCheckWhenSSL()
        {
            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
        }

        dynamic getPatchInfo()
        {
            using (WebClient wc = new WebClient())
            {
                ignoreCertificateCheckWhenSSL();

                string remoteUri = "https://poedb.tw/fg/";
                string fileName = "pin.json";
                var json = wc.DownloadString(remoteUri + fileName);
                dynamic patch = JObject.Parse(json);

                return patch;
            }
        }

        static string CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        Boolean downloadAndVerifyPatch(string patch_md5)
        {
            using (WebClient wc = new WebClient())
            {
                string remoteUri = "https://poedb.tw/fg/";
                string fileName = patch_md5 + ".zip";
                if (!File.Exists(fileName))
                {
                    wc.DownloadFile(remoteUri + fileName, fileName);
                }
                if (!File.Exists(fileName))
                {
                    return false;
                }
                string md5sum = CalculateMD5(fileName);
                OutputLine(md5sum);
                if (patch_md5.Equals(md5sum))
                {
                    OutputLine("MD5 sum matched.");
                    return true;
                }
                File.Delete(fileName);
                OutputLine("MD5 sum not matched.");
                return false;
            }
        }

        private void buttonApplyChinese_Click(object objsender, EventArgs e)
        {
            if (!File.Exists(Properties.Settings.Default.ContentGGPK))
            {
                chooseGgpk();
            }
            string server_version = getServerVersion();
            if (String.IsNullOrEmpty(server_version))
            {
                return;
            }
            dynamic patch = getPatchInfo();
            string patch_version = patch.version;
            OutputLine("Patch Version: " + patch_version);
            if (server_version != patch_version)
            {
                OutputLine("Server Version not match Patch Version");
                return;
            }
            string promptValue = ShowDialog("https://poedb.tw/tw/chinese", "Pin Code");
            if (promptValue != (string)patch.pin)
            {
                OutputLine("pin code at https://poedb.tw/tw/chinese");
                return;
            }
            string patch_md5 = patch.md5;
            if (downloadAndVerifyPatch(patch_md5))
            {
                InitGgpk();
                HandlePatchArchive(patch_md5 + ".zip");
            }
        }

        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Width = 400, Text = text };
            textLabel.Click += delegate {
                System.Diagnostics.Process.Start(text);
            };
            TextBox textBox = new TextBox() { Left = 50, Top = 40, Width = 400 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }

        private void label2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://poedb.tw/tw/chinese");
        }
    }
}
