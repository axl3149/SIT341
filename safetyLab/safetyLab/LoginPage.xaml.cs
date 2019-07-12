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
		public LoginPage ()
		{
			InitializeComponent ();
		}

        void AcceptLogin(object sender, EventArgs e)
        {
            //Shit way to change pages. Look into Pop/Push functions.
            App.Current.MainPage = new SearchAndScanPage();
            App.Current.MainPage.BackgroundColor = Color.LightBlue;
        }

        void GotoPublishers(object sender, EventArgs e)
        {
            App.Current.MainPage = new PublishersPage();
            App.Current.MainPage.BackgroundColor = Color.LightBlue;
        }
    }
}