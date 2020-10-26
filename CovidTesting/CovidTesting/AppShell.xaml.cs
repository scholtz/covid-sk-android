using System;
using System.Collections.Generic;
using CovidTesting.ViewModels;
using CovidTesting.Views;
using Xamarin.Forms;

namespace CovidTesting
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(OcrAppPage), typeof(OcrAppPage));
        }

    }
}
