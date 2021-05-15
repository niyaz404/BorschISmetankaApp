using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Text.Json;
using Newtonsoft.Json;
using System.Text;
using System.Collections.ObjectModel;

namespace BorschISmetanka.Views
{
    public class Dish
    {
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("description")]
        public string description { get; set; }
        [JsonProperty("image")]
        public Uri image { get; set; }
        [JsonProperty("price")]
        public int price { get; set; }
        [JsonProperty("count")]
        public int count { get; set; }
        public Dish()
        {
            
        }
        public Dish(Dish dish)
        {
            name = dish.name;
            description = dish.description;
            image = dish.image;
            price = dish.price;
            count = 0;
        }
    }    
}
