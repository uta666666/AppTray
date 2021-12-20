using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTray.Models
{
    public class AppInfoFile : AppInfo
    {
        public AppInfoFile(string filePath) : base()
        {
            FilePath = filePath;
            AppDisplayName = Path.GetFileNameWithoutExtension(FilePath);
            Arguments = string.Empty;
            WorkDirectory = Path.GetDirectoryName(FilePath);

            SetIconAndBitmapSource(filePath);
        }
    }
}
