using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Threading;
using BorschISmetanka.Models;

namespace BorschISmetanka.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderPage : ContentPage
    {
        static StackLayout dishesStack;
        static Label orderAdd;
        public OrderPage()
        {
            InitializeComponent();
            dishesStack = new StackLayout { Orientation = StackOrientation.Horizontal };
            orderAdd = new Label();
            Grid.SetRow(orderAdd, 2);
            grid.Children.Add(orderAdd);
            photoScrollView.Content = dishesStack;
        }
        CancellationTokenSource cts;
        
        async Task GetCurrentLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }

        protected override void OnDisappearing()
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
            base.OnDisappearing();
        }
        public static void CurrentOrder(Order order)
        {
            
            foreach (Dish d in order.dishes)
                dishesStack.Children.Add(new Image { Source = d.image });
            orderAdd.Text = order.address.GetAddress();
        }
    }
}