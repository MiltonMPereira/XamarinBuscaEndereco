using BuscaCEP.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using BuscaCEP.ViewModels;
using System;
using Plugin.Share;
using Plugin.Share.Abstractions;

namespace BuscaCEP.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        string urlMapa = string.Empty;
        public MapPage(LatLong coordenadas)
        {
            InitializeComponent();
            BindingContext = new MapsViewModel();
            urlMapa = "http://maps.google.com/maps?q=loc:" + coordenadas.latitude + "," + coordenadas.longitude;

            WebView browser = new WebView();
            browser.Source = new UrlWebViewSource { Url = urlMapa };
            this.Content = browser;
        }
        async void ShareButtonClicked(object sender, EventArgs e)
        {
            if(urlMapa == "")
                throw new InvalidOperationException("Erro ao compartilhar localização.");

            await CrossShare.Current.Share(new ShareMessage { Url = urlMapa });
        }
    }
}