﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Core;
using MapEditor.Models;
using MapEditor.Models.BusinessEntities;
using MapEditor.Models.Settings;
using MapEditor.Views.Windows;

namespace MapEditor.ViewModels
{
    /// <summary>
    /// Класс бизнес-логики окна настроек.
    /// </summary>
    public class SettingsWindowViewModel:ObservableObject
    {
        public Settings Settings
        {
            get=>GetOrCreate<Settings>(); 
            set=>SetAndNotify(value);
        }

        public BusinessEntity SelectedBusinessEntity
        {
            get=>GetOrCreate<BusinessEntity>();
            set=>SetAndNotify(value);
        }

        private ICommand? _closeCommand;
        /// <summary>
        /// Закрывает окно.
        /// </summary>
        public ICommand CloseCommand => _closeCommand ??= new RelayCommand(f =>
        {
            if(f is not SettingsWindow sw) return;
            sw.Close();
        });

        private ICommand? _addBusinessObjectCommand;
        /// <summary>
        /// Добавляет сущность с сервера.
        /// </summary>
        public ICommand AddBusinessObjectCommand => _addBusinessObjectCommand ??= new RelayCommand(f =>
        {
            Settings.NetworkSettings.BusinessEntities.Add(new BusinessEntity());
        });

        private ICommand? _deleteBusinessObjectCommand;
        /// <summary>
        /// Удаляет сущность.
        /// </summary>
        public ICommand DeleteBusinessObjectCommand => _deleteBusinessObjectCommand ??= new RelayCommand(f =>
        {
            Settings.NetworkSettings.BusinessEntities.Remove(SelectedBusinessEntity);
        });
    }
}
