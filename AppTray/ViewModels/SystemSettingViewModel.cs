using AppTray.Commons;
using AppTray.Models;
using Livet.Messaging;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppTray.ViewModels
{
    public class SystemSettingViewModel : BindableBase
    {
        public SystemSettingViewModel()
        {
            IsOpenOnTaskBar = new ReactiveProperty<bool>();
            Opacity = new ReactiveProperty<double>();
            CanClose = new ReactiveProperty<bool>();

            OKCommand = new ReactiveCommand();
            CancelCommand = new ReactiveCommand();
            HotKeyCommand = new ReactiveCommand();
        }

        public SystemSettingViewModel(SystemSetting setting, HotKey hotkey) : this()
        {
            _hotKey = hotkey;

            OKCommand.Subscribe(() =>
            {
                IsUpdate = true;
                CanClose.Value = true;
            });

            CancelCommand.Subscribe(() =>
            {
                IsUpdate = false;
                CanClose.Value = true;//キャンセルは常にtrue
            });

            HotKeyCommand.Subscribe(() =>
            {
                using (var vm = new HotKeySettingViewModel(_hotKey))
                {
                    Messenger.Raise(new TransitionMessage(vm, "HotKeySettingWindowMessageKey"));
                    if (vm.IsUpdate)
                    {
                        _hotKey = new HotKey() { Key = vm.Key, Modifiers = vm.ModifierKeys };
                        IsUpdateHotKey = true;
                    }
                }
            });

            SetProperty(setting, hotkey);
        }

        private void SetProperty(SystemSetting setting, HotKey hotkey)
        {
            IsOpenOnTaskBar.Value = setting.IsOpenOnTaskBar;
            Opacity.Value = setting.Opacity;
        }


        public (SystemSetting settings, HotKey hotKey) Return()
        {
            var sysSettings = new SystemSetting()
            {
                IsOpenOnTaskBar = IsOpenOnTaskBar.Value,
                Opacity = Opacity.Value
            };

            return (sysSettings, _hotKey);
        }

        private HotKey _hotKey;

        public ReactiveCommand OKCommand { get; private set; }
        public ReactiveCommand CancelCommand { get; private set; }
        public ReactiveCommand HotKeyCommand { get; private set; }


        public ReactiveProperty<bool> IsOpenOnTaskBar { get; set; }
        public ReactiveProperty<double> Opacity { get; set; }
        public ReactiveProperty<bool> CanClose { get; set; }

        public bool IsUpdate { get; private set; }
        public bool IsUpdateHotKey { get; private set; }
    }
}
