using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using BorschISmetanka.Models;

namespace BorschISmetanka.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BasketPage : ContentPage
    {
        List<string> userAddresses = AddressListPage.addressList;
        static StackLayout BasketStack = new StackLayout();
        static public List <Dish> DishesInBasket = new List<Dish>();
        static public List<Dish> basketList = new List<Dish>();
        static public bool needToRefresh = true;//true, когда необходимо обновить корзину
        static public bool cacheNeedToRefresh = false;//true, когда необходимо переписать кэш
        static public string Summ = 0.ToString();
        static public int Sum;
        public static Picker addressPicker;        
        /// <summary>
        public bool b = true;
        /// </summary>


        static string path1 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public BasketPage()
        {
            InitializeComponent();
            //BindingContext = this;
            addressPicker = new Picker();
            addressPicker.SelectedIndexChanged += picker_SelectedIndexChanged;
            pickerStack.Children.Add(addressPicker);
            Basket.Content = BasketStack;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (needToRefresh)
            {
                RefreshBasket();
                needToRefresh = false;
            }
        }
        void RefreshPicker()
        {
            addressPicker.Items.Clear();
            if (App.USER != null)
            {
                for (int i = 0; i < App.USER.addresses.Count; i++)
                {
                    addressPicker.Items.Add(App.USER.addresses[i].GetAddress());
                    if (App.USER.addresses[i].IsFavorite)
                        addressPicker.SelectedItem = addressPicker.Items[i];
                }
            }
            else
            {

            }
        }
        public static void AddToPicker(Address add)
        {
            addressPicker.Items.Add(add.GetAddress());
            addressPicker.SelectedIndex=0;
        }
        protected async override void OnDisappearing()
        {
            base.OnDisappearing();
            if (cacheNeedToRefresh)
                await RefreshCache();
        }
        void AddProduct(Dish dish)
        {
            backLabel.Text = "";
            Image image = new Image { Source =dish.image };
            long price = dish.price;            
            //string price = ""; Цена блюда
            Label Name = new Label
            {
                Text = dish.name,
                FontAttributes = FontAttributes.Bold,
                FontSize = 18,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start
            };
            Grid.SetColumn(image, 0);
            Grid.SetColumn(Name, 1);
            Grid internalGrid1 = new Grid();
            internalGrid1.Children.Add(image);
            internalGrid1.Children.Add(Name);
            Stepper counter = new Stepper
            {
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,
                Value = dish.count
            };
            counter.ValueChanged += StepperValue_Change;
            Label PriceLabel = new Label
            {
                Text = (price * counter.Value).ToString() + " руб",
                FontAttributes = FontAttributes.Bold,
                FontSize = 15,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center
            };
            Grid.SetColumn(PriceLabel, 0);
            Grid.SetColumn(counter, 1);
            Grid internalGrid2 = new Grid();
            internalGrid2.Children.Add(PriceLabel);
            internalGrid2.Children.Add(counter);
            internalGrid2.ColumnDefinitions.Add(new ColumnDefinition { Width = 70 });
            BoxView line = new BoxView { BackgroundColor = Color.Silver };
            Grid.SetRow(internalGrid1, 0);
            Grid.SetRow(line, 1);
            Grid.SetRow(internalGrid2, 2);
            Grid externalGrid = new Grid();
            externalGrid.RowDefinitions.Add(new RowDefinition { Height = 80 });
            externalGrid.RowDefinitions.Add(new RowDefinition { Height = 2 });
            externalGrid.RowDefinitions.Add(new RowDefinition { Height = 50 });
            externalGrid.Children.Add(internalGrid1);
            externalGrid.Children.Add(line);
            externalGrid.Children.Add(internalGrid2);
            Frame frame = new Frame();
            frame.Content = externalGrid;
            BasketStack.Children.Add(frame);
        }       
        public void RemoveDish(Dish dish)
        {
            dish.count = 0;
            int index = DishesInBasket.IndexOf(dish);
            DishesInBasket.RemoveAt(index);
            BasketStack.Children.RemoveAt(index);
            MenuPage.RemoveFromDishList(index);
            if(DishesInBasket.Count==0)
                backLabel.Text = "Корзина пуста";
        }
        public void RefreshBasket()
        {
            BasketStack.Children.Clear();
            DishesInBasket.Clear();
            using (StreamReader reader = new StreamReader(File.OpenRead(App.basketCachePath)))/*(FileStream openStream = File.OpenRead(AppShell.basketCachePath))*/
            {
                string s = reader.ReadToEnd();
                var dishes = JsonConvert.DeserializeObject<List<Dish>>(s);
                if(dishes!=null)
                foreach (Dish dish in dishes)
                {
                    DishesInBasket.Add(dish);
                    AddProduct(dish);
                }
            }
            CalculateBasketPrice();
        }
        public void AddToOrdersCache(Order newOrder)
        {
            List<Order> orders = new List<Order>();
            string cacheJson;
            using (StreamReader reader = new StreamReader(File.OpenRead(App.ordersCachePath)))
            {
                cacheJson = reader.ReadToEnd();
            }
            if (String.IsNullOrEmpty(cacheJson))
            {
                using (StreamWriter writer = new StreamWriter(File.OpenWrite(App.ordersCachePath)))
                {
                    orders.Add(newOrder);
                    string s = JsonConvert.SerializeObject(orders);
                    writer.Write(s);
                    writer.Close();
                }
            }
            else
            {
                using (StreamReader reader = new StreamReader(File.OpenRead(App.ordersCachePath)))
                {
                    string s = reader.ReadToEnd();
                    orders = JsonConvert.DeserializeObject<List<Order>>(s);
                    orders.Add(newOrder);
                }
                using (StreamWriter writer = new StreamWriter(File.Create(App.ordersCachePath)))
                {
                    string list = JsonConvert.SerializeObject(orders);
                    writer.Write(list);
                    writer.Close();
                }
            }
        }
        void CalculateBasketPrice()
        {
            Sum = 0;
            foreach (Dish d in DishesInBasket)
            {
                Sum += CalculateDishPrice(d.count,d.price);
            }
            sumLabel.Text = Sum.ToString() + " руб";
        }
        public static int CalculateDishPrice(int count, int price)
        {
            return count * price;
        }
        public void StepperValue_Change(object sender,ValueChangedEventArgs e)
        {
            Stepper stepper = (Stepper)sender;
            foreach(Dish d in DishesInBasket)
            {
                if(d.name== ((Label)((Grid)((Grid)((Grid)(stepper.Parent)).Parent).Children[0]).Children[1]).Text)
                {
                    if (stepper.Value == 0)
                    {
                        RemoveDish(d);
                    }
                    else
                    {
                        ((Label)((Grid)(stepper.Parent)).Children[0]).Text = (Convert.ToInt32(d.price) * stepper.Value).ToString() + " руб";
                        d.count = Convert.ToInt32(stepper.Value);
                        DishesInBasket[DishesInBasket.IndexOf(d)].count = d.count;
                    }
                    break;
                }
            }
            cacheNeedToRefresh = true;
            CalculateBasketPrice();
            //int index = productList[0].IndexOf(((Label)((Grid)((Grid)((Grid)(stepper.Parent)).Parent).Children[0]).Children[1]).Text);
            //if (stepper.Value == 0)
            //    RemoveProduct(index);
            //else
            //    ((Label)((Grid)(stepper.Parent)).Children[0]).Text = (Convert.ToInt32(productList[1][index]) * stepper.Value).ToString();
;        }
        static async Task RefreshCache()
        {
            using (FileStream stream = File.Create(App.basketCachePath)) ;
            using (StreamWriter writer = new StreamWriter(File.OpenWrite(App.basketCachePath)))
            {
                string s = JsonConvert.SerializeObject(DishesInBasket);
                writer.Write(s);
                writer.Close();
            }
            cacheNeedToRefresh = false;
        }
        private bool _isExpanded = false;

        async void SwipeGestureRecognizer_SwipedDown(Object sender, SwipedEventArgs e)
        {
            if (_isExpanded)
            {
                await orderFrame.TranslateTo(0, 0, 200, Easing.CubicInOut);
                _isExpanded = false;
                orderFrame.Opacity = 0;
                orderingBtn1.IsEnabled = true;
                orderingBtn1.Opacity = 1;
                line.IsVisible = true;
                sumLabel.IsVisible = true;
            }
        }
        void picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(addressPicker.SelectedItem!=null&&payPicker.SelectedItem!=null)
            orderingBtn2.IsEnabled = true;
        }
        async void  ToOrderBtn_Click(object sender, EventArgs e)
        {
            if (App.USER == null)
            {
                await DisplayAlert("Авторизуйтесь,чтобы сделать заказ", "", "Ок");
            }
            else
            {
                if (DishesInBasket.Count != 0)
                    if (!_isExpanded)
                    {
                        await orderFrame.TranslateTo(0, -500, 200, Easing.CubicInOut);
                        RefreshPicker();
                        orderFrame.Opacity = 1;
                        orderingBtn1.IsEnabled = false;
                        orderingBtn1.Opacity = 0;
                        line.IsVisible = false;
                        sumLabel.IsVisible = false;
                        _isExpanded = true;
                    }
            }
        }
        async void Checkout_Click(object sender, EventArgs e)
        {
            Order order = new Order {address= App.USER.addresses[addressPicker.SelectedIndex],date=DateTime.Now,sum=Sum, dishes=DishesInBasket, с_id=App.USER.id};
            order.dishesJson = JsonConvert.SerializeObject(order.dishes);
            order.addressString = order.address.GetAddress();
            string orderJson = JsonConvert.SerializeObject(order);
            await App.postAsync(App.ip +"Orders", JsonConvert.SerializeObject(order));
            try
            {
                var response = await App.getAsync(App.ip + "Orders/customer/" + App.USER.id + "/" + order.date.ToString("dd HH:mm"),100);
                var orderFromResponse = JsonConvert.DeserializeObject<Order>(response);
                order.leadTime = orderFromResponse.leadTime;
                order.id = orderFromResponse.id;
            }
            catch(Exception ex)
            {
                await DisplayAlert("Отсутствует соединение с сервером", "Проверьте соединение", "Ок");
            }
            App.USER.orders.Add(order);
            App.WriteToCache();
            //AddToOrdersCache(order);
            DishesInBasket.Clear();
            await orderFrame.TranslateTo(0, 0, 200, Easing.CubicInOut);            
            using (FileStream stream = File.Create(App.basketCachePath));
            orderFrame.Opacity = 0;
            orderingBtn1.IsEnabled = true;
            orderingBtn1.Opacity = 1;
            line.IsVisible = true;
            sumLabel.IsVisible = true;
            RefreshBasket();
            MenuPage.dishesnameList.Clear();
            OrderPage.needToRefresh=true;
            _isExpanded = false;
            backLabel.Text = "Корзина пуста";
            Sum = 0;
            await Shell.Current.GoToAsync("///Orders");
        }
        async void AddAddress_Click(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddressListPage());
        }
}
}