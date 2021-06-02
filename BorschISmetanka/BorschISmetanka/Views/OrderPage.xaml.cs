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
using Plugin.Geolocator;
using System.Timers;

namespace BorschISmetanka.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderPage : ContentPage
    {
        static StackLayout dishesStack;
        static Label orderAdd;
        public ObservableCollection<Order> Orders { get; set; } = new ObservableCollection<Order>();
        bool tappedOnMap = false;
        System.Timers.Timer _timer;
        public List<string> AddressPointList { get; set; }
        public string currentStatus = "";

        //CarouselView carouselV = new CarouselView();

        static public bool needToRefresh;
        public OrderPage()
        {
            BindingContext = this;

            AddressPointList = new List<string>()
            {
                "72230 Ruaudin, France",
                "72100 Le Mans, France",
                "77500 Chelles, France"
            };
            InitializeComponent();
            dishesStack = new StackLayout { Orientation = StackOrientation.Horizontal };
            orderAdd = new Label();
            Grid.SetRow(orderAdd, 2);
            grid.Children.Add(orderAdd);
           // photoScrollView.Content = dishesStack;
            needToRefresh = true;
            //carouselV.SetBinding(ItemsView.ItemsSourceProperty, "Orders");
            
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            //if (needToRefresh)
                RefreshOrderList();
            await GetCurrentLocation();
        }
        public void RefreshPage()
        {
            _timer = new System.Timers.Timer();
            _timer.AutoReset = true;
            _timer.Interval = 3000;
            _timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            _timer.Start();
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (grid.Children.Count != 3)
                Device.BeginInvokeOnMainThread(() =>
                {
                    for(int i=0;i<orderStack.Children.Count;i++)
                    {
                        if(App.USER!=null)
                        ((Label)((Grid)((Grid)((Frame)orderStack.Children[i]).Content).Children[9]).Children[1]).Text = App.USER.orders[i].status;
                    }
                });
        }

        Frame orderFrame(Order order)
        {
            order.Status();
            Label dateLabel = new Label { Text = order.date.ToString("dd.MM.yyyy HH:mm"), TextColor = Color.Black, FontSize = 20, FontAttributes = FontAttributes.Bold, HorizontalOptions = LayoutOptions.Start };
            BoxView line1 = new BoxView { BackgroundColor = Color.Silver};
            Label label1 = new Label { Text = "Доставка", FontSize = 15, HorizontalOptions = LayoutOptions.Start};
            Label addressLabel = new Label { Text = order.addressString, TextColor = Color.Black, FontSize = 18, HorizontalOptions = LayoutOptions.Start, VerticalOptions=LayoutOptions.Center};
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
            //Binding binding = new Binding { Source = order, Path="status" };
            //statusLabel.SetBinding(Label.TextProperty, binding);
            Label label4 = new Label { Text = "Доставят до", HorizontalOptions = LayoutOptions.Start, TextColor = Color.Black, FontSize = 18 };
            Label timeLabel = new Label { Text = order.date.AddMinutes(order.leadTime).ToString("HH:mm"), HorizontalOptions = LayoutOptions.End, TextColor = Color.Black, FontSize = 18 };//нужно изменить время
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
                //var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                //cts = new CancellationTokenSource();
                //var location =await  Geolocation.GetLocationAsync(request, cts.Token);
                var locator = CrossGeolocator.Current;
                var position = await locator.GetPositionAsync(new TimeSpan(100000));
                Polygon polygon = new Polygon
                {
                    StrokeWidth = 8,
                    StrokeColor = Color.FromHex("#1BA1E2"),
                    FillColor = Color.FromHex("#881BA1E2"),
                    Geopath =
                    {
                        new Position(position.Latitude, position.Longitude),
                        new Position(USATU.Latitude, USATU.Longitude),
                    }
                };
                MyMap.MapElements.Add(polygon);
            }


            catch (Exception ex)
            {

            }
            //catch (FeatureNotSupportedException fnsEx)
            //{
            //    // Handle not supported on device exception
            //}
            //catch (FeatureNotEnabledException fneEx)
            //{
            //    // Handle not enabled on device exception
            //}
            //catch (PermissionException pEx)
            //{
            //    // Handle permission exception
            //}
            //catch (Exception ex)
            //{
            //    // Unable to get location
            //}
        }
        public async void RefreshOrderList()
        {
            if (App.USER!=null)
            {
                var jsonOrders = await App.getAsync(App.ip + "Orders/customer/allorders/" + App.USER.id,100);
                var orders = JsonConvert.DeserializeObject<List<Order>>(jsonOrders);
                foreach (Order o in orders)
                    o.dishes = JsonConvert.DeserializeObject<List<Dish>>(o.dishesJson);
                App.USER.orders = orders;
                orderStack.Children.Clear();
                if (orders.Count != 0)
                {
                    foreach (Order o in orders)
                    {
                        Frame frame = orderFrame(o);
                        orderStack.Children.Add(frame);                        
                    }
                    needToRefresh = false;
                }
                RefreshPage();
            }
            else
            {
                grid.Children.RemoveAt(2);
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
        public async Task NavigateToBuilding25()
        {
            var location = new Location(54.725019721084095, 55.94091973567985);
            var options = new MapLaunchOptions { NavigationMode = NavigationMode.Driving };

            await Xamarin.Essentials.Map.OpenAsync(location, options);
        }
    }
    public class MapTest
    {
        //directionsDisplay = new google.maps.DirectionsRenderer();  
    }
}