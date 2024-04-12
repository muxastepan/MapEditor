using System.Windows;
using MapEditor.ViewModels;

namespace MapEditor.Helpers
{
    public class MainWindowViewModelBindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            return new MainWindowViewModelBindingProxy();
        }

        public MainWindowViewModel Data
        {
            get { return (MainWindowViewModel)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(MainWindowViewModel), typeof(MainWindowViewModelBindingProxy), new UIPropertyMetadata(null));
    }
}
