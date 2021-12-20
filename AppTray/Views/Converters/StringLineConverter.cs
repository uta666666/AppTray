using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace AppTray.Views.Converters
{
    internal class StringLineConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var src = value as IEnumerable<string>;
            if (src == null)
            {
                return null;
            }
            return string.Join(Environment.NewLine, src);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var src = value as string;
            if (string.IsNullOrEmpty(src))
            {
                return new List<string>();
            }
            return src.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
        }


        private static StringLineConverter _converter;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new StringLineConverter());
        }
    }
}
