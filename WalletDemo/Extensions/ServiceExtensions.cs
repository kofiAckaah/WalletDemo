using System;
using System.Collections.Generic;
using DAL.DbContexts;
using Domain.Entities;
using Infrastructure.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Shared.Interfaces;

namespace WalletDemo.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigureServiceCollection(this IServiceCollection services, IConfiguration config)
        {
            services.AddDatabase(config);

            services.AddAuthentication(opts=>
                    opts.DefaultAuthenticateScheme = DefaultAuthenticationTypes.ApplicationCookie)
                .AddCookie(DefaultAuthenticationTypes.ApplicationCookie, configuration =>
                {
                    configuration.Cookie.Name = "AuthLogin";
                    configuration.LoginPath = "/api/login";
                });

            services.RegisterServices();
            services.SetupSwagger();

            return services;
        }

        private static void AddDatabase(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<WalletDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
            services.AddIdentity<ApplicationUser, ApplicationRole>(opts=>opts.ConfigureIdentityOptions())
                .AddEntityFrameworkStores<WalletDbContext>()
                .AddDefaultTokenProviders();
            services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();
        }

        private static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IAuthService, AuthenticationService>();

            services.AddScoped<IWalletService, WalletService>();
        }

        private static void SetupSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "WalletAPI",
                    License = new OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });
                var cookieSecurityScheme = new OpenApiSecurityScheme
                {
                    BearerFormat = "Cookies",
                    Name = "Cookie Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = CookieAuthenticationDefaults.AuthenticationScheme,
                    Description = "Put **_ONLY_** your cookie token on textbox below!",

                    Reference = new OpenApiReference
                    {
                        Id = CookieAuthenticationDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                setup.AddSecurityDefinition(cookieSecurityScheme.Reference.Id, cookieSecurityScheme);

                setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { cookieSecurityScheme, new List<string>() }
                });

            });
        }
    }
}
