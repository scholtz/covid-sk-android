using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services.Media;

namespace CovidTesting.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OcrAppPage : ContentPage
	{
		private readonly ITesseractApi tesseractApi;
		private readonly IDevice device;

		public OcrAppPage()
		{
			InitializeComponent();

			tesseractApi = Resolver.Resolve<ITesseractApi>();
			device = Resolver.Resolve<IDevice>();
		}

		public async void ChoosePictureClicked(object sender, EventArgs e)
		{
			ChoosePictureButton.Text = "Working...";
			TakePictureButton.IsEnabled = false;
			ChoosePictureButton.IsEnabled = false;

			if (!tesseractApi.Initialized)
			{
				await tesseractApi.Init("eng");
			}

			var photo = await SelectPicture();
			if (photo != null)
			{
				var imageBytes = new byte[photo.Source.Length];
				photo.Source.Position = 0;
				photo.Source.Read(imageBytes, 0, (int)photo.Source.Length);
				photo.Source.Position = 0;

				TakenImage.Source = ImageSource.FromStream(() => photo.Source);
				var tessResult = await tesseractApi.SetImage(imageBytes);
				if (tessResult)
				{
					RecognizedTextLabel.Text = tesseractApi.Text;
				}
			}

			ChoosePictureButton.Text = "...or choose from gallery";
			TakePictureButton.IsEnabled = true;
			ChoosePictureButton.IsEnabled = true;
		}

		public async void TakePictureClicked(object sender, EventArgs e)
		{
			TakePictureButton.Text = "Working...";
			TakePictureButton.IsEnabled = false;
			ChoosePictureButton.IsEnabled = false;

			if (!tesseractApi.Initialized)
			{
				await tesseractApi.Init("eng");
			}

			var photo = await TakePic();
			if (photo != null)
			{
				using (var memoryStream = new MemoryStream())
				{
					photo.GetStream().CopyTo(memoryStream);
					var data = memoryStream.ToArray();
					memoryStream.Position = 0;

					TakenImage.Source = ImageSource.FromStream(() => memoryStream);
					var tessResult = await tesseractApi.SetImage(data);
					if (tessResult)
					{
						RecognizedTextLabel.Text = tesseractApi.Text;
					}/**/
				}
			}

			TakePictureButton.Text = "Take a picture";
			TakePictureButton.IsEnabled = true;
			ChoosePictureButton.IsEnabled = true;
		}

		private Task<Plugin.Media.Abstractions.MediaFile> TakePic()
		{

			return CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
			{
				Directory = "Test",
				SaveToAlbum = true,
				CompressionQuality = 75,
				CustomPhotoSize = 50,
				PhotoSize = PhotoSize.MaxWidthHeight,
				MaxWidthHeight = 2000,
				DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Front
			});
			/*

			var mediaStorageOptions = new CameraMediaStorageOptions
			{
				DefaultCamera = CameraDevice.Rear,
				
			};
			var mediaFile = await device.MediaPicker.TakePhotoAsync(mediaStorageOptions);

			return mediaFile;
			/**/
		}

		private async Task<XLabs.Platform.Services.Media.MediaFile> SelectPicture()
		{
			var mediaFile = await device.MediaPicker.SelectPhotoAsync(new CameraMediaStorageOptions
			{
				DefaultCamera = XLabs.Platform.Services.Media.CameraDevice.Rear,
				MaxPixelDimension = 400
			});

			return mediaFile;
		}

	}
}