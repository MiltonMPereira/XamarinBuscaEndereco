using BuscaCEP.Clients;
using BuscaCEP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BuscaCEP.ViewModels
{
    class CepsViewModel : ViewModelBase
    {
        private string _CEPBusca;
        public string CEPBusca
        {
            get => _CEPBusca;
            set
            {
                _CEPBusca = value;
                OnPropertyChanged();
            }
        }

        private List<BuscaCepResult> ceps;
        public List<BuscaCepResult> Ceps
        {
            get { return ceps; }
            set
            {
                ceps = value;
            }
        }
        public CepsViewModel()
        {

        }
        private Command _BuscarCommand;

        public Command BuscarCommand => _BuscarCommand ?? (_BuscarCommand = new Command(async () => await BuscarCommandExecute(), () => IsNotBusy));
        

        async Task BuscarCommandExecute()
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;
                BuscarCommand.ChangeCanExecute();

                ceps = await ViaCepHttpClient.Current.BuscarCep(_CEPBusca);

                OnPropertyChanged(nameof(Ceps));
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
                BuscarCommand.ChangeCanExecute();
            }
        }
    }
}
