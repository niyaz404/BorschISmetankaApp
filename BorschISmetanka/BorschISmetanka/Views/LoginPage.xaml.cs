using Android.App;
using BorschISmetanka.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BorschISmetanka.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile), Activity(NoHistory =true)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();            
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (!CacheIsEmpty())
                await Shell.Current.GoToAsync("Profile");
        }
        private async void ToRegistrationPageBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Registration());
        }
        private async void Login()
        {
            errorLabel.Text = "";
            try
            {
                var json = await App.getAsync(App.ip + "Checker/" + numEntry.Text + "/" + pswEntry.Text,5);
                var jsonUser = JsonConvert.DeserializeObject<User>(json);
                if (jsonUser != null)
                {

                    App.Authorization(jsonUser);
                    numEntry.Text = "";
                    pswEntry.Text = "";
                    errorLabel.Text = "";
                    OrderPage.needToRefresh = true;
                    await Navigation.PushAsync(new ProfilePage());
                }
                else
                {
                    errorLabel.Text = "Неверный логин или пароль";
                }
            }
            catch(Exception ex)
            {
                errorLabel.Text = "Отсутствует подключение";
            }
        }
        private bool CacheIsEmpty()
        {
            string cache;
            using (StreamReader reader = new StreamReader(File.OpenRead(App.userCachePath)))
            {
               cache = reader.ReadToEnd();
                if (String.IsNullOrEmpty(cache)||cache=="null")
                    return true;                
            }
            var userJson = JsonConvert.DeserializeObject<User>(cache);
            App.Authorization(userJson);
            return false;
        }
        private void Login_Clicked(object sender, EventArgs e)
        {
            if (CheckData())
                Login();
        }

        private void numEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Entry entry = (Entry)sender;
            //if (entry.Text == "+")
            //    entry.Text = "";
            //else
            //{                
            //    if (entry.Text.Length == 1)
            //        entry.Text = "+7"+ entry.Text;
            //    else
            //        entry.Text = $"{entry.Text:+#(###)##-##-##}";
            //}
        }
        private bool CheckData()
        {            
            if (String.IsNullOrEmpty(numEntry.Text))
            {
                errorLabel.Text = "Введите номер телефона";
                return false;
            }
            if (numEntry.Text.Length != 11)
            {
                errorLabel.Text = "Неверный формат номера";
                return false;
            }
            if (String.IsNullOrEmpty(pswEntry.Text))
            {
                errorLabel.Text = "Введите пароль";
                return false;
            }
            errorLabel.Text = "";
            return true;
        }
    }
}