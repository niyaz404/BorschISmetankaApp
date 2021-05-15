using BorschISmetanka.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace BorschISmetanka.Models
{
    public class Recipes_cat
    {
        [JsonProperty("category")]
        public string category { get; set; }
        [JsonProperty("recipes")]
        public ObservableCollection<Dish> dish { get; set; }
    }
}
