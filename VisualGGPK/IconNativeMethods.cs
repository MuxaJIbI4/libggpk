using System;
using System.Runtime.InteropServices;

namespace VisualGGPK
{
    internal class IconNativeMethods
    {
        public enum SHGFI_Flag : uint
        {
            SHGFI_LARGEICON             = 0x000000000,
            SHGFI_SMALLICON             = 0x000000001,
            SHGFI_OPENICON              = 0x000000002,
            SHGFI_SHELLICONSIZE         = 0x000000004,
            SHGFI_USEFILEATTRIBUTES     = 0x000000010,
            SHGFI_ADDOVERLAYS           = 0x000000020,
            SHGFI_ICON                  = 0x000000100,
            SHGFI_DISPLAYNAME           = 0x000000200,
            SHGFI_TYPENAME              = 0x000000400,
            SHGFI_ICONLOCATION          = 0x000001000,
            SHGFI_EXETYPE               = 0x000002000,
            SHGFI_SYSICONINDEX          = 0x000004000,
            SHGFI_LINKOVERLAY           = 0x000008000,
            SHGFI_ATTR_SPECIFIED        = 0x000020000,
        }

        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };

        [DllImport("user32.dll")]
        public static extern int DestroyIcon(IntPtr hIcon);

        [DllImport("shell32.dll", CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern IntPtr ExtractIcon(IntPtr hInst, string lpszExeFileName, int nIconIndex);

        [DllImport("Shell32.dll", BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int SHGetFileInfo(string pszPath, int dwFileAttributes, ref SHFILEINFO psfi, int cbFileInfo, SHGFI_Flag uFlags);

        [DllImport("Shell32.dll")]
        public static extern int SHGetFileInfo(IntPtr pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, int cbFileInfo, SHGFI_Flag uFlags);
    }
}
