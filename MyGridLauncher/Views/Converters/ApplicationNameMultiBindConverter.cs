using AppTray.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AppTray.Views.Converters {
    class ApplicationNameMultiBindConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            if (!(values[0] is ButtonInfo && values[1] is string)) {
                return null;
            }

            var buttonInfo = (values[0] as ButtonInfo);
            var param = values[1].ToString();
            var appInfo = buttonInfo[(int.Parse(param))];
            if (appInfo == null) {
                return null;
            } else {
                return appInfo.AppDisplayName;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
