using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuthDemo.Services
{
    public class TokenStorage : ITokenStorage
    {
        public async Task StoreTokensAsync(string accessToken, string refreshToken, int expiresIn)
        {
            // Implement the logic to store the access token, refresh token, and expiration time
        }
    }

    public interface ITokenStorage
    {
        Task StoreTokensAsync(string accessToken, string refreshToken, int expiresIn);
    }
}