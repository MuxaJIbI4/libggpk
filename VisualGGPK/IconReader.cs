using System;
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

        #region OfExtension

        ///<summary>
        /// Get the icon of an extension
        ///</summary>
        ///<param name="filename">filename</param>
        ///<param name="overlay">bool symlink overlay</param>
        ///<returns>Icon</returns>
        public static Icon OfExtension(string filename, bool overlay = false)
        {
            string filepath;
            var extension = filename.Split('.');
            var dirpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cache");
            Directory.CreateDirectory(dirpath);
            if (String.IsNullOrEmpty(filename) || extension.Length == 1)
            {
                filepath = Path.Combine(dirpath, "dummy_file");
            }
            else
            {
                filepath = Path.Combine(dirpath, String.Join(".", "dummy", extension[extension.Length - 1]));
            }
            if (File.Exists(filepath) == false)
            {
                File.Create(filepath);
            }
            var icon = OfPath(filepath, true, true, overlay);
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
            var dirpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cache", "dummy");
            Directory.CreateDirectory(dirpath);
            var icon = OfPath(dirpath, true, true, overlay);
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
        public static Icon OfPath(string filepath, bool small = true, bool checkdisk = true, bool overlay = false)
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

