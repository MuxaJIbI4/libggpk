using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Ionic.Zip;
using LibGGPK;
using LibGGPK.Records;
using Microsoft.Win32;

namespace PoEGGPKPatcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private static GrindingGearsPackageContainer _content;
        private static Dictionary<string, FileRecord> _recordsByPath;
        private MainViewModel _vm;

        public MainWindow()
        {
            InitializeComponent();

            var ggpkPath = SearchContentGgpk();
            if (File.Exists(ggpkPath))
            {
                textBoxContentGGPK.Text = ggpkPath;
                OutputLine(ggpkPath);
            }

            // apply default label colors
            //ApplyLabelColor();

            if (CultureInfo.CurrentCulture.Name.Equals("zh-TW"))
                SetChineseText();

            _vm = new MainViewModel();
            DataContext = _vm;
        }

        private void Output(string msg)
        {
            textBoxOutput.Dispatcher.BeginInvoke(new Action(() =>
            {
                textBoxOutput.Text += msg;
            }), null);
        }

        private void OutputLine(string msg)
        {
            Output(msg + "\r\n");
        }

        /// <summary>
        /// search PoE Content.ggpl file using Windows registry and known possible locations
        /// </summary>
        /// <returns></returns>
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

        private void SetChineseText()
        {
            labelContentGGPKPath.Content = "Content.ggpk 路徑";
            buttonSelectPOE.Content = "選擇 POE";
            buttonApplyZIP.Content = "套用 ZIP";
            buttonClose.Content = "關閉";

            groupBoxFontSize.Content = "字體大小";
            labelSmallFont.Content = "小字";
            labelNormalFont.Content = "中字";
            labelLargeFont.Content = "大字";
            buttonApplyFont.Content = "修改字體";

            groupBoxColor.Content = "顏色修改(R, G, B)";
            labelUnique.Content = "獨特";
            labelRare.Content = "稀有";
            labelMagic.Content = "魔法";
            labelGem.Content = "寶石";
            labelCurrency.Content = "通貨";
            buttonApplyColor.Content = "修改顏色";

            groupBoxQuality.Content = "螢幕畫質(0最好,10最差)";
            buttonApplyQuality.Content = "修改畫質";
        }

        #region Event Handlers

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// changes "texture_quality" in PoE config
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnApplyQualityClick(object sender, RoutedEventArgs e)
        {
            string[] configs = { "production_Config.ini", "garena_sg_production_Config.ini" };
            foreach (var fname in configs)
            {
                var config = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\My Games\Path of Exile\" + fname;
                if (!File.Exists(config)) continue;

                OutputLine("Loading " + config);
                string line;
                var lines = "";

                // Read the file and display it line by line.
                var file = new StreamReader(config);
                while ((line = file.ReadLine()) != null)
                {
                    if (line.StartsWith("texture_quality="))
                    {
                        var quality = Convert.ToInt32(textBoxQuality.Text);
                        if (quality >= 0 && quality <= 10)
                        {
                            line = "texture_quality=" + quality;
                            OutputLine(line);
                        }
                    }
                    lines += line + "\r\n";
                }
                file.Close();
                File.WriteAllText(config, lines);
            }
        }

        /// <summary>
        /// changes fonts of Text in the game. Changes Content.ggpk
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnApplyFontClick(object sender, RoutedEventArgs e)
        {
            InitGgpk();

            if (_content == null)
                return;

            const string commonUi = "Metadata\\UI\\Common.ui";
            if (!_recordsByPath.ContainsKey(commonUi))
            {
                OutputLine(@"Error: Not found Metadata\\UI\\Common.ui in GGPK gile");
                return;
            }

            var datBytes = _recordsByPath[commonUi].ReadFileContent(textBoxContentGGPK.Text);
            var c = '\ufeff';
            var lines = c.ToString();

            using (var datStream = new MemoryStream(datBytes))
            using (var reader = new StreamReader(datStream, Encoding.Unicode))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("const $globalFontSizeSmall  = "))
                    {
                        var small = Convert.ToInt32(textBoxSmallFont.Text);
                        if (small > 10 && small < 100)
                        {
                            OutputLine("Small:" + line.Substring(30, 2) + " to " + small);
                            line = "const $globalFontSizeSmall  = " + small + ";";
                        }
                    }
                    else if (line.Contains("const $globalFontSizeNormal = "))
                    {
                        var normal = Convert.ToInt32(textBoxNormalFont.Text);
                        if (normal > 10 && normal < 100)
                        {
                            OutputLine("Normal:" + line.Substring(30, 2) + " to " + normal);
                            line = "const $globalFontSizeNormal = " + normal + ";";
                        }
                    }
                    else if (line.Contains("const $globalFontSizeLarge  = "))
                    {
                        var large = Convert.ToInt32(textBoxLargeFont.Text);
                        if (large > 10 && large < 100)
                        {
                            OutputLine("Large:" + line.Substring(30, 2) + " to " + large);
                            line = "const $globalFontSizeLarge  = " + large + ";";
                        }
                    }
                    lines += line + "\r\n";
                }
            }
            _recordsByPath[commonUi].ReplaceContents(textBoxContentGGPK.Text, Encoding.Unicode.GetBytes(lines), _content.FreeRoot);
            OutputLine("Font Size Changed.");
        }


        /// <summary>
        /// changes Metadata\\UI\\named_colours.txt inside ggpk file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnApplyItemsColorClick(object sender, RoutedEventArgs e)
        {
            InitGgpk();

            if (_content == null)
                return;

            const string common_ui = "Metadata\\UI\\named_colours.txt";
            if (!_recordsByPath.ContainsKey(common_ui)) return;

            var datBytes = _recordsByPath[common_ui].ReadFileContent(textBoxContentGGPK.Text);
            var c = '\ufeff';
            var lines = c.ToString();
            using (var datStream = new MemoryStream(datBytes))
            {
                using (var reader = new StreamReader(datStream, Encoding.Unicode))
                {
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        Color color;
                        if (line.Contains("uniqueitem rgb"))
                        {
                            color = _vm.UniqueColor;
                            line = string.Format("uniqueitem rgb({0},{1},{2})", color.R, color.G, color.B);
                        }
                        else if (line.Contains("rareitem rgb"))
                        {
                            color = _vm.RareColor;
                            line = string.Format("rareitem rgb({0},{1},{2})", color.R, color.G, color.B);
                        }
                        else if (line.Contains("magicitem rgb"))
                        {
                            color = _vm.MagicColor;
                            line = string.Format("magicitem rgb({0},{1},{2})", color.R, color.G, color.B);
                        }
                        else if (line.Contains("gemitem rgb"))
                        {
                            color = _vm.GemColor;
                            line = string.Format("gemitem rgb({0},{1},{2})", color.R, color.G, color.B);
                        }
                        else if (line.Contains("currencyitem rgb"))
                        {
                            color = _vm.CurrencyColor;
                            line = string.Format("currencyitem rgb({0},{1},{2})", color.R, color.G, color.B);
                        }
                        lines += line + "\r\n";
                    }

                }
            }
            _recordsByPath[common_ui].ReplaceContents(textBoxContentGGPK.Text, Encoding.Unicode.GetBytes(lines), _content.FreeRoot);
            OutputLine("Color Changed.");
        }

        private void OnSelectGgpkFileClick(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                CheckFileExists = true,
                Filter = "GGPK Pack File|*.ggpk"
            };
            if (textBoxContentGGPK.Text != string.Empty)
                ofd.InitialDirectory = Path.GetDirectoryName(textBoxContentGGPK.Text);
            if (ofd.ShowDialog() == true)
            {
                if (!File.Exists(ofd.FileName))
                {
                    Close();
                }
                else
                {
                    textBoxContentGGPK.Text = ofd.FileName;
                    OutputLine(textBoxContentGGPK.Text);
                    InitGgpk();
                }
            }
        }

        private void OnApplyZipClick(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                CheckFileExists = true,
                Filter = "ZIP File|*.zip"
            };
            if (ofd.ShowDialog() == true)
            {
                if (!File.Exists(ofd.FileName))
                {
                    Close();
                }
                else
                {
                    OutputLine(ofd.FileName);
                    InitGgpk();
                    var archivePath = ofd.FileName;
                    HandlePatchArchive(archivePath);
                }
            }
        }

        private void OnExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion

        /// <summary>
        /// parses GGPK file
        /// </summary>
        private void InitGgpk()
        {
            if (_content != null)
                return;

            var ggpkPath = textBoxContentGGPK.Text;
            if (!File.Exists(ggpkPath))
            {
                OutputLine(string.Format("GGPK {0} not exists.", ggpkPath));
                return;
            }
            OutputLine(string.Format("Parsing {0}", ggpkPath));

            _content = new GrindingGearsPackageContainer();
            _content.Read(ggpkPath, Output);

            _recordsByPath = new Dictionary<string, FileRecord>(_content.RecordOffsets.Count);
            DirectoryTreeNode.TraverseTreePostorder(
                _content.DirectoryRoot,
                null,
                n => _recordsByPath.Add(n.GetDirectoryPath() + n.Name, n));

            textBoxContentGGPK.IsEnabled = false;
            buttonSelectPOE.IsEnabled = false;

            CreateExampleRegistryFile(ggpkPath);
        }

        private static void CreateExampleRegistryFile(string ggpkPath)
        {
            var reg = "Windows Registry Editor Version 5.00" + Environment.NewLine;
            reg += Environment.NewLine;
            reg += "[HKEY_CURRENT_USER\\Software\\GrindingGearGames\\Path of Exile]" + Environment.NewLine;
            reg += "\"InstallLocation\"=\"" + Path.GetDirectoryName(ggpkPath).Replace("\\", "\\\\") + "\\\\\"" + Environment.NewLine;
            File.WriteAllText("GGG.reg", reg, Encoding.Unicode);
        }

        private void HandlePatchArchive(string archivePath)
        {
            using (var zipFile = new ZipFile(archivePath))
            {
                var versionCheck = false;
                var needVersionCheck = false;
                OutputLine(string.Format("Archive {0}", archivePath));
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

                    var fixedFileName = item.FileName;
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

                        _recordsByPath[fixedFileName].ReplaceContents(textBoxContentGGPK.Text, replacementData, _content.FreeRoot);
                    }
                }
                OutputLine("Content.ggpk is Fine.");
            }
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
