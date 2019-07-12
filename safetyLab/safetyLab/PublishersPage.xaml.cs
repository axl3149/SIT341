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
	public partial class PublishersPage : ContentPage
	{
		public PublishersPage ()
		{
			InitializeComponent ();
		}

        public void Back(object sender, EventArgs e)
        {
            App.Current.MainPage = new LoginPage();
            App.Current.MainPage.BackgroundColor = Color.LightBlue;
        }
	}
}