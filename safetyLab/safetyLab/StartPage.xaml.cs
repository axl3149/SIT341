﻿using System;
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
    public partial class StartPage : TabbedPage
    {

        public static ZXingScannerPage Scanner = new ZXingScannerPage();

        public static ContentPage resultsContent = new ContentPage();
        public static ContentPage favouritesContent = new ContentPage();
        public static ContentPage recentsContent = new ContentPage();


        string chemicalName = null;

        Label header = new Label();

        //Temp chemical names. Need to next link up to TRACIE SQL calls for 'Search'
        public static string[] chemicalNames = {
            "acid", "water", "dirt", "table", "sulfate", "cyanide", "sodium", "alocohol",
        };


        bool textFound = false;

        public static ListView mainList = new ListView();
        public static ListView favouritesList = new ListView();
        public static ListView recentsList = new ListView();

        public static List<string> favourites = new List<string>();
        public static List<string> recents = new List<string>();

        public static WebView webView = new WebView();

        public static string chosenChemical;

        public StartPage()
        {
            InitializeComponent();
            Children.Clear();
            
            mainList.ItemsSource = chemicalNames;
            mainList.SeparatorColor = Color.Black;
            mainList.HorizontalOptions = LayoutOptions.Center;

            mainList.ItemTapped += async (sender, e) =>
            {
                chosenChemical = e.Item.ToString();
                AddToRecents();
                await Navigation.PushAsync(new ResultsPage());
            };


            favouritesList.ItemTapped += async (sender, e) =>
            {
                chosenChemical = e.Item.ToString();
                await Navigation.PushAsync(new ResultsPage());
            };

            resultsContent.Content = mainList;
            resultsContent.Title = "Search";
            Children.Add(resultsContent);

            favouritesList.ItemsSource = favourites;
            favouritesList.SeparatorColor = Color.Black;
            favouritesList.HorizontalOptions = LayoutOptions.Center;

            Scanner.Title = "Scan";
            Children.Add(Scanner);

            favouritesContent.Title = "Favourites";
            favouritesContent.Content = favouritesList;
            Children.Add(favouritesContent);

            recentsContent.Title = "Recents";
            recentsContent.Content = recentsList;
            Children.Add(recentsContent);
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
                    //webView.Source = "https://vhost2.intranet-sites.deakin.edu.au/scripts/RiskAssessment.php?ID=13902";
                    //Content = webView;

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
            }

            if (textFound == false)
            {
                DisplayAlert("Search Results", "No chemical results found.", "Try again");
            }


            textFound = false;

            this.CurrentPage = resultsContent;
        }

        public void AutoSearch(object sender, EventArgs e)
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
            }

            textFound = false;

            this.CurrentPage = resultsContent;
        }

        public void AddToRecents()
        {
            string recent = chosenChemical;

            bool foundChemical = false;
            int foundIndex = 0;

            for (int i = 0; i < recents.Count; i++)
            {
                if (recents[i] != chosenChemical)
                {
                    foundChemical = false;
                    foundIndex = i;
                }
                else if (recents[i] == chosenChemical)
                {
                    foundChemical = true;
                    foundIndex = i;
                    break;
                }
            }

            if (foundChemical == false)
            {
                recents.Add(recent);
            }

            int maxRecents = 10;
            if(recents.Count > maxRecents)
            {
                recents.RemoveAt(0);
            }

            recentsList.ItemsSource = null;
            recentsList.ItemsSource = recents;
        }
    }
}