using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Zhai.Famil.Common.ExtensionMethods;
using Zhai.Famil.Controls;
using ScrollViewer = System.Windows.Controls.ScrollViewer;

namespace IconPark.Xaml.IconsView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : FamilWindow
    {
        MainWindowViewModel ViewModel => this.DataContext as MainWindowViewModel;

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new MainWindowViewModel();

            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var scrollViewer = this.IconsListBox.GetGrandChild<ScrollViewer>();

            if (scrollViewer != null)
            {
                scrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;
            }
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (!ViewModel.IsOptionsPaneShow)
            {
                if (sender is ScrollViewer scrollViewer && scrollViewer.ScrollableHeight > this.ActualHeight)
                {
                    if (e.VerticalOffset > 100 && Drawer.IsTopDrawerOpen)
                    {
                        ViewModel.IsBannerPaneShow = false;
                    }
                    else if (e.VerticalOffset < 10 && !Drawer.IsTopDrawerOpen)
                    {
                        ViewModel.IsBannerPaneShow = true;
                    }
                }
                else
                {
                    ViewModel.IsOptionsPaneShow = true;
                    ViewModel.IsBannerPaneShow = false;
                }
            }
        }

        private void SearchIconTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (Zhai.Famil.Controls.TextBox)sender;
            textBox.Dispatcher.BeginInvoke(new Action(textBox.SelectAll));
        }

        private void SearchIconTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                this.ViewModel.SearchIcon();
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new AboutWindow
            {
                Theme = this.Theme,
                Owner = App.Current.MainWindow
            };

            window.ShowDialog();
        }

        private void ArrowUpButton_Click(object sender, RoutedEventArgs e)
        {
            var scrollViewer = this.IconsListBox.GetGrandChild<ScrollViewer>();

            if (scrollViewer != null)
            {
                scrollViewer.ScrollToTop();
            }
        }
    }
}
