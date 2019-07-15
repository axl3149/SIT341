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