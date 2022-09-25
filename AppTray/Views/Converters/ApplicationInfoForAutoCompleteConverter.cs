using AppTray.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using AppTray.Commons;
using System.Windows.Markup;

namespace AppTray.Views.Converters {
    public class ApplicationInfoForAutoCompleteConverter : MarkupExtension, IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if(value == null) return null;

            if (!(value is ButtonInfo)) {
                throw new ArgumentException();
            }

            IEnumerable<AppInfo> unionList = null;
            foreach (var apps in (value as ButtonInfo).GetButtonInfoAllPage().Select(n => n.Value.Values)) {
                if (unionList == null) {
                    unionList = apps;
                    continue;
                }
                unionList = unionList.Concat(apps);
            }

            unionList = unionList.Concat((value as ButtonInfo).GetInstalledAppInfo());

            return unionList.Distinct(n => n.AppDisplayName);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }


        private static ApplicationInfoForAutoCompleteConverter _converter;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new ApplicationInfoForAutoCompleteConverter());
        }
    }
}
