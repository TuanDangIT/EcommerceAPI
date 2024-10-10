using Ecommerce.Modules.Users.Core.DTO;
using Ecommerce.Shared.Abstractions.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Core.Services
{
    public interface IIdentityService
    {
        Task<JsonWebToken> SignInAsync(SignInDto dto);
        Task SignUpAsync(SignUpDto dto);
        //Task<UserDto?> GetAsync(Guid id);
        //Task<UserDto?> GetAsync(string email);
        //Task UpdateAsync(UserDto userDto);
        //Task DeleteAsync(Guid id);
        Task<JsonWebToken> RefreshTokenAsync(TokenDto dto);
    }
}
