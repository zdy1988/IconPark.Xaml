using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Ink;
using System.Windows.Media;
using Zhai.Famil.Common.ExtensionMethods;
using Zhai.Famil.Common.Mvvm;
using Zhai.Famil.Common.Mvvm.Command;

namespace IconPark.Xaml.IconsView
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

        private Color? strokeColor = null;
        public Color? StrokeColor
        {
            get => strokeColor;
            set => Set(() => StrokeColor, ref strokeColor, value);
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
            StrokeColor = null;
            StrokeLineJoin = PenLineJoin.Round;
            StrokeLineCap = PenLineCap.Round;

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

            Clipboard.SetText($"<IconPark:Icon Kind=\"{item.Key}\" Foreground=\"{opts.Color}\" StrokeThickness=\"{opts.StrokeWidth}\" StrokeLineJoin=\"{opts.StrokeLineJoin}\" StrokeLineCap=\"{opts.StrokeLineCap}\"/>");

        })).Value;

        public RelayCommand<KeyValuePair<IconKind, EnumSourceItem>> ExecuteCopyXAMLSourceCodeCommand => new Lazy<RelayCommand<KeyValuePair<IconKind, EnumSourceItem>>>(() => new RelayCommand<KeyValuePair<IconKind, EnumSourceItem>>(item =>
        {
            var data = Icon.GetData(item.Key);

            var opts = Options.Resolve(this, true);

            Clipboard.SetText(@$"<Viewbox FlowDirection=""LeftToRight"" Width=""{opts.Size}"" Height=""{opts.Size}"">
                                    <Path Data=""{data}""
                                          Stroke=""{opts.Color}"" 
                                          StrokeThickness=""{opts.StrokeWidth}""
                                          StrokeLineJoin=""{opts.StrokeLineJoin}""
                                          StrokeStartLineCap=""{opts.StrokeLineCap}""
                                          StrokeEndLineCap=""{opts.StrokeLineCap}""
                                          VerticalAlignment=""Center""
                                          HorizontalAlignment=""Center""
                                          Stretch=""Uniform""/>
                                </Viewbox>");

        })).Value;

        public RelayCommand<KeyValuePair<IconKind, EnumSourceItem>> ExecuteCopySVGCommand => new Lazy<RelayCommand<KeyValuePair<IconKind, EnumSourceItem>>>(() => new RelayCommand<KeyValuePair<IconKind, EnumSourceItem>>(item =>
        {
            var data = Icon.GetData(item.Key);

            var opts = Options.Resolve(this);

            string svg = $"<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
            $"<svg width=\"20\" height=\"20\" viewBox=\"0 0 48 48\" fill=\"none\" xmlns=\"http://www.w3.org/2000/svg\">" +
            $"<path d=\"{data}\" fill=\"none\" stroke=\"{opts.Color}\" stroke-width=\"{opts.StrokeWidth}\" stroke-linejoin=\"{opts.StrokeLineJoin}\" stroke-linecap=\"{opts.StrokeLineCap}\"/>" +
            $"</svg>";

            Clipboard.SetText(svg);

        })).Value;

        public RelayCommand<KeyValuePair<IconKind, EnumSourceItem>> ExecuteCopyReactCommand => new Lazy<RelayCommand<KeyValuePair<IconKind, EnumSourceItem>>>(() => new RelayCommand<KeyValuePair<IconKind, EnumSourceItem>>(item =>
        {
            var opts = Options.Resolve(this);

            Clipboard.SetText($"<{item.Key} theme=\"outline\" size=\"{opts.Size}\" fill=\"{opts.Color}\" strokeWidth=\"{opts.StrokeWidth}\" strokeLinecap=\"{opts.StrokeLineJoin}\" strokeLinejoin=\"{opts.StrokeLineCap}\"/> ");

        })).Value;

        public RelayCommand<KeyValuePair<IconKind, EnumSourceItem>> ExecuteCopyVueCommand => new Lazy<RelayCommand<KeyValuePair<IconKind, EnumSourceItem>>>(() => new RelayCommand<KeyValuePair<IconKind, EnumSourceItem>>(item =>
        {
            string key = Regex.Replace(item.Key.ToString(), "[A-Z]", delegate (Match m)
            {
                return $"-{m.Value.ToLower()}";
            })[1..];

            var opts = Options.Resolve(this);

            Clipboard.SetText($"<{key} theme=\"outline\" size=\"{opts.Size}\" fill=\"{opts.Color}\" :strokeWidth=\"{opts.StrokeWidth}\" :strokeLinecap=\"{opts.StrokeLineJoin}\" :strokeLinejoin=\"{opts.StrokeLineCap}\"/>");

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

        public double Size => ViewModel.IconSize;
        public string Color => string.Concat("#", (ViewModel.StrokeColor == null ? App.Current.MainWindow.Foreground.ToColor() : ViewModel.StrokeColor).ToString().AsSpan(3));
        public double StrokeWidth => ViewModel.StrokeThickness;
        public string StrokeLineJoin => IsXaml ? ViewModel.StrokeLineJoin.ToString() : ViewModel.StrokeLineJoin.ToString().ToLower();
        public string StrokeLineCap => IsXaml ? ViewModel.StrokeLineCap.ToString() : (ViewModel.StrokeLineCap == PenLineCap.Flat ? "butt" : ViewModel.StrokeLineCap.ToString().ToLower());

        public static Options Resolve(MainWindowViewModel viewModel) => new Options(viewModel);

        public static Options Resolve(MainWindowViewModel viewModel, bool isXaml) => new Options(viewModel, isXaml);
    }
}
