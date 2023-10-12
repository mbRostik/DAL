using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using DAL;
using Microsoft.EntityFrameworkCore;
using DAL.Repositories.Contracts;
using DAL.Repositories;
using BLL.Services.Contracts;
using BLL.Services;
using System.IO.IsolatedStorage;
using WPF.Managers;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WPF.ViewModel;

namespace WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost _host;

        protected override void OnStartup(StartupEventArgs e)
        {
             _host = Host.CreateDefaultBuilder()
               .ConfigureServices((hostBuilderContext, serviceCollection) =>
               {
                   serviceCollection.AddDbContext<MyContext>(options =>
                   {
                       options.UseSqlServer("Data Source=DESKTOP-VIMRQAL\\SQLEXPRESS;Initial Catalog=WPF;Integrated Security=True;Connect Timeout=60;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
                   }, ServiceLifetime.Singleton);
                   serviceCollection.AddSingleton<IUnitOfWork, UnitOfWork>();
                   serviceCollection.AddSingleton<ICoinRepository, CoinRepository>();
                   serviceCollection.AddSingleton<ICoinManager, CoinManager>();
                   serviceCollection.AddSingleton<CoinViewModel>();
                   serviceCollection.AddSingleton<HomeViewModel>();
                   serviceCollection.AddSingleton<AddCoinViewModel>();
                   serviceCollection.AddSingleton<NavigationViewModel>();
                   serviceCollection.AddSingleton<MainWindow>();
                   serviceCollection.AddSingleton<IAuthManager, AuthManager>();
                   serviceCollection.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<MyContext>();

                   serviceCollection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(options =>
                   {
                       options.TokenValidationParameters = new TokenValidationParameters
                       {
                           ValidateIssuer = false,
                           ValidateAudience = false,
                           ValidateLifetime = true,
                           ValidateIssuerSigningKey = true,
                           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("QwErTyUiOpKjHbIDDbhfd1nvdf12f3"))
                       };
                   });


               })
               .Build();

            _host.Start();

            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.DataContext = _host.Services.GetRequiredService<NavigationViewModel>();
            MainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            if (_host != null)
            {
                await _host.StopAsync();
                _host.Dispose();
            }

            base.OnExit(e);
        }
    }
}
