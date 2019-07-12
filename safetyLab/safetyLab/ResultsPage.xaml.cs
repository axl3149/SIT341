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
	public partial class ResultsPage : ContentPage
	{
		public ResultsPage ()
		{
			InitializeComponent ();
		}

        void Back(object sender, EventArgs e)
        {
            App.Current.MainPage = new SearchPage();
            App.Current.MainPage.BackgroundColor = Color.LightBlue;
        }

    }
}