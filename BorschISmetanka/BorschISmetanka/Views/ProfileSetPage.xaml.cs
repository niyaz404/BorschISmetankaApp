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
            this.userName.Text = App.USER.name;
            this.userEmail.Text = App.USER.e_mail;
            this.userNum.Text = App.USER.number;
            this.userBday.Text = App.USER.bday;
            this.switchSubscribe.IsToggled=App.USER.subscribe;
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