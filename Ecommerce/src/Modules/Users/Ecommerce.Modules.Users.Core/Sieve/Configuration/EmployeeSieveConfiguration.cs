using Ecommerce.Modules.Users.Core.Entities;
using Sieve.Services;

namespace Ecommerce.Modules.Users.Core.Sieve.Configuration
{
    internal class EmployeeSieveConfiguration : ISieveConfiguration
    {
        public void Configure(SievePropertyMapper mapper)
        {
            mapper.Property<Employee>(e => e.Id)
                .CanFilter();
            mapper.Property<Employee>(e => e.FirstName)
                .CanFilter();
            mapper.Property<Employee>(e => e.LastName)
                .CanFilter();
            mapper.Property<Employee>(e => e.Email)
                .CanFilter();
            mapper.Property<Employee>(e => e.Username)
                .CanFilter();
            mapper.Property<Employee>(e => e.Role.Name)
                .CanFilter();
            mapper.Property<Employee>(e => e.JobPosition)
                .CanFilter();
            mapper.Property<Employee>(e => e.CreatedAt)
                .CanFilter()
                .CanSort();
        }
    }
}
