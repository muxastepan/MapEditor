using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Core;
using MapEditor.Models;
using MapEditor.Models.MapElements;
using MapEditor.Models.MapElements.BindingMapElements;
using MapEditor.Views.Windows;
using NavigationApp.Models;
using WebApiNET;

namespace MapEditor.ViewModels
{

    public enum ClearType
    {
        Node,
        Area
    }
    public class BusinessElementSelectionWindowViewModel : ObservableObject
    {
        public ObservableCollection<BusinessEntity> BusinessEntities
        {
            get => GetOrCreate<ObservableCollection<BusinessEntity>>();
            set => SetAndNotify(value);
        }

        public BusinessElement? SelectedBusinessElement
        {
            get => GetOrCreate<BusinessElement>();
            set => SetAndNotify(value);
        }

        public BindingMapElement SelectedMapElement
        {
            get => GetOrCreate<BindingMapElement>();
            set => SetAndNotify(value);
        }

        private async void AddMapElementToBusinessElement(BusinessElement businessElement, MapElement mapElement)
        {
            var entity = BusinessEntities.FirstOrDefault(item => item.BusinessElements.Contains(businessElement));
            var id = businessElement.Fields.FirstOrDefault(item => item.IsPrimary);
            if (id is null) return;
            if (entity is null) return;
            switch (mapElement)
            {
                case VisualNode vn:
                    businessElement.NodeField=vn.Node.Id;
                    break;
                case VisualArea va:
                    businessElement.AreaField = va.Area.Id;
                    break;
            }
            await WebApi.UpdateData<BusinessElement>(businessElement, id.Value, entity.Url);
        }

        private async void ClearBusinessElement(BusinessElement businessElement,ClearType clearType)
        {
            var entity = BusinessEntities.FirstOrDefault(item => item.BusinessElements.Contains(businessElement));
            var id = businessElement.Fields.FirstOrDefault(item => item.IsPrimary);
            if (id is null) return;
            if (entity is null) return;

            switch (clearType)
            {
                case ClearType.Node:
                    businessElement.NodeField=0;
                    break;
                case ClearType.Area:
                    businessElement.AreaField = 0;
                    break;
                default:
                    return;
            }

            await WebApi.UpdateData<BusinessElement>(businessElement, id.Value, entity.Url);
        }

        private ICommand? _onClosing;
        public ICommand OnClosing => _onClosing ??= new RelayCommand(f =>
        {
            SelectedMapElement.IsSelected = false;
        });

        private ICommand? _cancelCommand;
        public ICommand CancelCommand => _cancelCommand ??= new RelayCommand(f =>
        {
            if (f is not BusinessElementSelectionWindow besw) return;
            besw.Close();
        });

        private ICommand? _clearBusinessElementCommand;

        public ICommand ClearBusinessElementCommand => _clearBusinessElementCommand ??= new RelayCommand(async f =>
        {
            if(f is not object[] args) return;
            if (args[1] is not BusinessElement be) return;
            if (args[0] is not ClearType clearType) return;
            ClearBusinessElement(be,clearType);
            

        });

        private ICommand? _linkCommand;
        public ICommand LinkCommand => _linkCommand ??= new RelayCommand(f =>
        {
            if (SelectedBusinessElement is null) return;
            AddMapElementToBusinessElement(SelectedBusinessElement, SelectedMapElement);
            SelectedMapElement.IsLinked = true;
        }, f => SelectedBusinessElement is not null);

    }
}
