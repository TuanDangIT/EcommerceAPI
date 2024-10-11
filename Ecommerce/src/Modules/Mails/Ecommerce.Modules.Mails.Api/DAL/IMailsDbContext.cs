using Ecommerce.Modules.Mails.Api.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Mails.Api.DAL
{
    internal interface IMailsDbContext
    {
        DbSet<Customer> Customers { get; set; }
        DbSet<Mail> Mails { get; set; }
        Task<int> SaveChangesAsync();
    }
}
