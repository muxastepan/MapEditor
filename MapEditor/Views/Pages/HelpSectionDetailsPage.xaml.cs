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
using MapEditor.Models.Help;

namespace MapEditor.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для HelpSectionDetailsPage.xaml
    /// </summary>
    public partial class HelpSectionDetailsPage : Page
    {
        public HelpSectionDetailsPage(HelpSection helpSection)
        {
            HelpSection = helpSection;
            InitializeComponent();
        }
        public HelpSection HelpSection { get; }
    }
}
