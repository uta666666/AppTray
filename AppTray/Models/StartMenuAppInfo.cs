using AppTray.Commons;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace AppTray.Models
{
    public class StartMenuAppInfo
    {
        public StartMenuAppInfo()
        {
            //string uninstall_path = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall";
            //string uninstall_path_x86 = "SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall";

            //_appInfo = GetUninstallList(uninstall_path).Union(GetUninstallList(uninstall_path_x86)).ToList();

            _appInfo = GetStartMenuApps();
        }

        private List<AppInfo> _appInfo;

        public List<AppInfo> AppInfo { get { return _appInfo; } }

        //private static List<string> GetUninstallList(string uninstallPath)
        //{
        //    List<string> ret = new List<string>();

        //    Microsoft.Win32.RegistryKey uninstall = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(uninstallPath, false);
        //    if (uninstall != null)
        //    {
        //        foreach (string subKey in uninstall.GetSubKeyNames())
        //        {
        //            string appName = null;
        //            Microsoft.Win32.RegistryKey appkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(uninstallPath + "\\" + subKey, false);

        //            if (appkey.GetValue("DisplayName") != null)
        //                appName = appkey.GetValue("DisplayName").ToString();
        //            else
        //                appName = subKey;

        //            ret.Add(appName);
        //        }
        //    }

        //    return ret;
        //}

        private List<AppInfo> GetStartMenuApps()
        {
            var list = new List<AppInfo>();

            if (StaticValues.SystemSetting.IsSearchInStartMenu)
            {
                GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), list);
                GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu), list);
            }
            return list;
        }

        private void GetFiles(string dir, List<AppInfo> list)
        {
            try
            {
                var files = Directory.GetFiles(dir, "*.lnk", SearchOption.TopDirectoryOnly);
                foreach (string file in files)
                {
                    list.Add(new AppInfoLink(file));
                }
                var dirs = Directory.GetDirectories(dir);
                foreach (var subDir in dirs)
                {
                    GetFiles(subDir, list);
                }
            }
            catch { }

        }
    }
}
