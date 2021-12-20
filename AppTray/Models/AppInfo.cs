using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using AppTray.Commons;

namespace AppTray.Models
{
    public class AppInfo
    {

        [DllImport("Shell32.dll", CharSet = CharSet.Auto, BestFitMapping = false, EntryPoint = "ExtractAssociatedIcon")]
        public static extern IntPtr IntExtractAssociatedIcon(HandleRef hInst, StringBuilder iconPath, ref int index);
        // ExtractIconEx 複数の引数指定方法により、オーバーロード定義する。
        //[DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
        //protected static extern uint ExtractIconEx(string lpszFile, int nIconIndex,
        //    IntPtr[] phiconLarge, IntPtr[] phiconSmall, uint nIcons);
        //[DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
        //protected static extern uint ExtractIconEx(string lpszFile, int nIconIndex,
        //    IntPtr[] phiconLarge, IntPtr phiconSmall, uint nIcons);
        //[DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
        //protected static extern uint ExtractIconEx(string lpszFile, int nIconIndex,
        //    IntPtr phiconLarge, IntPtr[] phiconSmall, uint nIcons);
        //[DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
        //protected static extern uint ExtractIconEx(string lpszFile, int nIconIndex,
        //    IntPtr phiconLarge, IntPtr phiconSmall, uint nIcons);
        // DestroyIcon の定義
        //[DllImport("User32.dll", CharSet = CharSet.Unicode)]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //protected static extern bool DestroyIcon(IntPtr hIcon);


        public AppInfo() { }

        public void SetBitmapSource()
        {
            if (Icon == null)
            {
                return;
            }
            ImageSource = ConvertBitmapToBitmapSource();
        }

        private BitmapSource ConvertBitmapToBitmapSource()
        {
            IntPtr hbitmap = ConvertIconToBitmap().GetHbitmap();
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        private Bitmap ConvertIconToBitmap()
        {
            return Icon.ToBitmap();
        }

        protected void SetIconAndBitmapSource(string filePath)
        {
            var url = new Uri(filePath);
            if (!url.IsUnc)
            {
                Icon = Icon.ExtractAssociatedIcon(filePath);
                SetBitmapSource();
                return;
            }
            int index = 0;
            HandleRef NullHandleRef = new HandleRef(null, IntPtr.Zero);
            IntPtr hIcon = IntExtractAssociatedIcon(NullHandleRef, new StringBuilder(260).Append(filePath), ref index);
            Icon = Icon.FromHandle(hIcon);
            SetBitmapSource();

            //IntPtr[] hLargeIcon = new IntPtr[1] { IntPtr.Zero };
            //IntPtr[] hSmallIcon = new IntPtr[1] { IntPtr.Zero };
            //try {
            //    ExtractIconEx(filePath, 0, hLargeIcon, hSmallIcon, 1);
            //    if (hLargeIcon[0] != IntPtr.Zero) {
            //        using (Icon largeIcon = Icon.FromHandle(hLargeIcon[0])) {
            //            Icon = largeIcon;
            //        }
            //        SetBitmapSource(hLargeIcon[0], new Int32Rect(0, 0, Icon.Width, Icon.Height));
            //    } else if (hSmallIcon[0] != IntPtr.Zero) {
            //        using (Icon smallIcon = Icon.FromHandle(hSmallIcon[0])) {
            //            Icon = smallIcon;
            //        }
            //        SetBitmapSource(hSmallIcon[0], new Int32Rect(0, 0, Icon.Width, Icon.Height));
            //    }
            //} finally {
            //    foreach (IntPtr ptr in hLargeIcon) {
            //        if (ptr != IntPtr.Zero) {
            //            DestroyIcon(ptr);
            //        }
            //    }
            //    foreach (IntPtr ptr in hSmallIcon) {
            //        if (ptr != IntPtr.Zero) {
            //            DestroyIcon(ptr);
            //        }
            //    }
            //}
        }

        private void SetBitmapSource(IntPtr hicon, Int32Rect rect)
        {
            ImageSource = ConvertHIconToBitmapSource(hicon, rect);
        }

        private BitmapSource ConvertHIconToBitmapSource(IntPtr hicon, Int32Rect rect)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(hicon, rect, BitmapSizeOptions.FromEmptyOptions());
        }

        public virtual bool Exist()
        {
            return File.Exists(FilePath) || Directory.Exists(FilePath);
        }

        public virtual void Execute(bool isAdmin)
        {
            try
            {
                if (!Exist())
                {
                    return;
                }
                Process p = new Process();
                p.StartInfo.FileName = FilePath;
                if (isAdmin)
                {
                    p.StartInfo.Verb = "RunAs";
                }
                p.StartInfo.Arguments = Arguments;
                p.StartInfo.WorkingDirectory = WorkDirectory;
                p.Start();
            }
            catch
            {
                //mushi
            }
        }

        public string AppDisplayName { get; set; }

        public AppCategory Category { get; set; }

        public string FilePath { get; set; }

        public string Arguments { get; set; }

        public string WorkDirectory { get; set; }

        public List<string> Command { get; set; }

        public bool IsAdmin { get; set; }

        [JsonConverter(typeof(IconJsonConverter))]
        public Icon Icon { get; set; }

        //[JsonConverter(typeof(BitmapSourceJsonConverter))]
        [JsonIgnore]
        public BitmapSource ImageSource { get; set; }
    }
}
