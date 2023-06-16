using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Markup;
using System.Windows.Media;

namespace IconPark.Xaml
{
    [MarkupExtensionReturnType(typeof(Icon))]
    public class IconExtension : MarkupExtension
    {
        public IconExtension()
        { }

        public IconExtension(IconKind kind)
        {
            Kind = kind;
        }

        public IconExtension(IconKind kind, double size)
        {
            Kind = kind;
            Size = size;
        }

        [ConstructorArgument("kind")]
        public IconKind Kind { get; set; }

        [ConstructorArgument("size")]
        public double? Size { get; set; }

        [ConstructorArgument("strokeThickness")]
        public double? StrokeThickness { get; set; }

        [ConstructorArgument("strokeLineJoin")]
        public PenLineJoin? StrokeLineJoin { get; set; }

        [ConstructorArgument("strokeLineCap")]
        public PenLineCap? StrokeLineCap { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var result = new Icon { Kind = Kind };

            if (Size.HasValue)
            {
                result.Height = Size.Value;
                result.Width = Size.Value;
            }

            if (StrokeThickness.HasValue)
            {
                result.StrokeThickness = StrokeThickness.Value;
            }

            if (StrokeLineJoin.HasValue)
            {
                result.StrokeLineJoin = StrokeLineJoin.Value;
            }

            if (StrokeLineCap.HasValue)
            {
                result.StrokeLineCap = StrokeLineCap.Value;
            }

            return result;
        }
    }
}
