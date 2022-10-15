using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Text.RegularExpressions;

namespace AppTray.Models
{
    public class AppInfoLink : AppInfo
    {
        public AppInfoLink() : base() { }

        public AppInfoLink(string filePath) : base()
        {
            var objShortcut = LoadLink(filePath);
            string fullFilePath = ReplaceEnvironmentValue(objShortcut.TargetPath);
            if (File.Exists(fullFilePath))
            {
                FilePath = fullFilePath;
                Arguments = ReplaceEnvironmentValue(objShortcut.Arguments);
                Arguments = ReplaceEnvironmentValue(objShortcut.Arguments);
                WorkDirectory = ReplaceEnvironmentValue(objShortcut.WorkingDirectory);
            }
            else
            {
                FilePath = filePath;
            }
            AppDisplayName = Path.GetFileNameWithoutExtension(filePath);
            SetIconAndBitmapSource(FilePath);
        }

        private IWshRuntimeLibrary.IWshShortcut LoadLink(string filePath)
        {
            var shell = new IWshRuntimeLibrary.WshShell();
            var objShortcut = shell.CreateShortcut(filePath) as IWshRuntimeLibrary.IWshShortcut;
            return objShortcut;
        }

        private string ReplaceEnvironmentValue(string src)
        {
            Regex regex = new Regex("%.+%");
            var mutches = regex.Matches(src);
            foreach (Match match in mutches)
            {
                if (!match.Success)
                {
                    continue;
                }
                string envVal = Environment.GetEnvironmentVariable(match.Value.Trim('%'));
                src = src.Replace(match.Value, envVal);
            }
            return src;
        }
    }
}
