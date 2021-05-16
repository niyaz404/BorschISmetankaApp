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
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.IO;

namespace BorschISmetanka.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderPage : ContentPage
    {
        static StackLayout dishesStack;
        static Label orderAdd;
        public ObservableCollection<Order> Orders { get; set; } = new ObservableCollection<Order>();
        bool tappedOnMap = false;

        //CarouselView carouselV = new CarouselView();

        static public bool needToRefresh;
        public OrderPage()
        {
            InitializeComponent();
            dishesStack = new StackLayout { Orientation = StackOrientation.Horizontal };
            orderAdd = new Label();
            Grid.SetRow(orderAdd, 2);
            grid.Children.Add(orderAdd);
           // photoScrollView.Content = dishesStack;
            needToRefresh = true;
            //carouselV.SetBinding(ItemsView.ItemsSourceProperty, "Orders");
            
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (needToRefresh)
                RefreshOrderList();
        }
        Frame orderFrame(Order order)
        {
            Label dateLabel = new Label { Text = order.date.ToString("dd.MM.yyyy HH:mm"), TextColor = Color.Black, FontSize = 20, FontAttributes = FontAttributes.Bold, HorizontalOptions = LayoutOptions.Start };
            BoxView line1 = new BoxView { BackgroundColor = Color.Silver};
            Label label1 = new Label { Text = "Доставка", FontSize = 15, HorizontalOptions = LayoutOptions.Start};
            Label addressLabel = new Label { Text = order.address.GetAddress(), TextColor = Color.Black, FontSize = 18, HorizontalOptions = LayoutOptions.Start, VerticalOptions=LayoutOptions.Center};
            BoxView line2 = new BoxView { BackgroundColor = Color.Silver };
            StackLayout pictureStack = new StackLayout { Orientation=StackOrientation.Horizontal };
            foreach(Dish d in order.dishes)
            {
                pictureStack.Children.Add(new Image { Source = d.image });
            }
            ScrollView pictureScrollView = new ScrollView { Content = pictureStack , Orientation=ScrollOrientation.Horizontal};
            BoxView line3 = new BoxView { BackgroundColor = Color.Silver };
            Label label2 = new Label { Text = "Сумма", HorizontalOptions = LayoutOptions.Start, VerticalOptions=LayoutOptions.Center, TextColor = Color.Black, FontSize = 18 };
            Label sumLabel = new Label { Text = order.sum.ToString()+" руб", HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, TextColor = Color.Black, FontSize = 18 };
            Grid.SetColumn(label2, 0);
            Grid.SetColumn(sumLabel, 1);
            Grid sumGrid = new Grid { Children = { label2, sumLabel } };
            BoxView line4 = new BoxView { BackgroundColor = Color.Silver };
            Label label3 = new Label { Text = "Статус заказа", HorizontalOptions = LayoutOptions.Start, TextColor = Color.Black, FontSize = 18 };
            Label statusLabel = new Label { Text = order.status, HorizontalOptions = LayoutOptions.End, TextColor = Color.Black, FontSize = 18 };
            Label label4 = new Label { Text = "Доставят до", HorizontalOptions = LayoutOptions.Start, TextColor = Color.Black, FontSize = 18 };
            Label timeLabel = new Label { Text = order.date.ToString("HH:mm"), HorizontalOptions = LayoutOptions.End, TextColor = Color.Black, FontSize = 18 };//нужно изменить время
            Grid.SetColumn(label3, 0);
            Grid.SetColumn(statusLabel, 1);
            Grid.SetColumn(label4, 0);
            Grid.SetColumn(timeLabel, 1);
            Grid.SetRow(label4, 1);
            Grid.SetRow(timeLabel, 1);
            Grid statusGrid = new Grid { Children = { label3, statusLabel, label4, timeLabel} };
            Grid.SetRow(dateLabel,0);
            Grid.SetRow(line1, 1);
            Grid.SetRow(label1, 2);
            Grid.SetRow(addressLabel, 3);
            Grid.SetRow(line2, 4);
            Grid.SetRow(pictureScrollView, 5);
            Grid.SetRow(line3, 6);
            Grid.SetRow(sumGrid, 7);
            Grid.SetRow(line4, 8);
            Grid.SetRow(statusGrid, 9);
            RowDefinition rd1 = new RowDefinition { Height = 30 };
            RowDefinition rd2 = new RowDefinition { Height = 2 };
            RowDefinition rd3 = new RowDefinition { Height = 25 };
            RowDefinition rd4 = new RowDefinition { Height = 45 };
            RowDefinition rd5 = new RowDefinition { Height = 2 };
            RowDefinition rd6 = new RowDefinition { Height = 70 };
            RowDefinition rd7 = new RowDefinition { Height = 2 };
            RowDefinition rd8 = new RowDefinition { Height = 40 };
            RowDefinition rd9 = new RowDefinition { Height = 2 };
            RowDefinition rd10 = new RowDefinition { Height = 80 };
            Grid frameGrid = new Grid { Children = { dateLabel, line1, label1, addressLabel, line2, pictureScrollView, line3, sumGrid, line4, statusGrid }, RowDefinitions = {rd1,rd2,rd3,rd4,rd5,rd6,rd7,rd8,rd9,rd10} };
            Frame frame= new Frame { Content = frameGrid, HasShadow=true, CornerRadius=5 };
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
            tapGestureRecognizer.NumberOfTapsRequired = 1;
            frame.GestureRecognizers.Add(tapGestureRecognizer);
            return frame;
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
        void RefreshOrderList()
        {
            
            using (StreamReader reader = new StreamReader(File.OpenRead(App.ordersCachePath)))
            {
                string ordersJson = reader.ReadToEnd();
                if (!String.IsNullOrEmpty(ordersJson))
                {
                    var orders = JsonConvert.DeserializeObject<List<Order>>(ordersJson);
                    Orders.Clear();
                    foreach (Order order in orders)
                    {
                        Orders.Add(order);
                        //}
                        //CarouselView car = new CarouselView();
                        //car.ItemsSource.GetEnumerator.
                        //    gridddd
                    }
                    Frame frame = orderFrame(Orders[Orders.Count - 1]);
                    Grid.SetRow(frame, 2);
                    grid.Children.Add(frame);
                    //grid.RowDefinitions[0].Height = 200;
                }
                needToRefresh = false;
                //carouselV.ItemsSource = Orders;
            }
           
            //foreach(Frame frame in carouselV.ItemsSource)
            //{

            //}
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

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (!tappedOnMap)
            {
                //mainScrollV.HeightRequest += 200;
                //grid.RowDefinitions[0].Height = 400;
                ////grid.HeightRequest += 200;
                ////mainScrollV.Orientation = ScrollOrientation.Neither;
                //tappedOnMap = true;
                
            }
            else
            {
                //mainScrollV.HeightRequest -= 200;
                //grid.RowDefinitions[0].Height = 200;
                ////grid.HeightRequest -= 200;
                ////mainScrollV.Orientation = ScrollOrientation.Vertical;
                //tappedOnMap = false;
            }

        }
    }
}