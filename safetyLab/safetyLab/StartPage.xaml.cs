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
    public partial class StartPage : TabbedPage
    {
        public static ZXingScannerPage ScannerPage = new ZXingScannerPage();

        public static ContentPage resultsContent = new ContentPage();
        public static ContentPage favouritesContent = new ContentPage();
        public static ContentPage recentsContent = new ContentPage();
        public static ContentPage contactContent = new ContentPage();

        string chemicalName = null;

        Label header = new Label();

        const int numChemicals = 8;
        public int[] chemicalIDs =
        {
            0, 1, 2, 3, 4, 5, 6, 7
        };
        public string[] chemicalNames =
        {
            "0: acid", "1: water", "2: dirt", "3: table", "4: sulfate", "5: cyanide", "6: sodium", "7: alocohol"
        };


        bool textFound = false;

        public static ListView mainList = new ListView();
        public static ListView favouritesList = new ListView();
        public static ListView recentsList = new ListView();

        public static List<string> favourites = new List<string>();
        public static List<string> recents = new List<string>();

        public static string chosenChemical;

        public StartPage()
        {
            this.BarBackgroundColor = Color.FromRgb(66, 175, 178);

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

            recentsList.ItemTapped += async (sender, e) =>
            {
                chosenChemical = e.Item.ToString();
                await Navigation.PushAsync(new ResultsPage());
            };

            //SEARCH RESULTS
            resultsContent.Content = mainList;
            resultsContent.Title = "Search";
            resultsContent.IconImageSource = "search_icon.png";
            Children.Add(resultsContent);

            //FAVOURITES
            favouritesList.ItemsSource = favourites;
            favouritesList.SeparatorColor = Color.Black;
            favouritesList.HorizontalOptions = LayoutOptions.Center;

            favouritesContent.Title = "Favourites";
            favouritesContent.Content = favouritesList;
            favouritesContent.IconImageSource = "fave.png";
            Children.Add(favouritesContent);

            //RECENTS
            recentsContent.Title = "Recents";
            recentsContent.Content = recentsList;
            recentsContent.IconImageSource = "recents_icon.png";
            Children.Add(recentsContent);

            //CONTACTS
            contactContent.Title = "Contacts";
            contactContent.IconImageSource = "phone_icon.png";
          

            StackLayout contactStack = new StackLayout();
            Button security = new Button { Text = "Deakin Security", HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center, FontSize = 24, BackgroundColor = Color.FromRgb(66, 175, 178)
            };
            security.Clicked += (sender, e) => SecurityClicked();
            contactStack.Children.Add(security);

            Button emergency = new Button { Text = "Emergency Service (000) ", HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center, FontSize = 24,
                BackgroundColor = Color.FromRgb(66, 175, 178)
            };
            emergency.Clicked += (sender, e) => EmergencyClicked();
            contactStack.Children.Add(emergency);

            Button medical = new Button { Text = "Deakin Medical (Building B)", HorizontalOptions = LayoutOptions.Fill,
                BackgroundColor = Color.FromRgb(66, 175, 178), VerticalOptions = LayoutOptions.Center, FontSize = 24 };
            medical.Clicked += (sender, e) => MedicalClicked();
            contactStack.Children.Add(medical);

            contactContent.Content = contactStack;

            Children.Add(contactContent);
        }

        //For QR camera scanning focus
        public void ScannerFocus()
        {
            while (ScannerPage.Result == null)
            {
                System.Threading.Thread.Sleep(2000);
                ScannerPage.AutoFocus();
            }
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

        //Auto complete search taken out for now
        /*public void AutoSearch(object sender, EventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;

            int id = System.Convert.ToInt32(searchBar.Text);

            textFound = false;

            List<string> foundNames = new List<string>();

            for (int i = 0; i < chemicalNames.Length; i++)
            {
                if (i == id)
                {
                    foundNames.Add(chemicalNames[i]);
                    textFound = true;
                }
            }

            mainList.ItemsSource = foundNames;

            if (textFound == false)
            {
                DisplayAlert("Search Results", "No chemical results found.", "Try again");
            }

            textFound = false;

            this.CurrentPage = resultsContent;
        }*/

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