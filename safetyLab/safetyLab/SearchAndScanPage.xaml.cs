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
	public partial class SearchAndScanPage : ContentPage
	{
		public SearchAndScanPage ()
		{
			InitializeComponent ();
		}

        public void Search(object sender, EventArgs e)
        {
            App.Current.MainPage = new SearchPage();
        }

        public void Scan(object sender, EventArgs e) //QR Function
        {

        }
	}
}