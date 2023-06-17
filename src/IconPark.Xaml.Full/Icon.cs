using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Markup;

namespace IconPark.Xaml.Full
{
    public class Icon : Control
    {
        static Icon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Icon), new FrameworkPropertyMetadata(typeof(Icon)));
        }

        public static readonly DependencyProperty KindProperty = DependencyProperty.Register(nameof(Kind), typeof(IconKind), typeof(Icon), new PropertyMetadata(default(IconKind), OnKindPropertyChanged));
        public IconKind Kind
        {
            get => (IconKind)GetValue(KindProperty);
            set => SetValue(KindProperty, value);
        }

        public static readonly DependencyProperty ColorLayerProperty = DependencyProperty.Register(nameof(ColorLayer), typeof(IconColorLayer), typeof(Icon), new PropertyMetadata(default(IconColorLayer)));
        public IconColorLayer ColorLayer
        {
            get => (IconColorLayer)GetValue(ColorLayerProperty);
            set => SetValue(ColorLayerProperty, value);
        }

        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(Icon), new PropertyMetadata(2.0));
        public double StrokeThickness
        {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }

        public static readonly DependencyProperty StrokeLineJoinProperty = DependencyProperty.Register(nameof(StrokeLineJoin), typeof(PenLineJoin), typeof(Icon), new PropertyMetadata(PenLineJoin.Round));
        public PenLineJoin StrokeLineJoin
        {
            get => (PenLineJoin)GetValue(StrokeLineJoinProperty);
            set => SetValue(StrokeLineJoinProperty, value);
        }

        public static readonly DependencyProperty StrokeLineCapProperty = DependencyProperty.Register(nameof(StrokeLineCap), typeof(PenLineCap), typeof(Icon), new PropertyMetadata(PenLineCap.Round));
        public PenLineCap StrokeLineCap
        {
            get => (PenLineCap)GetValue(StrokeLineCapProperty);
            set => SetValue(StrokeLineCapProperty, value);
        }


        private static void OnKindPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {

        }
    }
}
