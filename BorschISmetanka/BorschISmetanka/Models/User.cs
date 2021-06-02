using BorschISmetanka.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BorschISmetanka.Models
{
    public class User
    {
        [JsonProperty("C_Name")]
        public string name { get; set; }
        [JsonProperty("Birthday")]
        public DateTime bday { get; set; }
        [JsonProperty("Email")]
        public string e_mail { get; set; }
        [JsonProperty("Number")]
        public string number { get; set; }
        [JsonProperty("c_Id")]
        public int id { get; set; }
        public List<Address> addresses = new List<Address>();
        public List<Order> orders = new List<Order>();
        public bool subscribe { get; set; }//подписка на рассылку
        [JsonProperty("Password")]        
        public string psw { get; set; }
        [JsonProperty("C_bonus")]
        public int bonusCnt { get; set; }
        public User()
        {

        }
    }
}
