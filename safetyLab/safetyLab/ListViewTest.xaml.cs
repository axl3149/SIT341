using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace safetyLab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListViewTest : ContentPage
    {
        public ListViewTest()
        {
            InitializeComponent();
            
            ListView list = new ListView();
            list.ItemsSource = new[] { "1", "2", "3" };

            list.ItemTapped += async (sender, e) => {
                await Navigation.PushAsync(new SearchPage());
                ((ListView)sender).SelectedItem = null; // de-select the row
            };

            Content = list;
        }
    }
}