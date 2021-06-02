using Android.App;
using BorschISmetanka.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BorschISmetanka.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [Activity(NoHistory =true)]
    public partial class Registration : ContentPage
    {
        public Registration()
        {
            InitializeComponent();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            var existingPages = Navigation.NavigationStack.ToList();
            Navigation.RemovePage(existingPages[1]);
        }
        private async void Registration_Clicked(object sender, EventArgs e)
        {
            if (CheckData())
            {
                User newUser = new User
                {
                    name = nameEntry.Text,
                    e_mail = emailEntry.Text,
                    number = numEntry.Text,
                    bday = bdayPicker.Date,
                    psw=pswEntry.Text,
                    bonusCnt=0
                };                
                var json = await App.getAsync(App.ip+"Checker/" + newUser.number,7);
                var jsonObject = JsonConvert.DeserializeObject(json);
                if (jsonObject.ToString() == "1")
                {
                    //newUser.id = 1;
                    await App.postAsync(App.ip+"Customers", JsonConvert.SerializeObject(newUser));
                    json = await App.getAsync(App.ip + "Checker/id/"+newUser.number,7);
                    jsonObject= JsonConvert.DeserializeObject(json);
                    newUser.id = Convert.ToInt32(jsonObject);
                    App.Authorization(newUser);
                    await Navigation.PushAsync(new ProfilePage());
                }
                else
                {
                    errorLabel.Text = "Пользователь с такими данными уже существует";
                }
                //App.Authorization(newUser);
                //await Shell.Current.GoToAsync("Profile");
            }
            
            
        }      

        private bool CheckData()
        {
            errorLabel.Text = "";
            if (String.IsNullOrEmpty(nameEntry.Text))
            {
                errorLabel.Text = "Введите имя";
                return false;
            }
            if (String.IsNullOrEmpty(numEntry.Text))
            {
                errorLabel.Text = "Введите номер";
                return false;
            }
            if (numEntry.Text.Length != 11||!CheckNumValid(numEntry.Text))
            {
                errorLabel.Text = "Неверный формат номера";
                return false;
            }
            if (String.IsNullOrEmpty(pswEntry.Text))
            {
                errorLabel.Text = "Придумайте пароль";
                return false;
            }                
            
            if (String.IsNullOrEmpty(emailEntry.Text))
            {
                errorLabel.Text = "Введите электронную почту";
                return false;
            }
            if (!CheckEmailValid(emailEntry.Text))
            {
                errorLabel.Text = "Неверный формат электронной почты";
                return false;
            }
            if (DateTime.Now.Year - bdayPicker.Date.Year < 14)
            {
                errorLabel.Text = "Введите корректную дату рождения";
                return false;
            }
                return true;
        }
        static public bool CheckEmailValid(string email)
        {
            string pattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";
            return Regex.IsMatch(email, pattern);
        }
        static public bool CheckNumValid(string num)
        {
            string pattern = @"^[7-8]{1}[9]{1}\d{9}";
            return Regex.IsMatch(num, pattern);
        }
        private void numEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Entry entry = (Entry)sender;
            //if (entry.Text == "+")
            //    entry.Text = "";
            //else
            //if (entry.Text.Length == 1)
            //    entry.Text = "8";
        }
        private void nameEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (((Entry)sender).Text.Length == 1)
                ((Entry)sender).Text=((Entry)sender).Text.ToUpper();
        }
    }
}