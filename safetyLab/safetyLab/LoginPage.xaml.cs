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
	public partial class LoginPage : ContentPage
	{
        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            //WebView test. Maybe could use search engine results for Chemical names?
            WebView web = new WebView
            {
                Source = "https://www.chemsupply.com.au/documents/SO0116_AU.pdf"
            };
        
            Content = web;
		}

        async void AcceptLogin(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SearchAndScanPage());
        }

        async void GotoPublishers(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PublishersPage());
        }
    }
}