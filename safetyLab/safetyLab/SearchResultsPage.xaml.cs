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
	public partial class SearchResultsPage : ContentPage
	{
        public static List<Button> results = new List<Button>();

		public SearchResultsPage ()
		{
			InitializeComponent ();

            var stack = new StackLayout();

            for(int i = 0; i < results.Count; i++)
            {
                results[i].Clicked += (sender, args) => SearchResult(sender, args);
                stack.Children.Add(results[i]);
            }

            var scrollView = new ScrollView();
            Content = scrollView;
            scrollView.Content = stack;

            results.Clear();
		}

        public void SearchResult(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ResultsPage());
        }
    }
}