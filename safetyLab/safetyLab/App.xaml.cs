using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace safetyLab
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            NavigationPage nav = new NavigationPage(new StartPage());
            nav.BarBackgroundColor = StartPage.navBarColor;
            MainPage = nav;
        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {

        }

        protected override void OnResume()
        {

        }
    }
}
