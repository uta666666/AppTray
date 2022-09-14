using AppTray.Commons;
using Livet;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using AppTray.Models;
using Reactive.Bindings.Extensions;

namespace AppTray.ViewModels
{
    public class CommandViewModel : BaseSettingViewModel
    {
        public CommandViewModel()
        {
            CanClose = new ReactiveProperty<bool>(false);
            AppDisplayName = new ReactiveProperty<string>();
            Command = new ReactiveProperty<List<string>>(new List<string>());
            IsAdmin = new ReactiveProperty<bool>(false);

            OKCommand = new[] { Command.Select(x => x.Where(y => !string.IsNullOrWhiteSpace(y)).Count() == 0), AppDisplayName.Select(x => string.IsNullOrWhiteSpace(x))}.CombineLatestValuesAreAllFalse().ToReactiveCommand();
            OKCommand.Subscribe(() =>
            {
                UpdateAppInfo();
                IsUpdate = true;
                CanClose.Value = true;
            });

            CancelCommand = new ReactiveCommand();
            CancelCommand.Subscribe(() =>
            {
                IsUpdate = false;
                CanClose.Value = true;
            });
        }

        private void UpdateAppInfo()
        {
            _appInfo.Category = AppCategory.Command;
            _appInfo.FilePath = String.Empty;
            _appInfo.Arguments = String.Empty;
            _appInfo.WorkDirectory = string.Empty;

            if (_appInfo.AppDisplayName != AppDisplayName.Value)
            {
                _appInfo.AppDisplayName = AppDisplayName.Value;
            }
            if (_appInfo.IsAdmin != IsAdmin.Value)
            {
                _appInfo.IsAdmin = IsAdmin.Value;
            }
            //if (_appInfo.Command != Command.Value)
            //{
                _appInfo.Command = Command.Value;
            //}
        }

        public override void SetAppInfo(AppInfo info)
        {
            if (info is AppInfoCmd)
            {
                _appInfo = info as AppInfoCmd;

                AppDisplayName.Value = _appInfo.AppDisplayName;
                Command.Value = _appInfo.Command;
                IsAdmin.Value = _appInfo.IsAdmin;
            }
            else
            {
                _appInfo = new AppInfoCmd();
            }
        }

        public override AppInfo GetAppInfo()
        {
            return _appInfo;
        }


        private AppInfoCmd _appInfo;


        public ReactiveCommand OKCommand { get; private set; }
        public ReactiveCommand CancelCommand { get; private set; }


        public ReactiveProperty<bool> CanClose { get; set; }

        public ReactiveProperty<string> AppDisplayName { get; set; }

        public ReactiveProperty<List<string>> Command { get; set; }

        public ReactiveProperty<bool> IsAdmin { get; set; }
    }
}
