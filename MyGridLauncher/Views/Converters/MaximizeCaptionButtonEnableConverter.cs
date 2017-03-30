using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MyToolsLauncher.Views.Converters {
    public class MaximizeCaptionButtonEnableConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return value is ResizeMode && (ResizeMode)value != ResizeMode.CanMinimize;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
