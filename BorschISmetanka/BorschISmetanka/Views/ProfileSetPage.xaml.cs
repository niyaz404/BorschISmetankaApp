using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            userBday.Text = App.USER.bday.ToString("dd.MM.yyyy");
            switchSubscribe.IsToggled = App.USER.subscribe;
        }
        private async void BackToProfilePage(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        private async void OkBtn_Click(object sender, EventArgs e)
        {
            if (userNum.Text == App.USER.number && userEmail.Text == App.USER.e_mail)
                await Navigation.PopAsync();
            else
            {
                if (CheckData())
                {
                    //var json = await App.getAsync(App.ip + "Checker/"+ userNum.Text + "/" + "rrr");
                    var json = await App.getAsync(App.ip + "Checker/check/" + userEmail.Text + "/" + userNum.Text,5);
                    var response = JsonConvert.DeserializeObject<int>(json);
                    if ((int)response == 0)
                    {
                        App.USER.number = userNum.Text;
                        App.USER.e_mail = userEmail.Text;
                        App.USER.subscribe = switchSubscribe.IsToggled;
                        App.WriteToCache();
                        await App.putAsync(App.ip + "Customers/" + App.USER.id, JsonConvert.SerializeObject(App.USER));
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        if ((int)response == -1 && App.USER.number != userNum.Text)
                            await DisplayAlert("Этот номер уже занят", "", "Ок");
                        else
                        {
                            if (App.USER.e_mail != userEmail.Text)
                                await DisplayAlert("Эта почта уже занята", "", "Ок");
                        }
                    }
                }
            }
        }
        private bool CheckData()
        {            
            if (String.IsNullOrEmpty(userNum.Text))
            {
                DisplayAlert("Введите номер", "", "Ок");
                return false;
            }
            if (userNum.Text.Length != 11)
            {
                DisplayAlert("Неверный формат номера", "", "Ок");
                return false;
            }
            string pattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";
            if (String.IsNullOrEmpty(userEmail.Text))
            {
                DisplayAlert("Введите электронную почту", "", "Ок");
                return false;
            }
            if (!Regex.IsMatch(userEmail.Text, pattern))
            {
                DisplayAlert("Неверный формат электронной почты", "", "Ок");
                return false;
            }
            return true;
        }
    }
}