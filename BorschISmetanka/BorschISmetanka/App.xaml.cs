using Android.Webkit;
using BorschISmetanka.Models;
using BorschISmetanka.Services;
using BorschISmetanka.Views;
using Newtonsoft.Json;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BorschISmetanka
{
    public partial class App : Application
    {

        public static string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public static string cacheFolderPath = folderPath + "/cache";
        public static string basketCachePath = cacheFolderPath + "/BasketCache.json";
        public static string userCachePath = cacheFolderPath + "/UserCache.json";
        string userJson = "{\r\n  \"addresses\": [\r\n    {\r\n      \"city\": \"Уфа\",\r\n      \"street\": \", Мингажева\",\r\n      \"house\": \", 160/1\",\r\n      \"flat\": \", 918\",\r\n      \"IsFavorite\": true\r\n    },\r\n    {\r\n      \"city\": \"Уфа\",\r\n      \"street\": \", Мингажева\",\r\n      \"house\": \", 160\",\r\n      \"flat\": \", 777\",\r\n      \"IsFavorite\": false\r\n    }\r\n  ],\r\n  \"name\": \"Нияз\",\r\n  \"bday\": \"14.07.2000\",\r\n  \"e_mail\": \"nijaz_galiev@mail.ru\",\r\n  \"number\": \"89174734313\",\r\n  \"subscribe\": true\r\n}";
        static public User USER; // наш пользователь
        public App()
        {
            InitializeComponent();
            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();

        }

        protected override void OnStart()
        {           
            CreateCacheFile();
            SetUserInfo();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
        static void CreateCacheFile()
        {
            if (!Directory.Exists(cacheFolderPath))
                Directory.CreateDirectory(cacheFolderPath);
            using (FileStream stream = File.Create(basketCachePath));
            using (FileStream stream = File.Create(userCachePath)) ;
        }
        void SetUserInfo()
        {
            /*
             * Тут читаем Json файл из кэша, если он не пустой, 
             * берем информацию о пользователе из него,
             * если пустой переходим на страницу авторизации
            */
            USER = JsonConvert.DeserializeObject<User>(userJson);//КАК БУДТО БЫ кэш не пустой, userJson - полученная инфа о пользователе
        }
    }
}
