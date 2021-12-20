using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTray.Models
{
    public class AppInfoCmd : AppInfo
    {
        public AppInfoCmd() : base()
        {
            string windir = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
            var cmd = Path.Combine(Path.Combine(windir, "system32"), "cmd.exe");
            SetIconAndBitmapSource(cmd);
        }

        public override bool Exist()
        {
            return true;
        }

        public override void Execute(bool isAdmin)
        {
            foreach (var c in Command.Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                try
                {
                    Process p = new Process();
                    //p.StartInfo.FileName = "cmd.exe";
                    p.StartInfo.FileName = c;
                    //p.StartInfo.Arguments = Command;
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.UseShellExecute = isAdmin;
                    if (isAdmin)
                    {
                        p.StartInfo.Verb = "RunAs";
                    }
                    p.Start();
                }
                catch
                {
                    //mushi
                }
            }
        }
    }
}
