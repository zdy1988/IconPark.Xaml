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
    public class BoolToPenLineCapConverter : ConverterMarkupExtensionBase<BoolToPenLineCapConverter>, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PenLineCap mode)
            {
                return mode == (PenLineCap)Enum.Parse(typeof(PenLineCap), parameter.ToString());
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(bool)value)
            {
                return null;
            }

            return (PenLineCap)Enum.Parse(typeof(PenLineCap), parameter.ToString());
        }
    }
}
