using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Abstractions.Auth
{
    public interface IAuthManager
    {
        JsonWebToken GenerateAccessToken(string userId, string username, string role);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
