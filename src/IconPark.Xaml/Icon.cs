using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Markup;

namespace IconPark.Xaml
{
    public class Icon : Control
    {
        static Icon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Icon), new FrameworkPropertyMetadata(typeof(Icon)));
        }

        private static readonly Lazy<IDictionary<IconKind, string>> _dataIndex = new Lazy<IDictionary<IconKind, string>>(IconDataFactory.Create);

        public static readonly DependencyProperty KindProperty = DependencyProperty.Register(nameof(Kind), typeof(IconKind), typeof(Icon), new PropertyMetadata(default(IconKind), OnKindPropertyChanged));
        public IconKind Kind
        {
            get => (IconKind)GetValue(KindProperty);
            set => SetValue(KindProperty, value);
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

        // ReSharper disable once StaticMemberInGenericType
        private static readonly DependencyPropertyKey DataPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Data), typeof(string), typeof(Icon), new PropertyMetadata(""));
        public static readonly DependencyProperty DataProperty = DataPropertyKey.DependencyProperty;
        /// <summary>
        /// Gets the icon path data for the current <see cref="Kind"/>.
        /// </summary>
        [TypeConverter(typeof(GeometryConverter))]
        public string Data
        {
            get => (string)GetValue(DataProperty);
            private set => SetValue(DataPropertyKey, value);
        }

        private static void OnKindPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            ((Icon)dependencyObject).UpdateData();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateData();
        }

        private void UpdateData()
        {
            string data = null;
            _dataIndex.Value?.TryGetValue(Kind, out data);
            Data = data;
        }

        public static string GetData(IconKind kind)
        {
            string data = null;
            _dataIndex.Value?.TryGetValue(kind, out data);
            return data;
        }
    }
}
