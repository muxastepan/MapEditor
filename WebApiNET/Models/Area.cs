using Core;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace NavigationApp.Models
{
    public class Area:ObservableObject
    {
        public Area()
        {
            NaviPoints.CollectionChanged += NaviPoints_CollectionChanged;
        }

        private void NaviPoints_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (INotifyPropertyChanged item in e.OldItems)
                    item.PropertyChanged -= item_PropertyChanged;
            }
            if (e.NewItems != null)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                    item.PropertyChanged += item_PropertyChanged;
            }
            OnPropertyChanged(nameof(NaviPoints));
        }

        private void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(NaviPoints));
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("points")]
        public ObservableCollection<NaviPoint> NaviPoints
        {
            get => GetOrCreate(new ObservableCollection<NaviPoint>()); 
            set => SetAndNotify(value);
        }

        [JsonProperty("floor")]
        public int Floor { get; set; }
    }
}
