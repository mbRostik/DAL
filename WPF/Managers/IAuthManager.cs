using DAL.Models;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Managers
{
    public interface IAuthManager
    {
        public IsolatedStorageFile _isoStore { get; set; }

        public Task<UserWithToken> LoginUser(User_Model user);

        public Task<bool> isLoggedIn();

        public void Exit();
    }
}
