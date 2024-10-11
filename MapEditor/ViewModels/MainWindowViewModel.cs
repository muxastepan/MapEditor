using Core;
using MapEditor.Models;
using NavigationApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MapEditor.Helpers;
using MapEditor.Models.BusinessEntities;
using MapEditor.Models.MapElements;
using MapEditor.Models.MapElements.Factories;
using MapEditor.Models.Settings;
using MapEditor.Views.Controls;
using MapEditor.Views.Windows;
using WebApiNET;
using WebApiNET.Utilities;
using MapEditor.Models.MapElements.BindingMapElements;
using Newtonsoft.Json.Linq;

namespace MapEditor.ViewModels
{
    public enum ToolType
    {
        Cursor,
        Hand,
        Point,
        Area,
        Route
    }

    public class MainWindowViewModel : ObservableObject
    {
        public MainWindowViewModel()
        {
            Areas.CollectionChanged += Areas_CollectionChanged;
            Links.CollectionChanged += Links_CollectionChanged;
            Settings = _settingsManager.Read();
            NotificationService = new NotificationService(4,4);
            WebApi.Host = Settings.NetworkSettings.ApiUrl;
            WebApi.UseApiSuffix = Settings.NetworkSettings.UseApiSuffix;
            LoadResources();
        }

        private readonly SettingsManager _settingsManager = new("settings.json");

        #region Properties

        public NotificationService NotificationService
        {
            get => GetOrCreate<NotificationService>();
            set => SetAndNotify(value);
        }
        public Settings Settings
        {
            get=>GetOrCreate<Settings>(); 
            set=>SetAndNotify(value);
        }

        public ItemsControl MainItemsControl
        {
            get => GetOrCreate<ItemsControl>();
            set => SetAndNotify(value);
        }

        public ObservableCollection<Floor> Floors
        {
            get => GetOrCreate<ObservableCollection<Floor>>();
            set => SetAndNotify(value);
        }

        public ToolType SelectedTool
        {
            get => GetOrCreate(ToolType.Cursor);
            set => SetAndNotify(value,callback:SelectedToolChanged);
        }

        public Floor? SelectedFloor
        {
            get => GetOrCreate<Floor>();
            set => SetAndNotify(value, callback: SelectedFloorChanged);
        }

        public ObservableCollection<VisualNode> Nodes
        {
            get => GetOrCreate(new ObservableCollection<VisualNode>());
            set => SetAndNotify(value);
        }

        public ObservableCollection<Link> Links
        {
            get => GetOrCreate(new ObservableCollection<Link>());
            set => SetAndNotify(value);
        }

        public object? DraggingElement
        {
            get => GetOrCreate<object>();
            set => SetAndNotify(value);
        }

        public ObservableCollection<VisualArea> Areas
        {
            get => GetOrCreate(new ObservableCollection<VisualArea>());
            set => SetAndNotify(value);
        }

        public VisualArea? CreatingArea
        {
            get => GetOrCreate<VisualArea>();
            set => SetAndNotify(value);
        }

        public ObservableCollection<NaviPoint> Route
        {
            get => GetOrCreate(new ObservableCollection<NaviPoint>());
            set => SetAndNotify(value);
        }

        public ObservableCollection<NaviPoint> FloorRoute
        {
            get => GetOrCreate(new ObservableCollection<NaviPoint>());
            set => SetAndNotify(value);
        }

        #endregion

