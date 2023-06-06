using Microsoft.AspNetCore.Builder;

namespace WalletDemo.Extensions
{
    public static class AppBuilderConfiguration
    {
        public static IApplicationBuilder ConfigureAppBuilder(this IApplicationBuilder app)
        {

            return app;
        }
    }
}
