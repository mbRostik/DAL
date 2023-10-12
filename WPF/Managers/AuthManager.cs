using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Managers
{
    public class AuthManager:IAuthManager, INotifyPropertyChanged
    {
        public IsolatedStorageFile _isoStore {get; set;}
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;
        private MyContext _context;
        public AuthManager(UserManager<User> _userManager,  MyContext context, SignInManager<User> _signInManager)
        {
            _isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
            userManager = _userManager;
            signInManager = _signInManager;
            _context = context;

        }


        public async Task<UserWithToken> LoginUser(User_Model user)
        {

            var temp = await userManager.FindByEmailAsync(user.Email);

            if (temp == null)
            {
                return null;
            }

            bool isPasswordValid = await userManager.CheckPasswordAsync(temp, user.Password);

            if (!isPasswordValid)
            {
                return null; 
            }




            UserWithToken userWithToken = new UserWithToken(temp);

            if (temp != null)
            {

                RefreshToken refreshToken = GenerateRefreshToken();

                temp.RefreshTokens.Add(refreshToken);
                await _context.SaveChangesAsync();

                userWithToken = new UserWithToken(temp);
                userWithToken.RefreshToken = refreshToken.Token;
            }

            if (userWithToken == null)
            {

                return null;
            }
            userWithToken.AccessToken = GenerateAccessToken(temp.Id);



            if (_isoStore.FileExists("Test.txt"))
            {
                _isoStore.DeleteFile("Test.txt");
            }

            using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream("Test.txt", FileMode.CreateNew, _isoStore))
            {
                using (StreamWriter writer = new StreamWriter(isoStream))
                {
                    writer.Write(userWithToken.RefreshToken+" ");

                    writer.Write(userWithToken.AccessToken);
                }
            }

            

           

            return userWithToken;
        }

        private RefreshToken GenerateRefreshToken()
        {
            RefreshToken refreshToken = new RefreshToken();

            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {


                rng.GetBytes(randomNumber);

                var base64String = Convert.ToBase64String(randomNumber);

                // Удаление символа "+"
                var sanitizedToken = base64String.Replace("+", "");

                refreshToken.Token = sanitizedToken;


            }
            //refreshToken.ExpiryDate = DateTime.UtcNow.AddMonths(6);
            refreshToken.ExpiryDate = DateTime.Now.AddHours(1);
            return refreshToken;
        }

        private string GenerateAccessToken(string userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("QwErTyUiOpKjHbIDDbhfd1nvdf12f3");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, Convert.ToString(userId))
                }),
                Expires = DateTime.UtcNow.AddSeconds(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> isLoggedIn()
        {
           

            if (!_isoStore.FileExists("Test.txt"))
                return true;

            string token = "";

            using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream("Test.txt", FileMode.Open, _isoStore))
            {
                using (StreamReader reader = new StreamReader(isoStream))
                {
                    string values = reader.ReadToEnd();
                    string[] valueArray = values.Split(' ');
                    token = valueArray[0];
                }
            }

            if (_context.refreshTokens.FirstOrDefault(e => e.Token == token) != null)
            {

                if (_context.refreshTokens.FirstOrDefault(m => m.Token == token).ExpiryDate > DateTime.Now)
                {

                    return false ;
                }
                
                else
                {

                    var temp = await FindRefreshToken(token);
                    _context.refreshTokens.Remove(temp);
                    _context.SaveChanges();
                    _isoStore.DeleteFile("Test.txt");
                    return true;
                }
            }
            else
            {
                return true ;
            }
        }


        private async Task<RefreshToken> FindRefreshToken(string token)
        {
            return await _context.refreshTokens.FirstOrDefaultAsync(t => t.Token == token);
        }

        public virtual void Dispose() { }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Exit()
        {
            if (_isoStore.FileExists("Test.txt"))
            {
                _isoStore.DeleteFile("Test.txt");
            }
        }
    }
}
