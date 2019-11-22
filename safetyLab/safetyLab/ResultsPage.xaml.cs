using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

/*
 * Scanning result page for QR codes. Scanning works with ZXing library, function on StartPage.
 * Scanned output is a HTML file hosted on Deakin's TRACIE site. Template is in html folder in project.
 */

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
                //TODO: For now, show 'no result' page. Need to eventually show an emply template.
                webSource.Source = "https://tracie.deakin.edu.au/scripts/chemrisk.php?ID=#";
            }

            Content = webSource;   
        }


        public void Search(object sender, EventArgs e)
        {
            SearchBar search = sender as SearchBar;
            chemicalID = search.Text;

            if (chemicalID != null)
            {
                webSource.Source = "https://tracie.deakin.edu.au/scripts/chemrisk.php?ID=" + chemicalID;
            }
        }
    }
}