using AppTray.Commons;
using AppTray.Models;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            OKCommand.Subscribe(() =>
            {
                IsUpdate = true;
                CanClose.Value = true;
            });

            CancelCommand = new ReactiveCommand();
            CancelCommand.Subscribe(() =>
            {
                IsUpdate = false;
                CanClose.Value = true;//キャンセルは常にtrue
            });
        }

        public void SetProperty(SystemSetting setting)
        {
            IsOpenOnTaskBar.Value = setting.IsOpenOnTaskBar;
            Opacity.Value = setting.Opacity;
        }

        public SystemSetting Return()
        {
            return new SystemSetting()
            {
                IsOpenOnTaskBar = IsOpenOnTaskBar.Value,
                Opacity = Opacity.Value
            };
        }


        public ReactiveCommand OKCommand { get; private set; }
        public ReactiveCommand CancelCommand { get; private set; }


        public ReactiveProperty<bool> IsOpenOnTaskBar { get; set; }
        public ReactiveProperty<double> Opacity { get; set; }
        public ReactiveProperty<bool> CanClose { get; set; }

        public bool IsUpdate { get; private set; }
    }
}
