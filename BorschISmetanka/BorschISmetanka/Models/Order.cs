using BorschISmetanka.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace BorschISmetanka.Models
{
    public class Order
    {
        public DateTime date { get; set; }
        public Address address { get; set; }
        public List<Dish> dishes = new List<Dish>();
        public string status { get; set; }
        public int sum { get; set; }
        //public Order(Address add, List<Dish> list)
        //{
        //    address = add;
        //    dishes = list;
        //}
    }
}
