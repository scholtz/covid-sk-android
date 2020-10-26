using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Tesseract.Droid;
using Tesseract;
using XLabs.Ioc;
using XLabs.Ioc.TinyIOC;
using XLabs.Platform.Device;
using TinyIoC;

namespace CovidTesting.Droid
{
    [Activity(Label = "CovidTesting", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);


            //ITesseractApi api = new TesseractApi(this.BaseContext, AssetsDeployment.OncePerVersion);
            var container = TinyIoCContainer.Current;
            container.Register<IDevice>(AndroidDevice.CurrentDevice);
            container.Register<ITesseractApi>((cont, parameters) =>
            {
                return new TesseractApi(ApplicationContext, Tesseract.Droid.AssetsDeployment.OncePerInitialization);
            });
            Resolver.SetResolver(new TinyResolver(container));

            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}