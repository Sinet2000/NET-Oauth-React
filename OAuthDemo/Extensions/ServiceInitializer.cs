using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuthDemo.Extensions
{
    public static partial class ServiceInitializer
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>()
                    .AddScoped<ITokenStorage, TokenStorage>();

            return services;
        }
    }
}