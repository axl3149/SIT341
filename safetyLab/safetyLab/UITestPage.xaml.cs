using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace safetyLab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UITestPage : ContentPage
    {
        WebView webView;

        string chemicalName = null;
        public string[] chemicalNames = { "acid", "water" };
        bool textFound = false;

        List<Button> results = new List<Button>();

        ContentPage SearchContent;

        public UITestPage()
        {
            InitializeComponent();
            webView = new WebView();
        }

        public void Scan(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ZXingScannerPage());
        }

        public void Search(object sender, EventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;

            chemicalName = searchBar.Text;

            results.Clear();

            SearchContent = new ContentPage();

            StackLayout stack = new StackLayout();

            textFound = false;

            if (chemicalName != null)
            {
                for (int i = 0; i < chemicalNames.Length; i++)
                {
                    if (chemicalNames[i].Contains((chemicalName)))
                    {
                        Button result = new Button();
                        result.BackgroundColor = Color.White;
                        result.Text = chemicalNames[i];
                        result.Clicked += (send, args) => Result(send, args); //Performance problems?

                        results.Add(result);

                        stack.Children.Add(results[i]);

                        textFound = true;
                    }
                }

                SearchContent.Content = stack;
                Content = SearchContent.Content;
            }

            if (textFound == false)
            {
                DisplayAlert("Search Results", "No chemical results found.", "Try again");
            }
        }

        public void Result(object sender, EventArgs e)
        {
            webView.Source = "https://vhost2.intranet-sites.deakin.edu.au/scripts/RiskAssessment.php?ID=5305";
            Content = webView;
        }
    }
}