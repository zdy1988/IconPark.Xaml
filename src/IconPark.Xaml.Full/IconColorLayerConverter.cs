using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace IconPark.Xaml.Full
{
    internal class IconColorLayerConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext typeDescriptorContext, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                string[] items = GetStringArray(sourceType.ToString());

                return items.Where(s => s.StartsWith("#") || s.StartsWith("$")).Any();
            }

            return false;
        }

        public override object ConvertFrom(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object source)
        {
            if (source is string @string)
            {
                return FromString(@string);
            }

            throw GetConvertFromException(source);
        }

        [SecurityCritical]
        public override object ConvertTo(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object value, Type destinationType)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (null == destinationType)
            {
                throw new ArgumentNullException("destinationType");
            }

            if (!(value is Thickness))
            {
                throw new ArgumentException("value");
            }

            IconColorLayer colorLayer = (IconColorLayer)value;

            if (destinationType == typeof(string))
            {
                return ToString(colorLayer);
            }

            if (destinationType == typeof(InstanceDescriptor))
            {
                ConstructorInfo constructor = typeof(IconColorLayer).GetConstructor(new Type[4]
                {
                    typeof(Color),
                    typeof(Color),
                    typeof(Color),
                    typeof(Color)
                });

                return new InstanceDescriptor(constructor, new object[4] { colorLayer.OuterStrokeColor, colorLayer.OuterFillColor, colorLayer.InnerStrokeColor, colorLayer.InnerFillColor });
            }

            throw new ArgumentException(destinationType.FullName);
        }

        internal static string ToString(IconColorLayer colors)
        {
            var stringBuilder = new StringBuilder(64);
            stringBuilder.Append(colors.OuterStrokeColor.ToString());
            stringBuilder.Append(Separator[0]);
            stringBuilder.Append(colors.OuterFillColor.ToString());
            stringBuilder.Append(Separator[0]);
            stringBuilder.Append(colors.InnerStrokeColor.ToString());
            stringBuilder.Append(Separator[0]);
            stringBuilder.Append(colors.InnerFillColor.ToString());
            return stringBuilder.ToString();
        }

        static readonly char[] Separator = new char[] { ' ', ',' };

        protected virtual string[] GetStringArray(string input)
        {
            if (!String.IsNullOrWhiteSpace(input))
            {
                string[] result = input.Split(Separator);
                Array.ForEach(result, s => s.Trim());
                return result;
            }
            else
                return new string[0];
        }

        protected IconColorLayer FromString(string s)
        {
            var items = GetStringArray(s);

            Color[] array = new Color[4];

            var isFilled = false;

            for (var i=0;i< items.Length;i++)
            {
                if (i >= 4)
                {
                    break;
                }

                var color = items[i];

                isFilled = color.StartsWith("$");

                if (isFilled)
                {
                    color = color.Replace("$", "#");
                }

                array[i] = (Color)ColorConverter.ConvertFromString(color);
            }

            return items.Length switch
            {
                1 => isFilled ? new IconColorLayer(array[0], isFilled) : new IconColorLayer(array[0]),
                2 => new IconColorLayer(array[0], array[1]),
                4 => new IconColorLayer(array[0], array[1], array[2], array[3]),
                _ => throw new FormatException(),
            };
        }
    }
}