        #region PropertyChangedHandlers
        private void Areas_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Areas));
        }

        private void Links_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Links));
        }

        private void SelectedToolChanged(PropertyChangingArgs<ToolType> args)
        {
            
            UnselectAllMapElements();
            if(Route.Any()) Route.Clear();
            FloorRoute = new ObservableCollection<NaviPoint>();
        }


        private void SelectedFloorChanged(PropertyChangingArgs<Floor?> args)
        {
            FilterMapElementsByFloor();
        }
        #endregion

        #region Methods
        private async void LoadResources()
        {
            Settings = await _settingsManager.UpdateFromApi();
            await GetFloors();
            await GetBusinessElements();
            await GetAreas();
            await GetNodes();
            if (Floors?.Count > 0) SelectedFloor = Floors[0];
        }

        private void UnlinkEmptyMapElements()
        {
            foreach (var bindingMapElement in Nodes.Concat<BindingMapElement>(Areas))
            {
                if(bindingMapElement.BindedBusinessElement is null) continue;
                if (Settings.NetworkSettings.BusinessEntities.Any(be => be.BusinessElements.Contains(bindingMapElement.BindedBusinessElement))) continue;
                bindingMapElement.BindedBusinessElement = null;
                bindingMapElement.IsLinked = false;
            }
        }

        private void LinkMapElements()
        {
            foreach (var visualNode in Nodes)
            {
                var businessEntity = Settings.NetworkSettings.BusinessEntities.FirstOrDefault(be =>
                    be.BusinessElements.Any(element => element.NodeField == visualNode.Node.Id));
                var businessElement =
                    businessEntity?.BusinessElements.FirstOrDefault(element => element.NodeField == visualNode.Node.Id);

                if (businessElement == null) continue;

                visualNode.BindedBusinessElement = businessElement;
                visualNode.IsLinked = true;
            }

            foreach (var visualArea in Areas)
            {
                var businessEntity = Settings.NetworkSettings.BusinessEntities.FirstOrDefault(be =>
                    be.BusinessElements.Any(element => element.AreaField == visualArea.Area.Id));
                var businessElement =
                    businessEntity?.BusinessElements.FirstOrDefault(element => element.AreaField == visualArea.Area.Id);

                if (businessElement == null) continue;

                visualArea.BindedBusinessElement = businessElement;
                visualArea.IsLinked = true;
            }
        }

        private void ClearResources()
        {
            Floors.Clear();
            FloorRoute.Clear();
            Areas.Clear();
            Nodes.Clear();
            Links.Clear();
        }

        private void UnselectAllMapElements()
        {
            foreach (var visualNode in Nodes)
            {
                visualNode.IsSelected = false;
            }

            foreach (var area in Areas)
            {
                area.IsSelected = false;
            }

            foreach (var link in Links)
            {
                link.IsSelected = false;
            }
        }

        private async Task GetBusinessElements()
        {
            foreach (var businessEntity in Settings.NetworkSettings.BusinessEntities)
            {
                businessEntity.BusinessElements.Clear();
                var fields = businessEntity.FieldNames.Select(fieldName => fieldName.Key).ToList();
                var rawBusinessElements = await WebApi.GetDynamicDataArray(businessEntity.Url);
                if(rawBusinessElements is null) continue;
                foreach (var element in rawBusinessElements)
                {
                    var newBusinessElement = new BusinessElement();
                    
                    foreach (var field in fields)
                    {
                        var declaredKey = businessEntity.FieldNames.First(item => item.Key == field);
                        var subFields = field.Split('.');
                        var value= element;
                        foreach (var subField in subFields)
                        {
                            if (value[subField] !=null)
                                value = value[subField];
                        }
                        newBusinessElement.Fields.Add(new Field
                        {
                            IsPrimary = declaredKey.IsPrimary,
                            PrimaryGroupName = businessEntity.Url,
                            IsVisible = declaredKey.IsVisible,
                            Key = field,
                            Value = value.ToString(),
                            VerboseName = declaredKey.VerboseName,
                            IsIndex = declaredKey.IsIndex
                        });
                    }
                    newBusinessElement.ParentBusinessEntity = businessEntity;
                    if (element["node"] is null || element["area"] is null) continue;

                    newBusinessElement.NodeField = element["node"].ToObject<int?>();
                    newBusinessElement.AreaField = element["area"].ToObject<int?>();

                    businessEntity.BusinessElements.Add(newBusinessElement);
                }
            }
            LinkMapElements();
        }
        private async Task GetFloors()
        {
            Floors = await WebApi.GetData<ObservableCollection<Floor>>();
            foreach (var floor in Floors)
            {
                floor.DisposableImage = new DisposableImage(await WebApi.DownloadIFile(floor.Image, uriKind: UriKind.Relative));
            }
        }

        private async Task GetAreas()
        {
            var areas = await WebApi.GetData<ObservableCollection<Area>>();
            
            foreach (var area in areas)
            {
                var bindedBusinessEntity = Settings.NetworkSettings.BusinessEntities.FirstOrDefault(entity =>
                    entity.BusinessElements.Any(item => item.AreaField == area.Id));
                var bindedBusinessElement =
                    bindedBusinessEntity?.BusinessElements.FirstOrDefault(element => element.AreaField == area.Id);
                
                
                Areas.Add(new VisualArea
                {
                    Area = area,
                    IsLinked = bindedBusinessElement is not null,
                    BindedBusinessElement = bindedBusinessElement,
                    IsFinished = true,
                    PointWidth = Settings.VisualSettings.AreaPointWidth,
                    PointHeight = Settings.VisualSettings.AreaPointHeight,
                });
            }
        }

        private async Task GetNodes()
        {
            var nodes = await WebApi.GetData<List<Node>>();
            if (nodes is null) return;
            foreach (var node in nodes)
            {
                var bindedBusinessEntity = Settings.NetworkSettings.BusinessEntities.FirstOrDefault(entity =>
                    entity.BusinessElements.Any(item => item.NodeField == node.Id));
                var bindedBusinessElement =
                    bindedBusinessEntity?.BusinessElements.FirstOrDefault(element => element.NodeField == node.Id);
                node.GetNeighbors(nodes);
                var visualNode = new VisualNode
                {
                    Height = Settings.VisualSettings.NodePointHeight,
                    Width = Settings.VisualSettings.NodePointWidth,
                    Node = node,
                    VisualCoordinates = new Point(node.Point.X - Settings.VisualSettings.NodePointWidth / 2,
                        node.Point.Y - Settings.VisualSettings.NodePointHeight / 2),
                    IsLinked = bindedBusinessElement is not null,
                    BindedBusinessElement = bindedBusinessElement,
                };
                visualNode.Color = visualNode.RouteTypeColor;
                var linkedFloor = Floors.FirstOrDefault(floor=>floor.Id==node.Neighbors.FirstOrDefault(item=>item.Point.Floor!=node.Point.Floor)?.Point.Floor)?.Name;
                if (linkedFloor is not null)
                    visualNode.LinkedFloor = linkedFloor;
                Nodes.Add(visualNode);

            }

            foreach (var visualNode in Nodes)
            {
                if (visualNode.Node.Neighbors.Count == 0) continue;
                foreach (var node in Nodes.Where(item => visualNode.Node.Neighbors.Contains(item.Node)))
                {
                    Links.Add(new Link
                    {
                        From = visualNode,
                        To = node,
                    });
                }
            }
        }

        private void FilterMapElementsByFloor()
        {
            if(SelectedFloor is null) return;
            foreach (var visualNode in Nodes)
            {
                visualNode.IsVisible = visualNode.Node.Point.Floor == SelectedFloor.Id;
            }

            foreach (var link in Links)
            {
                link.IsVisible = link.From.Node.Point.Floor == SelectedFloor.Id && link.To.Node.Point.Floor == SelectedFloor.Id;
            }

            foreach (var area in Areas)
            {
                area.IsVisible = area.Area.Floor == SelectedFloor.Id;
            }

            FloorRoute = new ObservableCollection<NaviPoint>(Route.Where(point => point.Floor == SelectedFloor.Id));
        }


        private async Task<MapElement> ExecuteMapElementFactoryCreator<T>(ICollection<T> managingCollection,
            Point position, MapElementFactory factory) where T : MapElement, new()
        {
            var element = await factory.Create(position, Settings.VisualSettings, SelectedFloor);
            managingCollection.Add(element as T);
            return element;
        }

        private async Task CreateMapElement<T>(ICollection<T> managingCollection,Point position) where T : MapElement, new()
        {
            object obj = new T();
            switch (obj)
            {
                case VisualNode:
                    await ExecuteMapElementFactoryCreator(managingCollection, position, new VisualNodeFactory());
                    break;

                case VisualArea:
                    if (CreatingArea is null)
                    {
                        var va = await ExecuteMapElementFactoryCreator(managingCollection, position, new VisualAreaFactory());
                        CreatingArea = va as VisualArea;
                    }
                    else
                    {
                        CreatingArea.AddPoint(position,SelectedFloor);
                    }
                    break;
            };
        }

        private async Task DeleteSelectedMapElements<T>(ICollection<T> managingCollection) where T:MapElement, new()
        {
            var selectedElements = managingCollection.Where(element => element.IsSelected).ToList();
            bool? res = true;
            if (selectedElements.Count == 0) res = null;
            foreach (var selectedElement in selectedElements)
            {
                managingCollection.Remove(selectedElement);
                if (!await selectedElement.Delete()) res = false;
                if (selectedElement is VisualNode node)
                {
                    await DeleteEmptyLinks(node);
                }
            }

            switch (res)
            {
                case null:
                    return;
                case true:
                    NotificationService.AddNotification("Выбранные элементы удалены", NotificationType.Success);
                    break;
                default:
                    NotificationService.AddNotification("Выбранные элементы не удалились из-за ошибки на сервере", NotificationType.Failure);
                    break;
            }

        }

        private async Task DeleteEmptyLinks(VisualNode? node)
        {
            if(node is null) return;

            var emptyLinks = Links.Where(link => link.From==node || link.To==node).ToList();
            foreach (var emptyLink in emptyLinks)
            {
                Links.Remove(emptyLink);
                await emptyLink.Delete();
            }

        }

        private async Task<bool> DeleteMapElement(MapElement mapElement)
        {
            switch (mapElement)
            {
                case VisualNode vn:
                    Nodes.Remove(vn);
                    await DeleteEmptyLinks(vn);
                    break;
                case VisualArea va:
                    Areas.Remove(va);
                    break;
                case Link l:
                    Links.Remove(l);
                    break;
            }
           return await mapElement.Delete();
        }

        #endregion

        #region Commands

        private ICommand? _linkMapElementToBusinessElementCommand;

        public ICommand LinkMapElementToBusinessElementCommand =>
            _linkMapElementToBusinessElementCommand ??= new RelayCommand(async f =>
            {
                if(f is not BindingMapElement me) return;
                UnselectAllMapElements();
                me.IsSelected = true;
                
                var selectionWindow = new BusinessElementSelectionWindow
                {
                    DataContext = new BusinessElementSelectionWindowViewModel
                    {
                        BusinessEntities = Settings.NetworkSettings.BusinessEntities,
                        SelectedMapElement = me,
                        MapElements = Areas.Concat<BindingMapElement>(Nodes),
                        NotificationService = NotificationService
                    }
                };
                selectionWindow.Show();
            });

        private ICommand? _selectToolCommand;

        public ICommand SelectToolCommand => _selectToolCommand ??= new RelayCommand(async f =>
        {
            if (f is ToolType toolType) SelectedTool = toolType;
        });

        private ICommand? _createMapElementCommand;

        public ICommand CreateMapElementCommand => _createMapElementCommand ??= new RelayCommand(async f =>
        {
            if (f is not MouseEventArgs mouseArgs) return;
            var pos = mouseArgs.GetPosition(mouseArgs.Source as IInputElement);
            switch (SelectedTool)
            {
                case ToolType.Point:
                    await CreateMapElement(Nodes,pos);
                    if(Nodes.Last().Node.Id!=0)
                    {
                        NotificationService.AddNotification("Точка успешно создана", NotificationType.Success);
                    }
                    else
                    {
                        NotificationService.AddNotification("Точка не была создана из-за ошибки на сервере",
                            NotificationType.Failure);
                        Nodes.RemoveAt(Nodes.Count-1);
                    }
                    break;
                case ToolType.Area:
                    await CreateMapElement(Areas,pos);
                    break;
                case ToolType.Cursor:
                case ToolType.Hand:
                case ToolType.Route:
                default:
                    return;
            }
        });

        private ICommand? _editAreaCommand;

        public ICommand EditAreaCommand => _editAreaCommand ??= new RelayCommand(async f =>
        {
            if(f is not VisualArea va) return;
            if (CreatingArea is null)
            {
                CreatingArea = va;
            }
            else
            {
                await CreatingArea.StopEditing();
                CreatingArea = va;
            }
            CreatingArea.IsEditing = true;
        });

        private ICommand? _deleteMapElementCommand;

        public ICommand DeleteMapElementCommand => _deleteMapElementCommand ??= new RelayCommand(async f =>
        {
            if (f is MapElement me)
            {
                if (await DeleteMapElement(me)) NotificationService.AddNotification("Элемент удален", NotificationType.Success);
                else NotificationService.AddNotification("Элемент не удален из-за ошибки на сервере", NotificationType.Failure);
                return;
            }
            switch (SelectedTool)
            {
                case ToolType.Point:

                    await DeleteSelectedMapElements(Links);
                    await DeleteSelectedMapElements(Nodes);
                    break;
                case ToolType.Area:
                    await DeleteSelectedMapElements(Areas);
                    if (f is NaviPoint np) CreatingArea.Area.NaviPoints.Remove(np);
                    break;
                case ToolType.Cursor:

                    await DeleteSelectedMapElements(Links);
                    await DeleteSelectedMapElements(Nodes);
                    await DeleteSelectedMapElements(Areas);

                    break;
                case ToolType.Hand:
                case ToolType.Route:
                default:
                    return;
            }
        });


        private ICommand? _linkNodesCommand;

        public ICommand LinkNodesCommand => _linkNodesCommand ??= new RelayCommand(async f =>
        {
            if (!Keyboard.IsKeyDown(Key.LeftShift) && !Keyboard.IsKeyDown(Key.RightShift)) return;
            if(SelectedTool!=ToolType.Point) return;
            if(f is not VisualNode secondNode) return;
            var count = Nodes.Count(node => node.IsSelected);
            if (count < 2) return;

            var linkingNodes = Nodes.Where(node => node.IsSelected && node!=secondNode);
            var firstNode = linkingNodes.Last();
            if (count > 2)
            {
                foreach (var visualNode in linkingNodes)
                {
                    if (visualNode!= firstNode)
                    {
                        visualNode.IsSelected = false;
                    }
                }
            }
            if (await Link.LinkNodes(firstNode, secondNode)) NotificationService.AddNotification("Точки соединены", NotificationType.Success);
            else NotificationService.AddNotification("Точки не соединены из-за ошибки на сервере", NotificationType.Failure);

            if (Links.Contains(Links.FirstOrDefault(link => link.From == firstNode && link.To == secondNode))) return;
            Links.Add(new Link
            {
                From = firstNode,
                To = secondNode,
            });
            firstNode.IsSelected = false;

            if (firstNode.Node.Point.Floor == secondNode.Node.Point.Floor) return;

            firstNode.LinkedFloor = Floors.FirstOrDefault(floor => floor.Id == secondNode.Node.Point.Floor).Name;
            secondNode.LinkedFloor = Floors.FirstOrDefault(floor => floor.Id == firstNode.Node.Point.Floor).Name;
            
        });

        private ICommand? _finishAreaCommand;

        public ICommand FinishAreaCommand => _finishAreaCommand ??= new RelayCommand(async f =>
        {
            var res= await CreatingArea.StopEditing();
            if (res)
            {
                NotificationService.AddNotification("Область создана или изменена успешно", NotificationType.Success);
            }
            else
            {
                NotificationService.AddNotification("Область не создана или не изменена из-за ошибки на сервере",
                    NotificationType.Failure);
            }
            CreatingArea = null;
        });

        private ICommand? _dragCommand;

        public ICommand DragCommand => _dragCommand ??= new RelayCommand(async f =>
        {
            if(f is not MouseButtonEventArgs e) return;
            if (SelectedTool != ToolType.Hand)
            {
                e.Handled = false;
                return;
            }
            DraggingElement = (e.Source as FrameworkElement).DataContext;
            while (e.LeftButton == MouseButtonState.Pressed)
            {
                var pos = e.GetPosition(MainItemsControl);
                switch (DraggingElement)
                {
                    case VisualNode vn:
                        vn.Node.Point.X = pos.X;
                        vn.Node.Point.Y = pos.Y;
                        vn.VisualCoordinates = new Point(pos.X - Settings.VisualSettings.NodePointWidth / 2, pos.Y - Settings.VisualSettings.NodePointHeight / 2);
                        break;
                    case NaviPoint np:
                        np.X = pos.X;
                        np.Y = pos.Y;
                        break;
                }

                await Task.Delay(20);
            }

            switch (DraggingElement)
            {
                case VisualNode vn:
                    var result = await WebApi.UpdateData<Node>(vn.Node,vn.Node.Id.ToString());
                    if(result) NotificationService.AddNotification("Координаты точки изменены",NotificationType.Success);
                    else NotificationService.AddNotification("Координаты точки не изменились из-за ошибки на сервере",NotificationType.Failure);
                    
                    break;
            }
            
            e.Handled = true;
        });

        private ICommand? _makeRouteCommand;

        public ICommand MakeRouteCommand => _makeRouteCommand ??= new RelayCommand(async f =>
        {
            if(SelectedTool!=ToolType.Route) return;
            var count = Nodes.Count(node => node.IsSelected);
            if (count < 2) return;
            var selectedNodes = Nodes.Where(node => node.IsSelected); 
            if (count > 2)
            {
                NotificationService.AddNotification("Выбрано больше двух точек для построения маршрута", NotificationType.Warning);
                selectedNodes = selectedNodes.TakeLast(2);
                foreach (var node in Nodes)
                {
                    node.IsSelected = selectedNodes.Contains(node);
                }

            }
            Route = await WebApi.GetData<ObservableCollection<NaviPoint>>($"?from={selectedNodes.First().Node.Id}&to={selectedNodes.Last().Node.Id}");
            
            FloorRoute = new ObservableCollection<NaviPoint>(Route?.Where(point => point.Floor == SelectedFloor.Id));
            if (FloorRoute.Count == 0)
            {
                NotificationService.AddNotification("Маршрут не построен (возможно пути не существует)", NotificationType.Failure);
                return;
            }
            NotificationService.AddNotification("Маршрут успешно построен",NotificationType.Success);
        });

        private ICommand? _onLoaded;
        public ICommand OnLoaded => _onLoaded ??= new RelayCommand(f =>
        {
            if(f is ItemsControl mainItemsControl) MainItemsControl = mainItemsControl;
        });

        private ICommand? _openSettingsCommand;

        public ICommand OpenSettingsCommand => _openSettingsCommand ??= new RelayCommand(f =>
        {
            var settingsWindow = new SettingsWindow
                { DataContext = new SettingsWindowViewModel { Settings = Settings } };
            settingsWindow.Closed += SettingsWindow_Closed;
            settingsWindow.Show();
        });

        private async void SettingsWindow_Closed(object? sender, EventArgs e)
        {
            _settingsManager.Write(Settings);
            NotificationService.AddNotification("Настройки сохранены",NotificationType.Success);
            foreach (var visualNode in Nodes)
            {
                visualNode.Height = Settings.VisualSettings.NodePointHeight;
                visualNode.Width = Settings.VisualSettings.NodePointWidth;
                visualNode.VisualCoordinates = new Point(visualNode.Node.Point.X - Settings.VisualSettings.NodePointWidth /2, visualNode.Node.Point.Y-Settings.VisualSettings.NodePointHeight /2);
            }

            foreach (var visualArea in Areas)
            {
                visualArea.PointHeight = Settings.VisualSettings.AreaPointHeight;
                visualArea.PointWidth = Settings.VisualSettings.AreaPointWidth;
            }

            if (WebApi.Host != Settings.NetworkSettings.ApiUrl || WebApi.UseApiSuffix!= Settings.NetworkSettings.UseApiSuffix)
            {
                WebApi.Host = Settings.NetworkSettings.ApiUrl;
                WebApi.UseApiSuffix = Settings.NetworkSettings.UseApiSuffix;
                ClearResources();
                LoadResources();
            }
            UnlinkEmptyMapElements();
            await GetBusinessElements();

        }

        private ICommand? _openHelpWindowCommand;
        public ICommand OpenHelpWindowCommand => _openHelpWindowCommand ??= new RelayCommand(f =>
        {
            var window = new HelpWindow();
            window.Show();
        });

        #endregion
    }
}
