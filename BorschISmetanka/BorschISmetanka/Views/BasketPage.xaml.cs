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
            for (int i = 0; i < App.USER.addresses.Count; i++) 
            {
                    addressPicker.Items.Add(App.USER.addresses[i].GetAddress());
                if (App.USER.addresses[i].IsFavorite)
                    addressPicker.SelectedItem = addressPicker.Items[i];
            }
        }
        public static void AddToPicker(Address add)
        {
            addressPicker.Items.Add(add.GetAddress());
        }
        protected async override void OnDisappearing()
        {
            base.OnDisappearing();
            if (cacheNeedToRefresh)
                await RefreshCache();
        }
        void AddProduct(Dish dish)
        {
            Image image = new Image { Source =dish.image };
            long price = dish.price;            
            //string price = ""; Цена блюда
            int Price = 350;
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
        public void AddDish(List<Dish> dishesList,int index)//кривое добавление в корзину
        {
            if (dishesList.Count==basketList.Count)
            {               
                ((Stepper)((Grid)((Grid)((Frame)BasketStack.Children[index]).Content).Children[2]).Children[1]).Value++;
            }
            else
            {
                basketList.Add(dishesList[index]);
                Image image = new Image { Source = basketList[basketList.Count - 1].image };
                //string price = ""; Цена блюда
                int Price = 350;
                Label Name = new Label
                {
                    Text = basketList[basketList.Count - 1].name,
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
                    Value = 1
                };
                counter.ValueChanged += StepperValue_Change;
                Label PriceLabel = new Label
                {
                    Text = (Price * counter.Value).ToString() + "руб",
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 18,
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
        }

        static public void RemoveDish(Dish dish)
        {
            int index = DishesInBasket.IndexOf(dish);
            DishesInBasket.RemoveAt(index);
            BasketStack.Children.RemoveAt(index);
            MenuPage.RemoveFromDishList(index);
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
            CalculateSum();
        }
        void AddToOrdersCache(Order newOrder)
        {
            List<Order> orders = new List<Order>();
            if (b)
            {
                using (StreamWriter writer = new StreamWriter(App.ordersCachePath)) /*(FileStream writer = File.OpenWrite(AppShell.basketCachePath))*/
                {
                    orders.Add(newOrder);
                    string s = JsonConvert.SerializeObject(orders);
                    writer.Write(s);
                    writer.Close();
                }
                b = false;
            }
            else
            {
                using (StreamReader reader = new StreamReader(File.OpenRead(App.basketCachePath)))
                {
                    string s = reader.ReadToEnd();
                    orders = JsonConvert.DeserializeObject<List<Order>>(s);
                    orders.Add(newOrder);
                }
                using (StreamWriter writer = new StreamWriter(File.Create(App.basketCachePath)))
                {
                    string list = JsonConvert.SerializeObject(orders);
                    writer.Write(list);
                    writer.Close();
                }
            }
        }
        void CalculateSum()
        {
            Sum = 0;
            foreach (Dish d in DishesInBasket)
            {
                Sum += d.count * d.price;
            }
            sumLabel.Text = Sum.ToString() + " руб";
        }
        public void StepperValue_Change(object sender,ValueChangedEventArgs e)
        {
            Stepper stepper = (Stepper)sender;
            foreach(Dish d in DishesInBasket)
            {
                if(d.name== ((Label)((Grid)((Grid)((Grid)(stepper.Parent)).Parent).Children[0]).Children[1]).Text)
                {
                    if (stepper.Value == 0)
                        RemoveDish(d);
                    else
                    {
                        ((Label)((Grid)(stepper.Parent)).Children[0]).Text = (Convert.ToInt32(d.price) * stepper.Value).ToString()+" руб";
                        d.count=Convert.ToInt32(stepper.Value);
                        DishesInBasket[DishesInBasket.IndexOf(d)].count = d.count;
                    }
                    break;
                }
            }
            cacheNeedToRefresh = true;
            CalculateSum();
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
            if(DishesInBasket.Count!=0)
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
        async void Checkout_Click(object sender, EventArgs e)
        {
            //Order order = new Order(App.USER.addresses[addressPicker.SelectedIndex], DishesInBasket);
            //int Sum = 0;
            //foreach(Dish d in DishesInBasket)
            //{
            //    Sum += d.price;
            //}
            Order order = new Order {address= App.USER.addresses[addressPicker.SelectedIndex], date=DateTime.Now, status="Готовится", sum=Sum, dishes=DishesInBasket};
            ////////////
            string orderJson = JsonConvert.SerializeObject(order);
            ////////////
            //OrderPage.CurrentOrder(order);
            AddToOrdersCache(order);
            DishesInBasket.Clear();
            //using (FileStream stream = File.Create(App.basketCachePath));
            //File.WriteAllText(App.basketCachePath, String.Empty);
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
        }
        async void AddAddress_Click(object sender, EventArgs e)
        {
            await Navigation.PushAsync(ProfilePage.addressPage);
        }
}
}