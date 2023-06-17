using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Zhai.Famil.Converters;

namespace IconPark.Xaml.Full.IconsView.Converters
{
    internal class IconModeToVisibilityConverter : ConverterMarkupExtensionBase<IconModeToVisibilityConverter>, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IconMode mode)
            {
                return mode == (IconMode)Enum.Parse(typeof(IconMode), parameter.ToString()) ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
