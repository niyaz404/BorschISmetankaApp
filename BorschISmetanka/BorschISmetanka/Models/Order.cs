using BorschISmetanka.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace BorschISmetanka.Models
{
    public class Order
    {
        Timer _timer { get; set; }//для обновления статуса заказа
        [JsonProperty("O_Start")]
        public DateTime date { get; set; }//время оформления заказа
        [JsonProperty("O_End")]
        public int leadTime { get; set; }//длительность выполнения заказа
        [JsonProperty("address")]
        public Address address { get; set; }//адрес
        [JsonProperty("O_Addr")]
        public string addressString { get; set; }//адрес в формате строки  
        [JsonProperty("dishes")]        
        public string dishesJson { get; set; }//блюда в заказе в формате Json(для хранения в бд)
        [JsonProperty("dishesList")]
        public List<Dish> dishes = new List<Dish>();//блюда в заказе
        public string status { get; set; }//статус заказа
        [JsonProperty("price")]
        public int sum { get; set; }//стоимость заказа
        [JsonProperty("C_Id")]
        public int с_id { get; set; }//id заказчика

        [JsonProperty("o_Id")]
        public int id { get; set; }//id заказа
        public void Status()
        {
            _timer = new Timer();
            _timer.AutoReset = true;
            _timer.Interval = 3000;
            _timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            _timer.Start();
        }

        private async void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                status = await App.getAsync(App.ip + "Orders/status/" + id,50);
            }
            catch(Exception ex)
            {

            }
            if (status == "Выполнен")
                _timer.Enabled = false;
        }
    }
}
