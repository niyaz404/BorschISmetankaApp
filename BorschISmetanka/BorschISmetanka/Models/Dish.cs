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
        [JsonProperty("id")]
        public int id {get;set;}
        [JsonProperty("D_Title")]
        public string name { get; set; }

        [JsonProperty("description")]
        public string description { get; set; }
        [JsonProperty("image")]
        public Uri image { get; set; }
        [JsonProperty("D_Price")]
        public int price { get; set; }        
        [JsonProperty("Category")]
        public string category { get; set; }//раздел в меню
        public int count { get; set; }//количество, применяется при добавлении в корзину
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
