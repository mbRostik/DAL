using DAL.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {


        public MyContext databaseContext { get; }

        public ICoinRepository CoinRepository { get; }

        public UnitOfWork(
           MyContext databaseContext,
           ICoinRepository CoinRepository
           )
        {
            this.databaseContext = databaseContext;
            this.CoinRepository = CoinRepository;

        }
        public async Task SaveChangesAsync()
        {
            await databaseContext.SaveChangesAsync();
        }
    }
}
