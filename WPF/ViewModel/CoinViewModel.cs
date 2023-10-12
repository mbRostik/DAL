using BLL.Services;
using BLL.Services.Contracts;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF.Utilities;

namespace WPF.ViewModel
{
    public class CoinViewModel : BaseViewModel
    {
        private List<Coin> _coin;
        public List<Coin> Coin
        {
            get { return _coin; }
            set
            {
                _coin = value;
                OnPropertyChanged(nameof(Coin));
            }
        }

        private readonly ICoinManager _coinManager;

        public ICommand EditCoinCommand { get; }
        public ICommand DeleteCoinCommand { get; }

        public CoinViewModel(ICoinManager coinManager)
        {
            _coinManager = coinManager;
            Coin = new List<Coin>();
            EditCoinCommand = new RelayCommand(EditCoin);
            DeleteCoinCommand = new RelayCommand(DeleteCoin);

            LoadCoin();
        }

        public async Task LoadCoin()
        {
            var coin = await _coinManager.UnitOfWork.CoinRepository.GetAllAsync();

            Coin = coin.ToList();

            OnPropertyChanged(nameof(Coin));
        }

        private async void EditCoin(object parameter)
        {
            if (parameter is Coin selectedCoin)
            {
                await _coinManager.UnitOfWork.CoinRepository.UpdateAsync(selectedCoin);
                await _coinManager.UnitOfWork.SaveChangesAsync();
                await LoadCoin();
            }
        }

        private async void DeleteCoin(object parameter)
        {
            if (parameter is Coin selectedCoin)
            {
                await _coinManager.UnitOfWork.CoinRepository.DeleteAsync(selectedCoin);
                await _coinManager.UnitOfWork.SaveChangesAsync();
                await LoadCoin();
            }
        }
    }
}