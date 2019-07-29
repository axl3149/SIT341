using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace safetyLab
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SearchAndScanPage : ContentPage
	{
		public SearchAndScanPage ()
		{
			InitializeComponent ();
		}

        async void Search(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SearchPage());
        }

        async void Scan(object sender, EventArgs e) //QR Function
        {
            var Scanner = new ZXingScannerPage();

            Scanner.OnScanResult += (result) =>
            {
                Scanner.IsScanning = false;

                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();
                    await DisplayAlert("Scanned Barcode", result.Text, "OK");
                });
            };

            await Navigation.PushAsync(Scanner);
        }
	}
}