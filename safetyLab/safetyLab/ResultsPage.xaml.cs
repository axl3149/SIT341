using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace safetyLab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultsPage : ContentPage
    {
        public ResultsPage ()
        {
            InitializeComponent();

            //Database.ConnectAndSetup();
            //Database.QueryResults();

            WebView webSource = new WebView();

            //Temp HTML file hosting. Can't find an easier way to load it locally without hassle.
            webSource.Source = "https://6e2c04ea-6a9f-4656-90eb-e53e79f77218.htmlpasta.com/";
            Content = webSource;
        }

        public void AddToFavourites(object sender, EventArgs e)
        {
            string favourite = StartPage.chosenChemical;

            bool foundChemical = false;
            int foundIndex = 0;

            for(int i = 0; i < StartPage.favourites.Count; i++)
            {
                if(StartPage.favourites[i] != StartPage.chosenChemical)
                {
                    foundChemical = false;
                    foundIndex = i;
                }   
                else if(StartPage.favourites[i] == StartPage.chosenChemical)
                {
                    foundChemical = true;
                    foundIndex = i;
                    break;
                }
            }

            if(foundChemical == false)
            {
                StartPage.favourites.Add(favourite);
                DisplayAlert("Added", "Added " + StartPage.chosenChemical + " to Favourites", "OK");
            }
            else if(foundChemical == true)
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