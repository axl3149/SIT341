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
    public partial class StartPage : ContentPage
    {
        public static ZXingScannerPage Scanner = new ZXingScannerPage();

        string chemicalName = null;

        Label header = new Label();

        public static string[] chemicalNames = {
            "acid", "water", "dirt", "table", "sulfate", "cyanide", "sodium", "alocohol",
        };

        //If we ever need multiple elements per list element, use Cell subclasses
        public TextCell[] textCells = new TextCell[chemicalNames.Length];

        bool textFound = false;

        public static ListView mainList = new ListView();

        public static List<string> favourites = new List<string>();

        public static WebView webView = new WebView();

        public static string chosenChemical;

        public StartPage()
        {
            InitializeComponent();

            header.Text = "Search";
            header.TextColor = Color.GhostWhite;
            header.FontSize = 32;
            header.HorizontalOptions = LayoutOptions.CenterAndExpand;

            mainList.Header = header;
            mainList.ItemsSource = chemicalNames;
            mainList.SeparatorColor = Color.GhostWhite;
            mainList.HorizontalOptions = LayoutOptions.Center;

            mainList.ItemTapped += async (sender, e) =>
            {
                chosenChemical = e.Item.ToString();
                await Navigation.PushAsync(new ResultsPage());
            };

            //webView.Source = "https://vhost2.intranet-sites.deakin.edu.au/scripts/RiskAssessment.php?ID=13902";
            //Content = webView;
            Content = mainList;
        }
        
        //For QR camera scanning focus
        public void ScannerFocus()
        {
            while (Scanner.Result == null)
            {
                System.Threading.Thread.Sleep(2000);
                Scanner.AutoFocus();
            }
        }

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
                    //Jason's idea of displaying information via this link
                    //webView.Source = "https://vhost2.intranet-sites.deakin.edu.au/scripts/RiskAssessment.php?ID=" + Scanner.Result.Text;
                    webView.Source = "https://vhost2.intranet-sites.deakin.edu.au/scripts/RiskAssessment.php?ID=13902";
                    Content = webView;

                    await Navigation.PopAsync();
                    await DisplayAlert("Chemical ID: ", result.Text, "OK");
                });
            };
        }

        public void Search(object sender, EventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;

            chemicalName = searchBar.Text;

            textFound = false;

            List<string> foundNames = new List<string>();

            if (chemicalName != null)
            {
                for (int i = 0; i < chemicalNames.Length; i++)
                {
                    if (chemicalNames[i].Contains(chemicalName.ToLower()))
                    {
                        foundNames.Add(chemicalNames[i]);
                        textFound = true;
                    }
                }

                mainList.ItemsSource = foundNames;
                Content = mainList;
            }

            if (textFound == false)
            {
                DisplayAlert("Search Results", "No chemical results found.", "Try again");
            }

            header.Text = "Search";
            mainList.Header = header;

            textFound = false;
        }

        async void Result(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            chosenChemical = b.Text;
            await Navigation.PushAsync(new ResultsPage());
        }

        public void ShowFavourites(object sender, EventArgs e)
        {            
            if(favourites.Count == 0)
            {
                DisplayAlert("Favourites", "No favourites found", "OK");
                return;
            }

            header.Text = "Favourites";
            mainList.Header = header;
            mainList.ItemsSource = favourites;
            Content = mainList;
        }
    }
}