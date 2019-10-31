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
    public partial class AuditPage : ContentPage
    {
        public static string scannedChemicalID;

        public ZXingScannerPage ScannerPageAudit;

        public WebView webView;

        public AuditPage()
        {
            InitializeComponent();

            webView = new WebView();
            webView.Source = "https://4705c20c-72e2-4e54-ba1a-b9449fb9fc5d.htmlpasta.com/";
            Content = webView;
        }

        public void Search(object sender, EventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            scannedChemicalID = searchBar.Text;
            //TODO: webView.Source = "audit.html";
        }


        public async void Scan(object sender, EventArgs e)
        {
            ScannerPageAudit = new ZXingScannerPage();
            ScannerPageAudit.Title = "Scanning for Audit...";
            await Navigation.PushAsync(ScannerPageAudit);

            ScannerPageAudit.OnScanResult += (result) =>
            {
                ScannerPageAudit.IsScanning = false;
                ResultsPage.scannedChemicalID = result.Text;

                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PushAsync(new AuditPage());
                    await DisplayAlert("Chemical ID: ", result.Text, "OK");
                });
            };
        }
    }
}