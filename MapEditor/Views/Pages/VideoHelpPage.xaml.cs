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
using Core;
using MapEditor.Helpers.MyNamespace;
using MapEditor.Models.Help;

namespace MapEditor.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для VideoHelpPage.xaml
    /// </summary>
    public partial class VideoHelpPage : Page
    {
        public VideoHelpPage(VideoHelpSection videoHelpSection)
        {
            VideoHelpSection = videoHelpSection;
            InitializeComponent();
        }

        public VideoHelpSection VideoHelpSection { get; set; }

        private void MediaElement_OnMediaEnded(object sender, RoutedEventArgs e)
        {
            References.Visibility = Visibility.Visible;
        }

        private ICommand? _navigateCommand;
        public ICommand NavigateCommand => _navigateCommand ??= new RelayCommand(f =>
        {
            if (f is not VideoHelpSection section) return;
            if (NavigationService is null) return;
            NavigationService.Navigate(new VideoHelpPage(section));

        });

        private ICommand? _skipCommand;
        public ICommand SkipCommand => _skipCommand ??= new RelayCommand(f =>
        {
            MediaElement.Position = TimeSpan.MaxValue;

        }, f => MediaElement.Position != MediaElement.NaturalDuration);

        private ICommand? _exitCommand;
        public ICommand ExitCommand => _exitCommand ??= new RelayCommand(f =>
        {
            if (NavigationService is null) return;
            while (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }

            var window = this.FindAncestor<Window>();
            if (window is null) return;
            window.WindowState = WindowState.Normal;
        });
    }
}
