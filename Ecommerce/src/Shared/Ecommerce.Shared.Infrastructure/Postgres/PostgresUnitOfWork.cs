using Ecommerce.Shared.Abstractions.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Postgres
{
    public abstract class PostgresUnitOfWork<T> : IUnitOfWork where T : DbContext
    {
        private readonly T _dbContext;

        protected PostgresUnitOfWork(T dbContext)
        {
            _dbContext = dbContext;
        }

        public IDbTransaction BeginTransaction()
            => _dbContext.Database.BeginTransaction().GetDbTransaction();

        //public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        //    => await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
