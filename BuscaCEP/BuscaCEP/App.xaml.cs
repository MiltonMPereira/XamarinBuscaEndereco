using BuscaCEP.Pages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace BuscaCEP
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new CepsPage());
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
