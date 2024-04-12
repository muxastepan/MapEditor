using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Core;
using MapEditor.Models;
using MapEditor.Views.Windows;

namespace MapEditor.ViewModels
{
    public class SettingsWindowViewModel:ObservableObject
    {
        public Settings Settings
        {
            get=>GetOrCreate<Settings>(); 
            set=>SetAndNotify(value);
        }

        private ICommand? _closeCommand;
        public ICommand CloseCommand => _closeCommand ??= new RelayCommand(f =>
        {
            if(f is not SettingsWindow sw) return;
            sw.Close();
        });
    }
}
