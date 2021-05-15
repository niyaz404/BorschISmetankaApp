using System;
using System.Collections.Generic;
using System.Text;

namespace BorschISmetanka.Views
{
    public class Address
    {
        public string city { get; set; }
        public string street { get; set; }
        public string house { get; set; }
        public string flat { get; set; }
        public bool IsFavorite { get; set; }
        public Address(string c, string s, string h, string f, bool sub)
        {
            city = c;
            street = s;
            house = h;
            flat = f;
            IsFavorite = sub;
        }
        public string GetAddress()
        {
            return city + " " + street + " " + house + " " + flat;
        }
    }
}
