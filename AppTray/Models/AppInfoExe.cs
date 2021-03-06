﻿using System;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;

namespace AppTray.Models {
    public class AppInfoExe : AppInfo {

        public AppInfoExe(string filePath) : base() {
            FilePath = filePath;
            AppDisplayName = Path.GetFileNameWithoutExtension(FilePath);
            Arguments = string.Empty;
            WorkDirectory = Path.GetDirectoryName(FilePath);

            SetIconAndBitmapSource(filePath);
        }
    }
}
