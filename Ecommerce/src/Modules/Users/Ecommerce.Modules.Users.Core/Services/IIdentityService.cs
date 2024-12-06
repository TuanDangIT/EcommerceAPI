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
        Task SignUpAsync(SignUpDto dto, CancellationToken cancellationToken = default);
        Task<UserDto?> GetAsync(Guid id, CancellationToken cancellationToken = default);
        Task<JsonWebToken> RefreshTokenAsync(TokenDto dto, CancellationToken cancellationToken = default);
    }
}
