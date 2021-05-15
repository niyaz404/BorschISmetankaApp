using BorschISmetanka.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace BorschISmetanka.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}