using MyToolsLauncher.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using MyToolsLauncher.Commons;

namespace MyToolsLauncher.Views.Converters {
    public class ApplicationInfoForAutoCompleteConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
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
            return unionList.Distinct(n => n.FilePath);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
