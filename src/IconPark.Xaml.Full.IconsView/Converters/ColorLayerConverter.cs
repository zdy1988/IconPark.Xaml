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
    internal class ColorLayerConverter : ConverterMarkupExtensionBase<ColorLayerConverter>, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Length == 6)
            {


                if (values[0] is IconMode mode && values[2] is Color color2 && values[3] is Color color3 && values[4] is Color color4 && values[5] is SolidColorBrush brush)
                {
                    if (values[1] is Color color1)
                    {

                    }
                    else
                    {
                        color1 = brush.Color;
                    }

                    switch (mode)
                    {
                        case IconMode.Outline:
                            return new IconColorLayer(color1);
                        case IconMode.Filled:
                            return new IconColorLayer(color1, true);
                        case IconMode.TwoTone:
                            return new IconColorLayer(color1, color2);
                        case IconMode.MultiColor:
                            return new IconColorLayer(color1, color2, color3, color4);
                    }
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
