using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using Xamarin.Essentials;
using BorschISmetanka.Models;

namespace BorschISmetanka.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {    
        //static bool basketIsEmpty = true;
        bool loading = true;
        //private readonly HttpClient http = new HttpClient();
        public ObservableCollection<ScrollView> scrolls { get; set; } = new ObservableCollection<ScrollView>();
        public ObservableCollection<Dish> recipes_cat { get; set; } = new ObservableCollection<Dish>();
        public List<ObservableCollection<Dish>> My_menu = new List<ObservableCollection<Dish>>();
        public List<Dish> dishes = new List<Dish>();
        //static public List<Dish> dishesList = new List<Dish>();
        public static List<string> dishesnameList = new List<string>();//название добавленных в корзину блюд
        public MenuPage()
        {
            InitializeComponent();
            BindingContext = this;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (loading)
            {
                MenuLoad();
            }
        }
        void FillMenu()
        {
            foreach (ObservableCollection<Dish> dishes in My_menu)
            {
                scrolls.Add(new ScrollView { Content = FillMenuSection(dishes) });
            }
        }

        StackLayout FillMenuSection(ObservableCollection<Dish> dishes)
        {
            StackLayout stack = new StackLayout();
            foreach (Dish d in dishes)
            {
                stack.Children.Add(BuildDishItem(d));
            }
            return stack;
        }
        public Frame BuildDishItem(Dish dish)
        {
            Image image = new Image
            {
                Source = dish.image,
                Aspect = Aspect.AspectFill,
                HeightRequest = 150,
                WidthRequest = 150
            };
            Label price = new Label
            {
                Text = dish.price.ToString() + " руб",
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            Grid internalGrid1 = new Grid();
            Grid.SetRow(image, 0);
            Grid.SetRow(price, 1);
            internalGrid1.Children.Add(image);
            internalGrid1.Children.Add(price);
            internalGrid1.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            internalGrid1.RowDefinitions.Add(new RowDefinition { Height = 32 });
            Label name = new Label
            {
                Text = dish.name,
                FontAttributes = FontAttributes.Bold,
                FontSize = 20,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                LineBreakMode = LineBreakMode.WordWrap
            };
            Label description = new Label
            {
                Text = dish.description,
                FontAttributes = FontAttributes.Italic,
                FontSize = 15,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                MaxLines = 5,
                LineBreakMode = LineBreakMode.TailTruncation
            };
            Button ToBasketBtn = new Button
            {
                Text = "В корзину",
                TextColor = Color.LimeGreen,
                BackgroundColor = Color.White,
                BorderColor = Color.LimeGreen,
                BorderWidth = 1.5,
                CornerRadius = 5
            };
            ToBasketBtn.Clicked += ToBasket_Click;
            Grid internalGrid2 = new Grid();
            Grid.SetRow(name, 0);
            Grid.SetRow(description, 1);
            Grid.SetRow(ToBasketBtn, 2);
            internalGrid2.Children.Add(name);
            internalGrid2.Children.Add(description);
            internalGrid2.Children.Add(ToBasketBtn);
            internalGrid2.RowDefinitions.Add(new RowDefinition { Height = 30 });
            internalGrid2.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            internalGrid2.RowDefinitions.Add(new RowDefinition { Height = 40 });
            Grid externalGrid = new Grid();
            Grid.SetColumn(internalGrid1, 0);
            Grid.SetColumn(internalGrid2, 1);
            externalGrid.Children.Add(internalGrid1);
            externalGrid.Children.Add(internalGrid2);
            Frame frame = new Frame { Content = externalGrid };
            return frame;

        }       
        private async void MenuLoad()
        {
            try
            {
                string response = await App.getAsync(App.ip + "Dishes",300);
                if (response != "[]")
                {
                    dishes = JsonConvert.DeserializeObject<List<Dish>>(response);
                    foreach (Dish dish in dishes)
                    {
                        bool ok = true;
                        foreach (Dish dish1 in recipes_cat)
                        {
                            if (dish.category == dish1.category)
                            {
                                ok = false;
                                break;
                            }
                        }
                        if (ok)
                        {
                            recipes_cat.Add(dish);
                            Button button = new Button
                            {
                                Text = dish.category,
                                TextColor = Color.Gray,
                                BorderColor = Color.Gray,
                                BorderWidth = 2,
                                BackgroundColor = Color.White,
                                CornerRadius = 30
                            };
                            button.Clicked += MenuButton_Click;
                            ButtonStack.Children.Add(button);
                        }
                    }
                    foreach (Dish recipes_cat in recipes_cat)
                    {
                        ObservableCollection<Dish> d = new ObservableCollection<Dish>();
                        foreach (Dish dishes in dishes)
                        {
                            if (recipes_cat.category == dishes.category)
                            {
                                d.Add(dishes);
                            }
                        }
                        My_menu.Add(d);
                    }
                    FillMenu();
                    menuSV.Content = scrolls[0].Content;
                    CurrentButton(0);
                    loading = false;
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("Отсутствует подключение к серверу", "Проверьте соединение", "Ок");
            }
        }
        static public void RemoveFromDishList(int index)
        {
            dishesnameList.RemoveAt(index);
        }
        //sta
        static bool DishHasAdded(Dish dish, List<Dish> dishes)
        {
            foreach (Dish d in dishes)
            {
                if (d.name == dish.name)
                    return true;
            }
            return false;
        }
        static int DishesIndex(Dish dish)
        {
            return dishesnameList.IndexOf(dish.name);
        }
        static void AddToBasketCache(Dish dish)
        {
            string list;
            dish.count = 0;
            List<Dish> dishes = new List<Dish>();
            if (dishesnameList.Count == 0)
            {
                using (StreamWriter writer = new StreamWriter(App.basketCachePath)) /*(FileStream writer = File.OpenWrite(AppShell.basketCachePath))*/
                {
                    dishes.Add(dish);
                    dishes[0].count++;

                    dishesnameList.Add(dish.name);
                    string s = JsonConvert.SerializeObject(dishes);
                    writer.Write(s);
                    writer.Close();
                }
            }
            else
            {
                using (StreamReader reader = new StreamReader(File.OpenRead(App.basketCachePath)))
                {
                    string s = reader.ReadToEnd();
                    dishes = JsonConvert.DeserializeObject<List<Dish>>(s);
                    if (DishHasAdded(dish, dishes))
                        dishes[DishesIndex(dish)].count++;
                    else
                    {
                        dishes.Add(dish);
                        dishesnameList.Add(dish.name);
                        dishes[dishes.Count - 1].count++;
                    }
                }
                using (StreamWriter writer = new StreamWriter(File.Create(App.basketCachePath)))
                {
                    list = JsonConvert.SerializeObject(dishes);
                    writer.Write(list);
                    writer.Close();
                }
            }
        }
        public void ToBasket_Click(object sender, EventArgs e)
        {
            Grid grid = (Grid)(((Button)sender).Parent);
            string name = ((Label)(grid.Children[0])).Text;
            foreach (Dish d in dishes)
            {
                if (name == d.name)
                {
                    AddToBasketCache(d);
                    BasketPage.needToRefresh = true;
                    break;
                }
            }
        }
        public void MenuButton_Click(object sender, EventArgs e)
        {
            var indexOfIntegerValue = ButtonStack.Children.IndexOf((Button)sender);
            CurrentButton(indexOfIntegerValue);
            menuSV.Content = scrolls[indexOfIntegerValue].Content;
        }
        public void CurrentButton(int currentIndex)
        {
            for (int i = 0; i < ButtonStack.Children.Count; i++)
            {
                if (i == currentIndex)
                {
                    ((Button)ButtonStack.Children[i]).TextColor = Color.FromHex("e34f4f");
                    ((Button)ButtonStack.Children[i]).BorderColor = Color.FromHex("e34f4f");
                }
                else
                {
                    ((Button)ButtonStack.Children[i]).TextColor = Color.Gray;
                    ((Button)ButtonStack.Children[i]).BorderColor = Color.Gray;
                }
            }
        }
        public void OnPositionChange(object sender, PositionChangedEventArgs e)
        {
            CurrentButton(((CarouselView)sender).Position);
        }
    }
}