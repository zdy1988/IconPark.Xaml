using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Markup;

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

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var result = new Icon { Kind = Kind };

            if (Size.HasValue)
            {
                result.Height = Size.Value;
                result.Width = Size.Value;
            }

            return result;
        }
    }
}
