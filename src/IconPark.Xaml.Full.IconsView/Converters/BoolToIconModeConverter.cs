using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using Zhai.Famil.Converters;

namespace IconPark.Xaml.Full.IconsView.Converters
{
    public class BoolToIconModeConverter : ConverterMarkupExtensionBase<BoolToIconModeConverter>, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IconMode mode)
            {
                return mode == (IconMode)Enum.Parse(typeof(IconMode), parameter.ToString());
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(bool)value)
            {
                return null;
            }

            return (IconMode)Enum.Parse(typeof(IconMode), parameter.ToString());
        }
    }
}
