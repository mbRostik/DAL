using DAL.Models;
using DAL.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Contracts
{
    public interface ICoinManager
    {

        IUnitOfWork UnitOfWork { get; }

        Task<Coin> GetOneCoin();
    }
}
