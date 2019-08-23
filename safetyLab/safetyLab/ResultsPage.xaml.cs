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
        public static ScrollView generalScroll = new ScrollView();
        public static ScrollView hazardsScroll = new ScrollView();
        public static ScrollView emergencyScroll = new ScrollView();
        public static ScrollView infoScroll = new ScrollView();

        public static StackLayout generalStack = new StackLayout();
        public static StackLayout hazardsStack = new StackLayout();
        public static StackLayout emergencyStack = new StackLayout();
        public static StackLayout infoStack = new StackLayout();


        public ResultsPage ()
        {
            InitializeComponent();

            this.BarBackgroundColor = Color.DimGray;

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

            /*StartPage.webView.Source = "https://vhost2.intranet-sites.deakin.edu.au/scripts/RiskAssessment.php?ID=13902";

            WebView tempWeb = new WebView();
            tempWeb.Source = "https://www.pokemon.com/au";

            generalScroll.Content = StartPage.webView;
            hazardsScroll.Content = hazardsStack;
            emergencyScroll.Content = emergencyStack;
            infoScroll.Content = infoStack;

            general.Content = StartPage.webView;
            hazards.Content = hazardsScroll;
            emergency.Content = emergencyScroll;
            info.Content = infoScroll;

            general.Content = StartPage.webView;
            emergency.Content = tempWeb;
            tempWeb = new WebView();
            tempWeb.Source = "https://www.google.com.au";
            hazards.Content = tempWeb;
            tempWeb = new WebView();
            tempWeb.Source = "https://www.deakin.edu.au";
            info.Content = tempWeb;


            Children.Add(general);
            Children.Add(hazards);
            Children.Add(emergency);
            Children.Add(info);*/
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