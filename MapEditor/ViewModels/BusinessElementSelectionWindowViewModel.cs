using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Core;
using MapEditor.Helpers;
using MapEditor.Models;
using MapEditor.Models.BusinessEntities;
using MapEditor.Models.MapElements;
using MapEditor.Models.MapElements.BindingMapElements;
using MapEditor.Views.Windows;
using WebApiNET;

namespace MapEditor.ViewModels
{
    /// <summary>
    /// Тип отвязываемого объекта карты.
    /// </summary>
    public enum ClearType
    {
        Node,
        Area
    }

    /// <summary>
    /// Класс бизнес-логики окна связывания объектов карты с областями картами.
    /// </summary>
    public class BusinessElementSelectionWindowViewModel : ObservableObject
    {
        public ObservableCollection<BusinessEntity> BusinessEntities
        {
            get => GetOrCreate<ObservableCollection<BusinessEntity>>();
            set => SetAndNotify(value);
        }

        public BusinessEntity SelectedBusinessEntity
        {
            get => GetOrCreate<BusinessEntity>();
            set
            {
                value.FilteredBusinessElements = value.BusinessElements;
                SetAndNotify(value);
            }
        }

        public NotificationService NotificationService
        {
            get => GetOrCreate<NotificationService>();
            set => SetAndNotify(value);
        }

        public IEnumerable<BindingMapElement> MapElements
        {
            get => GetOrCreate<IEnumerable<BindingMapElement>>();
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


        /// <summary>
        /// Связывает элемент карты с сущностью.
        /// </summary>
        private async void AddMapElementToBusinessElement(BusinessElement businessElement, MapElement mapElement)
        {
            var entity = BusinessEntities.FirstOrDefault(item => item.BusinessElements.Contains(businessElement));
            var id = businessElement.Fields.FirstOrDefault(item => item.IsPrimary);
            if (id is null)
            {
                NotificationService.AddNotification("У элемента отсутсвует поле первичного ключа, проверьте настройки сети", NotificationType.Warning);
                return;
            };
            if (entity is null) return;
            switch (mapElement)
            {
                case VisualNode vn:
                    UnbindMapElement(MapElements.Where(element => element is VisualNode && element.BindedBusinessElement == businessElement));
                    businessElement.NodeField = vn.Node.Id;
                    break;
                case VisualArea va:
                    UnbindMapElement(MapElements.Where(element => element is VisualArea && element.BindedBusinessElement == businessElement));
                    businessElement.AreaField = va.Area.Id;
                    break;
            }
            SelectedMapElement.BindedBusinessElement = businessElement;
            if (await WebApi.UpdateData<BusinessElement>(businessElement, id.Value, entity.Url))
                NotificationService.AddNotification("Связь добавлена",NotificationType.Success);
            else NotificationService.AddNotification("Связь не была добавлена из-за ошибки на сервере", NotificationType.Failure);
        }

        /// <summary>
        /// Отвязывает элемент карты от сущности.
        /// </summary>
        private async void ClearBusinessElement(BusinessElement businessElement, ClearType clearType)
        {
            var entity = BusinessEntities.FirstOrDefault(item => item.BusinessElements.Contains(businessElement));
            var id = businessElement.Fields.FirstOrDefault(item => item.IsPrimary);
            if (id is null)
            {
                NotificationService.AddNotification("У элемента отсутсвует поле первичного ключа, проверьте настройки сети", NotificationType.Warning);
                return;
            };
            if (entity is null) return;
            switch (clearType)
            {
                case ClearType.Node:
                    UnbindMapElement(MapElements.Where(element => element is VisualNode && element.BindedBusinessElement == businessElement));
                    businessElement.NodeField = null;
                    break;
                case ClearType.Area:
                    UnbindMapElement(MapElements.Where(element => element is VisualArea && element.BindedBusinessElement == businessElement));
                    businessElement.AreaField = null;
                    break;
                default:
                    return;
            }

            if (await WebApi.UpdateData<BusinessElement>(businessElement, id.Value, entity.Url))
                NotificationService.AddNotification("Связь удалена", NotificationType.Success);
            else NotificationService.AddNotification("Связь не была удалена из-за ошибки на сервере", NotificationType.Failure);
        }

        private void UnbindMapElement(IEnumerable<BindingMapElement> managingCollection)
        {
            foreach (var element in managingCollection)
            {
                element.IsLinked = false;
                element.BindedBusinessElement = null;
            }
        }

        private ICommand? _onLoaded;
        public ICommand OnLoaded => _onLoaded ??= new RelayCommand(f =>
        {
            if(BusinessEntities.Count==0) return;
            SelectedBusinessEntity = BusinessEntities.First();
        });

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
            if (f is not object[] args) return;
            if (args[1] is not BusinessElement be) return;
            if (args[0] is not ClearType clearType) return;
            ClearBusinessElement(be, clearType);


        });

        private ICommand? _linkCommand;
        public ICommand LinkCommand => _linkCommand ??= new RelayCommand(f =>
        {
            if (SelectedBusinessElement is null) return;
            AddMapElementToBusinessElement(SelectedBusinessElement, SelectedMapElement);
            SelectedMapElement.IsLinked = true;
        }, f => SelectedBusinessElement is not null);

        private ICommand? _searchCommand;
        /// <summary>
        /// Поиск по индексируемым полям сущностей.
        /// </summary>
        public ICommand SearchCommand => _searchCommand ??= new RelayCommand(f =>
        {
            if(f is not string text) return;
            if (string.IsNullOrEmpty(text))
            {
                SelectedBusinessEntity.FilteredBusinessElements = SelectedBusinessEntity.BusinessElements;
                return;
            }

            SelectedBusinessEntity.FilteredBusinessElements = new ObservableCollection<BusinessElement>();
            foreach (var businessElement in SelectedBusinessEntity.BusinessElements)
            {
                foreach (var field in businessElement.Fields)
                {
                    if (!field.IsIndex) continue;
                    if (!field.Value.Contains(text)) continue;
                    SelectedBusinessEntity.FilteredBusinessElements.Add(businessElement);
                }
            }

            //SelectedBusinessEntity.FilteredBusinessElements = new ObservableCollection<BusinessElement>(
            //    from element in SelectedBusinessEntity.BusinessElements
            //    let fields = element.Fields
            //    where (
            //        from field in fields 
            //        where field.IsIndex
            //        select field.Value).Contains(text)
            //    select element);

        });

    }
}
