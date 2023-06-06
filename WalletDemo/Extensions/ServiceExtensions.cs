using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WalletDemo.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigureServiceCollection(this IServiceCollection services, IConfiguration config)
        {

            return services;
        }
    }
}
