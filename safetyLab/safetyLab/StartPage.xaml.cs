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
        WebView webView;
        public static ZXingScannerPage Scanner = new ZXingScannerPage();

        string chemicalName = null;
        public static string[] chemicalNames = {
            "acid", "water", "dirt", "table", "sulfate", "cyanide", "sodium", "alocohol",
            "acid", "water", "dirt", "table", "sulfate", "cyanide", "sodium", "alocohol",
            "acid", "water", "dirt", "table", "sulfate", "cyanide", "sodium", "alocohol"
        };

        bool textFound = false;

        public static ListView mainList = new ListView
        {
            ItemsSource = chemicalNames,
        };

        List<Button> results = new List<Button>();

        public static List<string> favourites = new List<string>();
        public static string chosenChemical;

        ContentPage SearchContent;
        public static StackLayout stack = new StackLayout();

        public StartPage()
        {
            InitializeComponent();
            webView = new WebView();
            webView.Source = "https://vhost2.intranet-sites.deakin.edu.au/";
            //Content = webView;

          
            mainList.ItemTapped += async (sender, e) =>
            {
                chosenChemical = e.Item.ToString();
                await Navigation.PushAsync(new ResultsPage());
            };
            Content = mainList;
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
                    webView.Source = "https://www.pokemon.com/us/";

                    stack.Children.Clear();

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

            stack.Children.Clear();

            Label title = new Label
            {
                Text = "SEARCH RESULTS",
                HorizontalOptions = LayoutOptions.Center
            };

            stack.Children.Add(title);

            textFound = false;

            List<string> foundNames = new List<string>();

            if (chemicalName != null)
            {
                int resultsIndex = 0;

                for (int i = 0; i < chemicalNames.Length; i++)
                {
                    if (chemicalNames[i].Contains(chemicalName.ToLower()))
                    {
                        //For old button layout
                        /*Button result = new Button();
                        result.BackgroundColor = Color.White;
                        result.Text = chemicalNames[i];
                        result.Clicked += (send, args) => Result(send, args); //Performance problems?

                        results.Add(result);
                        
                        stack.Children.Add(results[resultsIndex]);*/

                        //For new listview layout

                        foundNames.Add(chemicalNames[i]);

                        resultsIndex++;

                        textFound = true;
                    }
                }

                mainList.ItemsSource = foundNames;

                //SearchContent.Content = stack;
                SearchContent.Content = mainList;
                Content = SearchContent.Content;
            }

            if (textFound == false)
            {
                DisplayAlert("Search Results", "No chemical results found.", "Try again");
            }

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
            stack.Children.Clear();

            Label title = new Label();
            title.Text = "FAVOURITES";
            title.HorizontalOptions = LayoutOptions.Center;

            stack.Children.Add(title);
            
            for(int i = 0; i < favourites.Count; i++)
            {
                /*Button favourite = new Button();
                favourite.BackgroundColor = Color.White;
                favourite.Text = favourites[i].Text;
                favourite.Clicked += (send, args) => Result(send, args);

                stack.Children.Add(favourite);*/

                mainList.ItemsSource = favourites;
                Content = mainList;
            }
        }
    }
}