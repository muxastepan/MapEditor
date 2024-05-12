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
using System.Windows.Shapes;
using MapEditor.Models.Help;
using MapEditor.Models.Help.ContentBuilders;
using MapEditor.Views.Pages;

namespace MapEditor.Views.Windows
{

    /// <summary>
    /// Логика взаимодействия для HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        public HelpWindow()
        {
            InitializeComponent();
            Frame.NavigationService.Navigate(new HelpSectionsListPage(
                HelpContentBuilder.BuildContent()));
        }
    }
}
