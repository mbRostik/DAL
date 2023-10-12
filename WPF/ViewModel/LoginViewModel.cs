using BLL.Services;
using BLL.Services.Contracts;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF.Managers;
using WPF.Utilities;
using WPF.Views;

namespace WPF.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private User_Model _userModel;
        public User_Model UserModel
        {
            get { return _userModel; }
            set
            {
                _userModel = value;
                OnPropertyChanged(nameof(UserModel));
            }
        }

        NavigationViewModel navigation;

        private UserWithToken _userWithToken;

        public UserWithToken UserwithToken
        {
            get { return _userWithToken; }
            set
            {
                _userWithToken = value;
                OnPropertyChanged(nameof(UserwithToken));
            }
        }

        private IAuthManager _authManager;
        public IAuthManager AuthManager
        {
            get { return _authManager; }
            set
            {
                _authManager = value;
                OnPropertyChanged(nameof(AuthManager));
            }
        }

        public ICommand LogginCommand { get; }


        public async void CheckLogin(object parameter)
        {
            var model = await AuthManager.LoginUser(UserModel);
            if (model != null)
            {
                navigation.CheckAuthorizationStatus();
                navigation.CurrentView = new HomeViewModel();
            }
            
        }

        public LoginViewModel(IAuthManager authManager, NavigationViewModel nav)
        {
            _authManager = authManager;
            UserModel = new User_Model();
            navigation= nav;
            LogginCommand = new RelayCommand(CheckLogin);
        }
    }
}
