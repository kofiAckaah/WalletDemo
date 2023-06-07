using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Shared.Interfaces;

namespace WalletDemo.Extensions
{
    public static class AppBuilderConfiguration
    {
        public static IApplicationBuilder ConfigureAppBuilder(this IApplicationBuilder app)
        {
            app.ConfigureAuth();
            return app;
        }

        private static void ConfigureAuth(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                await next();
            });
            app.UseAuthentication();
            app.UseAuthorization();
            app.ConfigureSwagger();
            app.SeedDatabase();
        }
        public static void ConfigureSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", typeof(Program).Assembly.GetName().Name);
                options.RoutePrefix = "swagger";
                options.DisplayRequestDuration();
            });
        }

        private static void SeedDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var initializer = serviceScope.ServiceProvider.GetServices<IDatabaseSeeder>().First();

            initializer.SeedRoles();
        }
    }
}
