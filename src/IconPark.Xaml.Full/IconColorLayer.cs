using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace IconPark.Xaml.Full
{
    [TypeConverter(typeof(IconColorLayerConverter))]
    [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
    public struct IconColorLayer : IEquatable<IconColorLayer>
    {
        private Color _OuterStrokeColor;
        public Color OuterStrokeColor { get => _OuterStrokeColor; set => _OuterStrokeColor = value; }

        private Color _OuterFillColor;
        public Color OuterFillColor { get => _OuterFillColor; set => _OuterFillColor = value; }

        private Color _InnerStrokeColor;
        public Color InnerStrokeColor { get => _InnerStrokeColor; set => _InnerStrokeColor = value; }

        private Color _InnerFillColor;
        public Color InnerFillColor { get => _InnerFillColor; set => _InnerFillColor = value; }


        public Brush OuterStrokeColorBrush => new SolidColorBrush(_OuterStrokeColor);
        public Brush OuterFillColorBrush => new SolidColorBrush(_OuterFillColor);
        public Brush InnerStrokeColorBrush => new SolidColorBrush(_InnerStrokeColor);
        public Brush InnerFillColorBrush => new SolidColorBrush(_InnerFillColor);

        public IconColorLayer(Color color)
        {
            _OuterStrokeColor = color;
            _InnerStrokeColor = color;
        }

        public IconColorLayer(Color color, bool _ = true)
        {
            _OuterStrokeColor = color;
            _OuterFillColor = color;
            _InnerStrokeColor = (Color)ColorConverter.ConvertFromString("#FFF");
            _InnerFillColor = (Color)ColorConverter.ConvertFromString("#FFF");
        }

        public IconColorLayer(Color color1, Color color2)
        {
            _OuterStrokeColor = color1;
            _OuterFillColor = color2;
            _InnerStrokeColor = color1;
            _InnerFillColor = color2;
        }

        public IconColorLayer(Color color1, Color color2, Color color3, Color color4)
        {
            _OuterStrokeColor = color1;
            _OuterFillColor = color2;
            _InnerStrokeColor = color3;
            _InnerFillColor = color4;
        }


        public override bool Equals(object obj)
        {
            if (obj is IconColorLayer)
            {
                IconColorLayer colorLayer = (IconColorLayer)obj;
                return this == colorLayer;
            }

            return false;
        }

        public bool Equals(IconColorLayer other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return _OuterStrokeColor.GetHashCode() ^ _OuterFillColor.GetHashCode() ^ _InnerStrokeColor.GetHashCode() ^ _InnerFillColor.GetHashCode();
        }

        public static bool operator ==(IconColorLayer t1, IconColorLayer t2)
        {
            if ((t1._OuterStrokeColor == t2._OuterStrokeColor || (t1._OuterStrokeColor == null && t2._OuterStrokeColor == null)) && (t1._OuterFillColor == t2._OuterFillColor || (t1._OuterFillColor == null && t2._OuterFillColor == null)) && (t1._InnerStrokeColor == t2._InnerStrokeColor || (t1._InnerStrokeColor == null && t2._InnerStrokeColor == null)))
            {
                if (t1._InnerFillColor != t2._InnerFillColor)
                {
                    if (t1._InnerFillColor == null)
                    {
                        return t2._InnerFillColor == null;
                    }

                    return false;
                }

                return true;
            }

            return false;
        }

        public static bool operator !=(IconColorLayer color1, IconColorLayer color2)
        {
            return !(color1 == color2);
        }
    }
}
