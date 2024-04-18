﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using NavigationApp.Models;
using Newtonsoft.Json;

namespace MapEditor.Models
{
    public class BusinessElement:ObservableObject
    {
        [JsonIgnore]
        public ObservableCollection<Field> Fields
        {
            get => GetOrCreate(new ObservableCollection<Field>());
            set => SetAndNotify(value);
        }

        [JsonProperty("nodes")]
        public ObservableCollection<int> NodeField
        {
            get => GetOrCreate<ObservableCollection<int>>();
            set => SetAndNotify(value);
        }

        [JsonProperty("areas")]
        public ObservableCollection<int> AreasField
        {
            get => GetOrCreate(new ObservableCollection<int>());
            set => SetAndNotify(value);
        }
    }
}
