using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using Zhai.Famil.Converters;

namespace IconPark.Xaml.IconsView.Converters
{
    public class ColorToBrushConverter : ConverterMarkupExtensionBase<ColorToBrushConverter>, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Length == 2)
            {
                if (values[0] is Color color)
                {
                    return new SolidColorBrush(color);
                }
                else if (values[1] is Brush brush)
                {
                    return brush;
                }
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
