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
	public partial class SearchPage : ContentPage
	{
		public SearchPage ()
		{
			InitializeComponent ();
		}

        public void Back(object sender, EventArgs e)
        {
            App.Current.MainPage = new SearchAndScanPage();
            App.Current.MainPage.BackgroundColor = Color.LightBlue;
        }

        public void Search(object sender, EventArgs e)
        {
            App.Current.MainPage = new ResultsPage();
            App.Current.MainPage.BackgroundColor = Color.LightBlue;
        }

        public void Home(object sender, EventArgs e)
        {
            App.Current.MainPage = new SearchPage();
            App.Current.MainPage.BackgroundColor = Color.LightBlue;
        }
    }
}