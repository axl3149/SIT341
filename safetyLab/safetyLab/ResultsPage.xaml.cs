using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace safetyLab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultsPage : ContentPage
    {
        public static string chemicalID;
        public WebView webSource = new WebView();

        public ResultsPage ()
        {
            InitializeComponent();
            
            if (chemicalID != null)
            {
                webSource.Source = "https://tracie.deakin.edu.au/scripts/chemrisk.php?ID=" + chemicalID;
            }
            else
            {
                //For now, show empty HTML page
                webSource.Source = "https://tracie.deakin.edu.au/scripts/chemrisk.php?ID=#";
            }

            Content = webSource;   
        }


        public async void Search(object sender, EventArgs e)
        {
            SearchBar search = sender as SearchBar;
            chemicalID = search.Text;

            if (chemicalID == null || chemicalID == "" || chemicalID == " ")
            {
                await DisplayAlert("Search", "Enter a chemical ID to search.", "OK");
                return;
            }

            if (chemicalID != null)
            {
                webSource.Source = "https://tracie.deakin.edu.au/scripts/chemrisk.php?ID=" + chemicalID;
            }
        }
    }
}