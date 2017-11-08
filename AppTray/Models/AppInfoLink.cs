using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AppTray.Models {
    public class AppInfoLink : AppInfo {

        public AppInfoLink(string filePath) : base() {
            var objShortcut = LoadLink(filePath);
            if (File.Exists(objShortcut.TargetPath)) {
                FilePath = objShortcut.TargetPath;
                Arguments = objShortcut.Arguments;
                WorkDirectory = objShortcut.WorkingDirectory;
            } else {
                FilePath = filePath;
            }
            AppDisplayName = Path.GetFileNameWithoutExtension(filePath);
            SetIconAndBitmapSource(FilePath);
        }

        private IWshRuntimeLibrary.IWshShortcut LoadLink(string filePath) {
            var shell = new IWshRuntimeLibrary.WshShell();
            var objShortcut = shell.CreateShortcut(filePath) as IWshRuntimeLibrary.IWshShortcut;
            return objShortcut;
        }
    }
}
