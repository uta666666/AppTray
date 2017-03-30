using MyToolsLauncher.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MyToolsLauncher.Views.Converters {
    public class ImageSourceMultiBindConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            if (!(values[0] is ButtonInfo && values[1] is string)) {
                return null;
            }

            var buttonInfo = (values[0] as ButtonInfo);
            var param = values[1].ToString();
            var bi = buttonInfo[(int.Parse(param))];
            if (bi == null) {
                return null;
            } else {
                return bi.ImageSource;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
