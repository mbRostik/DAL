using BLL.Services;
using BLL.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF.Managers;
using WPF.Utilities;

namespace WPF.ViewModel
{
    public class NavigationViewModel : BaseViewModel
    {
        private bool _isLoggedIn;
        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set { _isLoggedIn = value; OnPropertyChanged("IsLoggedIn"); }
        }

        private bool _isLoggedInExit;
        public bool IsLoggedInExit
        {
            get { return _isLoggedInExit; }
            set { _isLoggedInExit = value; OnPropertyChanged("IsLoggedInExit"); }
        }

        private object _currentView;
        private readonly ICoinManager _coinManager;
        private IAuthManager _authManager;


        public void Home(object obj) => CurrentView = new HomeViewModel();
        public void Coin(object obj) => CurrentView = new CoinViewModel(_coinManager);
        public void AddCoin(object obj) => CurrentView = new AddCoinViewModel(_coinManager);

        public void Login(object obj) => CurrentView = new LoginViewModel(_authManager, this);

        public NavigationViewModel(ICoinManager coinManager, IAuthManager authManager)
        {
            _coinManager = coinManager;
            _authManager = authManager;

            HomeCommand = new RelayCommand(Home);
            CoinCommand = new RelayCommand(Coin);
            AddCoinCommand = new RelayCommand(AddCoin);
            LogginCommand = new RelayCommand(Login);
            ExitCommand = new RelayCommand(Exit);

            PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(CurrentView))
                {
                    UpdateLoginButtonVisibility();
                }
            };

            CheckAuthorizationStatus();
            CurrentView = new HomeViewModel();
        }

        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged("CurrentView"); }
        }

        public ICommand HomeCommand { get; set; }
        public ICommand CoinCommand { get; set; }
        public ICommand AddCoinCommand { get; set; }
        public ICommand LogginCommand { get; set; }

        public ICommand ExitCommand { get; set; }

        public async void Exit(object obj)
        {
             _authManager.Exit();
            CheckAuthorizationStatus();
        }
        public async void CheckAuthorizationStatus()
        {
            bool isLoggedIn = await _authManager.isLoggedIn();
            IsLoggedIn = isLoggedIn;
            if (IsLoggedIn == true)
            {
                IsLoggedInExit = false;
            }
            else
            {
                IsLoggedInExit = true;
            }
            UpdateLoginButtonVisibility();
        }



        private void UpdateLoginButtonVisibility()
        {
            IsLoggedIn = IsLoggedIn;
        }
    }
}
