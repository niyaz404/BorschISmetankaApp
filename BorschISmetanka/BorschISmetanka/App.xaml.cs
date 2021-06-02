using Android.Webkit;
using BorschISmetanka.Models;
using BorschISmetanka.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
        public static string ordersCachePath = cacheFolderPath + "/OrdersCache.json";
        public static bool token = false;
        string userJson = "{\r\n  \"addresses\": [\r\n    {\r\n      \"city\": \"Уфа\",\r\n      \"street\": \", Мингажева\",\r\n      \"house\": \", 160/1\",\r\n      \"flat\": \", 918\",\r\n      \"IsFavorite\": true\r\n    },\r\n    {\r\n      \"city\": \"Уфа\",\r\n      \"street\": \", Мингажева\",\r\n      \"house\": \", 160\",\r\n      \"flat\": \", 777\",\r\n      \"IsFavorite\": false\r\n    }\r\n  ],\r\n  \"name\": \"Нияз\",\r\n  \"bday\": \"14.07.2000\",\r\n  \"e_mail\": \"nijaz_galiev@mail.ru\",\r\n  \"number\": \"89174734313\",\r\n  \"subscribe\": true\r\n}";
        static public User USER; // наш пользователь
        public static readonly string GOOGLE_MAP_API_KEY = "AIzaSyCkjjziaIOJFAA5TPF8XnBT0ZDRQiFINjE";
        public static string ip = "http://192.168.0.102:3000/api/";        
        public App()
        {
            InitializeComponent();
            //DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }
        protected override void OnStart()
        {           
            CreateCacheFile();
            ReadFromCache();            
        }

        protected override void OnSleep()
        {
            RefreshCache();
        }

        protected override void OnResume()
        {
            ReadFromCache();
        }
        static void CreateCacheFile()
        {
            if (!Directory.Exists(cacheFolderPath))
                Directory.CreateDirectory(cacheFolderPath);
            if(!File.Exists(basketCachePath))
                using (FileStream stream = File.Create(basketCachePath));
            if (!File.Exists(userCachePath))
                using (FileStream stream = File.Create(userCachePath));
            if (!File.Exists(ordersCachePath))
                using (FileStream stream = File.Create(ordersCachePath)) ;
        }        
        void ReadFromCache()
        {
            using (StreamReader reader = new StreamReader(File.OpenRead(userCachePath)))
            {
                string userJson = reader.ReadToEnd();
                if (!String.IsNullOrEmpty(userJson))
                {
                    USER = JsonConvert.DeserializeObject<User>(userJson);
                }
            }
        }
        public static void WriteToCache()
        {
            using (StreamWriter writer = new StreamWriter(File.OpenWrite(userCachePath)))
            {
                string newUserCache = JsonConvert.SerializeObject(USER);
                writer.Write(newUserCache);
                writer.Close();
            };
        }
        private void RefreshCache()
        {
            using (StreamWriter writer = new StreamWriter(File.Create(userCachePath)))
            {
                string user = JsonConvert.SerializeObject(USER);
                writer.Write(user);
                writer.Close();
            }
        }
        public static async void Authorization(User newUser)
        {
                USER = newUser;
                WriteToCache(); 
        }        
        public static async Task<string> getAsync(string uri,int timeout) //get запрос к БД
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = delegate { return true; };
            HttpClient httpClient = new HttpClient(handler);
            httpClient.Timeout = TimeSpan.FromSeconds(timeout);
            var response = await httpClient.GetStringAsync(uri);
            return await Task.Run(() => response.ToString());
        }        
        public static async Task<string> postAsync(string uri, string data)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = delegate { return true; };
            var httpClient = new HttpClient(handler);
            httpClient.Timeout = TimeSpan.FromSeconds(10000);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(uri, content);
            response.EnsureSuccessStatusCode();
            var responseStr = await response.Content.ReadAsStringAsync();
            var result = await Task.Run(() => responseStr.ToString());
            return result;
        }
        public static async Task<string> putAsync(string uri, string data)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = delegate { return true; };
            var httpClient = new HttpClient(handler);
            httpClient.Timeout = TimeSpan.FromSeconds(10000);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync(uri, content);
            response.EnsureSuccessStatusCode();
            var responseStr = await response.Content.ReadAsStringAsync();
            var result = await Task.Run(() => responseStr.ToString());
            return result;
        }
    }
}
