using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using Zhai.Famil.Common.ExtensionMethods;
using Zhai.Famil.Common.Mvvm;
using Zhai.Famil.Common.Mvvm.Command;

namespace IconPark.Xaml.IconsView
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public Dictionary<IconKind, string> IconKinds => EnumExtensions.CreateEnumSource<IconKind>();

        private Dictionary<IconKind, string> icons;
        public Dictionary<IconKind, string> Icons
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

        public MainWindowViewModel()
        {
            Icons = IconKinds;
        }

        public async void SearchIcon()
        {
            if (string.IsNullOrWhiteSpace(IconKeyword))
            {
                Icons = IconKinds;
            }
            else
            {
                Icons = await Task.Run(() => {

                    var list = IconKinds
                         .Where(x => x.Key.ToString().IndexOf(IconKeyword, StringComparison.CurrentCultureIgnoreCase) >= 0 || x.Value.IndexOf(IconKeyword, StringComparison.CurrentCultureIgnoreCase) >= 0);

                    return new Dictionary<IconKind, string>(list);
                });
            }
        }


        #region Commands

        public RelayCommand<KeyValuePair<IconKind, string>> ExecuteCopyXAMLCommand => new Lazy<RelayCommand<KeyValuePair<IconKind, string>>>(() => new RelayCommand<KeyValuePair<IconKind, string>>(item =>
        {
            Clipboard.SetText($"<IconPark:Icon x:Name=\"PART_Icon\" Kind=\"{item.Key}\"/>");

        })).Value;

        public RelayCommand<KeyValuePair<IconKind, string>> ExecuteCopySVGCommand => new Lazy<RelayCommand<KeyValuePair<IconKind, string>>>(() => new RelayCommand<KeyValuePair<IconKind, string>>(item =>
        {
            var data = Icon.GetData(item.Key);

            string svg = $"<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
            $"<svg width=\"20\" height=\"20\" viewBox=\"0 0 48 48\" fill=\"none\" xmlns=\"http://www.w3.org/2000/svg\">" +
            $"<path d=\"{data}\" fill=\"none\" stroke=\"#333\" stroke-width=\"2\" stroke-linejoin=\"round\"/>" +
            $"</svg>";

            Clipboard.SetText(svg);

        })).Value;

        public RelayCommand<KeyValuePair<IconKind, string>> ExecuteCopyReactCommand => new Lazy<RelayCommand<KeyValuePair<IconKind, string>>>(() => new RelayCommand<KeyValuePair<IconKind, string>>(item =>
        {
            Clipboard.SetText($"<{item.Key} theme=\"outline\" size=\"20\" fill=\"#333\" strokeWidth={2}/>");

        })).Value;

        public RelayCommand<KeyValuePair<IconKind, string>> ExecuteCopyVueCommand => new Lazy<RelayCommand<KeyValuePair<IconKind, string>>>(() => new RelayCommand<KeyValuePair<IconKind, string>>(item =>
        {
            string key = Regex.Replace(item.Key.ToString(), "[A-Z]", delegate (Match m)
            {
                return $"-{m.Value.ToLower()}";
            })[1..];

            Clipboard.SetText($"<{key} theme=\"outline\" size=\"20\" fill=\"#333\" :strokeWidth=\"2\"/>");

        })).Value;

        #endregion
    }
}
