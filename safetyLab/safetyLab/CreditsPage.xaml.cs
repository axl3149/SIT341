using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

/*
 * Credits/About page showing creators and support on project.
 */ 

namespace safetyLab
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreditsPage : ContentPage
    {
        public CreditsPage()
        {
            InitializeComponent();

            BackgroundColor = StartPage.navBarColor;
            Title = "Credits";
        }
    }
}