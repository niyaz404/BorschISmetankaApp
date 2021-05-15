using BorschISmetanka.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace BorschISmetanka.Models
{
    public class Order
    {
        public Address address { get; set; }
        public List<Dish> dishes = new List<Dish>();
        public Order(Address add, List<Dish> list)
        {
            address = add;
            dishes = list;
        }
    }
}
