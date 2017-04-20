using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MyToolsLauncher.Views.Converters {
    public class ResizeCaptionButtonVisibilityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return value is ResizeMode && (ResizeMode)value != ResizeMode.NoResize ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
