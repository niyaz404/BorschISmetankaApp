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
    public partial class ProfileSetPage : ContentPage
    {
        public ProfileSetPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            userName.Text = App.USER.name;
            userEmail.Text = App.USER.e_mail;
            userNum.Text = App.USER.number;
            userBday.Text = App.USER.bday;
            switchSubscribe.IsToggled = App.USER.subscribe;
        }
        private async void BackToProfilePage(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        private async void OkBtn_Click(object sender, EventArgs e)
        {
            App.USER.number = userNum.Text;
            App.USER.e_mail = userEmail.Text;
            App.USER.subscribe = switchSubscribe.IsToggled;
            await Navigation.PopAsync();
        }
    }
}