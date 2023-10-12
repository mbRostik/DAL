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
    public class AddCoinViewModel : BaseViewModel
    {
        private readonly ICoinManager _coinManager;

        private string _coinName;
        public string CoinName
        {
            get { return _coinName; }
            set
            {
                _coinName = value;
                OnPropertyChanged(nameof(CoinName));
            }
        }

        private string _coinValue;
        public string CoinValue
        {
            get { return _coinValue; }
            set
            {
                _coinValue = value;
                OnPropertyChanged(nameof(CoinValue));
            }
        }

        public ICommand AddCoinCommand { get; }

        public AddCoinViewModel(ICoinManager coinManager)
        {
            _coinManager = coinManager;
            AddCoinCommand = new RelayCommand(AddCoinAsync, CanAddCoin);
        }

        private async void AddCoinAsync(object parameter)
        {
            Coin newCoin = new Coin()
            {
                Name = CoinName,
                Price = CoinValue
            };

            await _coinManager.UnitOfWork.CoinRepository.AddAsync(newCoin);
            await _coinManager.UnitOfWork.SaveChangesAsync();
        }

        private bool CanAddCoin(object parameter)
        {
            return !string.IsNullOrEmpty(CoinName) && !string.IsNullOrEmpty(CoinValue);
        }
    }


}
