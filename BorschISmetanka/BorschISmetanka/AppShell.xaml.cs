using BorschISmetanka.Models;
using BorschISmetanka.Views;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;


namespace BorschISmetanka
{
    public partial class AppShell : Shell
    {
        
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("Profile", typeof(ProfilePage));
            Routing.RegisterRoute("Registration", typeof(Registration));
        }
    }
}
