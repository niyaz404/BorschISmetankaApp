using BorschISmetanka.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace BorschISmetanka.Models
{
    public class User
    {
        public string name { get; set; }
        public string bday { get; set; }
        public string e_mail { get; set; }
        public string number { get; set; }
        int id { get; }
        public List<Address> addresses = new List<Address>();
        public bool subscribe { get; set; }//подписка на новости
        public User()
        {

        }
        User(int id)
        {
            this.id = id;
        }
    }
}
