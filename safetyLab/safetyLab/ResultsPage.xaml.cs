using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace safetyLab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultsPage : TabbedPage
    {
        public ResultsPage ()
        {
            InitializeComponent();

            this.BarBackgroundColor = Color.LightGray;

            ContentPage general = new ContentPage();
            general.Title = "General";

            ContentPage hazards = new ContentPage();
            hazards.Title = "Hazards";

            ContentPage emergency = new ContentPage();
            emergency.Title = "Emergency";

            ContentPage info = new ContentPage();
            info.Title = "Info";

            //Database.ConnectAndSetup();
            //Database.QueryResults();

            WebView webSource = new WebView();
            HtmlWebViewSource html = new HtmlWebViewSource();
            html.Html = @"<html><body>
                <h1>Xamarin.Forms</h1>
                <p>Welcome to WebView.</p>
                </body></html>";
            webSource.Source = html;
            general.Content = webSource;

            Children.Add(general);
            Children.Add(hazards);
            Children.Add(emergency);
            Children.Add(info);
        }

        public void AddToFavourites(object sender, EventArgs e)
        {
            string favourite = StartPage.chosenChemical;

            if(StartPage.favourites.Count == 0)
            {
                StartPage.favourites.Add(favourite);
                DisplayAlert("", "Added " + StartPage.chosenChemical + " to Favourites", "OK");
                return;
            }

            bool foundChemical = false;
            int foundIndex = 0;

            for(int i = 0; i < StartPage.favourites.Count; i++)
            {
                if(StartPage.favourites[i] != StartPage.chosenChemical)
                {
                    foundChemical = true;
                    foundIndex = i;
                }   
                else if(StartPage.favourites[i] == StartPage.chosenChemical)
                {
                    foundChemical = false;
                    foundIndex = i;
                    break;
                }
            }

            if(foundChemical)
            {
                StartPage.favourites.Add(favourite);
                DisplayAlert("Added", "Added " + StartPage.chosenChemical + " to Favourites", "OK");
            }
            else if(foundChemical == false)
            {
                StartPage.favourites.RemoveAt(foundIndex);
                DisplayAlert("Remove", StartPage.chosenChemical + " Removed from favourites", "OK");
            }

            //Something wrong with making ItemSource == favourites
            StartPage.mainList.ItemsSource = null;
        }
    }
}