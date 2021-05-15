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
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        static bool cacheExist = false;
        static string menu2 = "[\r\n  [\r\n    {\r\n      \"name\": \"Baboon\",\r\n      \"Location\": \"Africa & Asia\",\r\n      \"description\": \"Baboons are African and Arabian Old World monkeys belonging to the genus Papio, part of the subfamily Cercopithecinae.\",\r\n      \"image\": \"https://upload.wikimedia.org/wikipedia/commons/thumb/9/96/Portrait_Of_A_Baboon.jpg/314px-Portrait_Of_A_Baboon.jpg\",\r\n      \"price\": 10000,\r\n      \"Latitude\": -8.783195,\r\n      \"Longitude\": 34.508523\r\n    },\r\n    {\r\n      \"name\": \"Capuchin Monkey\",\r\n      \"Location\": \"Central & South America\",\r\n      \"description\": \"The capuchin monkeys are New World monkeys of the subfamily Cebinae. Prior to 2011, the subfamily contained only a single genus, Cebus.\",\r\n      \"image\": \"https://upload.wikimedia.org/wikipedia/commons/thumb/4/40/Capuchin_Costa_Rica.jpg/200px-Capuchin_Costa_Rica.jpg\",\r\n      \"Population\": 23000,\r\n      \"Latitude\": 12.769013,\r\n      \"Longitude\": -85.602364\r\n    },\r\n    {\r\n      \"name\": \"Blue Monkey\",\r\n      \"Location\": \"Central and East Africa\",\r\n      \"description\": \"The blue monkey or diademed monkey is a species of Old World monkey native to Central and East Africa, ranging from the upper Congo River basin east to the East African Rift and south to northern Angola and Zambia\",\r\n      \"image\": \"https://upload.wikimedia.org/wikipedia/commons/thumb/8/83/BlueMonkey.jpg/220px-BlueMonkey.jpg\",\r\n      \"price\": 12000,\r\n      \"Latitude\": 1.957709,\r\n      \"Longitude\": 37.297204\r\n    }\r\n  ],\r\n  [\r\n    {\r\n      \"name\": \"Squirrel Monkey\",\r\n      \"description\": \"The blue monkey or diademed monkey is a species of Old World monkey native to Central and East Africa, ranging from the upper Congo River basin east to the East African Rift and south to northern Angola and Zambia\",\r\n      \"image\": \"https://upload.wikimedia.org/wikipedia/commons/thumb/2/20/Saimiri_sciureus-1_Luc_Viatour.jpg/220px-Saimiri_sciureus-1_Luc_Viatour.jpg\",\r\n      \"price\": 11000,\r\n      \"count\": 0\r\n    },\r\n    {\r\n      \"name\": \"Golden Lion Tamarin\",\r\n      \"description\": \"The golden lion tamarin also known as the golden marmoset, is a small New World monkey of the family Callitrichidae.\",\r\n      \"image\": \"https://upload.wikimedia.org/wikipedia/commons/thumb/8/87/Golden_lion_tamarin_portrait3.jpg/220px-Golden_lion_tamarin_portrait3.jpg\",\r\n      \"price\": 300,\r\n      \"count\": 0\r\n    },\r\n    {\r\n      \"name\": \"Howler Monkey\",\r\n      \"description\": \"Howler monkeys are among the largest of the New World monkeys. Fifteen species are currently recognised. Previously classified in the family Cebidae, they are now placed in the family Atelidae.\",\r\n      \"image\": \"https://upload.wikimedia.org/wikipedia/commons/thumb/0/0d/Alouatta_guariba.jpg/200px-Alouatta_guariba.jpg\",\r\n      \"price\": 3200,\r\n      \"count\": 0\r\n    }\r\n  ]\r\n]";

        static string recipes = "[\r\n  {\r\n    \"category\": \"pizda\",\r\n    \"recipes\": \r\n    [\r\n      {\r\n        \"name\": \"dd\",\r\n        \"description\": \"dddd\",\r\n        \"image\": null,\r\n        \"price\": 3,\r\n        \"count\": 0\r\n      },\r\n      {\r\n        \"name\": \"aaaa\",\r\n        \"description\": \"dddd\",\r\n        \"image\": null,\r\n        \"price\": 3,\r\n        \"count\": 0\r\n      }\r\n    ]\r\n  },\r\n  {\r\n    \"category\": \"hui\",\r\n    \"recipes\": [\r\n      {\r\n        \"name\": \"dd\",\r\n        \"description\": \"dddd\",\r\n        \"image\": null,\r\n        \"price\": 3,\r\n        \"count\": 0\r\n      },\r\n      {\r\n        \"name\": \"aaaa\",\r\n        \"description\": \"dddd\",\r\n        \"image\": null,\r\n        \"price\": 3,\r\n        \"count\": 0\r\n      }\r\n    ]\r\n  }\r\n]";
        static string recipes2 = "[\r\n  {\r\n    \"category\": \"pizda\",\r\n    \"recipes\": \r\n    [\r\n      {\r\n        \"name\": \"Kasha\",\r\n        \"description\": \"vkusnaya\",\r\n        \"image\": null,\r\n        \"price\": 3,\r\n        \"count\": 0\r\n      },\r\n      {\r\n        \"name\": \"Kasha\",\r\n        \"description\": \"nevkusnaya\",\r\n        \"image\": null,\r\n        \"price\": 4,\r\n        \"count\": 0\r\n      }\r\n    ]\r\n  },\r\n  {\r\n    \"category\": \"hui\",\r\n    \"recipes\": [\r\n      {\r\n        \"name\": \"sup\",\r\n        \"description\": \"dddd\",\r\n        \"image\": null,\r\n        \"price\": 5,\r\n        \"count\": 0\r\n      },\r\n      {\r\n        \"name\": \"puyre\",\r\n        \"description\": \"dddd\",\r\n        \"image\": null,\r\n        \"price\": 6,\r\n        \"count\": 0\r\n      }\r\n    ]\r\n  }\r\n]";
        static string MENU = "[\r\n  {\r\n    \"category\": \"Завтраки\",\r\n    \"recipes\": [\r\n      {\r\n        \"name\": \"Каша\",\r\n        \"description\": \"Вкусная\",\r\n        \"image\": \"https://eda.ru/img/eda/c620x415i/s2.eda.ru/StaticContent/Photos/130822102925/190811173919/p_O.jpg\",\r\n        \"price\": 80,\r\n        \"count\": 0\r\n      },\r\n      {\r\n        \"name\": \"Каша\",\r\n        \"description\": \"Очень вкусная\",\r\n        \"image\": \"https://www.patee.ru/r/x6/05/0f/ce/960m.jpg\",\r\n        \"price\": 100,\r\n        \"count\": 0\r\n      },\r\n      {\r\n        \"name\": \"Сырники со сгущенкой\",\r\n        \"description\": \"Отрыв башки\",\r\n        \"image\": \"https://www.koolinar.ru/all_image/recipes/140/140944/recipe_4fe17d68-e1cc-437c-830a-675a42e65d49_w450.jpg\",\r\n        \"price\": 120,\r\n        \"count\": 0\r\n      }\r\n    ]\r\n  },\r\n  {\r\n    \"category\": \"Бизнес-ланчи\",\r\n    \"recipes\": [\r\n      {\r\n        \"name\": \"Бизнес-ланч 1\",\r\n        \"description\": \"Очень сытно и полезно\",\r\n        \"image\": \"https://canapeclub.ru/image/cache/catalog/product/30-10-2019/lanch%E2%84%961-500x500.jpg\",\r\n        \"price\": 240,\r\n        \"count\": 0\r\n      },\r\n      {\r\n        \"name\": \"Бизнес-ланчи 2\",\r\n        \"description\": \"Очень сытно,полезно да еще и вкусно\",\r\n        \"image\": \"https://kolesovremeni.ru/storage/app/media/uploaded-files/lanch.jpg\",\r\n        \"price\": 200,\r\n        \"count\": 0\r\n      },\r\n      {\r\n        \"name\": \"Бизнес-ланчи 2\",\r\n        \"description\": \"Очень сытно,полезно да еще и вкусно\",\r\n        \"image\": \"https://kolesovremeni.ru/storage/app/media/uploaded-files/lanch.jpg\",\r\n        \"price\": 200,\r\n        \"count\": 0\r\n      }\r\n    ]\r\n  },\r\n  {\r\n    \"category\": \"Супы\",\r\n    \"recipes\": [\r\n      {\r\n        \"name\": \"Борщ\",\r\n        \"description\": \"Это борщ\",\r\n        \"image\": \"https://eda.ru/img/eda/c620x415i/s2.eda.ru/StaticContent/Photos/130822102925/190811173919/p_O.jpg\",\r\n        \"price\": 80,\r\n        \"count\": 0\r\n      },\r\n      {\r\n        \"name\": \"Щи\",\r\n        \"description\": \"Такой же щи как я\",\r\n        \"image\": \"https://www.patee.ru/r/x6/05/0f/ce/960m.jpg\",\r\n        \"price\": 60,\r\n        \"count\": 0\r\n      },\r\n      {\r\n        \"name\": \"Лапша\",\r\n        \"description\": \"Тоже бомба\",\r\n        \"image\": \"https://www.koolinar.ru/all_image/recipes/140/140944/recipe_4fe17d68-e1cc-437c-830a-675a42e65d49_w450.jpg\",\r\n        \"price\": 60,\r\n        \"count\": 0\r\n      }\r\n    ]\r\n  },\r\n  {\r\n    \"category\": \"Десерты\",\r\n    \"recipes\": [\r\n      {\r\n        \"name\": \"Пирог\",\r\n        \"description\": \"Очень сытно и полезно\",\r\n        \"image\": \"https://canapeclub.ru/image/cache/catalog/product/30-10-2019/lanch%E2%84%961-500x500.jpg\",\r\n        \"price\": 120,\r\n        \"count\": 0\r\n      },\r\n      {\r\n        \"name\": \"Еще один пирог\",\r\n        \"description\": \"Очень сытно,полезно да еще и вкусно\",\r\n        \"image\": \"https://kolesovremeni.ru/storage/app/media/uploaded-files/lanch.jpg\",\r\n        \"price\": 140,\r\n        \"count\": 0\r\n      }\r\n    ]\r\n  }\r\n]";

        static bool casheHasUpdated = false;
        string menu = "[{\"count\":0,\"Name\":\"Baboon\",\"Details\":\"Baboons are African and Arabian Old World monkeys belonging to the genus Papio, part of the subfamily Cercopithecinae.\",\"Image\":\"https://upload.wikimedia.org/wikipedia/commons/thumb/9/96/Portrait_Of_A_Baboon.jpg/314px-Portrait_Of_A_Baboon.jpg\",\"Population\":10000},{\"count\":0,\"Name\":\"Capuchin Monkey\",\"Details\":\"The capuchin monkeys are New World monkeys of the subfamily Cebinae. Prior to 2011, the subfamily contained only a single genus, Cebus.\",\"Image\":\"https://upload.wikimedia.org/wikipedia/commons/thumb/4/40/Capuchin_Costa_Rica.jpg/200px-Capuchin_Costa_Rica.jpg\",\"Population\":23000},{\"count\":0,\"Name\":\"Blue Monkey\",\"Details\":\"The blue monkey or diademed monkey is a species of Old World monkey native to Central and East Africa, ranging from the upper Congo River basin east to the East African Rift and south to northern Angola and Zambia\",\"Image\":\"https://upload.wikimedia.org/wikipedia/commons/thumb/8/83/BlueMonkey.jpg/220px-BlueMonkey.jpg\",\"Population\":12000},{\"count\":0,\"Name\":\"Squirrel Monkey\",\"Details\":\"The squirrel monkeys are the New World monkeys of the genus Saimiri. They are the only genus in the subfamily Saimirinae. The name of the genus Saimiri is of Tupi origin, and was also used as an English name by early researchers.\",\"Image\":\"https://upload.wikimedia.org/wikipedia/commons/thumb/2/20/Saimiri_sciureus-1_Luc_Viatour.jpg/220px-Saimiri_sciureus-1_Luc_Viatour.jpg\",\"Population\":11000},{\"count\":0,\"Name\":\"Golden Lion Tamarin\",\"Details\":\"The golden lion tamarin also known as the golden marmoset, is a small New World monkey of the family Callitrichidae.\",\"Image\":\"https://upload.wikimedia.org/wikipedia/commons/thumb/8/87/Golden_lion_tamarin_portrait3.jpg/220px-Golden_lion_tamarin_portrait3.jpg\",\"Population\":19000}]";
        string newmenu = "[{\"count\":0,\"name\":\"Baboon\",\"description\":\"Baboons are African and Arabian Old World monkeys belonging to the genus Papio, part of the subfamily Cercopithecinae.\",\"image\":\"https://upload.wikimedia.org/wikipedia/commons/thumb/9/96/Portrait_Of_A_Baboon.jpg/314px-Portrait_Of_A_Baboon.jpg\",\"price\":10000},{\"count\":0,\"name\":\"Capuchin Monkey\",\"description\":\"The capuchin monkeys are New World monkeys of the subfamily Cebinae. Prior to 2011, the subfamily contained only a single genus, Cebus.\",\"image\":\"https://upload.wikimedia.org/wikipedia/commons/thumb/4/40/Capuchin_Costa_Rica.jpg/200px-Capuchin_Costa_Rica.jpg\",\"price\":23000},{\"count\":0,\"name\":\"Blue Monkey\",\"description\":\"The blue monkey or diademed monkey is a species of Old World monkey native to Central and East Africa, ranging from the upper Congo River basin east to the East African Rift and south to northern Angola and Zambia\",\"image\":\"https://upload.wikimedia.org/wikipedia/commons/thumb/8/83/BlueMonkey.jpg/220px-BlueMonkey.jpg\",\"price\":12000},{\"count\":0,\"name\":\"Squirrel Monkey\",\"description\":\"The squirrel monkeys are the New World monkeys of the genus Saimiri. They are the only genus in the subfamily Saimirinae. The name of the genus Saimiri is of Tupi origin, and was also used as an English name by early researchers.\",\"image\":\"https://upload.wikimedia.org/wikipedia/commons/thumb/2/20/Saimiri_sciureus-1_Luc_Viatour.jpg/220px-Saimiri_sciureus-1_Luc_Viatour.jpg\",\"price\":11000},{\"count\":0,\"name\":\"Golden Lion Tamarin\",\"description\":\"The golden lion tamarin also known as the golden marmoset, is a small New World monkey of the family Callitrichidae.\",\"image\":\"https://upload.wikimedia.org/wikipedia/commons/thumb/8/87/Golden_lion_tamarin_portrait3.jpg/220px-Golden_lion_tamarin_portrait3.jpg\",\"price\":19000}]";
        
        //public List<Button> menuBtn = new List<Button>();
        static bool basketIsEmpty = true;
        bool loading = true;
        static string path2 = "https://montemagno.com/Monkeys.json";
        private readonly HttpClient http = new HttpClient();
        List<Dish> allDishes=new List<Dish>();

        //public MyObservableCollection<Dish> Menu=new MyObservableCollection<Dish>();
        public ObservableCollection<Dish> Dishes { get; set; } = new ObservableCollection<Dish>();
        public ObservableCollection<razdel> razdely { get; set; } = new ObservableCollection<razdel>();
        public ObservableCollection<ScrollView> scrolls{ get; set; } = new ObservableCollection<ScrollView>();
        public ObservableCollection<Recipes_cat> recipes_cat { get; set; } = new ObservableCollection<Recipes_cat>();
        static public List<Dish> dishesList = new List<Dish>();
        //public ObservableCollection<ObservableCollection<Dish>> Menu = new ObservableCollection<ObservableCollection<Dish>>();
        static List<string> dishesnameList = new List<string>();//название добавленных в корзину блюд
        static CarouselView carousel = new CarouselView();
        //ObservableCollection<ScrollView> sv { get; set; }
        public MenuPage()
        {
            InitializeComponent();
            BindingContext = this;
            //Menu.Add(Z);
            //Menu.Add(BL);
            //Menu.Add(S);
            //Menu.Add(D);
            //LoadMenu();
        }
        void Carousel()
        {
            foreach (Recipes_cat rc in recipes_cat)
            {
                scrolls.Add(new ScrollView { Content = MenuStack(rc.dish) });
            }
            
        }

        StackLayout MenuStack(ObservableCollection<Dish> dishes)
        {
            StackLayout stack = new StackLayout();
            foreach(Dish d in dishes)
            {
                stack.Children.Add(MenuItem(d));
            }
            return stack;
        }
        public Frame MenuItem(Dish dish)
        {
            Image image = new Image 
            {
                Source = dish.image,
                Aspect=Aspect.AspectFill,
                HeightRequest=150,
                WidthRequest=150
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
                FontSize = 22,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            Label description = new Label
            {
                Text = dish.description,
                FontAttributes = FontAttributes.Italic,
                FontSize = 15,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                MaxLines=5,
                LineBreakMode=LineBreakMode.TailTruncation
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
            return new Frame { Content = externalGrid };

        }
        public void ButtonClick(object s, EventArgs e)
        {
            ((Button)s).Text = "fff";
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (loading)
            {
                //using(StreamReader reader=new StreamReader(pat+"/menu.json"))
                //{
                //    string s = reader.ReadToEnd();
                //    var dishes = JsonConvert.DeserializeObject<List<Dish1>>(s);
                //    Dishes.Clear();
                //    foreach (Dish1 dish in dishes)
                //    {
                //        Dishes1.Add(dish);
                //        ButtonStack.Children.Add(new Button
                //        {
                //            Text = dish.name,
                //            TextColor = Color.Gray,
                //            BorderColor = Color.Gray,
                //            BorderWidth = 2,
                //            BackgroundColor = Color.White,
                //            CornerRadius = 30
                //        });
                //    }
                //}
                //var dishes = JsonConvert.DeserializeObject<List<Dish>>(newmenu);

                var dishes= JsonConvert.DeserializeObject<List<Recipes_cat>>(MENU);
                //recipes_cat = JsonConvert.DeserializeObject<ObservableCollection<Recipes_cat>>(recipes);
                
                recipes_cat.Clear();
                foreach (Recipes_cat dish in dishes)
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
                Carousel();
                menuSV.Content = scrolls[0].Content;
                CurrentButton(0);
                loading = false;                
            }
        }
        static void RefreshDishList()
        {

        }
        static public void RemoveFromDishList(int index)
        {
            dishesnameList.RemoveAt(index);
        }
        static bool DishAdded(Dish dish, List<Dish> dishes)
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
        //static void CreateCacheFile()
        //{
        //    if (!Directory.Exists(AppShell.cacheFolderPath))
        //        Directory.CreateDirectory(AppShell.cacheFolderPath);
        //    if (!cacheExist)
        //    {
        //        using (FileStream stream = File.Create(AppShell.basketCachePath)) ;
        //        cacheExist = true;
        //    }
        //    //if (!File.Exists(AppShell.basketCachePath))
        //    //    File.Create(AppShell.basketCachePath);
        //}
        static void AddToBasketCache(Dish dish)
         {
            //CreateCacheFile();
            string list;
            List<Dish> dishes = new List<Dish>();
            if (dishesnameList.Count==0)
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
                    if (DishAdded(dish, dishes))
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
            foreach (Recipes_cat rc in recipes_cat) {
                foreach (Dish d in rc.dish)
                {
                    if (name == d.name)
                    {
                        AddToBasketCache(d);
                        BasketPage.needToRefresh = true;
                        break;
                    }
                }
            }
        }
        public void MenuButton_Click(object sender, EventArgs e)
        {
            var indexOfIntegerValue = ButtonStack.Children.IndexOf((Button)sender);
            CurrentButton(indexOfIntegerValue);
            //productCarousel.ScrollTo(indexOfIntegerValue);
            menuSV.Content = scrolls[indexOfIntegerValue].Content;
        }
        public void CurrentButton(int currentIndex)
        {
            for (int i = 0; i < ButtonStack.Children.Count; i++)
            {
                if (i == currentIndex)
                {
                    ((Button)ButtonStack.Children[i]).TextColor = Color.LimeGreen;
                    ((Button)ButtonStack.Children[i]).BorderColor = Color.LimeGreen;
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