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
using MapEditor.Views.Controls;
using WebApiNET;
using WebApiNET.Utilities;

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
            LoadResources();
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

        public Floor SelectedFloor
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

        public ObservableCollection<Area> Areas
        {
            get => GetOrCreate(new ObservableCollection<Area>());
            set => SetAndNotify(value);
        }

        public Area? CreatingArea
        {
            get => GetOrCreate<Area>();
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

        private void Areas_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Areas));
        }

        private void SelectedToolChanged(PropertyChangingArgs<ToolType> args)
        {
            foreach (var visualNode in Nodes)
            {
                visualNode.IsSelected = false;
            }

            foreach (var area in Areas)
            {
                area.IsSelected = false;
            }
            
            if(Route.Any()) Route.Clear();
            FloorRoute = new ObservableCollection<NaviPoint>();
        }


        private void SelectedFloorChanged(PropertyChangingArgs<Floor> args)
        {
            FilterMapElementsByFloor();
        }

        private async void LoadResources()
        {
            
            await GetAreas();
            await GetNodes();
            await GetFloors();
        }

        private async Task GetFloors()
        {
            Floors = await WebApi.GetData<ObservableCollection<Floor>>();
            foreach (var floor in Floors)
            {
                floor.DisposableImage = new DisposableImage(await WebApi.DownloadIFile(floor.Image, uriKind: UriKind.Absolute));
            }
            if (Floors?.Count > 0) SelectedFloor = Floors[0];
        }

        private async Task GetAreas()
        {
            Areas = await WebApi.GetData<ObservableCollection<Area>>();
        }

        private async Task GetNodes()
        {
            var nodes = await WebApi.GetData<List<Node>>();
            if (nodes is null) return;
            foreach (var node in nodes)
            {
                node.GetNeighbors(nodes);
                Nodes.Add(new VisualNode
                {
                    Height = 100,
                    Width = 100,
                    Node = node,
                    VisualCoordinates = new Point(node.Point.X + 50, node.Point.Y + 50)
                });
            }

            foreach (var visualNode in Nodes)
            {
                if (visualNode.Node.Neighbors.Count == 0) continue;
                foreach (var node in Nodes.Where(item => visualNode.Node.Neighbors.Contains(item.Node)))
                {
                    LinkNodes(visualNode, node);
                }
            }
        }

        private void FilterMapElementsByFloor()
        {
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
                area.IsVisible = area.Floor == SelectedFloor.Id;
            }

            FloorRoute = new ObservableCollection<NaviPoint>(Route.Where(point => point.Floor == SelectedFloor.Id));
        }

        private void LinkNodes(VisualNode firstNode, VisualNode secondNode)
        {
            Links.Add(new Link
            {
                From = firstNode,
                To = secondNode,
            });
        }

        private async Task CreateNode(Point position)
        {
            var newNode = new VisualNode
            {
                Height = 100,
                Width = 100,
                Node = new Node
                {
                    Point = new NaviPoint
                    {
                        Floor = SelectedFloor.Id,
                        X = position.X - 50,
                        Y = position.Y - 50
                    }
                },
                VisualCoordinates = position
            };
            Nodes.Add(newNode);
            var (response, result) = await WebApi.SendData<Node>(newNode.Node);
            newNode.Node.Id = result.Id;
        }

        private async Task CreateArea(Point position)
        {
            if (CreatingArea is null)
            {
                CreatingArea = new Area
                {
                    Floor = SelectedFloor.Id,
                    IsEditing = true,
                    IsVisible = true,
                };
                Areas.Add(CreatingArea);
            }
            CreatingArea.NaviPoints.Add(new NaviPoint
            {
                Floor = SelectedFloor.Id,
                X = position.X,
                Y = position.Y
            });
        }

        private async Task DeleteSelectedNodes()
        {
            var selectedNodes = Nodes.Where(node => node.IsSelected).ToList();
            foreach (var visualNode in selectedNodes)
            {
                Nodes.Remove(visualNode);
                Links = new ObservableCollection<Link>(Links.Where(link => link.From != visualNode && link.To != visualNode));
                await WebApi.DeleteData<Node>(visualNode.Node.Id.ToString());
            }
        }

        private async Task DeleteSelectedAreas()
        {
            var selectedAreas = Areas.Where(area => area.IsSelected).ToList();
            foreach (var selectedArea in selectedAreas)
            {
                Areas.Remove(selectedArea);
                await WebApi.DeleteData<Area>(selectedArea.Id.ToString());
            }
        }
        

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
                    await CreateNode(pos);
                    break;
                case ToolType.Area:
                    await CreateArea(pos);
                    break;
                default:
                    return;
            }
            
        });

        private ICommand? _deleteMapElementCommand;

        public ICommand DeleteMapElementCommand => _deleteMapElementCommand ??= new RelayCommand(async f =>
        {
            switch (SelectedTool)
            {
                case ToolType.Point:
                    await DeleteSelectedNodes();
                    break;
                case ToolType.Area:
                    await DeleteSelectedAreas();
                    if (f is NaviPoint np) CreatingArea.NaviPoints.Remove(np);
                    break;
                case ToolType.Cursor:
                    await DeleteSelectedNodes();
                    await DeleteSelectedAreas();
                    break;
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

            

            firstNode.Node.Neighbors.Add(secondNode.Node);
            secondNode.Node.Neighbors.Add(firstNode.Node);
            firstNode.Node.NeighborsKeys.Add(secondNode.Node.Id);
            secondNode.Node.NeighborsKeys.Add(firstNode.Node.Id);

            await WebApi.UpdateData<Node>(firstNode.Node, firstNode.Node.Id.ToString());
            await WebApi.UpdateData<Node>(secondNode.Node, secondNode.Node.Id.ToString());

            if (Links.Contains(Links.FirstOrDefault(link => link.From == firstNode && link.To == secondNode))) return;
            LinkNodes(firstNode, secondNode);
            firstNode.IsSelected = false;
        });

        private ICommand? _finishAreaCommand;

        public ICommand FinishAreaCommand => _finishAreaCommand ??= new RelayCommand(async f =>
        {
            CreatingArea.IsEditing = false;
            var (resp,result) = await WebApi.SendData<Area>(CreatingArea);
            CreatingArea.Id = result.Id;
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
                        vn.Node.Point.X = pos.X - 50;
                        vn.Node.Point.Y = pos.Y - 50;
                        vn.VisualCoordinates = pos;
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
                    await WebApi.UpdateData<Node>(vn.Node,vn.Node.Id.ToString());
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
                selectedNodes = selectedNodes.TakeLast(2);
                foreach (var node in Nodes)
                {
                    node.IsSelected = selectedNodes.Contains(node);
                }

            }
            var points = await WebApi.GetData<ObservableCollection<NaviPoint>>($"?from={selectedNodes.First().Node.Id}&to={selectedNodes.Last().Node.Id}");
            foreach (var point in points)
            {
                point.X += 50;
                point.Y += 50;
            }
            Route = points;
            FloorRoute = new ObservableCollection<NaviPoint>(points.Where(point => point.Floor == SelectedFloor.Id));

        });

        private ICommand? _onLoaded;
        public ICommand OnLoaded => _onLoaded ??= new RelayCommand(f =>
        {
            if(f is ItemsControl mainItemsControl) MainItemsControl = mainItemsControl;
        });
    }
}
