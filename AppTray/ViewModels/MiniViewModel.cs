using AppTray.Commons;
using AppTray.Models;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AppTray.ViewModels
{
    public class MiniViewModel : BindableBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MiniViewModel() {
            ButtonInfo = new ReactiveProperty<ButtonInfo>();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="buttonInfo"></param>
        public MiniViewModel(ButtonInfo buttonInfo) : this()
        {
            ButtonInfo.Value = buttonInfo;
        }

        //private ButtonInfo _buttonInfo;
        ///// <summary>
        ///// 登録アプリケーション情報
        ///// </summary>
        //public ButtonInfo ButtonInfo { get { return _buttonInfo; } }

        public ReactiveProperty<ButtonInfo> ButtonInfo { get; set; }

        /// <summary>
        /// AutoCompleteフィルタリング情報 
        /// </summary>
        public AutoCompleteFilterPredicate<object> ButtonInfoFilter
        {
            get
            {
                return (searchText, obj) => {
                    CompareInfo ci = CultureInfo.CurrentCulture.CompareInfo;
                    return ci.IndexOf((obj as AppInfo).AppDisplayName, searchText, CompareOptions.IgnoreWidth | CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType) >= 0;
                };
            }
        }
    }
}
