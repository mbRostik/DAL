using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Contracts
{
    public interface IUnitOfWork
    {
        MyContext databaseContext { get; }
        ICoinRepository CoinRepository { get; }

        Task SaveChangesAsync();
    }
}
