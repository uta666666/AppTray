using AppTray.Commons;
using AppTray.Models;
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

namespace AppTray.ViewModels {
    public class SettingViewModel : BindableBase {
        public SettingViewModel() {
            OKCommand = new RelayCommand(() => {
                if (HasChanges()) {
                    if (!File.Exists(FilePath) && !Directory.Exists(FilePath)) {
                        MessageBox.Show("ファイルが見つかりません。");
                        IsUpdate = true;
                        CanClose = false;
                        return;
                    }
                    IsUpdate = true;
                    CanClose = true;
                    return;
                }
                //キャンセル扱い
                IsUpdate = false;
                CanClose = true;
            });

            CancelCommand = new RelayCommand(() => {
                IsUpdate = false;
                CanClose = true;//キャンセルは常にtrue
            });

            WindowCloseCommand = new RelayCommand<Window>((w) => {
                IsUpdate = false;
                CanClose = true;
            });
        }

        private bool HasChanges() {
            if (_appInfo.FilePath != FilePath ||
                _appInfo.AppDisplayName != AppDisplayName ||
                _appInfo.Arguments != Arguments ||
                _appInfo.WorkDirectory != WorkDirectory ||
                _appInfo.ImageSource != AppIcon ||
                _appInfo.IsAdmin != IsAdmin) {
                return true;
            }
            return false;
        }

        public ICommand OKCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        public ICommand WindowCloseCommand { get; set; }

        private AppInfo _appInfo;

        public void SetAppInfo(AppInfo info) {
            _appInfo = info;
            AppDisplayName = info.AppDisplayName;
            FilePath = _appInfo.FilePath;
            Arguments = _appInfo.Arguments;
            AppIcon = _appInfo.ImageSource;
            WorkDirectory = _appInfo.WorkDirectory;
            IsAdmin = _appInfo.IsAdmin;
        }

        public AppInfo GetAppInfo() {
            if (_appInfo.FilePath != FilePath) {
                _appInfo = AppInfoFactory(FilePath);
            } else {
                if (_appInfo.AppDisplayName != AppDisplayName) {
                    _appInfo.AppDisplayName = AppDisplayName;
                }
                if (_appInfo.Arguments != Arguments) {
                    _appInfo.Arguments = Arguments;
                }
                if (_appInfo.WorkDirectory != WorkDirectory) {
                    _appInfo.WorkDirectory = WorkDirectory;
                }
                if (_appInfo.IsAdmin != IsAdmin) {
                    _appInfo.IsAdmin = IsAdmin;
                }
            }
            return _appInfo;
        }

        private AppInfo AppInfoFactory(string filePath) {
            var ext = Path.GetExtension(filePath);
            if (ext.ToLower() == ".exe") {
                return new AppInfoExe(filePath);
            } else if (ext.ToLower() == ".lnk") {
                return new AppInfoLink(filePath);
            } else if (Directory.Exists(filePath)) {
                return new FolderInfo(filePath);
            } else if (File.Exists(filePath)) {
                return new AppInfoFile(filePath);
            } else {
                throw new NotImplementedException();
            }
        }

        private string _appDisplayName;
        public string AppDisplayName {
            get {
                return _appDisplayName;
            }
            set {
                SetProperty(ref _appDisplayName, value);
            }
        }

        private string _filePath;
        public string FilePath {
            get {
                return _filePath;
            }
            set {
                SetProperty(ref _filePath, value);
            }
        }

        private string _arguments;
        public string Arguments {
            get {
                return _arguments;
            }
            set {
                SetProperty(ref _arguments, value);
            }
        }

        private string _workDirectory;
        public string WorkDirectory {
            get {
                return _workDirectory;
            }
            set {
                SetProperty(ref _workDirectory, value);
            }
        }

        private bool _isAdmin;
        public bool IsAdmin {
            get {
                return _isAdmin;
            }
            set {
                SetProperty(ref _isAdmin, value);
            }
        }

        public BitmapSource _icon;
        public BitmapSource AppIcon {
            get {
                return _icon;
            }
            set {
                SetProperty(ref _icon, value);
            }
        }

        private bool _canClose;
        public bool CanClose {
            get {
                return _canClose;
            }
            set {
                SetProperty(ref _canClose, value);
            }
        }

        public bool IsUpdate { get; set; }
    }
}
