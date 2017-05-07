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

namespace AppTray.Models {
    public class AppInfo {
        public AppInfo() { }

        public void SetBitmapSource() {
            if (Icon == null) {
                return;
            }
            ImageSource = ConvertBitmapToBitmapSource();
        }

        private BitmapSource ConvertBitmapToBitmapSource() {
            IntPtr hbitmap = ConvertIconToBitmap().GetHbitmap();
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        private Bitmap ConvertIconToBitmap() {
            return Icon.ToBitmap();
        }

        public bool Exist() {
            return File.Exists(FilePath);
        }

        public void Execute(bool isAdmin) {
            try {
                if (!Exist()) {
                    return;
                }
                Process p = new Process();
                p.StartInfo.FileName = FilePath;
                if (isAdmin) {
                    p.StartInfo.Verb = "RunAs";
                }
                p.StartInfo.Arguments = Arguments;
                p.StartInfo.WorkingDirectory = WorkDirectory;
                p.Start();
            } catch {
                //mushi
            }
        }

        public string AppDisplayName { get; set; }

        public string FilePath { get; set; }

        public string Arguments { get; set; }

        public string WorkDirectory { get; set; }

        [JsonConverter(typeof(IconJsonConverter))]
        public Icon Icon { get; set; }

        [JsonIgnore]
        public BitmapSource ImageSource { get; set; }
    }
}
