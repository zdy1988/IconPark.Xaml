using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace IconPark.Xaml.Full.IconsView.Converters
{
    internal class ListBoxItemTemplateConverter : IValueConverter
    {
        public DataTemplate NormalTemplate { get; set; }

        public DataTemplate SimpleTemplate { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isSimpled && isSimpled)
            {
                return SimpleTemplate;
            }

            return NormalTemplate;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
