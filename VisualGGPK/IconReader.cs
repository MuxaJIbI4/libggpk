using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace VisualGGPK
{
    internal sealed class IconReader : IconNativeMethods
    {
//        private Icon GetFileIcon(string fileName)
//        {
//            string extension = Path.GetExtension(fileName);
//            if (String.IsNullOrEmpty(extension))
//                return null;
//            var shinfo = new SHFILEINFO();
//            
//            // Will store a handle to the small icon
//            IntPtr hImgSmall = SHGetFileInfo(fileName, 0, ref shinfo, Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_SMALLICON);
//
//            // Get the small icon from the handle
//            var icon = (Icon)Icon.FromHandle(shinfo.hIcon).Clone();
//            DestroyIcon(shinfo.hIcon);
//
//            return icon;
//        }

        private static Icon _directoryIcon;
        private static Dictionary<string, Icon> _fileIcons = new Dictionary<string, Icon>();

        #region OfExtension

        ///<summary>
        /// Get the icon of an extension
        ///</summary>
        ///<param name="filename">filename</param>
        ///<param name="overlay">bool symlink overlay</param>
        ///<returns>Icon</returns>
        public static Icon OfExtension(string filename, bool overlay = false)
        {
            var extension = Path.GetExtension(filename) ?? String.Empty;
            if (_fileIcons.ContainsKey(extension))
                return _fileIcons[extension];
            
            var testName = (String.IsNullOrEmpty(filename) || String.IsNullOrEmpty(extension))
                ? "dummy_file"
                : "dummy" + extension;
            var dirpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_icon_cache");
            Directory.CreateDirectory(dirpath);
            string filepath = Path.Combine(dirpath, testName);
            
            if (File.Exists(filepath) == false)
                File.Create(filepath);
            var icon = OfPath(filepath, true, true, overlay);
            _fileIcons[extension] = icon;
            return icon;
        }
        #endregion

        #region OfFolder

        ///<summary>
        /// Get the icon of an folder
        ///</summary>
        ///<returns>Icon</returns>
        ///<param name="overlay">bool symlink overlay</param>
        public static Icon OfFolder(bool overlay = false)
        {
            if (_directoryIcon != null)
            {
                return _directoryIcon;
            }
            var dirpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_icon_cache", "dummy");
            Directory.CreateDirectory(dirpath);
            var icon = OfPath(dirpath, true, true, overlay);
            _directoryIcon = icon;
            return icon;
        }
        #endregion

        #region OfPath

        ///<summary>
        /// Get the normal,small assigned icon of the given path
        ///</summary>
        ///<param name="filepath">physical path</param>
        ///<param name="small">bool small icon</param>
        ///<param name="checkdisk">bool fileicon</param>
        ///<param name="overlay">bool symlink overlay</param>
        ///<returns>Icon</returns>
        private static Icon OfPath(string filepath, bool small = true, bool checkdisk = true, bool overlay = false)
        {
            SHGFI_Flag flags;
            var shinfo = new SHFILEINFO();
            if (small)
            {
                flags = SHGFI_Flag.SHGFI_ICON | SHGFI_Flag.SHGFI_SMALLICON;
            }
            else
            {
                flags = SHGFI_Flag.SHGFI_ICON | SHGFI_Flag.SHGFI_LARGEICON;
            }
            if (checkdisk == false)
            {
                flags |= SHGFI_Flag.SHGFI_USEFILEATTRIBUTES;
            }
            if (overlay)
            {
                flags |= SHGFI_Flag.SHGFI_LINKOVERLAY;
            }
            if (SHGetFileInfo(filepath, 0, ref shinfo, Marshal.SizeOf(shinfo), flags) == 0)
            {
                throw (new FileNotFoundException());
            }
            var tmp = Icon.FromHandle(shinfo.hIcon);
            var clone = (Icon)tmp.Clone();
            tmp.Dispose();
            if (DestroyIcon(shinfo.hIcon) != 0)
            {
                return clone;
            }
            return clone;
        }
        #endregion
    }
}

