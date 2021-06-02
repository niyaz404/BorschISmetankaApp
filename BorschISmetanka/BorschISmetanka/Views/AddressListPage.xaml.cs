using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BorschISmetanka.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddressListPage : ContentPage
    {
        static public int addressCnt;
        static public List<string> addressList;        
        static public int favAddressIndex;
        static bool _isExpanded = false;
        static StackLayout stack;
        
        public AddressListPage()
        {
            InitializeComponent();
            addressList = new List<string>();
            stack = new StackLayout { Padding = 15 };           
            scrollview.Content = stack;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadUserAddress();
        }
        void LoadUserAddress()
        {
            addressCnt = 0;
            favAddressIndex = 0;
            addressList.Clear();
            stack.Children.Clear();
            if (App.USER != null)
            {
                foreach (Address add in App.USER.addresses)
                {
                    AddAddress(add);
                }
            }
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
        public async void AddAddressBtn_Click(object sender, EventArgs e)
        {
            if (!_isExpanded)
            {
                await addressFrame.TranslateTo(0, -450, 200, Easing.CubicInOut);
                addressFrame.Opacity = 1;
                ((Button)sender).Text = "Добавить";
                _isExpanded = true;
            }
            else
            {
                if (cityEntry.Text != "" && streetEntry.Text != "" && houseEntry.Text != "")
                {
                    Address address = new Address(cityEntry.Text, streetEntry.Text, houseEntry.Text, flatEntry.Text,true);
                    if (App.USER!=null)
                    {
                        App.USER.addresses.Add(address);
                        App.WriteToCache();
                    }
                    else
                    {
                        BasketPage.AddToPicker(address);
                    }
                    cityEntry.Text = "";
                    streetEntry.Text = "";
                    houseEntry.Text = "";
                    flatEntry.Text = "";
                    cityEntry.BackgroundColor = Color.WhiteSmoke;
                    streetEntry.BackgroundColor = Color.WhiteSmoke;
                    houseEntry.BackgroundColor = Color.WhiteSmoke;
                    AddAddress(address.GetAddress());                    
                    await addressFrame.TranslateTo(0, 0, 200, Easing.CubicInOut);
                    _isExpanded = false;
                    ((Button)sender).Text = "Добавить новый адрес";
                    addressFrame.Opacity = 0;
                }
                else
                {
                    if(cityEntry.Text=="")
                        cityEntry.BackgroundColor = Color.Red;
                    else
                        cityEntry.BackgroundColor = Color.WhiteSmoke;
                    if (streetEntry.Text == "")
                        streetEntry.BackgroundColor = Color.Red;
                    else
                        streetEntry.BackgroundColor = Color.WhiteSmoke;
                    if (houseEntry.Text == "")
                        houseEntry.BackgroundColor = Color.Red;
                    else
                        houseEntry.BackgroundColor = Color.WhiteSmoke;
                }
            }
        }  
        void AddAddress(Address add)
        {
            if (addressCnt == App.USER.addresses.Count)
            {
                addressCnt = 0;
                favAddressIndex = 0;
                addressList.Clear();
            }
            stack.Children.Add(new AddressItem(add.GetAddress()));
            addressList.Add(add.GetAddress());
            addressCnt++;
            if (add.IsFavorite)
            {
                ((Grid)((SwipeView)stack.Children[favAddressIndex]).Content).BackgroundColor = Color.White;
                favAddressIndex = addressCnt - 1;
                ((Grid)((SwipeView)stack.Children[favAddressIndex]).Content).BackgroundColor = Color.LimeGreen;
            }
        }
        private void AddAddress(string label)
        {
            stack.Children.Add(new AddressItem(label));
            addressList.Add(label);
            addressCnt++;

            ((Grid)((SwipeView)stack.Children[favAddressIndex]).Content).BackgroundColor = Color.White;
            favAddressIndex = addressCnt - 1;
            ((Grid)((SwipeView)stack.Children[favAddressIndex]).Content).BackgroundColor = Color.LimeGreen;
            if (App.USER != null)
            {
                for (int i = 0; i < App.USER.addresses.Count - 1; i++)
                    App.USER.addresses[i].IsFavorite = false;
            }
        }
        public static void OnDeleteInvoked(object sender, EventArgs e)
        {
            string s = ((Label)((Grid)((SwipeView)((SwipeItems)((SwipeItem)sender).Parent).Parent).Content).Children[0]).Text;
            for (int i=0;i<addressList.Count;i++)
            {
                if (s == addressList[i])
                {
                    stack.Children.RemoveAt(i);
                    addressList.RemoveAt(i);
                    App.USER.addresses.RemoveAt(i);
                    addressCnt--;
                    break;
                }
            }
        }
        public static void OnFavoriteInvoked(object sender, EventArgs e)
        {
            string s = ((Label)((Grid)((SwipeView)((SwipeItems)((SwipeItem)sender).Parent).Parent).Content).Children[0]).Text;
            foreach(string address in addressList)
            {
                if (s == address)
                {
                    ((Grid)((SwipeView)stack.Children[favAddressIndex]).Content).BackgroundColor = Color.White;
                    App.USER.addresses[favAddressIndex].IsFavorite = false;
                    favAddressIndex = addressList.IndexOf(address);
                    App.USER.addresses[favAddressIndex].IsFavorite = true;
                    ((Grid)((SwipeView)stack.Children[favAddressIndex]).Content).BackgroundColor = Color.LimeGreen;
                    break;
                }
            }
        }
        async void SwipeGestureRecognizer_SwipedDown(System.Object sender, Xamarin.Forms.SwipedEventArgs e)
        {
            if (_isExpanded)
            {
                await addressFrame.TranslateTo(0, 0, 200, Easing.CubicInOut);
                _isExpanded = false;
                addressFrame.Opacity = 0;
            }
        }
        public class AddressItem : SwipeView
        {
            SwipeItem favSwipeItem;
            SwipeItem delSwipeItem;
            public bool favourite = true;

            public AddressItem(string label)
            {
                favSwipeItem = new SwipeItem
                {
                    Text = "Основной",
                    IconImageSource = "icon_Wfavourite.png",
                    BackgroundColor = Color.LimeGreen
                };
                favSwipeItem.Invoked += AddressListPage.OnFavoriteInvoked;

                delSwipeItem = new SwipeItem
                {
                    Text = "Удалить",
                    IconImageSource = "icon_Bdelete.png",
                    BackgroundColor = Color.Red
                };
                delSwipeItem.Invoked += AddressListPage.OnDeleteInvoked;

                List<SwipeItem> swipeItems = new List<SwipeItem>() { favSwipeItem, delSwipeItem };

                Grid grid = new Grid
                {
                    HeightRequest = 45,
                    WidthRequest = 320,
                    BackgroundColor = Color.White,
                    Padding = 5
                };
                grid.Children.Add(new Label
                {
                    Text = label,
                    TextColor = Color.Black,
                    FontSize = 16,
                    VerticalOptions = LayoutOptions.Center
                });
                RightItems = new SwipeItems(swipeItems);
                Content = grid;

            }
            
        }
    }
}