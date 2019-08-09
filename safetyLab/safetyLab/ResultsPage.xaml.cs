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

            ContentPage general = new ContentPage();
            general.Title = "General";

            ContentPage hazards = new ContentPage();
            hazards.Title = "Hazards";

            ContentPage emergency = new ContentPage();
            emergency.Title = "Emergency";

            ContentPage info = new ContentPage();
            info.Title = "Info";

            Database.ConnectAndSetup();
            Database.QueryResults();

            generalScroll.Content = generalStack;
            hazardsScroll.Content = hazardsStack;
            emergencyScroll.Content = emergencyStack;
            infoScroll.Content = infoStack;

            general.Content = generalScroll;
            hazards.Content = hazardsScroll;
            emergency.Content = emergencyScroll;
            info.Content = infoScroll;

            Children.Add(general);
            Children.Add(hazards);
            Children.Add(emergency);
            Children.Add(info);
        }

        public void AddToFavourites(object sender, EventArgs e)
        {
            Button favourite = new Button();
            favourite.Text = UITestPage.chosenChemical;

            if(UITestPage.favourites.Count == 0)
            {
                UITestPage.favourites.Add(favourite);
                DisplayAlert("", "Added " + UITestPage.chosenChemical + " to Favourites", "OK");
                return;
            }

            bool foundChemical = false;
            int foundIndex = 0;

            for(int i = 0; i < UITestPage.favourites.Count; i++)
            {
                if(UITestPage.favourites[i].Text != UITestPage.chosenChemical)
                {
                    foundChemical = true;
                    foundIndex = i;
                }   
                else if(UITestPage.favourites[i].Text == UITestPage.chosenChemical)
                {
                    foundChemical = false;
                    foundIndex = i;
                    break;
                }
            }

            if(foundChemical)
            {
                UITestPage.favourites.Add(favourite);
                DisplayAlert("Added", "Added " + UITestPage.chosenChemical + " to Favourites", "OK");
            }
            else if(foundChemical == false)
            {
                UITestPage.favourites.RemoveAt(foundIndex);
                DisplayAlert("Remove", UITestPage.chosenChemical + " Removed from favourites", "OK");
            }

            UITestPage.stack.Children.Clear();
        }
    }
}