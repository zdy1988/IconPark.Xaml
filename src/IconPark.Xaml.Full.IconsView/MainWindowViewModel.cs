using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Zhai.Famil.Common.ExtensionMethods;
using Zhai.Famil.Common.Mvvm;
using Zhai.Famil.Common.Mvvm.Command;

namespace IconPark.Xaml.Full.IconsView
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public Dictionary<IconKind, EnumSourceItem> IconKinds => EnumExtensions.CreateEnumSources<IconKind>();

        private Dictionary<IconKind, EnumSourceItem> icons;
        public Dictionary<IconKind, EnumSourceItem> Icons
        {
            get => icons;
            set => Set(() => Icons, ref icons, value);
        }

        private string iconKeyword;
        public string IconKeyword
        {
            get => iconKeyword;
            set => Set(() => IconKeyword, ref iconKeyword, value);
        }

        public ICollectionView IconCollectionView { get; }

        public MainWindowViewModel()
        {
            Icons = IconKinds;

            IconCollectionView = CollectionViewSource.GetDefaultView(Icons);
            IconCollectionView.GroupDescriptions.Add(new PropertyGroupDescription("Value.Category"));
        }


        public void SearchIcon()
        {
            IconCollectionView.Filter = o =>
            {
                if (o is KeyValuePair<IconKind, EnumSourceItem> x)
                {
                    return x.Key.ToString().Contains(IconKeyword, StringComparison.CurrentCultureIgnoreCase) || x.Value.Description.Contains(IconKeyword, StringComparison.CurrentCultureIgnoreCase);
                }

                return false;
            };

            IconCollectionView.Refresh();
        }

        #region Options

        private bool isBannerPaneShow = true;
        public bool IsBannerPaneShow
        {
            get => isBannerPaneShow;
            set => Set(() => IsBannerPaneShow, ref isBannerPaneShow, value);
        }

        private bool isOptionsPaneShow = false;
        public bool IsOptionsPaneShow
        {
            get => isOptionsPaneShow;
            set
            {
                if (Set(() => IsOptionsPaneShow, ref isOptionsPaneShow, value) && value)
                {
                    IsBannerPaneShow = false;
                }
            }
        }

        private double iconSize = 18.0;
        public double IconSize
        {
            get => iconSize;
            set => Set(() => IconSize, ref iconSize, value);
        }

        private double strokeThickness = 2.0;
        public double StrokeThickness
        {
            get => strokeThickness;
            set => Set(() => StrokeThickness, ref strokeThickness, value);
        }

        private IconMode iconMode = IconMode.Outline;
        public IconMode IconMode
        {
            get => iconMode;
            set => Set(() => IconMode, ref iconMode, value);
        }

        private Color? color1;
        public Color? Color1
        {
            get => color1;
            set => Set(() => Color1, ref color1, value);
        }

        private Color color2 = (Color)ColorConverter.ConvertFromString("#2F88FF");
        public Color Color2
        {
            get => color2;
            set => Set(() => Color2, ref color2, value);
        }

        private Color color3 = (Color)ColorConverter.ConvertFromString("#FFF");
        public Color Color3
        {
            get => color3;
            set => Set(() => Color3, ref color3, value);
        }

        private Color color4 = (Color)ColorConverter.ConvertFromString("#43CCF8");
        public Color Color4
        {
            get => color4;
            set => Set(() => Color4, ref color4, value);
        }


        private PenLineJoin strokeLineJoin= PenLineJoin.Round;
        public PenLineJoin StrokeLineJoin
        {
            get => strokeLineJoin;
            set
            {
                Set(() => StrokeLineJoin, ref strokeLineJoin, value);
            }
        }

        private PenLineCap strokeLineCap = PenLineCap.Round;
        public PenLineCap StrokeLineCap
        {
            get => strokeLineCap;
            set => Set(() => StrokeLineCap, ref strokeLineCap, value);
        }

        #endregion

        #region Commands

        public RelayCommand ExecuteResetOptionsCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            IconSize = 18.0;
            StrokeThickness = 2.0;
            StrokeLineJoin = PenLineJoin.Round;
            StrokeLineCap = PenLineCap.Round;

            IconMode = IconMode.Outline;
            Color1 = null;
            Color2 = (Color)ColorConverter.ConvertFromString("#2F88FF");
            Color3 = (Color)ColorConverter.ConvertFromString("#FFF");
            Color4 = (Color)ColorConverter.ConvertFromString("#43CCF8");

        })).Value;

        public RelayCommand<KeyValuePair<IconKind, EnumSourceItem>> ExecuteCopyKindCommand => new Lazy<RelayCommand<KeyValuePair<IconKind, EnumSourceItem>>>(() => new RelayCommand<KeyValuePair<IconKind, EnumSourceItem>>(item =>
        {
            Clipboard.SetText(item.Key.ToString());

        })).Value;

        public RelayCommand<KeyValuePair<IconKind, EnumSourceItem>> ExecuteCopyXAMLCommand => new Lazy<RelayCommand<KeyValuePair<IconKind, EnumSourceItem>>>(() => new RelayCommand<KeyValuePair<IconKind, EnumSourceItem>>(item =>
        {
            Clipboard.SetText($"<IconPark:Icon Kind=\"{item.Key}\"/>");

        })).Value;

        public RelayCommand<KeyValuePair<IconKind, EnumSourceItem>> ExecuteCopyXAMLWithOptionsCommand => new Lazy<RelayCommand<KeyValuePair<IconKind, EnumSourceItem>>>(() => new RelayCommand<KeyValuePair<IconKind, EnumSourceItem>>(item =>
        {
            var opts = Options.Resolve(this, true);

            var colorLayer = IconMode == IconMode.Filled ? opts.ColorLayer.Replace("#", "$") : opts.ColorLayer;

            Clipboard.SetText($"<IconPark:Icon Kind=\"{item.Key}\" ColorLayer=\"{colorLayer}\" StrokeThickness=\"{opts.StrokeWidth}\" StrokeLineJoin=\"{opts.StrokeLineJoin}\" StrokeLineCap=\"{opts.StrokeLineCap}\"/>");

        })).Value;

        public RelayCommand<KeyValuePair<IconKind, EnumSourceItem>> ExecuteCopyReactCommand => new Lazy<RelayCommand<KeyValuePair<IconKind, EnumSourceItem>>>(() => new RelayCommand<KeyValuePair<IconKind, EnumSourceItem>>(item =>
        {
            var opts = Options.Resolve(this);

            var colorLayer = IconMode == IconMode.TwoTone || IconMode == IconMode.MultiColor ? $"{{[{opts.ColorLayer}]}}" : opts.ColorLayer;

            Clipboard.SetText($"<{item.Key} theme=\"{opts.Mode}\" size=\"{opts.Size}\" fill=\"{colorLayer}\" strokeWidth=\"{opts.StrokeWidth}\" strokeLinecap=\"{opts.StrokeLineJoin}\" strokeLinejoin=\"{opts.StrokeLineCap}\"/> ");

        })).Value;

        public RelayCommand<KeyValuePair<IconKind, EnumSourceItem>> ExecuteCopyVueCommand => new Lazy<RelayCommand<KeyValuePair<IconKind, EnumSourceItem>>>(() => new RelayCommand<KeyValuePair<IconKind, EnumSourceItem>>(item =>
        {
            string key = Regex.Replace(item.Key.ToString(), "[A-Z]", delegate (Match m)
            {
                return $"-{m.Value.ToLower()}";
            })[1..];

            var opts = Options.Resolve(this);

            var colorLayer = IconMode == IconMode.TwoTone || IconMode == IconMode.MultiColor ? $"[{opts.ColorLayer}]" : opts.ColorLayer;

            Clipboard.SetText($"<{key} theme=\"{opts.Mode}\" size=\"{opts.Size}\" fill=\"{colorLayer}\" :strokeWidth=\"{opts.StrokeWidth}\" :strokeLinecap=\"{opts.StrokeLineJoin}\" :strokeLinejoin=\"{opts.StrokeLineCap}\"/>");

        })).Value;

        #endregion
    }

    internal class Options
    {
        MainWindowViewModel ViewModel;
        bool IsXaml = false;

        public Options(MainWindowViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public Options(MainWindowViewModel viewModel, bool isXaml)
            : this(viewModel)
        {
            IsXaml = isXaml;
        }

        string GetColorLayer()
        {
            Color color1 = ViewModel.Color1 == null ? App.Current.MainWindow.Foreground.ToColor() : ViewModel.Color1.Value;

            return ViewModel.IconMode switch
            {
                IconMode.Outline => GetColorString(color1),
                IconMode.Filled => GetColorString(color1),
                IconMode.TwoTone => GetColorString(color1, ViewModel.Color2),
                IconMode.MultiColor => GetColorString(color1, ViewModel.Color2, ViewModel.Color3, ViewModel.Color4),
                _ => "",
            };
        }

        string GetColorString(params Color[] colors)
        {
            return string.Join(" ", colors.Select(color => string.Concat("#", color.ToString().AsSpan(3))));
        }

        string GetMode()
        {
            return Regex.Replace(ViewModel.IconMode.ToString(), "[A-Z]", delegate (Match m)
            {
                return $"-{m.Value.ToLower()}";
            })[1..];
        }

        public double Size => ViewModel.IconSize;
        public string ColorLayer => GetColorLayer();
        public double StrokeWidth => ViewModel.StrokeThickness;
        public string StrokeLineJoin => IsXaml ? ViewModel.StrokeLineJoin.ToString() : ViewModel.StrokeLineJoin.ToString().ToLower();
        public string StrokeLineCap => IsXaml ? ViewModel.StrokeLineCap.ToString() : (ViewModel.StrokeLineCap == PenLineCap.Flat ? "butt" : ViewModel.StrokeLineCap.ToString().ToLower());
        public string Mode => GetMode();

        public static Options Resolve(MainWindowViewModel viewModel) => new Options(viewModel);

        public static Options Resolve(MainWindowViewModel viewModel, bool isXaml) => new Options(viewModel, isXaml);
    }
}
