using AppTray.Commons;
using AppTray.Models;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Reactive.Linq;
using Reactive.Bindings.Extensions;
using Livet.Messaging.IO;

namespace AppTray.ViewModels
{
    public class SettingViewModel : BaseSettingViewModel
    {
        public SettingViewModel()
        {
            AppDisplayName = new ReactiveProperty<string>();
            FilePath = new ReactiveProperty<string>();
            Arguments = new ReactiveProperty<string>();
            WorkDirectory = new ReactiveProperty<string>();
            IsAdmin = new ReactiveProperty<bool>();
            AppIcon = new ReactiveProperty<BitmapSource>();
            CanClose = new ReactiveProperty<bool>();

            SelectIconCommand = FilePath.Select(x => !string.IsNullOrWhiteSpace(x)).ToReactiveCommand();
            SelectIconCommand.Subscribe(async () =>
            {
                // メッセージを作成
                var message = new OpeningFileSelectionMessage("MessageKey_OpenFile");

                // メッセージを送信
                await Messenger.RaiseAsync(message);

                // メッセージへの応答がない場合は何もしない
                // ファイル選択ダイアログでキャンセルされた場合も応答なしになる
                if (message.Response == null) return;

                // 開いたファイルのパスを更新
                _appInfo.SetIconAndBitmapSource(message.Response.First());
                AppIcon.Value = _appInfo.ImageSource;
                IsUpdate = true;
            });

            OKCommand = new[] { AppDisplayName.Select(x => string.IsNullOrWhiteSpace(x)), FilePath.Select(x => string.IsNullOrWhiteSpace(x)) }.CombineLatestValuesAreAllFalse().ToReactiveCommand();
            OKCommand.Subscribe(() =>
            {
                if (HasChanges())
                {
                    if (!File.Exists(FilePath.Value) && !Directory.Exists(FilePath.Value))
                    {
                        MessageBox.Show("ファイルが見つかりません。");
                        IsUpdate = true;
                        CanClose.Value = false;
                        return;
                    }
                    UpdateAppInfo();
                    IsUpdate = true;
                    CanClose.Value = true;
                    return;
                }
                CanClose.Value = true;
            });

            CancelCommand = new ReactiveCommand();
            CancelCommand.Subscribe(() =>
            {
                IsUpdate = false;
                CanClose.Value = true;//キャンセルは常にtrue
            });
        }

        private bool HasChanges()
        {
            if (_appInfo.FilePath != FilePath.Value ||
                _appInfo.AppDisplayName != AppDisplayName.Value ||
                _appInfo.Arguments != Arguments.Value ||
                _appInfo.WorkDirectory != WorkDirectory.Value ||
                _appInfo.ImageSource != AppIcon.Value ||
                _appInfo.IsAdmin != IsAdmin.Value)
            {
                return true;
            }
            return false;
        }

        private void UpdateAppInfo()
        {
            if (_appInfo.FilePath != FilePath.Value)
            {
                //ファイルが変わったら作り直す
                _appInfo = AppInfoFactory(FilePath.Value);
            }

            if (_appInfo.AppDisplayName != AppDisplayName.Value)
            {
                _appInfo.AppDisplayName = AppDisplayName.Value;
            }
            if (_appInfo.Arguments != Arguments.Value)
            {
                _appInfo.Arguments = Arguments.Value;
            }
            if (_appInfo.WorkDirectory != WorkDirectory.Value)
            {
                _appInfo.WorkDirectory = WorkDirectory.Value;
            }
            if (_appInfo.IsAdmin != IsAdmin.Value)
            {
                _appInfo.IsAdmin = IsAdmin.Value;
            }
        }

        public override void SetAppInfo(AppInfo info)
        {
            _appInfo = info;
            AppDisplayName.Value = info.AppDisplayName;
            FilePath.Value = _appInfo.FilePath;
            Arguments.Value = _appInfo.Arguments;
            AppIcon.Value = _appInfo.ImageSource;
            WorkDirectory.Value = _appInfo.WorkDirectory;
            IsAdmin.Value = _appInfo.IsAdmin;
            IsUpdate = false;
        }

        public override AppInfo GetAppInfo()
        {
            return _appInfo;
        }

        private AppInfo AppInfoFactory(string filePath)
        {
            var ext = Path.GetExtension(filePath);
            if (ext.ToLower() == ".exe")
            {
                return new AppInfoExe(filePath);
            }
            else if (ext.ToLower() == ".lnk")
            {
                return new AppInfoLink(filePath);
            }
            else if (Directory.Exists(filePath))
            {
                return new FolderInfo(filePath);
            }
            else if (File.Exists(filePath))
            {
                return new AppInfoFile(filePath);
            }
            else
            {
                throw new NotImplementedException();
            }
        }


        private AppInfo _appInfo;


        public ReactiveCommand OKCommand { get; private set; }

        public ReactiveCommand CancelCommand { get; private set; }

        public ReactiveCommand SelectIconCommand { get; private set; }

        public ReactiveProperty<string> AppDisplayName { get; set; }

        public ReactiveProperty<string> FilePath { get; set; }

        public ReactiveProperty<string> Arguments { get; set; }

        public ReactiveProperty<string> WorkDirectory { get; set; }

        public ReactiveProperty<bool> IsAdmin { get; set; }

        public ReactiveProperty<BitmapSource> AppIcon { get; set; }

        public ReactiveProperty<bool> CanClose { get; set; }

    }
}
