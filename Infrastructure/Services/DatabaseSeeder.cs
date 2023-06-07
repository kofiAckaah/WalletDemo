using System;
using System.Linq;
using System.Threading;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Shared.Constants;
using Shared.Interfaces;

namespace Infrastructure.Services
{
    public class DatabaseSeeder : IDatabaseSeeder
    {
        private readonly IServiceProvider serviceProvider;
        private readonly RoleManager<ApplicationRole> roleManagers;
        private readonly IUnitOfWork unitOfWork;

        public DatabaseSeeder(IServiceProvider serviceProvider, RoleManager<ApplicationRole> roleManagers, IUnitOfWork unitOfWork)
        {
            this.serviceProvider = serviceProvider;
            this.roleManagers = roleManagers;
            this.unitOfWork = unitOfWork;
        }

        public async void SeedRoles()
        {
            //var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            string[] roleNames = { RoleConstants.AdminRole, RoleConstants.UserRole };

            foreach (var roleName in roleNames)
            {
                var roleExist = unitOfWork.Repository<ApplicationRole>().Entities.FirstOrDefault(r=>r.Name == roleName);
                if (roleExist == null)
                {
                    var role = new ApplicationRole()
                    {
                        Name = roleName,
                        NormalizedName = roleName.ToUpper(),
                        Id = Guid.NewGuid()
                    };
                    //create the roles and seed them to the database: Question 1
                    //await roleManagers.CreateAsync(new ApplicationRole(){Name = roleName});
                    unitOfWork.Repository<ApplicationRole>().AddAsync(role);
                    unitOfWork.Commit(CancellationToken.None);
                }
            }
        }
    }
    }
