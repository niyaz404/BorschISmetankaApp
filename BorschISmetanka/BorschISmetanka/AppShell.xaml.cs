using BorschISmetanka.Models;
using BorschISmetanka.ViewModels;
using BorschISmetanka.Views;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;


namespace BorschISmetanka
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        
        public AppShell()
        {
            InitializeComponent();            
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }
        

    }
}
