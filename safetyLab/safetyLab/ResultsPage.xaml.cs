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

            this.Title = "Chemical Found";

            if (scannedChemicalID != null)
            {
                webSource.Source = "https://tracie.deakin.edu.au/scripts/chemrisk.php?ID=" + scannedChemicalID;
            }
            else
            {
                webSource.Source = "https://tracie.deakin.edu.au/scripts/chemrisk.php?ID=";
            }

            //TODO: need openuri to work with sds links in html file
            //Device.OpenUri(new Uri("http://www.pdf995.com/samples/pdf.pdf"));

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

            StartPage.AddToRecents();
        }


        public void AddToFavourites(object sender, EventArgs e)
        {
            string favourite = StartPage.chemicalID;

            bool foundChemical = false;
            int foundIndex = 0;

            if (favourite != null)
            {
                for (int i = 0; i < StartPage.favourites.Count; i++)
                {
                    if (StartPage.favourites[i] != favourite)
                    {
                        foundChemical = false;
                        foundIndex = i;
                    }
                    else if (StartPage.favourites[i] == favourite)
                    {
                        foundChemical = true;
                        foundIndex = i;
                        break;
                    }
                }

                if (foundChemical == false)
                {
                    StartPage.favourites.Add(favourite);
                    DisplayAlert("Added", "Added " + StartPage.chosenChemical + " to Favourites", "OK");
                }
                else if (foundChemical == true)
                {
                    StartPage.favourites.RemoveAt(foundIndex);
                    DisplayAlert("Remove", StartPage.chosenChemical + " Removed from favourites", "OK");
                }

                //Something wrong with making ItemSource == favourites? Something about == null refreshing the list
                StartPage.favouritesList.ItemsSource = null;
                StartPage.favouritesList.ItemsSource = StartPage.favourites;
            }
        }
    }
}