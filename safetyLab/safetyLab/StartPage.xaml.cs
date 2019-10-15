//Copyright by Piumi 2019

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
        public static ZXingScannerPage ScannerPage = new ZXingScannerPage();

        public static ContentPage resultsContent = new ContentPage();
        public static ContentPage favouritesContent = new ContentPage();
        public static TabbedPage favouritesAndRecentsTabbed = new TabbedPage();
        public static ContentPage recentsContent = new ContentPage();
        public static ContentPage contactContent = new ContentPage();

        public static string chemicalID = null;
        public static string searchBarValue;

        public static ListView mainList = new ListView();
        public static ListView favouritesList = new ListView();
        public static ListView recentsList = new ListView();

        public static List<string> favourites = new List<string>();
        public static List<string> recents = new List<string>();

        public static string chosenChemical;

        public StartPage()
        {
            InitializeComponent();

            favouritesList.ItemTapped += async (sender, e) =>
            {
                chosenChemical = e.Item.ToString();
                await Navigation.PushAsync(new ResultsPage());
            };

            recentsList.ItemTapped += async (sender, e) =>
            {
                chosenChemical = e.Item.ToString();
                await Navigation.PushAsync(new ResultsPage());
            };

            //FAVOURITES
            favouritesList.ItemsSource = favourites;
            favouritesContent.Title = "Favourites";
            favouritesContent.Content = favouritesList;

            //RECENTS
            recentsContent.Title = "Recents";
            recentsContent.Content = recentsList;

            favouritesAndRecentsTabbed.Title = "Favourites & Recents";
            favouritesAndRecentsTabbed.Children.Add(favouritesContent);
            favouritesAndRecentsTabbed.Children.Add(recentsContent);

            //CONTACTS
            contactContent.Title = "Contacts";

            StackLayout contactStack = new StackLayout();
            Button security = new Button { Text = "Deakin Security", HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center, FontSize = 24, BackgroundColor = Color.FromRgb(66, 175, 178),
                TextColor = Color.White
            };
            security.Clicked += (sender, e) => SecurityClicked();
            contactStack.Children.Add(security);

            Button emergency = new Button { Text = "Emergency Service (000) ", HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center, FontSize = 24, BackgroundColor = Color.FromRgb(66, 175, 178),
                TextColor = Color.White
            };
            emergency.Clicked += (sender, e) => EmergencyClicked();
            contactStack.Children.Add(emergency);

            Button medical = new Button { Text = "Deakin Medical (Building B)", HorizontalOptions = LayoutOptions.Fill,
                BackgroundColor = Color.FromRgb(66, 175, 178), VerticalOptions = LayoutOptions.Center, FontSize = 24,
                TextColor = Color.White
            };
            medical.Clicked += (sender, e) => MedicalClicked();
            contactStack.Children.Add(medical);

            contactContent.Content = contactStack;
        }

        //For QR camera scanning focus
        public void ScannerFocus()
        {
            while (ScannerPage.Result == null)
            {
                Thread.Sleep(2000);
                ScannerPage.AutoFocus();
            }
        }


        public void SearchBarValue(object sender, EventArgs e)
        {
            SearchBar search = sender as SearchBar;
            searchBarValue = search.Text;
        }


        public async void SearchButton(object sender, EventArgs e)
        {
            if(searchBarValue == null || searchBarValue == "" || searchBarValue == " ")
            {
                await DisplayAlert("Search", "Enter a chemical ID to search", "OK");
                return;
            }

            AddToRecents();
            await Navigation.PushAsync(new ResultsPage());
        }


        public async void FavouritesButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(favouritesAndRecentsTabbed);
        }


        public async void RecentsButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(recentsContent);
        }


        public async void ContactsButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(contactContent);
        }


        public async void Scan(object sender, EventArgs e)
        {
            ScannerPage = new ZXingScannerPage();
            await Navigation.PushAsync(ScannerPage);

            Thread focusThread = new Thread(ScannerFocus);
            focusThread.Start();

            ScannerPage.OnScanResult += (result) =>
            {
                ScannerPage.IsScanning = false;
                ResultsPage.scannedChemicalID = result.Text;

                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PushAsync(new ResultsPage());
                    await DisplayAlert("Chemical ID: ", result.Text, "OK");
                });
            };
        }


        public void Search(object sender, EventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            chemicalID = searchBar.Text;
            ResultsPage.scannedChemicalID = chemicalID;
        }


        public void AddToRecents()
        {
            string recent = chemicalID;

            bool foundChemical = false;
            int foundIndex = 0;

            if (recent != null)
            {
                for (int i = 0; i < recents.Count; i++)
                {
                    if (recents[i] != recent)
                    {
                        foundChemical = false;
                        foundIndex = i;
                    }
                    else if (recents[i] == recent)
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
                if (recents.Count > maxRecents)
                {
                    recents.RemoveAt(0);
                }

                recentsList.ItemsSource = null;
                recentsList.ItemsSource = recents;
            }
        }


        async void SecurityClicked()
        {
            bool res = await DisplayAlert("Call", "Call Burwood Deakin security?", "Yes", "No");
            if (res)
            {
                Device.OpenUri(new Uri("tel:1800 062 579"));
            }
        }


        async void EmergencyClicked()
        {
            bool res = await DisplayAlert("Call", "Call emergency services?", "Yes", "No");
            if (res)
            {
                Device.OpenUri(new Uri("tel:000"));
            }
        }


        async void MedicalClicked()
        {
            bool res = await DisplayAlert("Call", "Call Burwood medical services?", "Yes", "No");
            if (res)
            {
                Device.OpenUri(new Uri("tel:9244 6300"));
            }
        }


        async void HospitalClicked()
        {
            bool res = await DisplayAlert("Call", "Call Box Hill hospital?", "Yes", "No");
            if (res)
            {
                Device.OpenUri(new Uri("tel:9895 3333"));
            }
        }
    }
}