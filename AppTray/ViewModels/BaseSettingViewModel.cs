using AppTray.Commons;
using AppTray.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTray.ViewModels
{
    public abstract class BaseSettingViewModel : BindableBase
    {
        public abstract void SetAppInfo(AppInfo info);

        public abstract AppInfo GetAppInfo();

        public bool IsUpdate { get; set; }
    }
}
