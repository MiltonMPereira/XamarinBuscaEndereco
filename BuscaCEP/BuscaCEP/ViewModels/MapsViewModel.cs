using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BuscaCEP.ViewModels
{
    class MapsViewModel : ViewModelBase
    {
        public MapsViewModel() : base()
        {

        }
        private Command _MapCommand;

        public Command MapCommand => _MapCommand ?? (_MapCommand = new Command(async () => await MapCommandExecute(), () => IsNotBusy));

        async Task MapCommandExecute()
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;
                MapCommand.ChangeCanExecute();
                /*
                ceps = await ViaCepHttpClient.Current.BuscarCep(_CEPBusca);


                OnPropertyChanged(nameof(ceps));
                OnPropertyChanged(nameof(HasCep));
                */
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
                MapCommand.ChangeCanExecute();
            }
        }
    }
}
