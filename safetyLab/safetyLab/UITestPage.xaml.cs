using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
    
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace safetyLab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UITestPage : ContentPage
    {
        WebView webView;
        public static ZXingScannerPage Scanner = new ZXingScannerPage();

        string chemicalName = null;
        public string[] chemicalNames = { "acid", "water", "dirt", "table", "sulfate", "cyanide", "sodium", "alocohol" };
        bool textFound = false;

        List<Button> results = new List<Button>();

        ContentPage SearchContent;

        public UITestPage()
        {
            InitializeComponent();
            webView = new WebView();
            webView.Source = "https://vhost2.intranet-sites.deakin.edu.au/";
            Content = webView;
        }

        /*public void ScannerFocus()
        {
            while (Scanner.Result == null)
            {
                System.Threading.Thread.Sleep(2000);
                Scanner.AutoFocus();
            }
        }*/

        public async void Scan(object sender, EventArgs e)
        {
            Scanner = new ZXingScannerPage();
            await Navigation.PushAsync(Scanner);

            //Thread focusThread = new Thread(ScannerFocus);
            //focusThread.Start();

            Scanner.OnScanResult += (result) =>
            {

                Scanner.IsScanning = false;

                Device.BeginInvokeOnMainThread(async () =>
                {
                    //webView.Source = "https://vhost2.intranet-sites.deakin.edu.au/scripts/RiskAssessment.php?ID=" + Scanner.Result.Text;
                    webView.Source = "https://vhost2.intranet-sites.deakin.edu.au/";
                    await Navigation.PopAsync();
                    await DisplayAlert("Chemical ID: ", result.Text, "OK");
                });
            };
        }

        public void Search(object sender, EventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;

            chemicalName = searchBar.Text;

            results.Clear();

            SearchContent = new ContentPage();

            StackLayout stack = new StackLayout();

            textFound = false;

            if (chemicalName != null)
            {
                int resultsIndex = 0;

                for (int i = 0; i < chemicalNames.Length; i++)
                {
                    if (chemicalNames[i].Contains((chemicalName)))
                    {
                        Button result = new Button();
                        result.BackgroundColor = Color.White;
                        result.Text = chemicalNames[i];
                        result.Clicked += (send, args) => Result(send, args); //Performance problems?

                        results.Add(result);

                        stack.Children.Add(results[resultsIndex]);
                        resultsIndex++;

                        textFound = true;
                    }
                }

                SearchContent.Content = stack;
                Content = SearchContent.Content;
            }

            if (textFound == false)
            {
                DisplayAlert("Search Results", "No chemical results found.", "Try again");
            }

            textFound = false;
        }

        public void Result(object sender, EventArgs e)
        {

        }
    }
}