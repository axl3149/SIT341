using System;
using System.Threading;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

/*
 * Main Page of application. 
 * Emergency contact buttons are handled here.
 * Scan function handled here.
 */

namespace safetyLab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {
        //TODO: Does Xamarin have a global 'colours.xml' file for both platforms?
        public static Color navBarColor = Color.FromRgb(47, 74, 91);

        //QR scanning uses ZXing package. Pushed onto Navigation stack from this page.
        public static ZXingScannerPage ScannerPage = new ZXingScannerPage();

        public static ContentPage contactContent = new ContentPage();
        public StackLayout contactStack = new StackLayout();
        
        public StartPage()
        {
            InitializeComponent();

            BackgroundColor = navBarColor;
        
            //EMERGENCY CONTACTS CONTENT
            contactContent.Title = "Emergency Contacts";
            contactContent.BackgroundColor = navBarColor;

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

            contactStack.Children.Add(securityButton);
            contactStack.Children.Add(emergencyButton);
            contactStack.Children.Add(poisonsButton);

            contactContent.Content = contactStack;
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
            ScannerPage = new ZXingScannerPage
            {
                Title = "Scan Chemical QR Code"
            };

            await Navigation.PushAsync(ScannerPage, true);

            ScannerPage.OnScanResult += (result) =>
            {
                ScannerPage.IsScanning = false; //Needed to stop scanner from constantly looping once scanned.
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