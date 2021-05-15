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
    public partial class ProfilePage : ContentPage
    {
        public Label userName;
        public Label bonusCnt;
        public Label addCnt;
        public static AddressListPage addressPage = new AddressListPage();
        static BonusPage bonusPage;
        static ProfileSetPage setPage;

        public ProfilePage()
        {            
            InitializeComponent();
            BuildPage();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            RefreshLabel();
        }
        void RefreshLabel()
        {
            addCnt.Text = App.USER.addresses.Count.ToString();
        }
        private void BuildPage()
        {
            userName = new Label
            {
                Text = App.USER.name,
                FontSize = 24,
                FontAttributes = FontAttributes.Bold,
                Padding = 10,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center
            };
            bonusCnt = new Label
            {
                Text = "0",
                FontSize = 16,
                TextColor = Color.LimeGreen,
                Padding = 10,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            addCnt = new Label
            {
                Text = App.USER.addresses.Count.ToString(),
                FontSize = 16,
                TextColor = Color.Black,
                Padding = 10,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            HeadGrid.Children.Add(userName);
            Grid.SetColumn(userName, 0);
            Grid.SetRow(userName, 0);
            bonusGrid.Children.Add(bonusCnt);
            Grid.SetColumn(bonusCnt, 1);
            Grid.SetRow(bonusCnt, 0);
            addressGrid.Children.Add(addCnt);
            Grid.SetColumn(addCnt, 1);
            Grid.SetRow(addCnt, 0);
            //addressPage = new AddressListPage();
            bonusPage = new BonusPage();
            setPage = new ProfileSetPage();
        }
        private async void setBtn_Click(object sender, EventArgs e)
        {            
            await Navigation.PushAsync(setPage);
        }
        private async void AddressBtn_Click(object sender, EventArgs e)
        {
            await Navigation.PushAsync(addressPage);            
        }
        private async void BonusBtn_Click(object sender, EventArgs e)
        {
            await Navigation.PushAsync(bonusPage);
        }
    }

}