using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace AppTray.Views.Converters {
    public class KeyStringConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (!(value is Key)) {
                throw new ArgumentException();
            }
            if ((Key)value == Key.None) {
                return string.Empty;
            }
            var conv = new KeyConverter();
            return conv.ConvertToString(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            if (!(value is string)) {
                throw new ArgumentException();
            }
            var conv = new KeyConverter();
            return conv.ConvertFromString(value.ToString());
        }
    }
}
