using Ecommerce.Shared.Infrastructure.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Tests
{
    public static class AuthHelper
    {
        private static readonly AuthManager _authManager;

        static AuthHelper()
        {
            _authManager = new AuthManager(new AuthOptions()
            {
                IssuerSigningKey = "ubeeg2aigeiDongei1Ni3oel5az2oes0vohd6ohweiphaoyahP231",
                Issuer = "ecommerce",
                Audience = "ecommerce",
                ExpiryInMinutes = 120,
            }, TimeProvider.System);
        }

        public static string CreateToken(string userId, string username, string role)
            => _authManager.GenerateAccessToken(userId, username, role).AccessToken;
    }
}
