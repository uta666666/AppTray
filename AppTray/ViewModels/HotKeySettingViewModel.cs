using AppTray.Commons;
using AppTray.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppTray.ViewModels {
    public class HotKeySettingViewModel : BindableBase {
        public HotKeySettingViewModel() {
            ModifierKeys = ModifierKeys.None;
            Key = Key.None;

            Initialize(new HotKey() { Key = Key.None, Modifiers = ModifierKeys.None });
        }

        public HotKeySettingViewModel(HotKey hotKey) {
            ModifierKeys = hotKey.Modifiers;
            Key = hotKey.Key;

            Initialize(hotKey);
        }

        private void Initialize(HotKey hotKey) {
            IsShift = false;
            IsCtrl = false;
            IsAlt = false;
            if ((ModifierKeys & ModifierKeys.Shift) == ModifierKeys.Shift) {
                IsShift = true;
            }
            if ((ModifierKeys & ModifierKeys.Control) == ModifierKeys.Control) {
                IsCtrl = true;
            }
            if ((ModifierKeys & ModifierKeys.Alt) == ModifierKeys.Alt) {
                IsAlt = true;
            }

            OKCommand = new RelayCommand(() => {
                ModifierKeys = ModifierKeys.None;
                if (_isShift) {
                    ModifierKeys |= ModifierKeys.Shift;
                }
                if (_isCtrl) {
                    ModifierKeys |= ModifierKeys.Control;
                }
                if (_isAlt) {
                    ModifierKeys |= ModifierKeys.Alt;
                }
                HotKey.Save(new HotKey { Key = Key, Modifiers = ModifierKeys });
                IsUpdate = true;
                CanClose = true;
            });

            CancelCommand = new RelayCommand(() => {
                IsUpdate = false;
                CanClose = true;
            });

            WindowCloseCommand = new RelayCommand(() => {
                IsUpdate = false;
                CanClose = true;
            });
        }

        public ICommand OKCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        public ICommand WindowCloseCommand { get; set; }

        private Key _key;
        public Key Key {
            get {
                return _key;
            }
            set {
                if (value == Key.None) {
                    return;
                }
                SetProperty(ref _key, value);
            }
        }

        private ModifierKeys _modifierKeys;
        public ModifierKeys ModifierKeys {
            get {
                return _modifierKeys;
            }
            set {
                SetProperty(ref _modifierKeys, value);
            }
        }

        private bool _isShift;
        public bool IsShift {
            get {
                return _isShift;
            }
            set {
                SetProperty(ref _isShift, value);
            }
        }

        private bool _isCtrl;
        public bool IsCtrl {
            get {
                return _isCtrl;
            }
            set {
                SetProperty(ref _isCtrl, value);
            }
        }

        private bool _isAlt;
        public bool IsAlt {
            get {
                return _isAlt;
            }
            set {
                SetProperty(ref _isAlt, value);
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
