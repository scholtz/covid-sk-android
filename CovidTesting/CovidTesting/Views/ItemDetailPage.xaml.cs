using System.ComponentModel;
using Xamarin.Forms;
using CovidTesting.ViewModels;

namespace CovidTesting.Views
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