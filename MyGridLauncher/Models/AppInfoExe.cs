using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AppTray.Models {
    public class AppInfoExe : AppInfo {

        public AppInfoExe(string filePath) : base() {
            FilePath = filePath;
            AppDisplayName = Path.GetFileNameWithoutExtension(FilePath);
            Icon = Icon.ExtractAssociatedIcon(filePath);
            Arguments = string.Empty;
            WorkDirectory = Path.GetDirectoryName(FilePath);
            SetBitmapSource();
        }
    }
}
