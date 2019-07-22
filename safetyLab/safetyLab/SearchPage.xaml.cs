﻿using System;
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
        public string[] chemicalNames = {"acid", "waterlake", "acidity", "watery"};
        bool textFound = false;

        StackLayout stack;
        Entry chemicalName = new Entry();
        Entry chemicalID = new Entry();

		public SearchPage ()
		{
			InitializeComponent ();

            stack = new StackLayout();

            chemicalName.Placeholder = "Chemical Name";
            chemicalID.Placeholder = "Chemical ID";

            stack.Children.Add(chemicalName);
            stack.Children.Add(chemicalID);

            Button button = new Button();
            button.Text = "Search";
            button.Clicked += (sender, args) => Search(sender, args);
            stack.Children.Add(button);

            Content = stack;
        }

        public void Search(object sender, EventArgs e)
        {
            SearchResultsPage.results.Clear();

            for (int i = 0; i < chemicalNames.Length; i++)
            {
                if(chemicalNames[i].Contains(chemicalName.Text))
                {
                    Button result = new Button();
                    result.BackgroundColor = Color.White;
                    result.Text = chemicalNames[i];

                    SearchResultsPage.results.Add(result);

                    textFound = true;
                }
            }

            if(textFound)
            {
                Navigation.PushAsync(new SearchResultsPage());
                return;
            }
            
            DisplayAlert("Search Results", "No chemical results found.", "Try again");
        }
    }
}