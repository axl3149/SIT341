using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace safetyLab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultsPage : ContentPage
    {
        public static string searchBarValue;

        public static string scannedChemicalID;
        public WebView webSource = new WebView();

        public ResultsPage ()
        {
            InitializeComponent();
            
            Title = "Chemical Found";

            if (scannedChemicalID != null)
            {
                webSource.Source = "https://tracie.deakin.edu.au/scripts/chemrisk.php?ID=" + scannedChemicalID;
            }
            else
            {
                webSource.Source = "https://tracie.deakin.edu.au/scripts/chemrisk.php?ID=#";
            }

            Content = webSource;   
        }


        public async void Search(object sender, EventArgs e)
        {
            SearchBar search = sender as SearchBar;
            searchBarValue = search.Text;
            scannedChemicalID = searchBarValue;

            if (searchBarValue == null || searchBarValue == "" || searchBarValue == " ")
            {
                await DisplayAlert("Search", "Enter a chemical ID to search.", "OK");
                return;
            }

            if (scannedChemicalID != null)
            {
                webSource.Source = "https://tracie.deakin.edu.au/scripts/chemrisk.php?ID=" + scannedChemicalID;
            }
        }
    }
}