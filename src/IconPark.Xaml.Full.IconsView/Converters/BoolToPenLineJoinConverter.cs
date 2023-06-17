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
    public class BoolToPenLineJoinConverter : ConverterMarkupExtensionBase<BoolToPenLineJoinConverter>, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PenLineJoin mode)
            {
                return mode == (PenLineJoin)Enum.Parse(typeof(PenLineJoin), parameter.ToString());
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(bool)value)
            {
                return null;
            }

            return (PenLineJoin)Enum.Parse(typeof(PenLineJoin), parameter.ToString());
        }
    }
}
