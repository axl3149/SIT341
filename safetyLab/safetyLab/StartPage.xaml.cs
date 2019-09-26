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
        public static TabbedPage mapTabbed = new TabbedPage();

        string chemicalName = null;

        Label header = new Label();

        //Temp chemical names. Need to next link up to TRACIE SQL calls for 'Search'
        public static string[] chemicalNames = {
            "acid", "water", "dirt", "table", "sulfate", "cyanide", "sodium", "alocohol",
            "acid", "water", "dirt", "table", "sulfate", "cyanide", "sodium", "alocohol",
            "acid", "water", "dirt", "table", "sulfate", "cyanide", "sodium", "alocohol",
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
            resultsContent.BackgroundImageSource = ImageSource.FromFile("search_background.png");
            Children.Add(resultsContent);

            //FAVOURITES
            favouritesList.ItemsSource = favourites;
            favouritesList.SeparatorColor = Color.Black;
            favouritesList.HorizontalOptions = LayoutOptions.Center;

            favouritesContent.Title = "Favourites";
            favouritesContent.Content = favouritesList;
            favouritesContent.BackgroundImageSource = ImageSource.FromFile("icon.png");
            favouritesContent.IconImageSource = "favourites_icon.png";
            Children.Add(favouritesContent);

            //RECENTS
            recentsContent.Title = "Recents";
            recentsContent.Content = recentsList;
            recentsContent.BackgroundImageSource = ImageSource.FromFile("recents_icon.png");
            recentsContent.IconImageSource = "recents_icon.png";
            Children.Add(recentsContent);

            //MAPS
            mapTabbed.Title = "Maps";
            mapTabbed.BarBackgroundColor = Color.FromRgb(66, 175, 178);
            mapTabbed.IconImageSource = "map_icon.png";

            StackLayout mapStackBurwood = new StackLayout();
            Image burwoodMap = new Image { Source = "deakin_burwood.jpg" };
            Button security = new Button { Text = "Deakin Security", HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, FontSize = 24 };
            security.Clicked += (sender, e) => SecurityClicked();
            mapStackBurwood.Children.Add(security); 

            Button emergency = new Button { Text = "Emergency Service (000) ", HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, FontSize = 24 };
            emergency.Clicked += (sender, e) => EmergencyClicked();
            mapStackBurwood.Children.Add(emergency);

            Button medical = new Button { Text = "Deakin Medical (Building B)", HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, FontSize = 24 };
            medical.Clicked += (sender, e) => MedicalClicked();
            mapStackBurwood.Children.Add(medical);

            Button hospital = new Button { Text = "Box Hill Hospital", HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, FontSize = 24 };
            //hospital.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { HospitalClicked(); }), NumberOfTapsRequired = 1 });
            hospital.Clicked += (sender, e) => HospitalClicked();
            mapStackBurwood.Children.Add(hospital);

            mapStackBurwood.Children.Add(burwoodMap);
            
            ScrollView mapScroll = new ScrollView();
            mapScroll.Content = mapStackBurwood;

            ContentPage mapContentBurwood = new ContentPage { Title = "Burwood" };
            mapContentBurwood.Content = mapScroll;

            mapTabbed.Children.Add(mapContentBurwood);

            StackLayout mapStackGeelong = new StackLayout();
            Image geelongMap = new Image { Source = "deakin_geelong.jpg" };
            mapStackGeelong.Children.Add(geelongMap);

            ContentPage mapContentGeelong = new ContentPage { Title = "Geelong" };
            mapContentGeelong.Content = mapStackGeelong;

            mapTabbed.Children.Add(mapContentGeelong);

            Children.Add(mapTabbed);
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

            //Thread focusThread = new Thread(ScannerFocus);
            //focusThread.Start();

            ScannerPage.OnScanResult += (result) =>
            {
                ScannerPage.IsScanning = false;

                Device.BeginInvokeOnMainThread(async () =>
                {
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