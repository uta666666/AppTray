using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MyToolsLauncher.Models {
    public static class ShieldIcon {
        const UInt32 SHGSI_ICON = 0x000000100;
        const UInt32 SHGSI_SMALLICON = 0x000000001;
        const UInt32 SIID_SHIELD = 0x00000004D;

        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        struct SHSTOCKICONINFO {
            public Int32 cbSize;
            public IntPtr hIcon;
            public Int32 iSysImageIndex;
            public Int32 iIcon;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szPath;
        }

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        static extern void SHGetStockIconInfo(UInt32 siid, UInt32 uFlags, ref SHSTOCKICONINFO sii);

        static ShieldIcon() {
            SHSTOCKICONINFO sii = new SHSTOCKICONINFO();
            sii.cbSize = Marshal.SizeOf(sii);
            SHGetStockIconInfo(SIID_SHIELD, SHGSI_ICON, ref sii);
            if (sii.hIcon != IntPtr.Zero) {
                NormalIcon = Icon.FromHandle(sii.hIcon);
            }

            SHGetStockIconInfo(SIID_SHIELD, SHGSI_ICON | SHGSI_SMALLICON, ref sii);
            if (sii.hIcon != IntPtr.Zero) {
                SmallIcon = Icon.FromHandle(sii.hIcon);
            }
        }

        public static Icon NormalIcon {
            get;
            private set;
        }

        public static Icon SmallIcon {
            get;
            private set;
        }

        public static BitmapSource GetBitmapSource(bool isSmallIcon) {
            IntPtr hbitmap;
            if (isSmallIcon) {
                hbitmap = SmallIcon.ToBitmap().GetHbitmap();
            } else {
                hbitmap = NormalIcon.ToBitmap().GetHbitmap();
            }
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }
    }
}
