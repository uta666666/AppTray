using System;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;

namespace AppTray.Models {
    public class AppInfoExe : AppInfo {
        // ExtractIconEx 複数の引数指定方法により、オーバーロード定義する。
        [DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
        private static extern uint ExtractIconEx(string lpszFile, int nIconIndex,
            IntPtr[] phiconLarge, IntPtr[] phiconSmall, uint nIcons);
        [DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
        private static extern uint ExtractIconEx(string lpszFile, int nIconIndex,
            IntPtr[] phiconLarge, IntPtr phiconSmall, uint nIcons);
        [DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
        private static extern uint ExtractIconEx(string lpszFile, int nIconIndex,
            IntPtr phiconLarge, IntPtr[] phiconSmall, uint nIcons);
        [DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
        private static extern uint ExtractIconEx(string lpszFile, int nIconIndex,
            IntPtr phiconLarge, IntPtr phiconSmall, uint nIcons);
        // DestroyIcon の定義
        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DestroyIcon(IntPtr hIcon);


        public AppInfoExe(string filePath) : base() {
            FilePath = filePath;
            AppDisplayName = Path.GetFileNameWithoutExtension(FilePath);
            var url = new Uri(filePath);
            if (url.IsUnc) {
                Icon = Icon.ExtractAssociatedIcon(filePath);
                SetBitmapSource();
            } else {
                IntPtr[] hLargeIcon = new IntPtr[1] { IntPtr.Zero };
                IntPtr[] hSmallIcon = new IntPtr[1] { IntPtr.Zero };
                try {
                    ExtractIconEx(filePath, 0, hLargeIcon, hSmallIcon, 1);
                    if (hLargeIcon[0] != IntPtr.Zero) {
                        using (Icon largeIcon = Icon.FromHandle(hLargeIcon[0])) {
                            Icon = largeIcon;
                        }
                        SetBitmapSource(hLargeIcon[0], new System.Windows.Int32Rect(0, 0, Icon.Width, Icon.Height));
                    } else if (hSmallIcon[0] != IntPtr.Zero) {
                        using (Icon smallIcon = Icon.FromHandle(hSmallIcon[0])) {
                            Icon = smallIcon;
                        }
                        SetBitmapSource(hSmallIcon[0], new System.Windows.Int32Rect(0, 0, Icon.Width, Icon.Height));
                    }
                } finally {
                    foreach (IntPtr ptr in hLargeIcon) {
                        if (ptr != IntPtr.Zero) {
                            DestroyIcon(ptr);
                        }
                    }
                    foreach (IntPtr ptr in hSmallIcon) {
                        if (ptr != IntPtr.Zero) {
                            DestroyIcon(ptr);
                        }
                    }
                }
            }
            Arguments = string.Empty;
            WorkDirectory = Path.GetDirectoryName(FilePath);
        }
    }
}
