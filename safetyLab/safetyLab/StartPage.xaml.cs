using System;
using System.Threading;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace safetyLab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {
        public static Color navBarColor = Color.FromRgb(47, 74, 91);
        public static ZXingScannerPage ScannerPage = new ZXingScannerPage();
        public static ContentPage contactContent = new ContentPage();
        public static ContentPage creditsContent = new ContentPage();

        public StartPage()
        {
            InitializeComponent();

            BackgroundColor = navBarColor;
        
            //CONTACTS
            contactContent.Title = "Emergency Contacts";
            contactContent.BackgroundColor = navBarColor;

            Grid contactGrid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star }
                },
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto }
                }
            };

            Button contactButton = new Button
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                FontSize = 24,
                BackgroundColor = navBarColor,
                CornerRadius = 30,
                BorderWidth = 5,
                BorderColor = Color.White,
                TextColor = Color.White
            };

            Button securityButton = new Button
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                FontSize = 24,
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = navBarColor,
                CornerRadius = 30,
                BorderWidth = 5,
                BorderColor = Color.White,
                TextColor = Color.White,
                Text = "Deakin Security"
            };
            securityButton.Clicked += (sender, e) => SecurityClicked();
            contactGrid.Children.Add(securityButton, 0, 0);

            Button emergencyButton = new Button
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                FontSize = 24,
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = navBarColor,
                CornerRadius = 30,
                BorderWidth = 5,
                BorderColor = Color.White,
                TextColor = Color.White,
                Text = "Emergency Service (000)"
            };
            emergencyButton.Clicked += (sender, e) => EmergencyClicked();
            contactGrid.Children.Add(emergencyButton, 0, 1);

            Button poisonsButton = new Button
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                FontSize = 24,
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = navBarColor,
                CornerRadius = 30,
                BorderWidth = 5,
                BorderColor = Color.White,
                TextColor = Color.White,
                Text = "Poisons Hotline"
            };
            poisonsButton.Clicked += (sender, e) => PosisonsClicked();
            contactGrid.Children.Add(poisonsButton, 0, 2);

            contactContent.Content = contactGrid;
        }

        //For QR camera scanning focus (most cameras don't need this as their own autofocus suffices)
        //Function currently not used in program
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
            await Navigation.PushAsync(new ResultsPage(), true);
        }

        public async void ContactsButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(contactContent, true);
        }

        public async void ShowCredits(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreditsPage(), true);
        }

        //QR CODE SEARCH
        public async void Scan(object sender, EventArgs e)
        {
            ScannerPage = new ZXingScannerPage();
            ScannerPage.Title = "Scan Chemical QR Code";
          
            await Navigation.PushAsync(ScannerPage, true);

            ScannerPage.OnScanResult += (result) =>
            {
                ScannerPage.IsScanning = false; //Needed to stop scanner from constantly looping. I think
                ResultsPage.chemicalID = result.Text;

                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PushAsync(new ResultsPage(), true);
                    await DisplayAlert("Chemical ID: ", result.Text, "OK");
                });
            };
        }

        //ID SEARCH
        public void Search(object sender, EventArgs e)
        {
            SearchBar searchBar = sender as SearchBar;
            ResultsPage.chemicalID = searchBar.Text;
        }

        //CONTACT BUTTON FUNCTIONS
        async void SecurityClicked()
        {
            bool res = await DisplayAlert("Call", "Call Burwood Deakin Security?", "Yes", "No");
            if (res)
            {
                await Launcher.TryOpenAsync("tel:1800 062 579");
            }
        }

        async void EmergencyClicked()
        {
            bool res = await DisplayAlert("Call", "Call Emergency Services?", "Yes", "No");
            if (res)
            {
                await Launcher.TryOpenAsync("tel:000");
            }
        }

        async void PosisonsClicked()
        {
            bool res = await DisplayAlert("Call", "Call Poisons Hotline?", "Yes", "No");
            if (res)
            {
                await Launcher.TryOpenAsync("tel:13 11 26");
            }
        }
    }
}