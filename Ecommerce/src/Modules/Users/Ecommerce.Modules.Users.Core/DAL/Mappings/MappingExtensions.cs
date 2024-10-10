using Ecommerce.Modules.Users.Core.DTO;
using Ecommerce.Modules.Users.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Core.DAL.Mappings
{
    internal static class MappingExtensions
    {
        public static CustomerBrowseDto AsBrowseDto(this Customer customer)
            => new()
            {
                Id = customer.Id,
                FullName = customer.FullName,
                Email = customer.Email,
                CreatedAt = customer.CreatedAt
            };
        public static CustomerDetailsDto AsDetailsDto(this Customer customer)
            => new()
            {
                Id = customer.Id,
                FullName = customer.FullName,
                Email = customer.Email,
                Username = customer.Username,
                Role = customer.Role.Name,
                UpdatedAt = customer.UpdatedAt,
                CreatedAt = customer.CreatedAt,
            };
        public static EmployeeBrowseDto AsBrowseDto(this Employee employee)
            => new()
            {
                Id = employee.Id,
                FullName = employee.FullName, 
                Email = employee.Email,
                Role = employee.Role.Name,
                CreatedAt = employee.CreatedAt,
            };
        public static EmployeeDetailsDto AsDetailsDto(this Employee employee)
            => new()
            {
                Id = employee.Id,
                FullName = employee.FullName,
                Email = employee.Email,
                Role = employee.Role.Name,
                JobPosition = employee.JobPosition,
                UpdatedAt = employee.UpdatedAt,
                CreatedAt = employee.CreatedAt,
            };
    }
}
