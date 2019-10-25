using System;
using System.Collections.Generic;
using System.Threading;
    
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace safetyLab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {
        //TODO: Look into putting into colours
        public static Color navBarColor = Color.FromRgb(133, 193, 233);

        public static ZXingScannerPage ScannerPage = new ZXingScannerPage();

        public static ContentPage resultsContent = new ContentPage();
        public static ContentPage favouritesContent = new ContentPage();
        public static ContentPage recentsContent = new ContentPage();
        public static ContentPage contactContent = new ContentPage();

        public static ListView mainList = new ListView();
        public static ListView favouritesList = new ListView();
        public static ListView recentsList = new ListView();

        public static List<string> favourites = new List<string>();
        public static List<string> recents = new List<string>();

        public static string chemicalID = null;
        public static string chosenChemical;

        public StartPage()
        {
            InitializeComponent();

            BackgroundColor = navBarColor;

            //Input for listviews
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

            //RECENTS
            recentsContent.Title = "Recently Searched Chemicals";
            recentsContent.Content = recentsList;

            //CONTACTS
            contactContent.Title = "Emergency Contacts";
            contactContent.BackgroundColor = navBarColor;

            //TODO: Could move below code in XAML later
            Grid contactGrid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star }
                },
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = GridLength.Star }
                }
            };

            StackLayout contactStack = new StackLayout();

            Button security = new Button { Text = "Deakin Security", HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill, FontSize = 24, BackgroundColor = navBarColor,
                CornerRadius = 30, BorderWidth = 5, BorderColor = Color.White,
                TextColor = Color.White
            };
            security.Clicked += (sender, e) => SecurityClicked();
            contactStack.Children.Add(security);
            contactGrid.Children.Add(security, 0, 0);

            Button emergency = new Button { Text = "Emergency Service (000) ", HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill, FontSize = 24, BackgroundColor = navBarColor,
                CornerRadius = 30,
                BorderWidth = 5,
                BorderColor = Color.White,
                TextColor = Color.White
            };
            emergency.Clicked += (sender, e) => EmergencyClicked();
            contactStack.Children.Add(emergency);
            contactGrid.Children.Add(emergency, 0, 1);

            Button medical = new Button { Text = "Poisons Hotline", HorizontalOptions = LayoutOptions.Fill,
                BackgroundColor = navBarColor, VerticalOptions = LayoutOptions.Fill, FontSize = 24,
                CornerRadius = 30,
                BorderWidth = 5,
                BorderColor = Color.White,
                TextColor = Color.White
            };
            medical.Clicked += (sender, e) => MedicalClicked();
            contactStack.Children.Add(medical);
            contactGrid.Children.Add(medical, 0, 2);

            contactContent.Content = contactGrid;
        }


        //For QR camera scanning focus (most cameras don't need this as their own autofocus suffices)
        public void ScannerFocus()
        {
            while (ScannerPage.Result == null)
            {
                Thread.Sleep(2000);
                ScannerPage.AutoFocus();
            }
        }


        //PAGE CHANGE BUTTONS
        public async void SearchButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ResultsPage());
        }


        public async void RecentsButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(recentsContent);
        }


        public async void ContactsButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(contactContent);
        }


        //QR SEARCH
        public async void Scan(object sender, EventArgs e)
        {
            ScannerPage = new ZXingScannerPage();
            ScannerPage.Title = "Scanning...";
            await Navigation.PushAsync(ScannerPage);

            //TODO: Autofocus delaying scan input?
            //Thread focusThread = new Thread(ScannerFocus);
            //focusThread.Start();

            ScannerPage.OnScanResult += (result) =>
            {
                ScannerPage.IsScanning = false;
                ResultsPage.scannedChemicalID = result.Text;
                AddToRecents();

                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PushAsync(new ResultsPage());
                    await DisplayAlert("Chemical ID: ", result.Text, "OK");
                });
            };
        }


        //ID SEARCH
        public void Search(object sender, EventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            chemicalID = searchBar.Text;
            ResultsPage.scannedChemicalID = chemicalID;
        }


        public static void AddToRecents()
        {
            string recent = ResultsPage.scannedChemicalID;

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

                int maxRecents = 10; //Max number of entries the list view will hold
                if (recents.Count > maxRecents)
                {
                    recents.RemoveAt(0);
                }

                recentsList.ItemsSource = null;
                recentsList.ItemsSource = recents;
            }
        }


        //CONTACT BUTTON FUNCTIONS
        async void SecurityClicked()
        {
            bool res = await DisplayAlert("Call", "Call Burwood Deakin Security?", "Yes", "No");
            if (res)
            {
                Device.OpenUri(new Uri("tel:1800 062 579"));
            }
        }


        async void EmergencyClicked()
        {
            bool res = await DisplayAlert("Call", "Call Emergency Services?", "Yes", "No");
            if (res)
            {
                Device.OpenUri(new Uri("tel:000"));
            }
        }


        async void MedicalClicked()
        {
            bool res = await DisplayAlert("Call", "Call Poisons Hotline?", "Yes", "No");
            if (res)
            {
                Device.OpenUri(new Uri("tel:131126"));
            }
        }
    }
}