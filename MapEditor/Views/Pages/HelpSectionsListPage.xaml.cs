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
    /// Логика взаимодействия для HelpSectionsListPage.xaml
    /// </summary>
    public partial class HelpSectionsListPage : Page
    {
        public HelpSectionsListPage(List<BaseHelpSection> helpSectionsList)
        {
            HelpSectionsList = helpSectionsList;
            InitializeComponent();
        }

        public List<BaseHelpSection> HelpSectionsList { get; }

        private ICommand? _navigateCommand;
        public ICommand NavigateCommand => _navigateCommand ??= new RelayCommand(f =>
        {
            if(f is not BaseHelpSection  section) return;
            if(NavigationService is null) return;
            switch (section)
            {
                case VideoHelpSection videoHelpSection:
                    var window = this.FindAncestor<Window>();
                    if (window is not null) window.WindowState = WindowState.Maximized;
                    NavigationService.Navigate(new VideoHelpPage(videoHelpSection));
                    break;
                case HelpCompositeSection helpCompositeSection:
                    NavigationService.Navigate(new HelpSectionsListPage(helpCompositeSection.InnerHelpSections));
                    break;
                case HelpSection helpSection:
                    NavigationService.Navigate(new HelpSectionDetailsPage(helpSection));
                    break;
            }
        });
    }
}
