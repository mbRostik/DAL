using BLL.Services.Contracts;
using DAL.Models;
using DAL.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CoinManager : ICoinManager
    {
        public IUnitOfWork UnitOfWork { get; set; }

        public CoinManager(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public async Task<Coin> GetOneCoin()
        {
            var coin = await UnitOfWork.CoinRepository.GetAllAsync();

            return coin.First();
        }
    }
}
