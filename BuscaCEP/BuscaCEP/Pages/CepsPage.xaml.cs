using BuscaCEP.Clients;
using BuscaCEP.Models;
using BuscaCEP.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BuscaCEP.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CepsPage : ContentPage
    {
        public CepsPage()
        {
            InitializeComponent();
            BindingContext = new CepsViewModel();
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                string cep = ((BuscaCepResult)e.Item).cep.Replace("-","");

                LatLong latlong = ViaCepHttpClient.Current.BuscarLatitudeLongitude(cep);

                Navigation.PushAsync(new MapPage(latlong));
            }
            catch(Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Erro", ex.Message, "Ok");
            }
        }
    }
}